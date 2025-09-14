using Azure;
using Ecommerce_Jair.Server.DTOs.Product;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Models.Results;
using Ecommerce_Jair.Server.Repositories.implementations;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Ecommerce_Jair.Server.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Ecommerce_Jair.Server.Services.implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<TResult<ProductCreatedDto>> CreateProductAsync(CreateProductDTO newProduct)
        {

            var validationResult = ValidateCreateProduct(newProduct);
            if (!validationResult.Success) return TResult<ProductCreatedDto>.Fail(validationResult.Error);

            var category = await _categoryRepository.GetByIdAsync(newProduct.CategoryId);
            if (category == null) return TResult<ProductCreatedDto>.Fail("Category not found");

            var product = new Product
            {
                ProductName = newProduct.ProductName,
                Description = newProduct.Description,
                Price = newProduct.Price,
                Stock = newProduct.Stock,
                Sku = newProduct.Sku,
                CategoryId = newProduct.CategoryId,
                ImageUrl = newProduct.ImageUrl
            };

            await _productRepository.CreateProductAsync(product);
            await _productRepository.SaveChangesAsync();

            var productDto = new ProductCreatedDto(
                product.ProductId,
                product.ProductName,
                product.Description,
                product.Price,
                product.Stock,
                product.Sku,
                product.CategoryId,
                category.CategoryName,
                product.ImageUrl,
                product.CreatedAt,
                product.UpdatedAt
            );

            return TResult<ProductCreatedDto>.Ok(productDto);

        }

        public async Task<TResult<bool>> DeleteProductAsync(int idProduct)
        {

            var productExists = await _productRepository.ExistProductByIdAsync(idProduct);
            if (!productExists)
            {
                return TResult<bool>.Fail("El Producto no existe ");
            }
            await _productRepository.DeleteProductAsync(idProduct);
            await _productRepository.SaveChangesAsync();
            return TResult<bool>.Ok(true);
        }

        public async Task<TResult<PagedResult<ProductListItem>>> GetAllProducts(ProductListCriteria criteria, CancellationToken ct = default)
        {
            var isvalid = ValidateCriteria(criteria);
            if (!isvalid.Success)
            {
                return TResult<PagedResult<ProductListItem>>.Fail(isvalid.Error);
            }
        

            if (criteria.CategoryId.HasValue)
            {
                var categoryExists = await _categoryRepository.ExistByIdAsync(criteria.CategoryId.Value);
                if (!categoryExists)
                {
                    return TResult<PagedResult<ProductListItem>>.Fail("Categoria no encontrada.");
                }
            }

            var result = await _productRepository.SearchAsync(criteria, ct);

            return TResult<PagedResult<ProductListItem>>.Ok(result);
        }

        public async Task<TResult<ProductDetailsDTO>> GetProductDetail(int idProduct)
        {
            var product = await _productRepository.GetProductAndCategory(idProduct);
            if (product == null)
            {
                return TResult<ProductDetailsDTO>.Fail("Producto no encontrado.");
            }

            var productDto = new ProductDetailsDTO(
                product.ProductId,
                product.ProductName,
                product.Description,
                product.Price,
                product.Stock,
                product.Sku,
                product.Category?.CategoryName ?? "Sin categoría", 
                product.ImageUrl,
                product.CreatedAt,
                product.UpdatedAt
            );

            return TResult<ProductDetailsDTO>.Ok(productDto); 

        }
        public async Task<TResult<ProductCreatedDto>> UpdateProductAsync(int productId, UpdateProductDto productDto)
        {
            // Obtener el producto por ID
            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return TResult<ProductCreatedDto>.Fail("Producto no encontrado.");
            }

            // Validaciones de negocio
            if (productDto.Price < 0)
            
                return TResult<ProductCreatedDto>.Fail("El precio no puede ser negativo.");
          
            if (productDto.Price == 0)
            
                return TResult<ProductCreatedDto>.Fail("El precio tiene que ser mayor que 0.");

            if (productDto.Stock < 0)

                return TResult<ProductCreatedDto>.Fail("El stock no puede ser negativo.");

            // Actualizar la entidad `product` con los datos del DTO
            if (productDto.ProductName != null)
                product.ProductName = productDto.ProductName;
            if (product.Description != null)
                product.Description = productDto.Description;
            if (productDto.Price.HasValue)
                product.Price = productDto.Price.Value;
            if (productDto.Stock.HasValue)
                product.Stock = productDto.Stock.Value;
            if (productDto.Sku != null)
                product.Sku = productDto.Sku;
            if (productDto.CategoryId.HasValue)
                product.CategoryId = productDto.CategoryId.Value;
            if (product.ImageUrl != null)
                product.ImageUrl = productDto.ImageUrl;
            product.UpdatedAt = DateTime.UtcNow;

            // Guardar los cambios en la base de datos
            await _productRepository.SaveChangesAsync();

            // Crear el DTO de respuesta
            var productCreatedDto = new ProductCreatedDto(
                product.ProductId,
                product.ProductName,
                product.Description,
                product.Price,
                product.Stock,
                product.Sku,
                product.CategoryId,
                product.Category?.CategoryName ?? "Sin categoría", // Si no tiene categoría, colocar "Sin categoría"
                product.ImageUrl,
                product.CreatedAt,
                product.UpdatedAt
            );

            return TResult<ProductCreatedDto>.Ok(productCreatedDto);
        }



        private Result ValidateCreateProduct(CreateProductDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.ProductName))
                return Result.Fail("Product name is required.");

            if (dto.Price <= 0)
                return Result.Fail("Price must be greater than zero.");

            if (dto.Stock < 0)
                return Result.Fail("Stock cannot be negative.");

            if (string.IsNullOrWhiteSpace(dto.Sku))
                return Result.Fail("SKU is required.");

            if (dto.CategoryId <= 0)
                return Result.Fail("Valid CategoryId is required.");

            return Result.Ok();
        }


        private Result ValidateCriteria(ProductListCriteria criteria)
        {
            if (criteria.Page < 1) return Result.Fail("Page must be >= 1");
            if (criteria.PageSize < 1 || criteria.PageSize > 100) return Result.Fail("PageSize must be between 1 and 100");
            if (criteria.PriceMin.HasValue && criteria.PriceMax.HasValue && criteria.PriceMin > criteria.PriceMax)
                return Result.Fail("PriceMin cannot be greater than PriceMax");

            if (criteria.Q?.Length > 100)
            {
                return Result.Fail("Search query (q) must be less than 100 characters");
            }
            var validSortFields = new[] { "CreatedAt", "ProductName", "Price" };
            if (!validSortFields.Contains(criteria.Sort))
            {
                return Result.Fail("Sort must be one of 'CreatedAt', 'ProductName or 'Price'");
            }

            var validDirs = new[] { "asc", "desc" };
            if (!validDirs.Contains(criteria.Dir))
            {
                return Result.Fail("Dir must be 'asc' or 'desc'");
            }
            return Result.Ok();
        }

    }
}
