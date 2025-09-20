using Ecommerce_Jair.Server.BD.context;
using Ecommerce_Jair.Server.DTOs.Product;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Ecommerce_Jair.Server.Repositories.implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceDbContext _context;
        public ProductRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<Product> CreateProductAsync(Product newProduct)
        {
            await _context.Products.AddAsync(newProduct);
            return newProduct;
        }

        public async Task<bool> DeleteProductAsync(int idProduct)
        {
            var product = await GetProductByIdAsync(idProduct);
            if (product == null)
            {
                return false;
            }
            product.IsActive = false;
            return true;
            
        }
        public Task<List<Product>> GetAllProductsAsync()
        {
            return _context.Products.ToListAsync();
        }

        public async Task<Product?> GetProductByIdAsync(int productId)
        {
            var productExists = await _context.Products.FirstOrDefaultAsync(c => c.ProductId == productId);
            if (productExists == null)
            {
                return null;
            }
            return productExists;
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductListItem>> SearchAsync(ProductListCriteria criteria, CancellationToken ct)
        {
            var query = _context.Products
                .AsNoTracking()  
                .Include(p => p.Category)
                .AsQueryable()
                ;  
            if (!string.IsNullOrWhiteSpace(criteria.Q))
            {
                var searchTerm = criteria.Q.Trim().ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(searchTerm) || p.Sku.ToLower().Contains(searchTerm));
            }

            if (criteria.CategoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == criteria.CategoryId.Value);
            }

            if (criteria.InStock == true)
            {
                query = query.Where(p => p.Stock > 0);
            }

            if (criteria.InStock == false)
            {
                query = query.Where(p => p.Stock == 0);
            }

            if (criteria.PriceMin.HasValue)
            {
                query = query.Where(p => p.Price >= criteria.PriceMin.Value);
            }

            if (criteria.PriceMax.HasValue)
            {
                query = query.Where(p => p.Price <= criteria.PriceMax.Value);
            }

           

            query = criteria.Sort switch
            {
                "ProductName" => criteria.Dir == "asc" ? query.OrderBy(p => p.ProductName) : query.OrderByDescending(p => p.ProductName),
                "Price" => criteria.Dir == "asc" ? query.OrderBy(p => p.Price) : query.OrderByDescending(p => p.Price),
                "CreatedAt" => criteria.Dir == "asc" ? query.OrderBy(p => p.UpdatedAt) : query.OrderByDescending(p => p.UpdatedAt),
                _ => query.OrderBy(p => p.UpdatedAt) 
            };

            var totalCount = await query.CountAsync(ct);

            var skip = (criteria.Page - 1) * criteria.PageSize;
            var products = await query
                .Skip(skip)
                .Take(criteria.PageSize)
                .ToListAsync(ct);

            var resultItems = products.Select(p => new ProductListItem
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Stock = p.Stock,
                Sku = p.Sku,
                ImageUrl = p.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.CategoryName,  
                UpdatedAt = p.UpdatedAt
            }).ToList();

            var result = new PagedResult<ProductListItem>
            {
                Items = resultItems,
                TotalCount = totalCount,
                Page = criteria.Page,
                PageSize = criteria.PageSize
            };

            return result;
        }

        public async Task<bool> ExistProductByIdAsync(int id) => 
            await _context.Products.AnyAsync(p => p.ProductId == id);

        public async Task<Product?> GetProductAndCategory(int idProduct)
        {
            var productExists = await ExistProductByIdAsync(idProduct);
            if (!productExists)
            {
                return null;
            }
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.ProductId == idProduct);
        }
    }
}
