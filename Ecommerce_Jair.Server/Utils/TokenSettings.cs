using Microsoft.IdentityModel.Tokens;

namespace Ecommerce_Jair.Server.Utils
{
    public class TokenSettings
    {
        public string SecretKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpiryMinutes { get; set; }
    }
}