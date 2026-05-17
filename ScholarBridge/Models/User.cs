using System;
using System.Collections.Generic;

namespace ScholarBridge.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? Email { get; set; }

    public string? PasswordHash { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public string? PhoneNumber { get; set; }

    public string? ProfileImagePath { get; set; }

    public virtual DonorDetail? DonorDetail { get; set; }

    public virtual OrganizationDetail? OrganizationDetail { get; set; }

    public virtual StudentDetail? StudentDetail { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
