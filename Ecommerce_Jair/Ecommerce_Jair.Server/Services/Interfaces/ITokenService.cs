using Ecommerce_Jair.Server.DTOs.Auth;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Models.Results;

namespace Ecommerce_Jair.Server.Services.Interfaces;

public interface ITokenService
{
    Task<TResult<string>> GenerateAccessToken(UserTokenDTO user);
    string GenerateRefreshToken();
    Task<TResult<string>> GenerateEmailConfirmationToken(int userId);
    Task<TResult<bool>> ValidateEmailConfirmationTokenAsync(int userId,string token);
    Task<TResult<bool>> ValidateRefreshToken(string token);
    DateTime GetTokenExpirationDate();
    DateTime GetRefreshTokenExpirationDate();

    
}