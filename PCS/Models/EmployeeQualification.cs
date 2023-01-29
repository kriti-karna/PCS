using System;
using System.Collections.Generic;

namespace PCS.Models;

public partial class EmployeeQualification
{
    public int Id { get; set; }

    public int EmpId { get; set; }

    public int QId { get; set; }

    public decimal Marks { get; set; }

    public virtual Employee Emp { get; set; } = null!;

    public virtual Qualification QIdNavigation { get; set; } = null!;
}
