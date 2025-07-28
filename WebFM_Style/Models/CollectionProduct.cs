using System;
using System.Collections.Generic;

namespace WebFM_Style.Models;

public partial class CollectionProduct
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int? CollectionId { get; set; }

    public bool? Status { get; set; }

    public DateTime? Cdt { get; set; }

    public virtual Collection? Collection { get; set; }

    public virtual Product? Product { get; set; }
}
