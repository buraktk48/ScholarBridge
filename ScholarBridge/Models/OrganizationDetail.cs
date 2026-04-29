using System;
using System.Collections.Generic;

namespace ScholarBridge.Models;

public partial class OrganizationDetail
{
    public int UserId { get; set; }

    public string? OrgName { get; set; }

    public string? TaxNumber { get; set; }

    public string? Description { get; set; }

    public bool? IsVerified { get; set; }

    public virtual ICollection<Scholarship> Scholarships { get; set; } = new List<Scholarship>();

    public virtual User User { get; set; } = null!;
}
