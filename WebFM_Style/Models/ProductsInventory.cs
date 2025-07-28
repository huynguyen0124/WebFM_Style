using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class ProductsInventory
{
    public int Id { get; set; }

    public int? Quantity { get; set; }

    public int? QuantitySold { get; set; }

    public virtual ICollection<ProductSizeColor> ProductSizeColors { get; set; } = new List<ProductSizeColor>();
}
