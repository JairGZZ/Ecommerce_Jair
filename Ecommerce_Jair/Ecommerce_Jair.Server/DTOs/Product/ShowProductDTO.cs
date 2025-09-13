namespace Ecommerce_Jair.Server.DTOs.Product
{
    public record ShowProductDTO(
        string ProductName,
        string? Description,
        decimal Price,
        int stock

        
        );
}
