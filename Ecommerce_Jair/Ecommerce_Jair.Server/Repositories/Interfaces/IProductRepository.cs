using Ecommerce_Jair.Server.DTOs.Product;
using Ecommerce_Jair.Server.Models;

namespace Ecommerce_Jair.Server.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> CreateProductAsync(Product newProduct);
        Task<Product?> EditProductAsync(int idProduct, Product newProduct);
        Task<bool> DeleteProductAsync(int idProduct);
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductByIdAsync(int idProduct);
        Task<Product?> GetProductAndCategory(int idProduct);
        Task<PagedResult<ProductListItem>> SearchAsync(ProductListCriteria criteria,CancellationToken ct);
        Task<bool> ExistProductByIdAsync(int id);
        Task SaveChangesAsync();
    }
}
