using Ecommerce_Jair.Server.Models;

namespace Ecommerce_Jair.Server.Repositories.Interfaces;

interface ITokenRefreshRepository
{
    Task AddAsync(RefreshToken token);

    Task<RefreshToken> GetByTokenAsync(string token);

    Task InvalidateAsync(string token);

    Task SaveChangesAsync();
}
