using Ecommerce_Jair.Server.Models;

namespace Ecommerce_Jair.Server.Repositories.Interfaces;

public interface IUserRoleRepository
{
    Task AssignRoleToUserAsync(UserRole userRole);
    Task SaveChangesAsync();
    Task<UserRole> GetUserRoleAsync(int userId);
}
