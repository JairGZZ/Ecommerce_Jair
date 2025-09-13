using System.ComponentModel.DataAnnotations;

namespace Ecommerce_Jair.Server.DTOs.Product
{
    public record CreateProductDTO(
        [Required]
        string ProductName,
        string? Description,
        [Required]
        decimal Price,
        [Required]
        int Stock,
        [Required]
        string Sku,
        [Required]
        int CategoryId,
        string? ImageUrl
        );
}
