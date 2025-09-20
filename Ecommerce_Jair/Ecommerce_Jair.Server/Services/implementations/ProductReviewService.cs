using Ecommerce_Jair.Server.DTOs.ProductReview;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Models.Results;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Ecommerce_Jair.Server.Services.Interfaces;

namespace Ecommerce_Jair.Server.Services.implementations
{
    public class ProductReviewService : IProductReviewService
    {
        private readonly IProductReviewRepository _productReviewRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;
        public ProductReviewService(IProductReviewRepository productReviewRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _productReviewRepository = productReviewRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }
        public async Task<TResult<ShowProductReviewDTO>> CreateProductReviewAsync(CreateProductReviewDTO newProductReview)
        {
            var productExists = await _productRepository.ExistProductByIdAsync(newProductReview.productId);
            if (!productExists)
            {
                return TResult<ShowProductReviewDTO>.Fail("El producto no existe");
            }
            var userExists = await _userRepository.UserExistsByIdAsync(newProductReview.userId);
            if (!userExists)
            {
                return TResult<ShowProductReviewDTO>.Fail("El usuario no existe");
            }
            var createdProductReview = MapCreateProductReviewDtoToProductReview(newProductReview);

            var showProduct = await _productReviewRepository.CreateProductReviewAsync(createdProductReview);
            await _productReviewRepository.SaveChangesAsync();


            return TResult<ShowProductReviewDTO>.Ok(MapProductReviewToShowProductReviewDto(showProduct));
        }
        public async Task<TResult<bool>> DeleteProductReviewAsync(int productReviewId)
        {
            var productReview = await _productReviewRepository.GetProductReviewByIdAsync(productReviewId);
            if (productReview == null)
            {
                return TResult<bool>.Fail("La reseña del producto no existe");
            }

            var result = await _productReviewRepository.DeleteProductReviewAsync(productReviewId);
            if (!result)
            {
                return TResult<bool>.Fail("No se pudo eliminar la reseña del producto");
            }

            return TResult<bool>.Ok(true);
        }
        private ProductReview MapCreateProductReviewDtoToProductReview(CreateProductReviewDTO newProductReview)
        {
            return new ProductReview
            {

                AdminApproved = false,
                ProductId = newProductReview.productId,
                UserId = newProductReview.userId,
                Rating = newProductReview.Rating,
                Comment = newProductReview.Comment

            };
        }
        private ShowProductReviewDTO MapProductReviewToShowProductReviewDto(ProductReview productReview)
        {
            return new ShowProductReviewDTO(
                productReview.ReviewId,
                productReview.ProductId,
                productReview.UserId,
                productReview.Rating,
                productReview.Comment,
                productReview.CreatedAt
                );
        }
    }
}
