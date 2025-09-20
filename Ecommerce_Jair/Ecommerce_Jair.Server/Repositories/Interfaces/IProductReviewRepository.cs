using Ecommerce_Jair.Server.Models;

namespace Ecommerce_Jair.Server.Repositories.Interfaces
{
    public interface IProductReviewRepository
    {
        public Task<ProductReview> CreateProductReviewAsync(ProductReview newProductReview);
        public Task<ProductReview?> GetProductReviewByIdAsync(int productReviewId);
        public Task<bool> DeleteProductReviewAsync(int productReviewId);  
        public Task SaveChangesAsync();
    }
}
