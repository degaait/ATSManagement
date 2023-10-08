using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblCivilJustice
{
    public Guid RequestId { get; set; }

    public string? RequestDetail { get; set; }

    public Guid? InistId { get; set; }

    public Guid? RequestedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? DepId { get; set; }

    public Guid? CaseTypeId { get; set; }

    public Guid? AssignedBy { get; set; }

    public DateTime? AssignedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string? AssingmentRemark { get; set; }

    public Guid? AssignedTo { get; set; }

    public Guid? ExternalRequestStatusId { get; set; }

    public Guid UserUpprovalStatus { get; set; }

    public Guid TeamUpprovalStatus { get; set; }

    public Guid DeputyUprovalStatus { get; set; }

    public string? TopStatus { get; set; }

    public Guid? PriorityId { get; set; }

    public Guid DepartmentUpprovalStatus { get; set; }

    public string? FinalReport { get; set; }

    public virtual TblInternalUser? AssignedByNavigation { get; set; }

    public virtual TblInternalUser? AssignedToNavigation { get; set; }

    public virtual TblCivilJusticeCaseType? CaseType { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual TblDecisionStatus DepartmentUpprovalStatusNavigation { get; set; } = null!;

    public virtual TblDecisionStatus DeputyUprovalStatusNavigation { get; set; } = null!;

    public virtual TblExternalRequestStatus? ExternalRequestStatus { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblPriority? Priority { get; set; }

    public virtual TblExternalUser? RequestedByNavigation { get; set; }

    public virtual ICollection<TblAdjornment> TblAdjornments { get; set; } = new List<TblAdjornment>();

    public virtual ICollection<TblCivilJusticeRequestActivity> TblCivilJusticeRequestActivities { get; set; } = new List<TblCivilJusticeRequestActivity>();

    public virtual ICollection<TblCivilJusticeRequestReply> TblCivilJusticeRequestReplies { get; set; } = new List<TblCivilJusticeRequestReply>();

    public virtual ICollection<TblWitnessEvidence> TblWitnessEvidences { get; set; } = new List<TblWitnessEvidence>();

    public virtual TblDecisionStatus TeamUpprovalStatusNavigation { get; set; } = null!;

    public virtual TblDecisionStatus UserUpprovalStatusNavigation { get; set; } = null!;
}
