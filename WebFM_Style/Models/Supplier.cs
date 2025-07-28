using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class Supplier
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool? Status { get; set; }

    public DateTime? Cdt { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();
}
