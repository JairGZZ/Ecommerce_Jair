using Ecommerce_Jair.Server.BD.context;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Jair.Server.Repositories.implementations;

public class TokenRefreshRepository : ITokenRefreshRepository
{
    private readonly EcommerceDbContext _context;

    public TokenRefreshRepository(EcommerceDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(UserTokens token)
    {
        await _context.UserTokens.AddAsync(token);
    }

    public async Task<UserTokens> GetByTokenAsync(string token)
    {
        var refreshToken = await _context.UserTokens.FirstOrDefaultAsync(t => t.Token == token);
        if (refreshToken == null) return null;
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
