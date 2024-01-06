using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblSpecificPlan
{
    public Guid SpecificPlanId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? InspectionPlanId { get; set; }

    public int? PlanCatId { get; set; }

    public bool? IsAssignedToUser { get; set; }

    public bool? IsAssignedToTeam { get; set; }

    public string? AssigningRemark { get; set; }

    public Guid? TeamId { get; set; }

    public Guid? AssigneeTypeId { get; set; }

    public Guid? InistId { get; set; }

    public Guid? ProId { get; set; }

    public virtual TblAssignementType? AssigneeType { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblInspectionPlan? InspectionPlan { get; set; }

    public virtual TblPlanCatagory? PlanCat { get; set; }

    public virtual TblInspectionStatus? Pro { get; set; }

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblPlanInistitution> TblPlanInistitutions { get; set; } = new List<TblPlanInistitution>();

    public virtual TblTeam? Team { get; set; }
}
