namespace Ecommerce_Jair.Server.DTOs.Product
{
    public record ProductCreatedDto(
    int ProductId,
    string ProductName,
    string? Description,
    decimal Price,
    int Stock,
    string Sku,
    int CategoryId,
    string CategoryName,
    string? ImageUrl,
    DateTime? CreatedAt,
    DateTime? UpdatedAt);

}
