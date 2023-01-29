using System;
using System.Collections.Generic;

namespace PCS.Models;

public partial class Qualification
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<EmployeeQualification> EmployeeQualifications { get; } = new List<EmployeeQualification>();
}
