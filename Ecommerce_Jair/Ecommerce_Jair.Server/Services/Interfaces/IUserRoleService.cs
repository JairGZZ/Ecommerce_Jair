using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Models.Results;

namespace Ecommerce_Jair.Server.Services.Interfaces;

public interface IUserRoleService
{
    Task<Result> AssignRoleToUserAsync(int userId);
    Task<Result> SaveChangesAsync();
    Task<TResult<UserRole>> GetUserRoleAsync(int userId);
}