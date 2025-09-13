namespace Ecommerce_Jair.Server.DTOs.Product
{
    public record ProductDetailsDTO(
    int ProductId,
    string ProductName,
    string Description,
    decimal Price,
    int Stock,
    string Sku,
    string CategoryName,
    string? ImageUrl,
    DateTime? CreatedAt,
    DateTime? UpdatedAt
    ); 

}
