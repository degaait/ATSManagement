using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblLegalStudiesDrafting
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

    public string? TopStatus { get; set; }

    public Guid? PriorityId { get; set; }

    public Guid? UserUpprovalStatus { get; set; }

    public Guid? TeamUpprovalStatus { get; set; }

    public Guid? DeputyUprovalStatus { get; set; }

    public Guid? DepartmentUpprovalStatus { get; set; }

    public Guid? DocId { get; set; }

    public Guid? QuestTypeId { get; set; }

    public string? FinalReport { get; set; }

    public string? DocumentFile { get; set; }

    public virtual TblInternalUser? AssignedByNavigation { get; set; }

    public virtual TblInternalUser? AssignedToNavigation { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual TblDecisionStatus? DepartmentUpprovalStatusNavigation { get; set; }

    public virtual TblDecisionStatus? DeputyUprovalStatusNavigation { get; set; }

    public virtual TblLegalDraftingDocType? Doc { get; set; }

    public virtual TblExternalRequestStatus? ExternalRequestStatus { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblPriority? Priority { get; set; }

    public virtual TblLegalDraftingQuestionType? QuestType { get; set; }

    public virtual TblExternalUser? RequestedByNavigation { get; set; }

    public virtual ICollection<TblLegalStudiesActivity> TblLegalStudiesActivities { get; set; } = new List<TblLegalStudiesActivity>();

    public virtual ICollection<TblLegalStudiesReplay> TblLegalStudiesReplays { get; set; } = new List<TblLegalStudiesReplay>();

    public virtual TblDecisionStatus? TeamUpprovalStatusNavigation { get; set; }

    public virtual TblDecisionStatus? UserUpprovalStatusNavigation { get; set; }
}
