using Ecommerce_Jair.Server.Models;

namespace Ecommerce_Jair.Server.Services.Interfaces;

public interface IUserRoleService
{
    Task AssignRoleToUserAsync(int userId);
    Task SaveChangesAsync();
    Task<UserRole> GetUserRoleAsync(int userId);
}