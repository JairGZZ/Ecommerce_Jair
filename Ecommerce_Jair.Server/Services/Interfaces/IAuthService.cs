using Ecommerce_Jair.Server.DTOs;

public interface IAuthService
{
    Task RegisterUserAsync(CreateUserDTO userDTO);
    Task<LoginResponseDTO> AuthenticateUserAsync(LoginRequestDTO loginRequestDTO);
    Task LogoutUserAsync(string refreshToken);
    Task<LoginResponseDTO> RefreshTokenAsync(string refreshToken);
    Task<bool> ResetPasswordAsync(string email, string newPassword);
    Task<bool> ChangePasswordAsync(string oldPassword, string newPassword);
    Task<bool> UpdateProfileAsync(string email, string name, string address);
    Task<bool> DeleteAccountAsync();
}