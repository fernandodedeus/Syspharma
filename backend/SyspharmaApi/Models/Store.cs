using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class Store : DbModel
{
    public int Idstore { get; set; }

    public string? Fantasyname { get; set; }

    public string Socialname { get; set; } = null!;

    public string? Cnpj { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool? Active { get; set; }

    [ValidateNever]
    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    [ValidateNever]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ValidateNever]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
