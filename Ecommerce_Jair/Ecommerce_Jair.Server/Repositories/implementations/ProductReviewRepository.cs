using Ecommerce_Jair.Server.BD.context;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Jair.Server.Repositories.implementations
{
    public class ProductReviewRepository : IProductReviewRepository
    {
        private readonly EcommerceDbContext _context;
        public ProductReviewRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<ProductReview> CreateProductReviewAsync(ProductReview newProductReview)
        {
            await _context.ProductReviews.AddAsync(newProductReview);
            return newProductReview;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> DeleteProductReviewAsync(int productReviewId)
        {
            var productReview = await GetProductReviewByIdAsync(productReviewId);
            if (productReview == null)
            {
                return false; // Product review not found
            }

            productReview.IsDeleted = true; // Soft delete
            _context.ProductReviews.Update(productReview);
            
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<ProductReview?> GetProductReviewByIdAsync(int productReviewId)
        => await _context.ProductReviews.FirstOrDefaultAsync(pr => pr.ReviewId == productReviewId);
          
    }
}
