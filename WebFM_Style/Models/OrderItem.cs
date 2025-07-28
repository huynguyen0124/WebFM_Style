using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class OrderItem
{
    public int OrderId { get; set; }

    public int ProductSizeColorId { get; set; }

    public int? Quantity { get; set; }

    public virtual Order Oder { get; set; } = null!;

    public virtual ProductSizeColor ProductSizeColor { get; set; } = null!;
}
