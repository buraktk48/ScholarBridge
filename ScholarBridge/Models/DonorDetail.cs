using System;
using System.Collections.Generic;

namespace ScholarBridge.Models;

public partial class DonorDetail
{
    public int UserId { get; set; }

    public string? Occupation { get; set; }

    public string? Bio { get; set; }

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    public virtual User User { get; set; } = null!;
}
