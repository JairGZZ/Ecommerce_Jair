using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Models.Results;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Ecommerce_Jair.Server.Services.Interfaces;

namespace Ecommerce_Jair.Server.Services.implementations;
public class UserRoleService : IUserRoleService
{
    private readonly IUserRoleRepository _userRoleRepository;
    public UserRoleService(IUserRoleRepository userRoleRepository)
    {
        _userRoleRepository = userRoleRepository;
    }
    public async Task<Result> AssignRoleToUserAsync(int userId)
    {
        var userRole = new UserRole
        {
            UserId = userId,
            RoleId = 2,
            AssignedDate = DateTime.Now,
            IsPrimary = true

        };
        await _userRoleRepository.AssignRoleToUserAsync(userRole);
        await _userRoleRepository.SaveChangesAsync();
        return Result.Ok();
    }
    public async Task<TResult<UserRole>> GetUserRoleAsync(int userId)
    {
        return TResult<UserRole>.Ok(await _userRoleRepository.GetUserRoleAsync(userId));
    }
    public async Task<Result> SaveChangesAsync()
    {
        await _userRoleRepository.SaveChangesAsync();
        return Result.Ok();
    }
}