using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblTeam
{
    public Guid TeamId { get; set; }

    public string? TeamName { get; set; }

    public Guid? DepId { get; set; }

    public Guid? TeamLeaderId { get; set; }

    public string? TeamDescription { get; set; }

    public string? TeamNameAmharic { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual ICollection<TblInspectionPlan> TblInspectionPlans { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblInternalUser> TblInternalUsers { get; set; } = new List<TblInternalUser>();

    public virtual ICollection<TblRequestDepartmentRelation> TblRequestDepartmentRelations { get; set; } = new List<TblRequestDepartmentRelation>();

    public virtual ICollection<TblSpecificPlan> TblSpecificPlans { get; set; } = new List<TblSpecificPlan>();

    public virtual TblInternalUser? TeamLeader { get; set; }
}
