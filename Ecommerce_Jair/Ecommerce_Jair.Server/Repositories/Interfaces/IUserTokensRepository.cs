using Ecommerce_Jair.Server.Models;

namespace Ecommerce_Jair.Server.Repositories.Interfaces;

public interface IUserTokensRepository
{
    Task AddAsync(UserTokens token);

    Task<UserTokens> GetByTokenAsync(string token,string tokenType);

    Task InvalidateAsync(string token);

    Task SaveChangesAsync();
}
