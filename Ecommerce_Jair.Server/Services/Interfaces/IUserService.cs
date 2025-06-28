using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.DTOs;

namespace Ecommerce_Jair.Server.Services.Interfaces
{

    public interface IUserService
    {
        Task<ShowUserDTO> GetUserByIdAsync(int userId);
        Task<List<ShowUserDTO>> GetAllUsersAsync();
        Task DeleteUserAsync(int userId);
        Task<bool> UpdateUserAsync(int userId, User user);
        Task CreateUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task UpdateLastLoginAsync(int id);
    }
}