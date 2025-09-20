namespace Ecommerce_Jair.Server.DTOs.ProductReview
{
    public record ShowProductReviewDTO(
        int ReviewId,
        int ProductId,
        int UserId,
        int? Rating,
        string? Comment,
        DateTime? CreatedAt
      

        );
}
