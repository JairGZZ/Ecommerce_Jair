using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Ecommerce_Jair.Server.Services.implementations;
public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(User user)
    {
        Claim[] claims =
        [
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
            new Claim(ClaimTypes.Email, user.Email),
        ];

        var cfg = _configuration.GetSection("Jwt");
        Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA" + cfg["Key"]);

        JwtSecurityToken token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(cfg["Key"])), SecurityAlgorithms.HmacSha256)
        );


        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

        return handler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        throw new NotImplementedException();
    }

    public DateTime GetTokenExpirationDate()
    {
        throw new NotImplementedException();
    }

    public DateTime GetRefreshTokenExpirationDate()
    {
        throw new NotImplementedException();
    }
}