using Ecommerce_Jair.Server.DTOs.Product;
using Ecommerce_Jair.Server.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce_Jair.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet("getallproducts")]
        public async Task<IActionResult> GetProducts([FromQuery] ProductListCriteria criteria, CancellationToken ct)
        {
            var result = await _productService.GetAllProducts(criteria, ct);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Data);
        }
        [HttpGet("getProduct")]
        public async Task<IActionResult> GetProduct(int idProduct)
        {
            var result = await _productService.GetProductDetail(idProduct);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Data);
        }
        [HttpPost("createProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO newProduct)
        {
           var result = await _productService.CreateProductAsync(newProduct);
            if (!result.Success)
            {
                return BadRequest(result.Error);
            }
            return Ok(result.Data);
        }
        [HttpDelete("deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct([FromRoute]int id)
        {
            var result = await _productService.DeleteProductAsync(id);

            if (!result.Success)
            {
                return BadRequest(result.Error);  
            }

            return Ok("Producto eliminado con éxito."); 
        }
        [HttpPatch("updateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] JsonPatchDocument<UpdateProductDto> dto)
        {
            var result = await _productService.UpdateProductAsync(id, dto);

            if (!result.Success)
            {
                return BadRequest(result.Error);
            }

            return Ok(result.Data);  
        }



    }
}
