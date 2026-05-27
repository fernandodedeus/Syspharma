using System;
using System.Collections.Generic;

namespace SyspharmaApi.Models;

public partial class Payment
{
    public int Idpayment { get; set; }

    public int Idorder { get; set; }

    public int Paymentmethod { get; set; }

    public decimal Amount { get; set; }

    public DateTime Paymentdate { get; set; }

    public int Installments { get; set; }

    public string? Authorizationcode { get; set; }

    public string? Cardlastdigits { get; set; }

    public string? Notes { get; set; }

    public DateTime Createdat { get; set; }

    public virtual Order IdorderNavigation { get; set; } = null!;
}
