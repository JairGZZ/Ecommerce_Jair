using Ecommerce_Jair.Server.Models;

namespace Ecommerce_Jair.Server.Services.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    DateTime GetTokenExpirationDate();
    DateTime GetRefreshTokenExpirationDate();
    
}