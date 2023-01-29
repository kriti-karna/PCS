using System;
using System.Collections.Generic;

namespace PCS.Models;

public partial class Employee
{
    public int Id { get; set; }

    public string EmpName { get; set; } = null!;

    public DateOnly Dob { get; set; }

    public string Gender { get; set; } = null!;

    public decimal? Salary { get; set; }

    public string? EntryBy { get; set; }

    public DateOnly EntryDate { get; set; }

    public virtual ICollection<EmployeeQualification> EmployeeQualifications { get; } = new List<EmployeeQualification>();
}
