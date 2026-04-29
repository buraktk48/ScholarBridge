using System;
using System.Collections.Generic;

namespace ScholarBridge.Models;

public partial class Application
{
    public int ApplicationId { get; set; }

    public int? UserFkId { get; set; }

    public int? ScholarshipFkId { get; set; }

    public string? Status { get; set; }

    public DateTime? AppliedAt { get; set; }

    public string? RejectionReason { get; set; }

    public virtual Scholarship? ScholarshipFk { get; set; }

    public virtual StudentDetail? UserFk { get; set; }
}
