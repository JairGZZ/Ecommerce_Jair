using Ecommerce_Jair.Server.Models;

namespace Ecommerce_Jair.Server.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAllCategories();
        Task<Category?> EditAsync(int categoryId, Category category);
        Task<bool> DeleteAsync(int idCategory);
        Task<Category> CreateAsync(Category newCategory);
        Task<Category?> GetByIdAsync(int categoryId);
        Task<bool> ExistByNameAsync(string categoryName);
        Task<bool> ExistByIdAsync(int id);
        Task<bool> HasSubCategoriesAsync(int categoryId);
        Task<bool> HasProductsAsync(int categoryId);
        Task<bool> ExistsByNameExceptIdAsync(string categoryName, int categoryId);
        Task SaveChangesAsync();





    }
}
