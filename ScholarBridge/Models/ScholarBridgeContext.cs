using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ScholarBridge.Models;

public partial class ScholarBridgeContext : DbContext
{
    public ScholarBridgeContext()
    {
    }

    public ScholarBridgeContext(DbContextOptions<ScholarBridgeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Donation> Donations { get; set; }

    public virtual DbSet<DonorDetail> DonorDetails { get; set; }

    public virtual DbSet<OrganizationDetail> OrganizationDetails { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Scholarship> Scholarships { get; set; }

    public virtual DbSet<StudentDetail> StudentDetails { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-QNLKSCG\\SQLEXPRESS;Database=ScholarBridge;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Application>(entity =>
        {
            entity.Property(e => e.AppliedAt).HasColumnType("datetime");
            entity.Property(e => e.RejectionReason).HasMaxLength(200);
            entity.Property(e => e.Status).HasMaxLength(20);

            entity.HasOne(d => d.ScholarshipFk).WithMany(p => p.Applications)
                .HasForeignKey(d => d.ScholarshipFkId)
                .HasConstraintName("FK_Applications_Scholarships");

            entity.HasOne(d => d.UserFk).WithMany(p => p.Applications)
                .HasForeignKey(d => d.UserFkId)
                .HasConstraintName("FK_Applications_StudentDetails");
        });

        modelBuilder.Entity<Donation>(entity =>
        {
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DonatedAt).HasColumnType("datetime");
            entity.Property(e => e.IsAnonymous)
                .HasMaxLength(10)
                .IsFixedLength();
            entity.Property(e => e.TransactionId).HasMaxLength(300);

            entity.HasOne(d => d.ScholarshipFk).WithMany(p => p.Donations)
                .HasForeignKey(d => d.ScholarshipFkId)
                .HasConstraintName("FK_Donations_Scholarships");

            entity.HasOne(d => d.UserFk).WithMany(p => p.Donations)
                .HasForeignKey(d => d.UserFkId)
                .HasConstraintName("FK_Donations_DonorDetails");
        });

        modelBuilder.Entity<DonorDetail>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Bio).HasMaxLength(50);
            entity.Property(e => e.Occupation).HasMaxLength(50);

            entity.HasOne(d => d.User).WithOne(p => p.DonorDetail)
                .HasForeignKey<DonorDetail>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DonorDetails_Users");
        });

        modelBuilder.Entity<OrganizationDetail>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.OrgName).HasMaxLength(90);
            entity.Property(e => e.TaxNumber).HasMaxLength(20);

            entity.HasOne(d => d.User).WithOne(p => p.OrganizationDetail)
                .HasForeignKey<OrganizationDetail>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OrganizationDetails_Users");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Scholarship>(entity =>
        {
            entity.Property(e => e.AmountPerStudent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.UserFk).WithMany(p => p.Scholarships)
                .HasForeignKey(d => d.UserFkId)
                .HasConstraintName("FK_Scholarships_OrganizationDetails");
        });

        modelBuilder.Entity<StudentDetail>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.Department).HasMaxLength(60);
            entity.Property(e => e.DormitoryPath).HasMaxLength(80);
            entity.Property(e => e.FamilyIncome).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Gpa)
                .HasColumnType("decimal(3, 2)")
                .HasColumnName("GPA");
            entity.Property(e => e.StudentCertificatePath).HasMaxLength(80);
            entity.Property(e => e.StudentName).HasMaxLength(100);
            entity.Property(e => e.StudentSurname).HasMaxLength(100);
            entity.Property(e => e.TranscriptPath).HasMaxLength(80);
            entity.Property(e => e.UniversityName).HasMaxLength(100);

            entity.HasOne(d => d.User).WithOne(p => p.StudentDetail)
                .HasForeignKey<StudentDetail>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentDetails_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.LastLoginAt).HasColumnType("datetime");
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasOne(d => d.RoleFk).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleFkId)
                .HasConstraintName("FK_UserRoles_Roles");

            entity.HasOne(d => d.UserFk).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserFkId)
                .HasConstraintName("FK_UserRoles_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
