using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class Order
{
    public int Idorder { get; set; }

    public int Idstore { get; set; }

    public int? Idcustomer { get; set; }

    public int Iduser { get; set; }

    public string Ordernumber { get; set; } = null!;

    public DateTime Orderdate { get; set; }

    public decimal Subtotal { get; set; }

    public decimal Discountvalue { get; set; }

    public decimal Totalvalue { get; set; }

    public int? Status { get; set; }

    public string? Notes { get; set; }

    public DateTime Createdat { get; set; }

    public virtual Customer? IdcustomerNavigation { get; set; }

    public virtual Store IdstoreNavigation { get; set; } = null!;

    public virtual User IduserNavigation { get; set; } = null!;

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
