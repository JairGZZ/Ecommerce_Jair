namespace Ecommerce_Jair.Server.DTOs;

public class LoginResponseDTO
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiresAt { get; set; }
}
