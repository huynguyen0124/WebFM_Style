using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public decimal? Total { get; set; }

    public DateTime? CreateDay { get; set; }

    public int? AddressId { get; set; }

    public int? DiscountId { get; set; }

    public byte? Status { get; set; }

    public virtual Account? Account { get; set; }

    public virtual Discount? Discount { get; set; }

    public virtual ICollection<OrderItem> OderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
