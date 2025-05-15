using System;
using System.Collections.Generic;

namespace AspOsyp.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual Catalog Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
