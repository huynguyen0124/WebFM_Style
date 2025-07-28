using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class Image
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public string? Url { get; set; }

    public bool? Status { get; set; }

    public virtual Product? Product { get; set; }
}
