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
    public async Task DeleteUserAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return;
        _context.Users.Remove(user);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);

        if (user == null) return null;
        return user;
    }

    public async Task<bool> UpdateUserAsync(int userId, User user)
    {
        var userUpdate = await _context.Users.FindAsync(userId);
        if (userUpdate == null) return false;
        userUpdate.FirstName = user.FirstName;
        userUpdate.LastName = user.LastName;
        userUpdate.Email = user.Email;
        userUpdate.PasswordHash = user.PasswordHash;
        userUpdate.PhoneNumber = user.PhoneNumber;
        _context.Users.Update(userUpdate);
        return true;
    }
    public async Task CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }
    public async Task<User> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
    public async Task<bool> UserExistsByIdAsync(int userId)
    {
        return await _context.Users.AnyAsync(u => u.UserId == userId);
    }   

    public async Task UpdateLastLoginAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return;
        user.LastLogin = DateTime.Now;
        _context.Users.Update(user);
    }
}
