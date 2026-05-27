using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class Customer
{
    public int Idcustomer { get; set; }

    public string? Name { get; set; }

    public string? Othername { get; set; }

    public string Document { get; set; } = null!;

    public string? Phone { get; set; }

    public DateTime Createdat { get; set; }

    public bool? Active { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
