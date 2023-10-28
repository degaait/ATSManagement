using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblRequest
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

    public Guid? TopStatus { get; set; }

    public Guid? PriorityId { get; set; }

    public Guid? UserUpprovalStatus { get; set; }

    public Guid? TeamUpprovalStatus { get; set; }

    public Guid? DeputyUprovalStatus { get; set; }

    public Guid? DepartmentUpprovalStatus { get; set; }

    public Guid? DocTypeId { get; set; }

    public Guid? QuestTypeId { get; set; }

    public string? FinalReport { get; set; }

    public string? DocumentFile { get; set; }

    public Guid? ServiceTypeId { get; set; }

    public int? RequestRound { get; set; }

    public virtual TblInternalUser? AssignedByNavigation { get; set; }

    public virtual TblCivilJusticeCaseType? CaseType { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual TblDecisionStatus? DepartmentUpprovalStatusNavigation { get; set; }

    public virtual TblDecisionStatus? DeputyUprovalStatusNavigation { get; set; }

    public virtual TblLegalDraftingDocType? DocType { get; set; }

    public virtual TblExternalRequestStatus? ExternalRequestStatus { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblPriority? Priority { get; set; }

    public virtual TblLegalDraftingQuestionType? QuestType { get; set; }

    public virtual TblExternalUser? RequestedByNavigation { get; set; }

    public virtual TblServiceType? ServiceType { get; set; }

    public virtual ICollection<TblActivity> TblActivities { get; set; } = new List<TblActivity>();

    public virtual ICollection<TblAdjornment> TblAdjornments { get; set; } = new List<TblAdjornment>();

    public virtual ICollection<TblDocumentHistory> TblDocumentHistories { get; set; } = new List<TblDocumentHistory>();

    public virtual ICollection<TblReplay> TblReplays { get; set; } = new List<TblReplay>();

    public virtual ICollection<TblRequestAssignee> TblRequestAssignees { get; set; } = new List<TblRequestAssignee>();

    public virtual ICollection<TblRequestPriorityQuestionsRelation> TblRequestPriorityQuestionsRelations { get; set; } = new List<TblRequestPriorityQuestionsRelation>();

    public virtual ICollection<TblWitnessEvidence> TblWitnessEvidences { get; set; } = new List<TblWitnessEvidence>();

    public virtual TblDecisionStatus? TeamUpprovalStatusNavigation { get; set; }

    public virtual TblDecisionStatus? UserUpprovalStatusNavigation { get; set; }
}
