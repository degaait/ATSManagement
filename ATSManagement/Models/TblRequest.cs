using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblRequest
{
    public Guid RequestId { get; set; }

    public string? RequestDetail { get; set; }

    public Guid? InistId { get; set; }

    public Guid? RequestedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public bool? IsAssignedTodepartment { get; set; }

    public Guid? CaseTypeId { get; set; }

    public Guid? AssignedBy { get; set; }

    public DateTime? AssignedDate { get; set; }

    public DateTime? DueDate { get; set; }

    public string? AssingmentRemark { get; set; }

    public Guid? AssignedTo { get; set; }

    public Guid? ExternalRequestStatusId { get; set; }

    public int? TopStatusId { get; set; }

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

    public bool? IsArchived { get; set; }

    public string? FullName { get; set; }

    public string? EmailAddress { get; set; }

    public string? PhoneNumber { get; set; }

    public Guid? TypeId { get; set; }

    public string? LetterofUpproval { get; set; }

    public string? SentReport { get; set; }

    public bool? IsSenttoInst { get; set; }

    public bool? IsReturned { get; set; }

    public string? ReturningRemark { get; set; }

    public DateTime? SentDate { get; set; }

    public string? SendingRemark { get; set; }

    public int? MoneyAmount { get; set; }

    public string? MoneyCurrency { get; set; }

    public string? Adrtype { get; set; }

    public string? Claimant { get; set; }

    public string? ActingAs { get; set; }

    public string? Respondent { get; set; }

    public string? Reasult { get; set; }

    public string? ResultDescription { get; set; }

    public string? CaseType { get; set; }

    public string? Specialization { get; set; }

    public string? Country { get; set; }

    public string? CourtCenter { get; set; }

    public DateTime? DateOfAdjournment { get; set; }

    public string? Remark { get; set; }

    public string? LitigationType { get; set; }

    public string? Jursidiction { get; set; }

    public string? Bench { get; set; }

    public string? Plaintiful { get; set; }

    public string? Defendent { get; set; }

    public DateTime? DateofJudgement { get; set; }

    public string? DeputyRemark { get; set; }

    public string? StatusDescription { get; set; }

    public string? TeamDesicionRemark { get; set; }

    public string? DepartmentDesicionRemark { get; set; }

    public string? DeputyDesicionRemark { get; set; }

    public string? FinalReportSummary { get; set; }

    public string? ContractNeServiceType { get; set; }

    public string? ContractNeStatus { get; set; }

    public string? ContractNeResult { get; set; }

    public string? LegalAdviceStatus { get; set; }

    public string? LegalAdviceResult { get; set; }

    public string? InternationalCaseStatus { get; set; }

    public string? InternationalCaseResult { get; set; }

    public string? AdrStatus { get; set; }

    public string? AdrResult { get; set; }
    public string? SecretaryFullName { get; set; } 
    public string? ListigationStatus { get; set; }

    public string? ListigationResult { get; set; }

    public int OrderId { get; set; }

    public string? OtherServiceType { get; set; }

    public string? OtherDocumentType { get; set; }

    public virtual TblInternalUser? AssignedByNavigation { get; set; }

    public virtual TblCivilJusticeCaseType? CaseTypeNavigation { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

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

    public virtual ICollection<TblCivilJusticeChat> TblCivilJusticeChats { get; set; } = new List<TblCivilJusticeChat>();

    public virtual ICollection<TblDesicionRemark> TblDesicionRemarks { get; set; } = new List<TblDesicionRemark>();

    public virtual ICollection<TblDocumentHistory> TblDocumentHistories { get; set; } = new List<TblDocumentHistory>();

    public virtual ICollection<TblFollowup> TblFollowups { get; set; } = new List<TblFollowup>();

    public virtual ICollection<TblLegalStudiesChat> TblLegalStudiesChats { get; set; } = new List<TblLegalStudiesChat>();

    public virtual ICollection<TblReplay> TblReplays { get; set; } = new List<TblReplay>();

    public virtual ICollection<TblRequestAssignee> TblRequestAssignees { get; set; } = new List<TblRequestAssignee>();

    public virtual ICollection<TblRequestDepartmentRelation> TblRequestDepartmentRelations { get; set; } = new List<TblRequestDepartmentRelation>();

    public virtual ICollection<TblRequestPriorityQuestionsRelation> TblRequestPriorityQuestionsRelations { get; set; } = new List<TblRequestPriorityQuestionsRelation>();

    public virtual ICollection<TblRound> TblRounds { get; set; } = new List<TblRound>();

    public virtual ICollection<TblWitnessEvidence> TblWitnessEvidences { get; set; } = new List<TblWitnessEvidence>();

    public virtual TblDecisionStatus? TeamUpprovalStatusNavigation { get; set; }

    public virtual TblTopStatus? TopStatus { get; set; }

    public virtual TblRequestAssignementType? Type { get; set; }

    public virtual TblDecisionStatus? UserUpprovalStatusNavigation { get; set; }
}
