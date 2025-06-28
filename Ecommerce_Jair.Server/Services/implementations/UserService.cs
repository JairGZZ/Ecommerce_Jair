using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.DTOs;
using Ecommerce_Jair.Server.Services.Interfaces;

public class UserService : IUserService
/// <summary>
/// The user repository.
/// </summary>
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ShowUserDTO> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if (user == null) return null;

        var userDTO = new ShowUserDTO
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        };
        return userDTO;

    }

    public async Task<List<ShowUserDTO>> GetAllUsersAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(user => new ShowUserDTO
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            PhoneNumber = user.PhoneNumber
        }).ToList();
    }

    public async Task DeleteUserAsync(int userId)
    {
        await _userRepository.DeleteUserAsync(userId);
        await _userRepository.SaveChangesAsync();
    }

    public async Task CreateUserAsync(User user)
    {
        await _userRepository.CreateUserAsync(user);
        await _userRepository.SaveChangesAsync();
    }


    public async Task<bool> UpdateUserAsync(int userId, User user)
    {
        bool isUpdated = await _userRepository.UpdateUserAsync(userId, user);
        if (!isUpdated) return false;

        await _userRepository.SaveChangesAsync();
        return true;
    }
    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _userRepository.GetUserByEmailAsync(email);
    }
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userRepository.EmailExistsAsync(email);
    }
    public async Task UpdateLastLoginAsync(int id)
    {
        await _userRepository.UpdateLastLoginAsync(id);
    }
}