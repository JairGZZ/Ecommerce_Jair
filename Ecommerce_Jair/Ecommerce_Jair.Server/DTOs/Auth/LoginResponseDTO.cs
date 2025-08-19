namespace Ecommerce_Jair.Server.DTOs;

public record LoginResponseDTO
(
    string Token,
     string RefreshToken,
     DateTime AccessTokenExpiresAt);
