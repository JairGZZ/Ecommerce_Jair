using System;
using System.Collections.Generic;

namespace Ecommerce_Jair.Server.BD.Models;

public partial class DiscountType
{
    public string TypeName { get; set; } = null!;

    public virtual ICollection<Discount> Discounts { get; set; } = new List<Discount>();
}
