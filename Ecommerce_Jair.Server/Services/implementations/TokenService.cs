using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce_Jair.Server.DTOs.Auth;
using Ecommerce_Jair.Server.Models;
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

    public TokenService(IOptions<TokenSettings> tokenSettings, IUserRoleService userRoleService,IUserTokensRepository userTokensRepository)
    {
        
        _userRoleService = userRoleService;
        _tokenSettings = tokenSettings.Value;
        _userTokensRepository = userTokensRepository;
    }

    public async Task<string> GenerateAccessToken(UserTokenDTO user)
    {
        var userRole = await _userRoleService.GetUserRoleAsync(user.UserId);
        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, userRole.RoleId.ToString()),
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

        return handler.WriteToken(token);
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

    public async Task<string> GenerateEmailConfirmationToken(int userId)
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
        await _userTokensRepository.AddAsync(userToken);
        await _userTokensRepository.SaveChangesAsync();
        return token;
        
    }
     
    public async Task<bool> ValidateEmailConfirmationTokenAsync(int userId, string token)
    {
        var tokenExists = await _userTokensRepository.GetByTokenAsync(token, TokenTypes.EmailConfirmationToken);
        if (tokenExists.UserId != userId )
        {
            return false;
        }
        if(tokenExists.ExpiresAt < DateTime.UtcNow)
        {
            return false;
        }
        if (tokenExists.Revoked)
        {
            return false;
        }
        tokenExists.Revoked = true;
        await _userTokensRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ValidateRefreshToken(string refreshToken)
    {
        var userTokenModel = await _userTokensRepository.GetByTokenAsync(refreshToken, TokenTypes.RefreshToken);

        if (userTokenModel == null || userTokenModel.Revoked || userTokenModel.ExpiresAt < DateTime.UtcNow) {
            return false;

        }
        return true;
    }
}