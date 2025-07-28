using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class Discount
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public decimal? DiscountPercent { get; set; }

    public int? Quantity { get; set; }

    public int? UseNumber { get; set; }

    public byte? Status { get; set; }

    public virtual ICollection<Order> Oders { get; set; } = new List<Order>();
}
