using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class Inventory : DbModel
{
    public int Idinventory { get; set; }

    public int Idproduct { get; set; }

    public int Idstore { get; set; }

    public decimal Quantity { get; set; }

    public decimal Minimum { get; set; }

    [ValidateNever]
    public virtual Product IdproductNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Store IdstoreNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual ICollection<InventoryMovement> InventoryMovements { get; set; } = new List<InventoryMovement>();
}
