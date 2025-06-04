using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Jair.Server.DTOs.Auth;
public class LogoutRequestDTO
{
    [Required]
    public string RefreshToken { get; set; }
}
