using System.Security.Authentication;
using Ecommerce_Jair.Server.DTOs;
using Ecommerce_Jair.Server.DTOs.Auth;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Ecommerce_Jair.Server.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

public class AuthService : IAuthService
{
    /// <summary>
    /// The repository for user data access.
    /// </summary>
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ITokenRefreshRepository _tokenRefreshRepository;
    public AuthService(IUserService userService, ITokenService tokenService, IPasswordHasher<User> passwordHasher, ITokenRefreshRepository tokenRefreshRepository)
    {
        _userService = userService;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
        _tokenRefreshRepository = tokenRefreshRepository;
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
    }

    public async Task<LoginResponseDTO> AuthenticateUserAsync(LoginRequestDTO loginRequestDTO)
    {
        var user = await _userService.GetUserByEmailAsync(loginRequestDTO.Email);
        if (user == null) throw new AuthenticationException("User Unauthenticated");
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, loginRequestDTO.Password);

        await _userService.UpdateLastLoginAsync(user.UserId);
        if (result == PasswordVerificationResult.Failed) throw new AuthenticationException("User Unauthenticated");
        var userTokenDTO = new UserTokenDTO
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };
        var accessToken = _tokenService.GenerateAccessToken(userTokenDTO);
        var refreshToken = _tokenService.GenerateRefreshToken();

        var refreshTokenModel = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.UserId,
            Revoked = false,
            ExpiresAt = _tokenService.GetRefreshTokenExpirationDate()
        };
        await _tokenRefreshRepository.AddAsync(refreshTokenModel);
        await _tokenRefreshRepository.SaveChangesAsync();

        return new LoginResponseDTO
        {
            Token = accessToken,
            RefreshToken = refreshToken,
            AccessTokenExpiresAt = _tokenService.GetTokenExpirationDate()
        };
    }

    public async Task<bool> LogoutUserAsync()
    {
        // Implement logout logic
        throw new NotImplementedException();
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