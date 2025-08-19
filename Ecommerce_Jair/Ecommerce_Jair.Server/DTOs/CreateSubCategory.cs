namespace Ecommerce_Jair.Server.DTOs
{
    public class CreateSubCategory
    {
        public string CategoryName { get; init; }
        public string Description { get; init; }
        public int ParentCategoryId { get; init; }
    }
}
