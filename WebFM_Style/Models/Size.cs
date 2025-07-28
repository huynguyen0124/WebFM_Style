using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class Size
{
    public int Id { get; set; }

    public string? Size1 { get; set; }

    public virtual ICollection<ProductSizeColor> ProductSizeColors { get; set; } = new List<ProductSizeColor>();
}
