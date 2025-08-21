using Ecommerce_Jair.Server.BD.context;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Jair.Server.Repositories.implementations;

public class UserRoleRepository : IUserRoleRepository
{
    private readonly EcommerceDbContext _context;
    public UserRoleRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public async Task AssignRoleToUserAsync(UserRole userRole)
    {
        await _context.UserRoles.AddAsync(userRole);
    }

    public async Task<UserRole> GetUserRoleAsync(int userId)
    {
         return await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
    }

    public async Task SaveChangesAsync()
    {
         await _context.SaveChangesAsync();
    }

}
