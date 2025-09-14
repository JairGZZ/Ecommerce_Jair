using Ecommerce_Jair.Server.DTOs.Product;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Models.Results;
using Microsoft.AspNetCore.JsonPatch;

namespace Ecommerce_Jair.Server.Services.Interfaces
{
    public interface IProductService
    {
        Task<TResult<ProductCreatedDto>> CreateProductAsync(CreateProductDTO newProduct);
        Task<TResult<PagedResult<ProductListItem>>> GetAllProducts(ProductListCriteria criteria, CancellationToken ct);
        Task<TResult<bool>> DeleteProductAsync(int idProduct);
        Task<TResult<ProductDetailsDTO>> GetProductDetail(int idProduct);
        Task<TResult<ProductCreatedDto>> UpdateProductAsync(int productId, UpdateProductDto dto);
    }
}
