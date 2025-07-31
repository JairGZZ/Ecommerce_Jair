using System.Buffers.Text;
using System.Security.Authentication;
using Ecommerce_Jair.Server.DTOs;
using Ecommerce_Jair.Server.DTOs.Auth;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Ecommerce_Jair.Server.Services.Interfaces;
using Ecommerce_Jair.Server.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
public class AuthService : IAuthService
{
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IUserTokensRepository _userTokensRepository;
    private readonly IUserRoleService _userRoleService;
    private readonly IEmailService _emailService;

    public AuthService(
        IUserService userService,
        ITokenService tokenService,
        IPasswordHasher<User> passwordHasher,
        IUserTokensRepository tokenRefreshRepository,
        IUserRoleService userRoleService,
        IEmailService emailService)
    {
        _userService = userService;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _userTokensRepository = tokenRefreshRepository;
        _userRoleService = userRoleService;
        _emailService = emailService;
    }
    public async Task RegisterUserAsync(CreateUserDTO userDTO)
    {
        var user = await _userService.GetUserByEmailAsync(userDTO.Email);
        if (user != null) throw new AuthenticationException("User already exists");
        if (userDTO.Password != userDTO.ConfirmPassword) throw new AuthenticationException("Passwords do not match");
        var newUser = new User
        {
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Email = userDTO.Email,
            PhoneNumber = userDTO.PhoneNumber,
            CreatedAt = DateTime.Now
        };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, userDTO.Password);
        await _userService.CreateUserAsync(newUser);

        string emailConfirmationToken = await _tokenService.GenerateEmailConfirmationToken(newUser.UserId);
        string link = GenerateEmailConfirmationLink("http://localhost:5105", newUser.UserId, emailConfirmationToken);
        await _emailService.SendEmailAsync(userDTO.Email, "Confirmacion de Correo", "Si mi logica de chimpance no me falla, esto deberia llegar sin problemas " + link);

        await _userRoleService.AssignRoleToUserAsync(newUser.UserId);
    }
    private string GenerateEmailConfirmationLink(string baseUrl,int idUser,string emailConfirmationToken)
    {
        return $"{baseUrl}/api/Token/ConfirmEmail?token={Uri.EscapeDataString(emailConfirmationToken)}&userId={idUser}";
    }


    public async Task<LoginResponseDTO> AuthenticateUserAsync(LoginRequestDTO loginRequestDTO)
    {
        // 1. Validar usuario y contraseña
        var user = await ValidateUserCredentialsAsync(loginRequestDTO.Email, loginRequestDTO.Password);

        // 2. Crear el DTO mínimo para generar tokens
        var userTokenDTO = new UserTokenDTO
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email
        };

        // 3. Generar accessToken y refreshToken
        var (accessToken, refreshToken) = await CreateTokensForUser(userTokenDTO);

        // 4. Guardar el refresh token en BD
        await SaveRefreshTokenAsync(user.UserId, refreshToken);

        // 5. Armar y devolver la respuesta
        return new LoginResponseDTO
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = _tokenService.GetTokenExpirationDate()
        };
    }

    /// <summary>
    /// 1) Obtiene el usuario por email, 2) verifica existencia y contraseña, 
    /// 3) actualiza LastLogin, 4) retorna la entidad User.
    /// </summary>
    private async Task<User> ValidateUserCredentialsAsync(string email, string password)
    {
        var user = await _userService.GetUserByEmailAsync(email);
        if (user == null)
            throw new AuthenticationException("Usuario no encontrado o credenciales inválidas");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            throw new AuthenticationException("Usuario no autenticado");

        // Actualizar la fecha/hora del último login
        await _userService.UpdateLastLoginAsync(user.UserId);

        return user;
    }

    /// <summary>
    /// Genera accessToken y refreshToken usando ITokenService.
    /// </summary>
    private async Task<(string accessToken, string refreshToken)> CreateTokensForUser(UserTokenDTO userTokenDTO)
    {
        
        string accessToken = await _tokenService.GenerateAccessToken(userTokenDTO);
        string refreshToken = _tokenService.GenerateRefreshToken();
        return (accessToken, refreshToken);
    }

    /// <summary>
    /// Persiste el refresh token en base de datos.
    /// </summary>
    private async Task SaveRefreshTokenAsync(int userId, string refreshToken)
    {
        var refreshTokenModel = new UserTokens
        {
            Token = refreshToken,
            UserId = userId,
            Revoked = false,
            ExpiresAt = _tokenService.GetRefreshTokenExpirationDate(),
            TokenType = "RefreshToken"
        };

        await _userTokensRepository.AddAsync(refreshTokenModel);
        await _userTokensRepository.SaveChangesAsync();
    }
    public async Task LogoutUserAsync(string refreshToken)
    {
        if (await _tokenService.ValidateRefreshToken(refreshToken))
        {

        }
        await _userTokensRepository.InvalidateAsync(refreshToken);
        await _userTokensRepository.SaveChangesAsync();
    }
    public async Task<LoginResponseDTO> RefreshTokenAsync(string refreshToken)
    {
        var refreshTokenModel = await _userTokensRepository.GetByTokenAsync(refreshToken, TokenTypes.RefreshToken);
        if (!await _tokenService.ValidateRefreshToken(refreshToken))
        {
            throw new AuthenticationException();

        }
        await _userTokensRepository.InvalidateAsync(refreshToken);
        await _userTokensRepository.SaveChangesAsync();
        var userTokenDTO = new UserTokenDTO
        {
            UserId = refreshTokenModel.UserId,
            FirstName = refreshTokenModel.User.FirstName,
            LastName = refreshTokenModel.User.LastName,
            Email = refreshTokenModel.User.Email
            
        };
        (string accessToken, string newRefreshToken) = await CreateTokensForUser(userTokenDTO);
        await SaveRefreshTokenAsync(refreshTokenModel.UserId, newRefreshToken);
        return new LoginResponseDTO
        {
            Token = accessToken,
            RefreshToken = newRefreshToken,
            AccessTokenExpiresAt = _tokenService.GetTokenExpirationDate()
        };
    }
    private void ValidateRefreshToke(UserTokens refreshTokenModel)
    {
        if (refreshTokenModel == null || refreshTokenModel.Revoked || refreshTokenModel.ExpiresAt < DateTime.UtcNow) 
            throw new SecurityTokenExpiredException("Token inválido o ya expirado");

    }
    

    public Task<bool> ResetPasswordAsync(string email, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateProfileAsync(string email, string name, string address)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAccountAsync()
    {
        throw new NotImplementedException();
    }
}
