using Ecommerce_Jair.Server.DTOs.Auth;
using Ecommerce_Jair.Server.Models;

namespace Ecommerce_Jair.Server.Services.Interfaces;

public interface ITokenService
{
    Task<string> GenerateAccessToken(UserTokenDTO user);
    string GenerateRefreshToken();
    Task<string> GenerateEmailConfirmationToken(int userId);
    Task<bool> ValidateEmailConfirmationTokenAsync(int userId,string token);
    Task<bool> ValidateRefreshToken(string token);
    DateTime GetTokenExpirationDate();
    DateTime GetRefreshTokenExpirationDate();

    
}