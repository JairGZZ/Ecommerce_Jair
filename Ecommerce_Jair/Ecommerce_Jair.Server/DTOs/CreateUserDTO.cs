using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Jair.Server.DTOs{

    public class CreateUserDTO
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;
        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;
        [Required]
        [EmailAddress(ErrorMessage = "Por favor, Ingrese un correo electronico valido")]
        public string Email { get; set; } = null!;
        [Required]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Password { get; set; } = null!;
        [Required]
        [Compare("Password",ErrorMessage ="Las contraseñas no coinciden")]
        public string ConfirmPassword { get; set; } = null!;
        [Required]
        [Phone(ErrorMessage = "Por favor, Ingrese un numero telefonico valido")]
        public string? PhoneNumber { get; set; }
    }

}
