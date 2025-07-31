using System.Threading.Tasks;
using Ecommerce_Jair.Server.Models;
public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int userId);
    Task<List<User>> GetAllUsersAsync();
    Task DeleteUserAsync(int userId);
    Task<bool> UpdateUserAsync(int userId, User user);
    Task CreateUserAsync(User user);
    Task SaveChangesAsync();
    Task<User> GetUserByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
    Task UpdateLastLoginAsync(int id);
    
}