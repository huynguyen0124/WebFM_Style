using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFM_Style.Models;

public partial class ReceiptProduct
{
    public int Id { get; set; }

    public int? ProductSizeColorId { get; set; }

    public int? Quantity { get; set; }
    public decimal? Price { get; set; }

    public string? Image { get; set; }

    public DateTime? CreateDay { get; set; }

    public byte? Status { get; set; }

    public virtual ProductSizeColor? ProductSizeColor { get; set; }
}
