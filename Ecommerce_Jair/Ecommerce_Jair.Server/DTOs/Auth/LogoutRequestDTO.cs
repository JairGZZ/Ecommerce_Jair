using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Jair.Server.DTOs.Auth;
public record LogoutRequestDTO
(
    [Required]
     string RefreshToken 
);
