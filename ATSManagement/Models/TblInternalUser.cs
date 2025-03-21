﻿using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInternalUser
{
    public Guid UserId { get; set; }

    public string? FirstName { get; set; }

    public string? MidleName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddress { get; set; }

    public bool IsSuperAdmin { get; set; }

    public bool IsTeamLeader { get; set; }

    public Guid? DepId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public bool IsDeputy { get; set; }

    public bool IsDepartmentHead { get; set; }

    public bool IsActive { get; set; }

    public Guid? TeamId { get; set; }

    public bool? IsDefaultUser { get; set; }

    public bool? IsSecretary { get; set; }

    public bool? IsBranchOfficeUser { get; set; }

    public bool? IsInternalRequestUser { get; set; }

    public int? BranchId { get; set; }

    public bool? IsDeputySecretary { get; set; }

    public bool? IsCivilJusticeSecretay { get; set; }

    public bool? IsLegalStudySecretary { get; set; }

    public virtual TblBranchOffice? Branch { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual ICollection<TblActivity> TblActivities { get; set; } = new List<TblActivity>();

    public virtual ICollection<TblAdjornment> TblAdjornments { get; set; } = new List<TblAdjornment>();

    public virtual ICollection<TblAdjournmentChat> TblAdjournmentChatSendByNavigations { get; set; } = new List<TblAdjournmentChat>();

    public virtual ICollection<TblAdjournmentChat> TblAdjournmentChatSendToNavigations { get; set; } = new List<TblAdjournmentChat>();

    public virtual ICollection<TblAdjournmentChat> TblAdjournmentChatUsers { get; set; } = new List<TblAdjournmentChat>();

    public virtual ICollection<TblAdractivitiesReport> TblAdractivitiesReports { get; set; } = new List<TblAdractivitiesReport>();

    public virtual ICollection<TblAppointmentChat> TblAppointmentChats { get; set; } = new List<TblAppointmentChat>();

    public virtual ICollection<TblAppointmentParticipant> TblAppointmentParticipants { get; set; } = new List<TblAppointmentParticipant>();

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlanAssignedByNavigations { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlanAssignedToNavigations { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblAssignee> TblAssignees { get; set; } = new List<TblAssignee>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeAssignedByNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeAssignedToNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJusticeChat> TblCivilJusticeChatSendByNavigations { get; set; } = new List<TblCivilJusticeChat>();

    public virtual ICollection<TblCivilJusticeChat> TblCivilJusticeChatSendToNavigations { get; set; } = new List<TblCivilJusticeChat>();

    public virtual ICollection<TblCivilJusticeChat> TblCivilJusticeChatUsers { get; set; } = new List<TblCivilJusticeChat>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeCreatedByNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJusticeRequestActivity> TblCivilJusticeRequestActivities { get; set; } = new List<TblCivilJusticeRequestActivity>();

    public virtual ICollection<TblCivilJusticeRequestReply> TblCivilJusticeRequestReplies { get; set; } = new List<TblCivilJusticeRequestReply>();

    public virtual ICollection<TblDebateWorkPerformanceReport> TblDebateWorkPerformanceReports { get; set; } = new List<TblDebateWorkPerformanceReport>();

    public virtual ICollection<TblDesicionRemark> TblDesicionRemarks { get; set; } = new List<TblDesicionRemark>();

    public virtual ICollection<TblDocumentHistory> TblDocumentHistories { get; set; } = new List<TblDocumentHistory>();

    public virtual ICollection<TblDraftContractExaminationReport> TblDraftContractExaminationReports { get; set; } = new List<TblDraftContractExaminationReport>();

    public virtual ICollection<TblFollowup> TblFollowups { get; set; } = new List<TblFollowup>();

    public virtual ICollection<TblInpectionActivite> TblInpectionActivites { get; set; } = new List<TblInpectionActivite>();

    public virtual ICollection<TblInspectionChat> TblInspectionChatSendByNavigations { get; set; } = new List<TblInspectionChat>();

    public virtual ICollection<TblInspectionChat> TblInspectionChatSendToNavigations { get; set; } = new List<TblInspectionChat>();

    public virtual ICollection<TblInspectionChat> TblInspectionChatUsers { get; set; } = new List<TblInspectionChat>();

    public virtual ICollection<TblInspectionInstitution> TblInspectionInstitutions { get; set; } = new List<TblInspectionInstitution>();

    public virtual ICollection<TblInspectionPlan> TblInspectionPlans { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblInspectionReplye> TblInspectionReplyes { get; set; } = new List<TblInspectionReplye>();

    public virtual ICollection<TblInspectionReportFile> TblInspectionReportFiles { get; set; } = new List<TblInspectionReportFile>();

    public virtual ICollection<TblInspectionReport> TblInspectionReports { get; set; } = new List<TblInspectionReport>();

    public virtual ICollection<TblInstotutionMonitoringReport> TblInstotutionMonitoringReports { get; set; } = new List<TblInstotutionMonitoringReport>();

    public virtual ICollection<TblInternalDocumentHistory> TblInternalDocumentHistories { get; set; } = new List<TblInternalDocumentHistory>();

    public virtual ICollection<TblInternalRequest> TblInternalRequestAssignedByNavigations { get; set; } = new List<TblInternalRequest>();

    public virtual ICollection<TblInternalRequestAssignee> TblInternalRequestAssignees { get; set; } = new List<TblInternalRequestAssignee>();

    public virtual ICollection<TblInternalRequestChat> TblInternalRequestChatSendByNavigations { get; set; } = new List<TblInternalRequestChat>();

    public virtual ICollection<TblInternalRequestChat> TblInternalRequestChatSendToNavigations { get; set; } = new List<TblInternalRequestChat>();

    public virtual ICollection<TblInternalRequestChat> TblInternalRequestChatUsers { get; set; } = new List<TblInternalRequestChat>();

    public virtual ICollection<TblInternalRequestReplay> TblInternalRequestReplays { get; set; } = new List<TblInternalRequestReplay>();

    public virtual ICollection<TblInternalRequest> TblInternalRequestRequestedByNavigations { get; set; } = new List<TblInternalRequest>();

    public virtual ICollection<TblLegalAdviceReport> TblLegalAdviceReports { get; set; } = new List<TblLegalAdviceReport>();

    public virtual ICollection<TblLegalStudiesActivity> TblLegalStudiesActivities { get; set; } = new List<TblLegalStudiesActivity>();

    public virtual ICollection<TblLegalStudiesChat> TblLegalStudiesChatSendByNavigations { get; set; } = new List<TblLegalStudiesChat>();

    public virtual ICollection<TblLegalStudiesChat> TblLegalStudiesChatSendToNavigations { get; set; } = new List<TblLegalStudiesChat>();

    public virtual ICollection<TblLegalStudiesChat> TblLegalStudiesChatUsers { get; set; } = new List<TblLegalStudiesChat>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingAssignedByNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingAssignedToNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingCreatedByNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesReplay> TblLegalStudiesReplays { get; set; } = new List<TblLegalStudiesReplay>();

    public virtual ICollection<TblNotification> TblNotificationCreatedByNavigations { get; set; } = new List<TblNotification>();

    public virtual ICollection<TblNotification> TblNotificationUsers { get; set; } = new List<TblNotification>();

    public virtual ICollection<TblRecomendation> TblRecomendations { get; set; } = new List<TblRecomendation>();

    public virtual ICollection<TblReplay> TblReplays { get; set; } = new List<TblReplay>();

    public virtual ICollection<TblReplyResponseWithExpert> TblReplyResponseWithExperts { get; set; } = new List<TblReplyResponseWithExpert>();

    public virtual ICollection<TblReplyResponseWithStateMinster> TblReplyResponseWithStateMinsters { get; set; } = new List<TblReplyResponseWithStateMinster>();

    public virtual ICollection<TblRequest> TblRequestAssignedByNavigations { get; set; } = new List<TblRequest>();

    public virtual ICollection<TblRequestAssignee> TblRequestAssignees { get; set; } = new List<TblRequestAssignee>();

    public virtual ICollection<TblRequest> TblRequestCreatedByNavigations { get; set; } = new List<TblRequest>();

    public virtual ICollection<TblSentInspection> TblSentInspections { get; set; } = new List<TblSentInspection>();

    public virtual ICollection<TblSpecificPlan> TblSpecificPlans { get; set; } = new List<TblSpecificPlan>();

    public virtual ICollection<TblTeam> TblTeams { get; set; } = new List<TblTeam>();

    public virtual ICollection<TblWitnessEvidence> TblWitnessEvidences { get; set; } = new List<TblWitnessEvidence>();

    public virtual TblTeam? Team { get; set; }
}
