using System;
using System.Collections.Generic;

namespace AspOsyp.Models;

public partial class OrderItem
{
    public int OrderItemsId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Catalog Product { get; set; } = null!;
}
