using Ecommerce_Jair.Server.BD.context;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly EcommerceDbContext _context;

    public UserRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public Task<bool> DeleteUserAsync(int userId)
    {
        var user = _context.Users.Find(userId);
        if (user == null) return Task.FromResult(false);
        _context.Users.Remove(user);
        return Task.FromResult(true);
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return _context.Users.ToListAsync();
    }

    public Task<User> GetUserByIdAsync(int userId)
    {
        var user = _context.Users.Find(userId);
        if (user == null) return Task.FromResult<User>(null);
        return Task.FromResult(user);
    }

    public Task<bool> UpdateUserAsync(int userId, User user)
    {
       
        _context.Users.Update(user);
        _context.SaveChangesAsync();
        return Task.FromResult(true);
    }
    public Task<bool> CreateUserAsync(User user)
    {
        _context.Users.Add(user);
        _context.SaveChangesAsync();
        return Task.FromResult(true);
    }
    
}
