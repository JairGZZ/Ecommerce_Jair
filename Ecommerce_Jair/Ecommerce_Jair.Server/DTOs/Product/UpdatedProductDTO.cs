namespace Ecommerce_Jair.Server.DTOs.Product
{
    public record UpdateProductDto
   (
       string? ProductName,
       string? Description,
       decimal? Price,
       int? Stock,
       string? Sku,
       int? CategoryId,
       string? ImageUrl
   );
}
