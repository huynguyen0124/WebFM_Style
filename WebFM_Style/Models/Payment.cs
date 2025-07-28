using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? OrdersId { get; set; }

    public int? PaymentMethodsId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? PaymentDate { get; set; }

    public byte? Status { get; set; }

    public virtual Order? Oders { get; set; }

    public virtual PaymentMethod? PaymentMethods { get; set; }
}
