using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class ProductSizeColor
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? SizeId { get; set; }

    public int? ColorId { get; set; }

    public int? ProductInventoryId { get; set; }
    public string? Code { get; set; }

    public virtual Color? Color { get; set; }

    public virtual ICollection<OrderItem> OderItems { get; set; } = new List<OrderItem>();

    public virtual Product? Product { get; set; }

    public virtual ProductsInventory? ProductInventory { get; set; }

    public virtual ICollection<ReceiptProduct> ReceiptProducts { get; set; } = new List<ReceiptProduct>();

    public virtual Size? Size { get; set; }
}
