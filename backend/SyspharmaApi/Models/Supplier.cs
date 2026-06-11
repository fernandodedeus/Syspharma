using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class Supplier
{
    public int Idsupplier { get; set; }

    public string Name { get; set; } = null!;

    public string? Tradename { get; set; }

    public string? Document { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool? Active { get; set; }

    public DateTime Createdat { get; set; }

    [ValidateNever]
    public virtual ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
}
