using System;
using System.Collections.Generic;

namespace Ecommerce_Jair.Server.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int UserId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string Status { get; set; } = null!;

    public string ShippingAddress { get; set; } = null!;

    public string PaymentMethod { get; set; } = null!;

    public string? AdminNotes { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual OrderStatus StatusNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
