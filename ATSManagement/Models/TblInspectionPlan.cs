using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInspectionPlan
{
    public Guid InspectionPlanId { get; set; }

    public string? PlanTitle { get; set; }

    public string? PlanDescription { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? InspectionYear { get; set; }

    public Guid? StatusId { get; set; }

    public string? AssigningRemark { get; set; }

    public virtual TblStatus? Status { get; set; }

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblPlanInistitution> TblPlanInistitutions { get; set; } = new List<TblPlanInistitution>();

    public virtual TblInternalUser? User { get; set; }
}
