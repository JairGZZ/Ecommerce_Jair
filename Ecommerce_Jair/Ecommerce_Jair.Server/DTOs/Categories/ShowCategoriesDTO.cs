using Ecommerce_Jair.Server.Models;

namespace Ecommerce_Jair.Server.DTOs.Categories
{
    public record ShowCategoriesDTO(
        string CategoryName,
        string Description,
        List<ShowCategoriesDTO> InverseParentCategory);
}
