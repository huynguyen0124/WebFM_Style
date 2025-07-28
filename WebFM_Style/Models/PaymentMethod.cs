using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class PaymentMethod
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
