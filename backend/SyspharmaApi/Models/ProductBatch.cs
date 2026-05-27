using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class ProductBatch
{
    public int Idbatch { get; set; }

    public int Idproduct { get; set; }

    public int? Idsupplier { get; set; }

    public string Batchcode { get; set; } = null!;

    public DateOnly Expirationdate { get; set; }

    public DateOnly? Manufacturingdate { get; set; }

    public decimal Quantity { get; set; }

    public decimal Unitcost { get; set; }

    public DateTime Createdat { get; set; }

    public virtual Product IdproductNavigation { get; set; } = null!;

    public virtual Supplier? IdsupplierNavigation { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
