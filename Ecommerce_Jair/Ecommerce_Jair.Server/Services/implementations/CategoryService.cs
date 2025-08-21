using Ecommerce_Jair.Server.DTOs.Categories;
using Ecommerce_Jair.Server.Models;
using Ecommerce_Jair.Server.Models.Results;
using Ecommerce_Jair.Server.Repositories.Interfaces;
using Ecommerce_Jair.Server.Services.Interfaces;

namespace Ecommerce_Jair.Server.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<TResult<Category>> CreateCategory(Category newCategory,int? parentCategoryId)
        {
            var existsCategoryName = await _categoryRepository.ExistByNameAsync(newCategory.CategoryName);
            if (existsCategoryName)
            {
                return TResult<Category>.Fail("Ya existe una categoría con este nombre");
            }
            if(parentCategoryId != null)
            {
                var existsParentCategory = await _categoryRepository.ExistByIdAsync(parentCategoryId.Value);
                if (!existsParentCategory)
                {
                    return TResult<Category>.Fail("No se pudo crear la subcategoría, la categoría principal no existe");
                }
            }
            newCategory.ParentCategoryId = parentCategoryId;
            await _categoryRepository.CreateAsync(newCategory);
            await _categoryRepository.SaveChangesAsync();

            return TResult<Category>.Ok(newCategory);
        }
        public async Task<Result> DeleteCategory(int categoryId)
        {
            var existsCategory = await _categoryRepository.ExistByIdAsync(categoryId);
            if (!existsCategory)
            {
                return Result.Fail("La categoría o subcategoría no existe");
            }
            var hasSubCategories = await _categoryRepository.HasSubCategoriesAsync(categoryId);
            if (hasSubCategories)
            {
                return Result.Fail("No se puede eliminar una categoria que tiene subCategorias");
            }
            var hasProducts = await _categoryRepository.HasProductsAsync(categoryId);
            if (hasProducts)
            {
                return Result.Fail("No se puede eliminar una categoría que tiene productos asociados");
            }

            await _categoryRepository.DeleteAsync(categoryId);
            await _categoryRepository.SaveChangesAsync();

            return Result.Ok();
        }
        public async Task<TResult<Category>> EditCategory(int categoryId, Category updatedCategory)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(categoryId);
            if (existingCategory == null)
            {
                return TResult<Category>.Fail("La categoría no existe");
            }
            var nameExists = await _categoryRepository.ExistsByNameExceptIdAsync(updatedCategory.CategoryName,categoryId);

            if (nameExists)
            { 
                return TResult<Category>.Fail("Ese Nombre ya existe"); 
            }
            var categoryEdited = await _categoryRepository.EditAsync(categoryId, updatedCategory);
            await _categoryRepository.SaveChangesAsync(); 
            return TResult<Category>.Ok(categoryEdited);
        }
        public async Task<TResult<List<ShowCategoriesDTO>>> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllCategories();
            return TResult<List<ShowCategoriesDTO>>.Ok(categories.Where(c => c.ParentCategoryId == null)
                .Select(c => MapToDto(c))
                .ToList());
        }
        private ShowCategoriesDTO MapToDto(Category category)
        {
            return new ShowCategoriesDTO(
                category.CategoryName,
                category.Description,
                category.InverseParentCategory
                .Select(sc => MapToDto(sc))
                .ToList());
        }
    }

}
