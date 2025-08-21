using Ecommerce_Jair.Server.DTOs.Categories;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Models.Results;

namespace Ecommerce_Jair.Server.Services.Interfaces
{
    public interface ICategoryService
    {

        Task<TResult<List<ShowCategoriesDTO>>> GetAllCategories();
        Task<TResult<Category>> EditCategory(int categoryId, Category category);
        Task<Result> DeleteCategory(int idCategory);
        Task<TResult<Category>> CreateCategory(Category newCategory,int? parentCategoryId);




    }
}
