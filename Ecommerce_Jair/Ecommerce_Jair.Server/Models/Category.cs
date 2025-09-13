using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ecommerce_Jair.Server.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string? Description { get; set; }

    public int? ParentCategoryId { get; set; }
    public bool IsActive { get; set; } = true;

    public virtual ICollection<Category> InverseParentCategory { get; set; } = new List<Category>();
    [JsonIgnore]
    public virtual Category? ParentCategory { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
