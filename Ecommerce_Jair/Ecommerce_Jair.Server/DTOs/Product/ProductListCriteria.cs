namespace Ecommerce_Jair.Server.DTOs.Product
{
    public sealed class ProductListCriteria
    {
        public int Page { get; init; } = 1;
        public int PageSize { get; init; } = 20;
        public string? Q { get; init; }
        public int? CategoryId { get; init; }
        public bool? InStock { get; init; }
        public decimal? PriceMin { get; init; }
        public decimal? PriceMax { get; init; }
        public string Sort { get; init; } = "CreatedAt"; 
        public string Dir { get; init; } = "desc";       
    }

}
