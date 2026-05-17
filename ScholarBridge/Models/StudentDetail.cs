using System;
using System.Collections.Generic;

namespace ScholarBridge.Models;

public partial class StudentDetail
{
    public int UserId { get; set; }

    public string? StudentName { get; set; }

    public string? StudentSurname { get; set; }

    public decimal? Gpa { get; set; }

    public string? Department { get; set; }

    public string? UniversityName { get; set; }

    public decimal? FamilyIncome { get; set; }

    public string? TranscriptPath { get; set; }

    public string? StudentCertificatePath { get; set; }

    public int? Grade { get; set; }

    public string? DormitoryPath { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<StudentFile> StudentFiles { get; set; } = new List<StudentFile>();

    public virtual User User { get; set; } = null!;
}
