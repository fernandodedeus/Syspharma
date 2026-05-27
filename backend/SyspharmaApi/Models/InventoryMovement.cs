using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class InventoryMovement
{
    public int IdinventoryMovement { get; set; }

    public int Idinventory { get; set; }

    public decimal Quantity { get; set; }

    public bool Isentry { get; set; }

    public decimal Balance { get; set; }

    public DateTime Datemov { get; set; }

    public virtual Inventory IdinventoryNavigation { get; set; } = null!;
}
