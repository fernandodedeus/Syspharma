using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class User : DbModel
{
    public int Iduser { get; set; }

    public int? Idstore { get; set; }

    public int Role { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Cpf { get; set; }

    public string Pass { get; set; } = null!;

    public string? Phone { get; set; }

    public bool Active { get; set; }

    public DateTime Createdat { get; set; }

    public string? Profilephotopath { get; set; }

    [ValidateNever]
    public virtual Store? IdstoreNavigation { get; set; }

    [ValidateNever]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ValidateNever]
    public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
}

public enum UserRole
{
    Admin = 1,
    Salesperson,
    Pharmacist,
    Cashier
}
