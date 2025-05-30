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

    public async Task AddAsync(RefreshToken token)
    {
        await _context.RefreshTokens.AddAsync(token);
    }

    public async Task<RefreshToken> GetByTokenAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        if (refreshToken == null) return null;
        return refreshToken;
    }

    public async Task InvalidateAsync(string token)
    {
        var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
        if (refreshToken == null) return;
        refreshToken.Revoked = true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
