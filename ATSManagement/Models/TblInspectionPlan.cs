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

    public Guid? YearId { get; set; }

    public Guid? ProId { get; set; }

    public string? AssigningRemark { get; set; }

    public Guid? TeamId { get; set; }

    public Guid? AssigneeTypeId { get; set; }

    public bool? IsAssignedToUser { get; set; }

    public Guid? IsUprovedByDeputy { get; set; }

    public Guid? IsUpprovedbyTeam { get; set; }

    public Guid? IsUpprovedbyDepartment { get; set; }

    public string? FinalReport { get; set; }

    public bool? FinalStatus { get; set; }

    public string? SendingRemark { get; set; }

    public string? ReturningRemark { get; set; }

    public bool? IsSenttoInst { get; set; }

    public bool? IsReturned { get; set; }

    public string? SentReport { get; set; }

    public bool? IsAssignedTeam { get; set; }

    public DateTime? ReturnDate { get; set; }

    public DateTime? SentDate { get; set; }

    public Guid? IsUserUproved { get; set; }

    public string? OfficialLetter { get; set; }

    public virtual TblAssignementType? AssigneeType { get; set; }

    public virtual TblDecisionStatus? IsUpprovedbyDepartmentNavigation { get; set; }

    public virtual TblDecisionStatus? IsUpprovedbyTeamNavigation { get; set; }

    public virtual TblDecisionStatus? IsUprovedByDeputyNavigation { get; set; }

    public virtual TblDecisionStatus? IsUserUprovedNavigation { get; set; }

    public virtual TblInspectionStatus? Pro { get; set; }

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblPlanInistitution> TblPlanInistitutions { get; set; } = new List<TblPlanInistitution>();

    public virtual ICollection<TblSentInspection> TblSentInspections { get; set; } = new List<TblSentInspection>();

    public virtual ICollection<TblSpecificPlan> TblSpecificPlans { get; set; } = new List<TblSpecificPlan>();

    public virtual TblTeam? Team { get; set; }

    public virtual TblInternalUser? User { get; set; }

    public virtual TblYear? Year { get; set; }
}
