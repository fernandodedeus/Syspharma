using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class Inventory
{
    public int Idinventory { get; set; }

    public int Idproduct { get; set; }

    public int Idstore { get; set; }

    public decimal Quantity { get; set; }

    public decimal Minimum { get; set; }

    public virtual Product IdproductNavigation { get; set; } = null!;

    public virtual Store IdstoreNavigation { get; set; } = null!;

    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();
}
