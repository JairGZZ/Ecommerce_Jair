namespace Ecommerce_Jair.Server.DTOs.Product
{
    public sealed class ProductListItem
    {
        public int ProductId { get; init; }
        public string ProductName { get; init; } = "";
        public decimal Price { get; init; }
        public int Stock { get; init; }
        public string Sku { get; init; } = "";
        public string? ImageUrl { get; init; }
        public int CategoryId { get; init; }
        public string CategoryName { get; init; } = "";
        public DateTime? UpdatedAt { get; init; }
        public bool InStock => Stock > 0;
    }

}
