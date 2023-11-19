using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

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

    public virtual TblDepartment? Dep { get; set; }

    public virtual ICollection<TblActivity> TblActivities { get; set; } = new List<TblActivity>();

    public virtual ICollection<TblAdjornment> TblAdjornments { get; set; } = new List<TblAdjornment>();

    public virtual ICollection<TblAppointmentParticipant> TblAppointmentParticipants { get; set; } = new List<TblAppointmentParticipant>();

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblAssignee> TblAssignees { get; set; } = new List<TblAssignee>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeAssignedByNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeAssignedToNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeCreatedByNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJusticeRequestActivity> TblCivilJusticeRequestActivities { get; set; } = new List<TblCivilJusticeRequestActivity>();

    public virtual ICollection<TblCivilJusticeRequestReply> TblCivilJusticeRequestReplies { get; set; } = new List<TblCivilJusticeRequestReply>();

    public virtual ICollection<TblDebateWorkPerformanceReport> TblDebateWorkPerformanceReports { get; set; } = new List<TblDebateWorkPerformanceReport>();

    public virtual ICollection<TblDocumentHistory> TblDocumentHistories { get; set; } = new List<TblDocumentHistory>();

    public virtual ICollection<TblDraftContractExaminationReport> TblDraftContractExaminationReports { get; set; } = new List<TblDraftContractExaminationReport>();

    public virtual ICollection<TblInspectionInstitution> TblInspectionInstitutions { get; set; } = new List<TblInspectionInstitution>();

    public virtual ICollection<TblInspectionPlan> TblInspectionPlans { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblInstotutionMonitoringReport> TblInstotutionMonitoringReports { get; set; } = new List<TblInstotutionMonitoringReport>();

    public virtual ICollection<TblLegalAdviceReport> TblLegalAdviceReports { get; set; } = new List<TblLegalAdviceReport>();

    public virtual ICollection<TblLegalStudiesActivity> TblLegalStudiesActivities { get; set; } = new List<TblLegalStudiesActivity>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingAssignedByNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingAssignedToNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingCreatedByNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesReplay> TblLegalStudiesReplays { get; set; } = new List<TblLegalStudiesReplay>();

    public virtual ICollection<TblRecomendation> TblRecomendations { get; set; } = new List<TblRecomendation>();

    public virtual ICollection<TblReplay> TblReplays { get; set; } = new List<TblReplay>();

    public virtual ICollection<TblRequest> TblRequestAssignedByNavigations { get; set; } = new List<TblRequest>();

    public virtual ICollection<TblRequestAssignee> TblRequestAssignees { get; set; } = new List<TblRequestAssignee>();

    public virtual ICollection<TblRequest> TblRequestCreatedByNavigations { get; set; } = new List<TblRequest>();

    public virtual ICollection<TblSpecificPlan> TblSpecificPlans { get; set; } = new List<TblSpecificPlan>();

    public virtual ICollection<TblTeam> TblTeams { get; set; } = new List<TblTeam>();

    public virtual ICollection<TblWitnessEvidence> TblWitnessEvidences { get; set; } = new List<TblWitnessEvidence>();

    public virtual TblTeam? Team { get; set; }
}
