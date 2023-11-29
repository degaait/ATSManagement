using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblAssignedYearlyPlan
{
    public Guid Id { get; set; }

    public Guid? AssignedTo { get; set; }

    public Guid? AssignedBy { get; set; }

    public Guid? PlanId { get; set; }

    public DateTime? AssignedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string? EvaluationCheckLists { get; set; }

    public string? EngagementLetter { get; set; }

    public string? MeetingLetter { get; set; }

    public string? Remark { get; set; }

    public string? ProgressStatus { get; set; }

    public Guid? StatusId { get; set; }

    public string? FinalReport { get; set; }

    public bool? IsUprovedByDeputy { get; set; }

    public bool? IsUpprovedbyTeam { get; set; }

    public bool? IsUpprovedbyDepartment { get; set; }

    public virtual TblInternalUser? AssignedByNavigation { get; set; }

    public virtual TblInternalUser? AssignedToNavigation { get; set; }

    public virtual TblInspectionPlan? Plan { get; set; }

    public virtual TblStatus? Status { get; set; }
}
