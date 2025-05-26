using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.DTOs;
using Ecommerce_Jair.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<User> GetUserByIdAsync(int userId)
    {
        return _userRepository.GetUserByIdAsync(userId);
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return _userRepository.GetAllUsersAsync();
    }

    public Task<bool> DeleteUserAsync(int userId)
    {
        return _userRepository.DeleteUserAsync(userId);
    }
  
    public Task<bool> CreateUserAsync(CreateUserDTO userDTO)
    {
        var user = new User
        {
            FirstName = userDTO.FirstName,
            LastName = userDTO.LastName,
            Email = userDTO.Email,
            PasswordHash = userDTO.Password,
            PhoneNumber = userDTO.PhoneNumber,
            CreatedAt = DateTime.Now
        };
        return _userRepository.CreateUserAsync(user);
    }

    public Task<bool> UpdateUserAsync(int userId, User user)
    {
        return _userRepository.UpdateUserAsync(userId, user);
    }
}