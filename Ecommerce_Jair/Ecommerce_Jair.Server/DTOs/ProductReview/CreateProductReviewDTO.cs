namespace Ecommerce_Jair.Server.DTOs.ProductReview
{
    public record CreateProductReviewDTO(
        int productId,
        int userId,
        int Rating,
        string? Comment

        );
}
