using Ecommerce_Jair.Server.BD.context;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Jair.Server.Repositories.implementations;

public class UserTokensRepository : IUserTokensRepository
{
    private readonly EcommerceDbContext _context;

    public UserTokensRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserTokens token)
    {
        await _context.UserTokens.AddAsync(token);
    }

    public async Task<UserTokens> GetByTokenAsync(string token,string tokenType)
    {
        var refreshToken = await _context.UserTokens
            .Where(t => t.Token == token && t.TokenType == tokenType)
            .Select(t => new UserTokens
            {
                Id = t.Id,
                UserId = t.UserId,
                Token = t.Token,
                ExpiresAt = t.ExpiresAt,
                Revoked = t.Revoked,
                TokenType = t.TokenType,
                User = new User
                {
                    FirstName = t.User.FirstName,
                    LastName = t.User.LastName,
                    Email = t.User.Email,
                    
                }
            })
            .FirstOrDefaultAsync();

        return refreshToken;
    }


    public async Task InvalidateAsync(string token)
    {
        var refreshToken = await _context.UserTokens.FirstOrDefaultAsync(t => t.Token == token);
        if (refreshToken == null) return;
        refreshToken.Revoked = true;
        _context.Update(refreshToken);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
