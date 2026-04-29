using System;
using System.Collections.Generic;

namespace ScholarBridge.Models;

public partial class Scholarship
{
    public int ScholarshipId { get; set; }

    public int? UserFkId { get; set; }

    public string? Title { get; set; }

    public DateOnly? Deadline { get; set; }

    public decimal? AmountPerStudent { get; set; }

    public int? Quota { get; set; }

    public string? RequirementSummary { get; set; }

    public bool? IsExpired { get; set; }

    public int? DurationInMonths { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<Donation> Donations { get; set; } = new List<Donation>();

    public virtual OrganizationDetail? UserFk { get; set; }
}
