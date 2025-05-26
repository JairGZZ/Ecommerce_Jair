interface IAuthService
{
    Task<bool> RegisterUserAsync(string email, string password);
    Task<bool> LoginUserAsync(string email, string password);
    Task<bool> LogoutUserAsync();
    Task<bool> ResetPasswordAsync(string email, string newPassword);
    Task<bool> ChangePasswordAsync(string oldPassword, string newPassword);
    Task<bool> UpdateProfileAsync(string email, string name, string address);
    Task<bool> DeleteAccountAsync();
}