using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce_Jair.Server.DTOs.Auth;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Models.Results;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Ecommerce_Jair.Server.Services.Interfaces;
using Ecommerce_Jair.Server.Utils;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce_Jair.Server.Services.implementations;

public class TokenService : ITokenService
{
    
    private readonly IUserRoleService _userRoleService;
    private readonly TokenSettings _tokenSettings;
    private readonly IUserTokensRepository _userTokensRepository;
    private readonly IUserRepository _userRepository;

    public TokenService(IOptions<TokenSettings> tokenSettings, IUserRoleService userRoleService,IUserTokensRepository userTokensRepository,IUserRepository userRepository)
    {
        
        _userRoleService = userRoleService;
        _tokenSettings = tokenSettings.Value;
        _userTokensRepository = userTokensRepository;
        _userRepository = userRepository;
    }

    public async Task<TResult<string>> GenerateAccessToken(UserTokenDTO user)
    {
        var userRole = await _userRoleService.GetUserRoleAsync(user.UserId);
        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, userRole.Data.RoleId.ToString()),
        ];
        var expires = DateTime.UtcNow.AddMinutes(_tokenSettings.ExpiryMinutes);
        byte[] keyBytes = Encoding.UTF8.GetBytes(_tokenSettings.SecretKey);
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(keyBytes);



        JwtSecurityToken token = new JwtSecurityToken(
            issuer: _tokenSettings.Issuer,
            audience: _tokenSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        );


        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        return TResult<String>.Ok(handler.WriteToken(token));
    }

    public string GenerateRefreshToken()
    {
        return TokenUtils.GenerateSecureToken();
    }

    public DateTime GetTokenExpirationDate()
    {
        return DateTime.UtcNow.AddMinutes(_tokenSettings.ExpiryMinutes);
    }

    public DateTime GetRefreshTokenExpirationDate()
    {
        return DateTime.UtcNow.AddDays(7);
    }

    public async Task<TResult<string>> GenerateEmailConfirmationToken(int userId)
    {
        var token = TokenUtils.GenerateSecureToken();
        var userToken = new UserTokens
        {
            UserId = userId,
            Token = token,
            TokenType = TokenTypes.EmailConfirmationToken,
            ExpiresAt = DateTime.UtcNow.AddHours(24),
            Revoked = false


        };
       
        await _userRepository.SaveChangesAsync();
        
        await _userTokensRepository.AddAsync(userToken);
        await _userTokensRepository.SaveChangesAsync();
        return TResult<string>.Ok(token);
        
    }
     
    public async Task<TResult<bool>> ValidateEmailConfirmationTokenAsync(int userId, string token)
    {
        var tokenExists = await _userTokensRepository.GetByTokenAsync(token, TokenTypes.EmailConfirmationToken);
        if (tokenExists.UserId != userId )
        {
            return TResult<bool>.Fail("Token Invalido");
        }
        if(tokenExists.ExpiresAt < DateTime.UtcNow)
        {
            return TResult<bool>.Fail("El token Expiro");
        }
        if (tokenExists.Revoked)
        {
            return TResult<bool>.Fail("Token Revocadi");
        }
        tokenExists.Revoked = true;
        var user = await _userRepository.GetUserByIdAsync(userId);
        user.IsEmailConfirmed = true;
        await _userTokensRepository.SaveChangesAsync();
        return TResult<bool>.Ok(true);
    }

    public async Task<TResult<bool>> ValidateRefreshToken(string refreshToken)
    {
        var userTokenModel = await _userTokensRepository.GetByTokenAsync(refreshToken, TokenTypes.RefreshToken);

        if (userTokenModel == null || userTokenModel.Revoked || userTokenModel.ExpiresAt < DateTime.UtcNow) {
            return TResult<bool>.Fail("Token expirado o invalido");

        }
        return TResult<bool>.Ok(true);
    }
}