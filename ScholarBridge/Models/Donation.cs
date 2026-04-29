using System;
using System.Collections.Generic;

namespace ScholarBridge.Models;

public partial class Donation
{
    public int DonationId { get; set; }

    public int? UserFkId { get; set; }

    public int? ScholarshipFkId { get; set; }

    public decimal? Amount { get; set; }

    public DateTime? DonatedAt { get; set; }

    public string? TransactionId { get; set; }

    public string? IsAnonymous { get; set; }

    public virtual Scholarship? ScholarshipFk { get; set; }

    public virtual DonorDetail? UserFk { get; set; }
}
