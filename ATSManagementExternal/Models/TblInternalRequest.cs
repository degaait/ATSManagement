using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblInternalRequest
{
    public Guid RequestId { get; set; }

    public string? RequestDetail { get; set; }

    public Guid? RequestedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? IsAssignedToexpert { get; set; }

    public Guid? AssignedBy { get; set; }

    public DateTime? AssignedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string? AssingmentRemark { get; set; }

    public Guid? RequestStatusId { get; set; }

    public Guid? UserUpprovalStatus { get; set; }

    public Guid? TeamUpprovalStatus { get; set; }

    public Guid? DepartmentUpprovalStatus { get; set; }

    public string? FinalReport { get; set; }

    public Guid? ServiceTypeId { get; set; }

    public bool? IsArchived { get; set; }

    public string? SentReport { get; set; }

    public DateTime? SentDate { get; set; }

    public string? Remark { get; set; }

    public string? StatusDescription { get; set; }

    public string? TeamDesicionRemark { get; set; }

    public string? DepartmentDesicionRemark { get; set; }

    public string? FinalReportSummary { get; set; }

    public int? OrderId { get; set; }

    public int? TopStatusId { get; set; }

    public virtual TblInternalUser? AssignedByNavigation { get; set; }

    public virtual TblDecisionStatus? DepartmentUpprovalStatusNavigation { get; set; }

    public virtual TblInternalRequestStatus? RequestStatus { get; set; }

    public virtual TblInternalUser? RequestedByNavigation { get; set; }

    public virtual TblInternalServiceType? ServiceType { get; set; }

    public virtual ICollection<TblInternalDocumentHistory> TblInternalDocumentHistories { get; set; } = new List<TblInternalDocumentHistory>();

    public virtual ICollection<TblInternalRequestAssignee> TblInternalRequestAssignees { get; set; } = new List<TblInternalRequestAssignee>();

    public virtual ICollection<TblInternalRequestReplay> TblInternalRequestReplays { get; set; } = new List<TblInternalRequestReplay>();

    public virtual TblDecisionStatus? TeamUpprovalStatusNavigation { get; set; }

    public virtual TblTopStatus? TopStatus { get; set; }

    public virtual TblDecisionStatus? UserUpprovalStatusNavigation { get; set; }
}
