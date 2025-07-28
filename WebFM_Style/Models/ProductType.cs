using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class ProductType
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public byte? Status { get; set; }

    public DateTime? Cdt { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
