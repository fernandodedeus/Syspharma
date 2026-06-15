using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class OrderItem
{
    public int Idorderitem { get; set; }

    public int Idorder { get; set; }

    public int Idproduct { get; set; }

    public int? Idbatch { get; set; }

    public decimal Quantity { get; set; }

    public decimal Unitprice { get; set; }

    public decimal Unitcost { get; set; }

    public decimal Discountvalue { get; set; }

    public decimal Totalvalue { get; set; }

    public string? Notes { get; set; }

    [ValidateNever]
    public virtual ProductBatch? IdbatchNavigation { get; set; }

    [ValidateNever]
    public virtual Order IdorderNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Product IdproductNavigation { get; set; } = null!;
}
