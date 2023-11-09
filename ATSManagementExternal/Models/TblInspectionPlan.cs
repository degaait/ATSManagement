using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblInspectionPlan
{
    public Guid InspectionPlanId { get; set; }

    public string? PlanTitle { get; set; }

    public string? PlanDescription { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public Guid? UserId { get; set; }

    public string? InspectionYear { get; set; }

    public Guid? StatusId { get; set; }

    public string? AssigningRemark { get; set; }

    public virtual TblStatus? Status { get; set; }

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblPlanInistitution> TblPlanInistitutions { get; set; } = new List<TblPlanInistitution>();

    public virtual ICollection<TblSpecificPlan> TblSpecificPlans { get; set; } = new List<TblSpecificPlan>();

    public virtual TblInternalUser? User { get; set; }
}
