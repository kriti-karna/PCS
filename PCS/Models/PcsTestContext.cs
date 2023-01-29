using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PCS.Models;

public partial class PcsTestContext : DbContext
{
    public PcsTestContext()
    {
    }

    public PcsTestContext(DbContextOptions<PcsTestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeQualification> EmployeeQualifications { get; set; }

    public virtual DbSet<Qualification> Qualifications { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost; Database=pcs_test; Username=postgres; Password=postgres; Port=5433");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pkey");

            entity.ToTable("employee");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Dob).HasColumnName("dob");
            entity.Property(e => e.EmpName)
                .HasColumnType("character varying")
                .HasColumnName("emp_name");
            entity.Property(e => e.EntryBy)
                .HasColumnType("character varying")
                .HasColumnName("entry_by");
            entity.Property(e => e.EntryDate).HasColumnName("entry_date");
            entity.Property(e => e.Gender)
                .HasColumnType("character varying")
                .HasColumnName("gender");
            entity.Property(e => e.Salary).HasColumnName("salary");
        });

        modelBuilder.Entity<EmployeeQualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_qualification_pkey");

            entity.ToTable("employee_qualification");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EmpId).HasColumnName("emp_id");
            entity.Property(e => e.Marks).HasColumnName("marks");
            entity.Property(e => e.QId).HasColumnName("q_id");

            entity.HasOne(d => d.Emp).WithMany(p => p.EmployeeQualifications)
                .HasForeignKey(d => d.EmpId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pk_emp");

            entity.HasOne(d => d.QIdNavigation).WithMany(p => p.EmployeeQualifications)
                .HasForeignKey(d => d.QId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("pk_qualification");
        });

        modelBuilder.Entity<Qualification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("qualification_pkey");

            entity.ToTable("qualification");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasColumnType("character varying")
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
