using System;
using System.Collections.Generic;

namespace Ecommerce_Jair.Server.Models;

public partial class OrderStatus
{
    public string StatusName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
