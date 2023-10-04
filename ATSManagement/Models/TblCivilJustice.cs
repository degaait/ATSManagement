using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

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

    public bool IsUpprovedByUser { get; set; }

    public bool IsUprovedByTeam { get; set; }

    public bool IsUprovedByDeputy { get; set; }

    public string? TopStatus { get; set; }

    public Guid? PriorityId { get; set; }

    public bool IsUprovedbyDepartment { get; set; }

    public virtual TblInternalUser? AssignedByNavigation { get; set; }

    public virtual TblInternalUser? AssignedToNavigation { get; set; }

    public virtual TblCivilJusticeCaseType? CaseType { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual TblExternalRequestStatus? ExternalRequestStatus { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblPriority? Priority { get; set; }

    public virtual TblExternalUser? RequestedByNavigation { get; set; }

    public virtual ICollection<TblCivilJusticeRequestActivity> TblCivilJusticeRequestActivities { get; set; } = new List<TblCivilJusticeRequestActivity>();

    public virtual ICollection<TblCivilJusticeRequestReply> TblCivilJusticeRequestReplies { get; set; } = new List<TblCivilJusticeRequestReply>();
}
