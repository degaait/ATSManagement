using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblAssignementType
{
    public Guid AssigneeTypeId { get; set; }

    public string? AssigneeType { get; set; }

    public virtual ICollection<TblInspectionPlan> TblInspectionPlans { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblRequestDepartmentRelation> TblRequestDepartmentRelations { get; set; } = new List<TblRequestDepartmentRelation>();
}
