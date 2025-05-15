using System;
using System.Collections.Generic;

namespace AspOsyp.Models;

public partial class Category
{
    public int CategoriesId { get; set; }

    public string CategoriesName { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Catalog> Catalogs { get; set; } = new List<Catalog>();
}
