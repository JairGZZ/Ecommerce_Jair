using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.DTOs;

namespace Ecommerce_Jair.Server.Services.Interfaces
{

    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> DeleteUserAsync(int userId);
        Task<bool> UpdateUserAsync(int userId, User user);
        Task<bool> CreateUserAsync(CreateUserDTO user);
    }
}