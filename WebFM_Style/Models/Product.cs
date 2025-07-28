using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebFM_Style.Models;

public partial class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }
    public double? Price { get; set; }

    public byte? Status { get; set; }

    public int? ProductTypeId { get; set; }

    public virtual ICollection<CollectionProduct> CollectionProducts { get; set; } = new List<CollectionProduct>();

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual ICollection<ProductSizeColor> ProductSizeColors { get; set; } = new List<ProductSizeColor>();

    public virtual ProductType? ProductType { get; set; }
}
