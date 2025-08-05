using Ecommerce_Jair.Server.DTOs;
using Ecommerce_Jair.Server.Models.Results;

public interface IAuthService
{
    Task<Result> RegisterUserAsync(CreateUserDTO userDTO);
    Task<TResult<LoginResponseDTO>> AuthenticateUserAsync(LoginRequestDTO loginRequestDTO);
    Task<Result> LogoutUserAsync(string refreshToken);
    Task<TResult<LoginResponseDTO>> RefreshTokenAsync(string refreshToken);
    Task<bool> ResetPasswordAsync(string email, string newPassword);
    Task<bool> ChangePasswordAsync(string oldPassword, string newPassword);
    Task<bool> UpdateProfileAsync(string email, string name, string address);
    Task<bool> DeleteAccountAsync();
}