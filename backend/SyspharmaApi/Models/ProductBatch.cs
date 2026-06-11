using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

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

    [ValidateNever] // método para dizer ao sistema de validação para ignorar a propriedade, pois o EntityFramework já preenche automaticamente via idproduct
    public virtual Product IdproductNavigation { get; set; } = null!;

    [ValidateNever] // método para dizer ao sistema de validação para ignorar a propriedade, pois o EntityFramework já preenche automaticamente via idproduct
    public virtual Supplier? IdsupplierNavigation { get; set; }

    [ValidateNever] // método para dizer ao sistema de validação para ignorar a propriedade, pois o EntityFramework já preenche automaticamente via idproduct
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
