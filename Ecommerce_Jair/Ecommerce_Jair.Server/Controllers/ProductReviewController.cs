using Ecommerce_Jair.Server.DTOs.ProductReview;
using Ecommerce_Jair.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Jair.Server.Controllers
{
    public class ProductReviewController : ControllerBase
    {
        private readonly IProductReviewService _productReviewService;
        public ProductReviewController(IProductReviewService productReviewService)
        {
            _productReviewService = productReviewService;
        }
        [HttpPost("createProductReview")]
        public async Task<IActionResult> CreateProductReview([FromBody] CreateProductReviewDTO newProductReview)
        {
            var result = await _productReviewService.CreateProductReviewAsync(newProductReview);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Data);
        }
        [HttpDelete("deleteProductReview/{productReviewId}")]
        public async Task<IActionResult> DeleteProductReview([FromRoute] int productReviewId)
        {
            var result = await _productReviewService.DeleteProductReviewAsync(productReviewId);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Data);
        }
    }
}
