using System.Threading.Tasks;
using Ecommerce_Jair.Server.Models;
public interface IUserRepository
{
    Task<User> GetUserByIdAsync(int userId);
    Task<List<User>> GetAllUsersAsync();
    Task<bool> DeleteUserAsync(int userId);
    Task<bool> UpdateUserAsync(int userId, User user);
    Task<bool> CreateUserAsync(User user);
}