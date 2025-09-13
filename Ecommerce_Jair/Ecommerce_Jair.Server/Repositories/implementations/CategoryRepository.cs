using Ecommerce_Jair.Server.BD.context;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Jair.Server.Repositories.implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly EcommerceDbContext _context;
        public CategoryRepository(EcommerceDbContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateAsync(Category newCategory)
        {
            await _context.Categories.AddAsync(newCategory);
            return newCategory;
        }
        public async Task<Category?> EditAsync(int categoryId, Category newCategory)
        {
            var category = await GetByIdAsync(categoryId);
            if (category == null)
            {
                return null;
            }
            category.CategoryName = newCategory.CategoryName;
            category.Description = newCategory.Description;
            return category;
        }

        public async Task<bool> DeleteAsync(int idCategory)
        {

            var category = await GetByIdAsync(idCategory);
            if (category == null)
            {
                return false;
            }
            category.IsActive = false;
            return true;
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.Include(c => c.InverseParentCategory).ToListAsync();
        }
        public async Task<Category?> GetByIdAsync(int categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(u => u.CategoryId == categoryId);
        }
        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        public async Task<bool> ExistByNameAsync(string categoryName)
            => await _context.Categories.AnyAsync(c => c.CategoryName == categoryName);
        public async Task<bool> ExistByIdAsync(int id)
             => await _context.Categories.AnyAsync(c => c.CategoryId == id);

        public async Task<bool> HasSubCategoriesAsync(int categoryId)
            => await _context.Categories.AnyAsync(c => c.ParentCategoryId == categoryId);

        public async Task<bool> HasProductsAsync(int categoryId)
            => await _context.Products.AnyAsync(p => p.CategoryId == categoryId);
        public async Task<bool> ExistsByNameExceptIdAsync(string categoryName, int categoryId)
            => await _context.Categories.AnyAsync(c => c.CategoryName == categoryName && c.CategoryId != categoryId);




    }
}
