using Ecommerce_Jair.Server.DTOs.Auth;

namespace Ecommerce_Jair.Server.Services.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessToken(UserTokenDTO user);
    string GenerateRefreshToken();
    DateTime GetTokenExpirationDate();
    DateTime GetRefreshTokenExpirationDate();

    
}