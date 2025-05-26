//public class AuthService : IAuthService
//{
//    private readonly IUserRepository _userRepository;

//    public AuthService(IUserRepository userRepository)
//    {
//        _userRepository = userRepository;
//    }

    //public async Task<bool> RegisterUserAsync(string email, string password)
    //{
    //    // Implement registration logic
    //    return await _userRepository.RegisterUserAsync(email, password);
    //}

    //public async Task<bool> LoginUserAsync(string email, string password)
    //{
    //    // Implement login logic
    //    return await _authenticationService.LoginAsync(email, password);
    //}

    //public async Task<bool> LogoutUserAsync()
    //{
    //    // Implement logout logic
    //    return await _authenticationService.LogoutAsync();
    //}

    //public Task<bool> ResetPasswordAsync(string email, string newPassword)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<bool> UpdateProfileAsync(string email, string name, string address)
    //{
    //    throw new NotImplementedException();
    //}

    //public Task<bool> DeleteAccountAsync()
    //{
    //    throw new NotImplementedException();
    //}
//}