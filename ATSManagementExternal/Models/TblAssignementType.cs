using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblAssignementType
{
    public Guid AssigneeTypeId { get; set; }

    public string? AssigneeType { get; set; }

    public string? AssigneeTypeAmharic { get; set; }

    public virtual ICollection<TblInspectionPlan> TblInspectionPlans { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblRequestDepartmentRelation> TblRequestDepartmentRelations { get; set; } = new List<TblRequestDepartmentRelation>();
}
