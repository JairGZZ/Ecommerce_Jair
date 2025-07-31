using System.Security.Cryptography;

namespace Ecommerce_Jair.Server.Utils
{
    public static class TokenUtils
    {

        public static string GenerateSecureToken(int length = 32)
    {
        var randomBytes = new byte[length];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }

}
}
