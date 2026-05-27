using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class Product
{
    public int Idproduct { get; set; }

    public string? Description { get; set; }

    public string Internalcode { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal Cost { get; set; }

    public DateTime Createdat { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
}
