using Ecommerce_Jair.Server.DTOs.ProductReview;
using Ecommerce_Jair.Server.Models.Results;

namespace Ecommerce_Jair.Server.Services.Interfaces
{
    public interface IProductReviewService
    {
        Task<TResult<ShowProductReviewDTO>> CreateProductReviewAsync(CreateProductReviewDTO newProductReview);
        Task<TResult<bool>> DeleteProductReviewAsync(int productReviewId);
    }
}
