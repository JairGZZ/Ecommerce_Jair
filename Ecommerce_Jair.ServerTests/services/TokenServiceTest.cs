using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Ecommerce_Jair.Server.DTOs.Auth;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Services.implementations;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace Ecommerce_Jair.ServerTests.services
{
    public class TokenServiceTests
    {
        private readonly IConfiguration _configuration;
        private readonly TokenService _tokenService;

        public TokenServiceTests()
        {
            _configuration = BuildJwtConfiguration();
            _tokenService = new TokenService(_configuration);
        }

        private IConfiguration BuildJwtConfiguration()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Jwt:Key", "mi_clave_super_secreta_para_pruebas" }
            };

            return new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Fact]
        public void GenerateAccessToken_NoDebeSerNuloNiVacio()
        {
            // Arrange
            var usuario = new UserTokenDTO
            {
                UserId = 123,
                FirstName = "Jair",
                LastName = "gz",
                Email = "jair.gz@example.com"
            };

            // Act
            string token = _tokenService.GenerateAccessToken(usuario);

            // Depuración
            Console.WriteLine("Token generado:");
            Console.WriteLine(token);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public void GenerateAccessToken_DebeContenerClaimsCorrectos()
        {
            // Arrange
            var usuario = new UserTokenDTO
            {
                UserId = 123,
                FirstName = "Jair",
                LastName = "gz",
                Email = "jair.gz@example.com"
            };

            // Act
            string tokenString = _tokenService.GenerateAccessToken(usuario);

            // Depuración
            Console.WriteLine("\nToken generado por GenerateAccessToken:");
            Console.WriteLine(tokenString);

            var handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwt = handler.ReadJwtToken(tokenString);

            // Mostrar claims
            Console.WriteLine("\nClaims encontrados:");
            foreach (var claim in jwt.Claims)
            {
                Console.WriteLine($"- {claim.Type}: {claim.Value}");
            }

            // Assert
            var claims = jwt.Claims.ToDictionary(c => c.Type, c => c.Value);

            Assert.Contains(ClaimTypes.NameIdentifier, claims);
            Assert.Equal("123", claims[ClaimTypes.NameIdentifier]);

            Assert.Contains(ClaimTypes.Name, claims);
            Assert.Equal("Jair gz", claims[ClaimTypes.Name]);

            Assert.Contains(ClaimTypes.Email, claims);
            Assert.Equal("jair.gz@example.com", claims[ClaimTypes.Email]);
        }
    }
}