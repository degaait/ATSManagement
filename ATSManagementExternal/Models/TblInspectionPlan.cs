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

    public Guid? YearId { get; set; }

    public Guid? StatusId { get; set; }

    public string? AssigningRemark { get; set; }

    public Guid? TeamId { get; set; }

    public Guid? AssigneeTypeId { get; set; }

    public bool? IsAssignedToUser { get; set; }

    public virtual TblAssignementType? AssigneeType { get; set; }

    public virtual TblStatus? Status { get; set; }

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblPlanInistitution> TblPlanInistitutions { get; set; } = new List<TblPlanInistitution>();

    public virtual ICollection<TblSpecificPlan> TblSpecificPlans { get; set; } = new List<TblSpecificPlan>();

    public virtual TblTeam? Team { get; set; }

    public virtual TblInternalUser? User { get; set; }

    public virtual TblYear? Year { get; set; }
}
