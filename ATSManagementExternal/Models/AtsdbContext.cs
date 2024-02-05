using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ATSManagementExternal.Models;
public partial class AtsdbContext : DbContext
{
    public AtsdbContext()
    {
    }

    public AtsdbContext(DbContextOptions<AtsdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblActivity> TblActivities { get; set; }

    public virtual DbSet<TblAdjornment> TblAdjornments { get; set; }

    public virtual DbSet<TblAdractivitiesReport> TblAdractivitiesReports { get; set; }

    public virtual DbSet<TblAdreventType> TblAdreventTypes { get; set; }

    public virtual DbSet<TblAppointment> TblAppointments { get; set; }

    public virtual DbSet<TblAppointmentParticipant> TblAppointmentParticipants { get; set; }

    public virtual DbSet<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; }

    public virtual DbSet<TblAssignee> TblAssignees { get; set; }

    public virtual DbSet<TblAssignementType> TblAssignementTypes { get; set; }

    public virtual DbSet<TblCivilJustice> TblCivilJustices { get; set; }

    public virtual DbSet<TblCivilJusticeCaseType> TblCivilJusticeCaseTypes { get; set; }

    public virtual DbSet<TblCivilJusticeRequestActivity> TblCivilJusticeRequestActivities { get; set; }

    public virtual DbSet<TblCivilJusticeRequestReply> TblCivilJusticeRequestReplys { get; set; }

    public virtual DbSet<TblCompanyEmail> TblCompanyEmails { get; set; }

    public virtual DbSet<TblContactInformation> TblContactInformations { get; set; }

    public virtual DbSet<TblDebatePerformance> TblDebatePerformances { get; set; }

    public virtual DbSet<TblDebatePerformanceEventType> TblDebatePerformanceEventTypes { get; set; }

    public virtual DbSet<TblDebateWorkPerformanceReport> TblDebateWorkPerformanceReports { get; set; }

    public virtual DbSet<TblDecisionStatus> TblDecisionStatuses { get; set; }

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblDocumentHistory> TblDocumentHistories { get; set; }

    public virtual DbSet<TblDraftContractExaminationReport> TblDraftContractExaminationReports { get; set; }

    public virtual DbSet<TblEvent> TblEvents { get; set; }

    public virtual DbSet<TblExternalMainMenu> TblExternalMainMenus { get; set; }

    public virtual DbSet<TblExternalRequest> TblExternalRequests { get; set; }

    public virtual DbSet<TblExternalRequestStatus> TblExternalRequestStatuses { get; set; }

    public virtual DbSet<TblExternalSubmenu> TblExternalSubmenus { get; set; }

    public virtual DbSet<TblExternalUser> TblExternalUsers { get; set; }

    public virtual DbSet<TblFollowup> TblFollowups { get; set; }

    public virtual DbSet<TblInistitution> TblInistitutions { get; set; }

    public virtual DbSet<TblInspectionInstitution> TblInspectionInstitutions { get; set; }

    public virtual DbSet<TblInspectionLaw> TblInspectionLaws { get; set; }

    public virtual DbSet<TblInspectionPlan> TblInspectionPlans { get; set; }

    public virtual DbSet<TblInspectionReplye> TblInspectionReplyes { get; set; }

    public virtual DbSet<TblInspectionStatus> TblInspectionStatuses { get; set; }

    public virtual DbSet<TblInstotutionMonitoringReport> TblInstotutionMonitoringReports { get; set; }

    public virtual DbSet<TblInternalUser> TblInternalUsers { get; set; }

    public virtual DbSet<TblLanguage> TblLanguages { get; set; }

    public virtual DbSet<TblLegalAdviceReport> TblLegalAdviceReports { get; set; }

    public virtual DbSet<TblLegalAdviceServantType> TblLegalAdviceServantTypes { get; set; }

    public virtual DbSet<TblLegalDraftingDocType> TblLegalDraftingDocTypes { get; set; }

    public virtual DbSet<TblLegalDraftingQuestionType> TblLegalDraftingQuestionTypes { get; set; }

    public virtual DbSet<TblLegalStudiesActivity> TblLegalStudiesActivities { get; set; }

    public virtual DbSet<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; }

    public virtual DbSet<TblLegalStudiesReplay> TblLegalStudiesReplays { get; set; }

    public virtual DbSet<TblMainMenu> TblMainMenus { get; set; }

    public virtual DbSet<TblMonth> TblMonths { get; set; }

    public virtual DbSet<TblNotification> TblNotifications { get; set; }

    public virtual DbSet<TblPlanCatagory> TblPlanCatagories { get; set; }

    public virtual DbSet<TblPlanInistitution> TblPlanInistitutions { get; set; }

    public virtual DbSet<TblPriority> TblPriorities { get; set; }

    public virtual DbSet<TblPriorityQuestion> TblPriorityQuestions { get; set; }

    public virtual DbSet<TblRecomendation> TblRecomendations { get; set; }

    public virtual DbSet<TblRecomendationStatus> TblRecomendationStatuses { get; set; }

    public virtual DbSet<TblReplay> TblReplays { get; set; }

    public virtual DbSet<TblReponseStatus> TblReponseStatuses { get; set; }

    public virtual DbSet<TblReportServiceType> TblReportServiceTypes { get; set; }

    public virtual DbSet<TblRequest> TblRequests { get; set; }

    public virtual DbSet<TblRequestAssignee> TblRequestAssignees { get; set; }

    public virtual DbSet<TblRequestAssignementType> TblRequestAssignementTypes { get; set; }

    public virtual DbSet<TblRequestDepartmentRelation> TblRequestDepartmentRelations { get; set; }

    public virtual DbSet<TblRequestPriorityQuestionsRelation> TblRequestPriorityQuestionsRelations { get; set; }

    public virtual DbSet<TblRequestType> TblRequestTypes { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblRound> TblRounds { get; set; }

    public virtual DbSet<TblSentInspection> TblSentInspections { get; set; }

    public virtual DbSet<TblServiceType> TblServiceTypes { get; set; }

    public virtual DbSet<TblSpecificPlan> TblSpecificPlans { get; set; }

    public virtual DbSet<TblStatus> TblStatuses { get; set; }

    public virtual DbSet<TblSubDebatePerformance> TblSubDebatePerformances { get; set; }

    public virtual DbSet<TblSubmenu> TblSubmenus { get; set; }

    public virtual DbSet<TblTeam> TblTeams { get; set; }

    public virtual DbSet<TblTermsAndCondition> TblTermsAndConditions { get; set; }

    public virtual DbSet<TblTopStatus> TblTopStatuses { get; set; }

    public virtual DbSet<TblWitnessEvidence> TblWitnessEvidences { get; set; }

    public virtual DbSet<TblYear> TblYears { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var configSection = configBuilder.GetSection("ConnectionStrings");
        var connectionString = configSection["ATSDB"] ?? null;
        optionsBuilder.UseSqlServer(connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<TblActivity>(entity =>
        {
            entity.HasKey(e => e.ActivityId);

            entity.ToTable("tbl_Activities");

            entity.Property(e => e.ActivityId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ActivityID");
            entity.Property(e => e.AddedDate).HasColumnType("datetime");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblActivities)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_Activities_tbl_InternalUsers");

            entity.HasOne(d => d.Request).WithMany(p => p.TblActivities)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Activities_tbl_Requests");
        });

        modelBuilder.Entity<TblAdjornment>(entity =>
        {
            entity.HasKey(e => e.AdjoryId);

            entity.ToTable("tbl_Adjornments");

            entity.Property(e => e.AdjoryId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AdjorneyDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblAdjornments)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_Adjornments_tbl_InternalUsers");

            entity.HasOne(d => d.Request).WithMany(p => p.TblAdjornments)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Adjornments_tbl_Requests");
        });

        modelBuilder.Entity<TblAdractivitiesReport>(entity =>
        {
            entity.ToTable("tbl_ADRActivitiesReport");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.Childrens).HasMaxLength(50);
            entity.Property(e => e.Hivpositives)
                .HasMaxLength(300)
                .HasColumnName("HIVPositives");
            entity.Property(e => e.Mens).HasMaxLength(300);
            entity.Property(e => e.MonthId).HasColumnName("MonthID");
            entity.Property(e => e.OtherServantType).HasMaxLength(300);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.WomeElders).HasMaxLength(300);
            entity.Property(e => e.Womens).HasMaxLength(50);
            entity.Property(e => e.YearId).HasColumnName("YearID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblAdractivitiesReports)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_ADRActivitiesReport_tbl_InternalUsers");

            entity.HasOne(d => d.Month).WithMany(p => p.TblAdractivitiesReports)
                .HasForeignKey(d => d.MonthId)
                .HasConstraintName("FK_tbl_ADRActivitiesReport_tbl_Months");

            entity.HasOne(d => d.Type).WithMany(p => p.TblAdractivitiesReports)
                .HasForeignKey(d => d.TypeId)
                .HasConstraintName("FK_tbl_ADRActivitiesReport_tbl_ADREventTypes");

            entity.HasOne(d => d.Year).WithMany(p => p.TblAdractivitiesReports)
                .HasForeignKey(d => d.YearId)
                .HasConstraintName("FK_tbl_ADRActivitiesReport_tbl_Years");
        });

        modelBuilder.Entity<TblAdreventType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("tbl_ADREventTypes");

            entity.Property(e => e.TypeId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("TypeID");
            entity.Property(e => e.TypeNames).HasMaxLength(450);
        });

        modelBuilder.Entity<TblAppointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId);

            entity.ToTable("tbl_Appointments");

            entity.Property(e => e.AppointmentId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("AppointmentID");
            entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.InistId).HasColumnName("InistID");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblAppointments)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_Appointments_tbl_Inistitutions");

            entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.TblAppointments)
                .HasForeignKey(d => d.RequestedBy)
                .HasConstraintName("FK_tbl_Appointments_tbl_ExternalUser");
        });

        modelBuilder.Entity<TblAppointmentParticipant>(entity =>
        {
            entity.ToTable("tbl_AppointmentParticipants");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Appointment).WithMany(p => p.TblAppointmentParticipants)
                .HasForeignKey(d => d.AppointmentId)
                .HasConstraintName("FK_tbl_AppointmentParticipants_tbl_AppointmentParticipants");

            entity.HasOne(d => d.User).WithMany(p => p.TblAppointmentParticipants)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tbl_AppointmentParticipants_tbl_InternalUsers");
        });

        modelBuilder.Entity<TblAssignedYearlyPlan>(entity =>
        {
            entity.ToTable("tbl_AssignedYearlyPlans");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.AssignedDate).HasColumnType("date");
            entity.Property(e => e.DueDate).HasColumnType("date");
            entity.Property(e => e.ProgressStatus).HasMaxLength(250);
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.Torattachment).HasColumnName("TORAttachment");

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.TblAssignedYearlyPlanAssignedByNavigations)
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK_tbl_AssignedYearlyPlans_tbl_AssignedBy");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TblAssignedYearlyPlanAssignedToNavigations)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK_tbl_AssignedYearlyPlans_tbl_InternalUsers");

            entity.HasOne(d => d.Plan).WithMany(p => p.TblAssignedYearlyPlans)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_AssignedYearlyPlans_tbl_InspectionPlans");

            entity.HasOne(d => d.SpecificPlan).WithMany(p => p.TblAssignedYearlyPlans)
                .HasForeignKey(d => d.SpecificPlanId)
                .HasConstraintName("FK_tbl_AssignedYearlyPlans_tbl_SpecificPlans");

            entity.HasOne(d => d.Status).WithMany(p => p.TblAssignedYearlyPlans)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_tbl_AssignedYearlyPlans_tbl_Status");
        });

        modelBuilder.Entity<TblAssignee>(entity =>
        {
            entity.ToTable("tbl_Assignees");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.TblAssignees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tbl_Assignees_tbl_InternalUsers");
        });

        modelBuilder.Entity<TblAssignementType>(entity =>
        {
            entity.HasKey(e => e.AssigneeTypeId).HasName("PK_tlb_AssignementTypes");

            entity.ToTable("tbl_AssignementTypes");

            entity.Property(e => e.AssigneeTypeId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("AssigneeTypeID");
            entity.Property(e => e.AssigneeType).HasMaxLength(150);
            entity.Property(e => e.AssigneeTypeAmharic).HasMaxLength(250);
        });

        modelBuilder.Entity<TblCivilJustice>(entity =>
        {
            entity.HasKey(e => e.RequestId);

            entity.ToTable("tbl_CivilJustice");

            entity.Property(e => e.RequestId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("RequestID");
            entity.Property(e => e.AssignedDate).HasColumnType("datetime");
            entity.Property(e => e.CaseTypeId).HasColumnName("CaseTypeID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.ExternalRequestStatusId).HasColumnName("ExternalRequestStatusID");
            entity.Property(e => e.InistId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.TopStatus).HasMaxLength(250);

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.TblCivilJusticeAssignedByNavigations)
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_InternalUsers1");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TblCivilJusticeAssignedToNavigations)
                .HasForeignKey(d => d.AssignedTo)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_InternalUsers");

            entity.HasOne(d => d.CaseType).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.CaseTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_CivilJusticeCaseType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblCivilJusticeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_InternalUsers2");

            entity.HasOne(d => d.Dep).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.DepId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_Department");

            entity.HasOne(d => d.DepartmentUpprovalStatusNavigation).WithMany(p => p.TblCivilJusticeDepartmentUpprovalStatusNavigations)
                .HasForeignKey(d => d.DepartmentUpprovalStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_DecisionStatus");

            entity.HasOne(d => d.DeputyUprovalStatusNavigation).WithMany(p => p.TblCivilJusticeDeputyUprovalStatusNavigations)
                .HasForeignKey(d => d.DeputyUprovalStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_DecisionStatus2");

            entity.HasOne(d => d.ExternalRequestStatus).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.ExternalRequestStatusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_ExternalRequestStatus");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.InistId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_Inistitutions");

            entity.HasOne(d => d.Priority).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.PriorityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_Priority");

            entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.RequestedBy)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_ExternalUser");

            entity.HasOne(d => d.TeamUpprovalStatusNavigation).WithMany(p => p.TblCivilJusticeTeamUpprovalStatusNavigations)
                .HasForeignKey(d => d.TeamUpprovalStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_DecisionStatus1");

            entity.HasOne(d => d.UserUpprovalStatusNavigation).WithMany(p => p.TblCivilJusticeUserUpprovalStatusNavigations)
                .HasForeignKey(d => d.UserUpprovalStatus)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_DecisionStatus3");
        });

        modelBuilder.Entity<TblCivilJusticeCaseType>(entity =>
        {
            entity.HasKey(e => e.CaseTypeId);

            entity.ToTable("tbl_CivilJusticeCaseType");

            entity.Property(e => e.CaseTypeId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("CaseTypeID");
            entity.Property(e => e.CaseTypeName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblCivilJusticeRequestActivity>(entity =>
        {
            entity.HasKey(e => e.ActivityId);

            entity.ToTable("tbl_CivilJusticeRequestActivity");

            entity.Property(e => e.ActivityId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ActivityID");
            entity.Property(e => e.AddedDate).HasColumnType("datetime");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblCivilJusticeRequestActivities)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_CivilJusticeRequestActivity_tbl_InternalUsers");

            entity.HasOne(d => d.Request).WithMany(p => p.TblCivilJusticeRequestActivities)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_CivilJusticeRequestActivity_tbl_CivilJustice");
        });

        modelBuilder.Entity<TblCivilJusticeRequestReply>(entity =>
        {
            entity.HasKey(e => e.ReplyId);

            entity.ToTable("tbl_CivilJusticeRequestReplys");

            entity.Property(e => e.ReplyId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ReplyDate).HasColumnType("datetime");

            entity.HasOne(d => d.ExternalReplayedByNavigation).WithMany(p => p.TblCivilJusticeRequestReplies)
                .HasForeignKey(d => d.ExternalReplayedBy)
                .HasConstraintName("FK_tbl_CivilJusticeRequestReplys_tbl_ExternalUser");

            entity.HasOne(d => d.InternalReplayedByNavigation).WithMany(p => p.TblCivilJusticeRequestReplies)
                .HasForeignKey(d => d.InternalReplayedBy)
                .HasConstraintName("FK_tbl_CivilJusticeRequestReplys_tbl_InternalUsers");

            entity.HasOne(d => d.Request).WithMany(p => p.TblCivilJusticeRequestReplies)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_CivilJusticeRequestReplys_tbl_CivilJustice");
        });

        modelBuilder.Entity<TblCompanyEmail>(entity =>
        {
            entity.HasKey(e => e.EmailId);

            entity.ToTable("tbl_CompanyEmails");

            entity.Property(e => e.EmailId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("EmailID");
            entity.Property(e => e.EmailAdress).HasMaxLength(350);
        });

        modelBuilder.Entity<TblContactInformation>(entity =>
        {
            entity.HasKey(e => e.ContactId);

            entity.ToTable("tbl_ContactInformations");

            entity.Property(e => e.ContactId).HasColumnName("ContactID");
            entity.Property(e => e.ContactCountry).HasMaxLength(100);
            entity.Property(e => e.ContactEmail).HasMaxLength(100);
            entity.Property(e => e.ContactPhoneNumber).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(350);
        });

        modelBuilder.Entity<TblDebatePerformance>(entity =>
        {
            entity.HasKey(e => e.PerformanceId);

            entity.ToTable("tbl_DebatePerformances");

            entity.Property(e => e.PerformanceId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PerformanceID");
        });

        modelBuilder.Entity<TblDebatePerformanceEventType>(entity =>
        {
            entity.ToTable("tbl_DebatePerformanceEventTypes");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.WorkPerformanceEventType).HasMaxLength(350);

            entity.HasOne(d => d.SubPerformance).WithMany(p => p.TblDebatePerformanceEventTypes)
                .HasForeignKey(d => d.SubPerformanceId)
                .HasConstraintName("FK_tbl_DebatePerformanceEventTypes_tbl_SubDebatePerformances");
        });

        modelBuilder.Entity<TblDebateWorkPerformanceReport>(entity =>
        {
            entity.HasKey(e => e.WorkPerformId);

            entity.ToTable("tbl_DebateWorkPerformanceReports");

            entity.Property(e => e.WorkPerformId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("WorkPerformID");
            entity.Property(e => e.Hivpositives).HasColumnName("HIVPositives");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MonthId).HasColumnName("MonthID");
            entity.Property(e => e.YearId).HasColumnName("YearID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblDebateWorkPerformanceReports)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_DebateWorkPerformanceReports_tbl_InternalUsers");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.TblDebateWorkPerformanceReports)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK_tbl_DebateWorkPerformanceReports_tbl_DebateWorkPerformanceReports");

            entity.HasOne(d => d.Month).WithMany(p => p.TblDebateWorkPerformanceReports)
                .HasForeignKey(d => d.MonthId)
                .HasConstraintName("FK_tbl_DebateWorkPerformanceReports_tbl_Months");

            entity.HasOne(d => d.SubPerformance).WithMany(p => p.TblDebateWorkPerformanceReports)
                .HasForeignKey(d => d.SubPerformanceId)
                .HasConstraintName("FK_tbl_DebateWorkPerformanceReports_tbl_SubDebatePerformances");

            entity.HasOne(d => d.Year).WithMany(p => p.TblDebateWorkPerformanceReports)
                .HasForeignKey(d => d.YearId)
                .HasConstraintName("FK_tbl_DebateWorkPerformanceReports_tbl_Years");
        });

        modelBuilder.Entity<TblDecisionStatus>(entity =>
        {
            entity.HasKey(e => e.DesStatusId);

            entity.ToTable("tbl_DecisionStatus");

            entity.Property(e => e.DesStatusId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.StatusDescription).HasMaxLength(350);
            entity.Property(e => e.StatusName).HasMaxLength(250);
            entity.Property(e => e.StatusNameAmharic).HasMaxLength(350);
        });

        modelBuilder.Entity<TblDepartment>(entity =>
        {
            entity.HasKey(e => e.DepId);

            entity.ToTable("tbl_Department");

            entity.Property(e => e.DepId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DepCode).HasMaxLength(50);
            entity.Property(e => e.DepName).HasMaxLength(300);
        });

        modelBuilder.Entity<TblDocumentHistory>(entity =>
        {
            entity.HasKey(e => e.HistoryId);

            entity.ToTable("tbl_DocumentHistories");

            entity.Property(e => e.HistoryId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("HistoryID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.InternalReplyId).HasColumnName("InternalReplyID");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");

            entity.HasOne(d => d.ExternalRepliedByNavigation).WithMany(p => p.TblDocumentHistories)
                .HasForeignKey(d => d.ExternalRepliedBy)
                .HasConstraintName("FK_tbl_DocumentHistories_tbl_ExternalUser");

            entity.HasOne(d => d.InternalReply).WithMany(p => p.TblDocumentHistories)
                .HasForeignKey(d => d.InternalReplyId)
                .HasConstraintName("FK_tbl_DocumentHistories_tbl_InternalUsers");

            entity.HasOne(d => d.Request).WithMany(p => p.TblDocumentHistories)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_DocumentHistories_tbl_Requests");
        });

        modelBuilder.Entity<TblDraftContractExaminationReport>(entity =>
        {
            entity.ToTable("tbl_DraftContractExaminationReport");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.MonthId).HasColumnName("MonthID");
            entity.Property(e => e.YearId).HasColumnName("YearID");

            entity.HasOne(d => d.Month).WithMany(p => p.TblDraftContractExaminationReports)
                .HasForeignKey(d => d.MonthId)
                .HasConstraintName("FK_tbl_DraftContractExaminationReport_tbl_Months");

            entity.HasOne(d => d.SubmittedByNavigation).WithMany(p => p.TblDraftContractExaminationReports)
                .HasForeignKey(d => d.SubmittedBy)
                .HasConstraintName("FK_tbl_DraftContractExaminationReport_tbl_InternalUsers");

            entity.HasOne(d => d.Year).WithMany(p => p.TblDraftContractExaminationReports)
                .HasForeignKey(d => d.YearId)
                .HasConstraintName("FK_tbl_DraftContractExaminationReport_tbl_Years");
        });

        modelBuilder.Entity<TblEvent>(entity =>
        {
            entity.HasKey(e => e.EventId);

            entity.ToTable("tbl_Events");

            entity.Property(e => e.EventId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("EventID");
            entity.Property(e => e.EventName).HasMaxLength(350);

            entity.HasOne(d => d.SubPerformance).WithMany(p => p.TblEvents)
                .HasForeignKey(d => d.SubPerformanceId)
                .HasConstraintName("FK_tbl_Events_tbl_SubDebatePerformances");
        });

        modelBuilder.Entity<TblExternalMainMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId);

            entity.ToTable("tbl_ExternalMainMenus");

            entity.Property(e => e.MenuId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("MenuID");
            entity.Property(e => e.ClassSvg).HasColumnName("Class_svg");
            entity.Property(e => e.MenuDescription).HasMaxLength(250);
            entity.Property(e => e.MenuName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblExternalRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId);

            entity.ToTable("tbl_ExternalRequests");

            entity.Property(e => e.RequestId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DepId).HasColumnName("DepID");
            entity.Property(e => e.ExterUserId).HasColumnName("ExterUserID");
            entity.Property(e => e.ExternalRequestStatusId).HasColumnName("ExternalRequestStatusID");
            entity.Property(e => e.IntId).HasColumnName("IntID");
            entity.Property(e => e.RequestedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Dep).WithMany(p => p.TblExternalRequests)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_tbl_ExternalRequests_tbl_Department");

            entity.HasOne(d => d.ExterUser).WithMany(p => p.TblExternalRequests)
                .HasForeignKey(d => d.ExterUserId)
                .HasConstraintName("FK_tbl_ExternalRequests_tbl_ExternalUser");

            entity.HasOne(d => d.ExternalRequestStatus).WithMany(p => p.TblExternalRequests)
                .HasForeignKey(d => d.ExternalRequestStatusId)
                .HasConstraintName("FK_tbl_ExternalRequests_tbl_ExternalRequestStatus");

            entity.HasOne(d => d.Int).WithMany(p => p.TblExternalRequests)
                .HasForeignKey(d => d.IntId)
                .HasConstraintName("FK_tbl_ExternalRequests_tbl_Inistitutions");
        });

        modelBuilder.Entity<TblExternalRequestStatus>(entity =>
        {
            entity.HasKey(e => e.ExternalRequestStatusId);

            entity.ToTable("tbl_ExternalRequestStatus");

            entity.Property(e => e.ExternalRequestStatusId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ExternalRequestStatusID");
            entity.Property(e => e.StatusName).HasMaxLength(250);
            entity.Property(e => e.StatusNameAmharic).HasMaxLength(250);
        });

        modelBuilder.Entity<TblExternalSubmenu>(entity =>
        {
            entity.ToTable("tbl_ExternalSubmenu");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.MenuId).HasColumnName("MenuID");

            entity.HasOne(d => d.Dep).WithMany(p => p.TblExternalSubmenus)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_tbl_ExternalSubmenu_tbl_Department");

            entity.HasOne(d => d.Menu).WithMany(p => p.TblExternalSubmenus)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK_tbl_ExternalSubmenu_tbl_ExternalMainMenus");
        });

        modelBuilder.Entity<TblExternalUser>(entity =>
        {
            entity.HasKey(e => e.ExterUserId);

            entity.ToTable("tbl_ExternalUser");

            entity.Property(e => e.ExterUserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ExterUserID");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(350);
            entity.Property(e => e.LastName).HasMaxLength(350);
            entity.Property(e => e.MiddleName).HasMaxLength(350);
            entity.Property(e => e.Password).HasMaxLength(350);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.UserName).HasMaxLength(250);

            entity.HasOne(d => d.Inist).WithMany(p => p.TblExternalUsers)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_ExternalUser_tbl_Inistitutions");
        });

        modelBuilder.Entity<TblFollowup>(entity =>
        {
            entity.HasKey(e => e.FollowUpId);

            entity.ToTable("tbl_Followups");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.FromExternal).HasColumnName("fromExternal");
            entity.Property(e => e.FromInternal).HasColumnName("fromInternal");
            entity.Property(e => e.InistId).HasColumnName("InistID");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");

            entity.HasOne(d => d.ExternalUser).WithMany(p => p.TblFollowups)
                .HasForeignKey(d => d.ExternalUserId)
                .HasConstraintName("FK_tbl_Followups_tbl_ExternalUser");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblFollowups)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_Followups_tbl_Inistitutions");

            entity.HasOne(d => d.Request).WithMany(p => p.TblFollowups)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK_tbl_Followups_tbl_Requests");

            entity.HasOne(d => d.User).WithMany(p => p.TblFollowups)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tbl_Followups_tbl_InternalUsers");
        });

        modelBuilder.Entity<TblInistitution>(entity =>
        {
            entity.HasKey(e => e.InistId);

            entity.ToTable("tbl_Inistitutions");

            entity.Property(e => e.InistId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description).HasMaxLength(350);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.NameAmharic).HasMaxLength(250);
        });

        modelBuilder.Entity<TblInspectionInstitution>(entity =>
        {
            entity.HasKey(e => e.SubMissionId);

            entity.ToTable("tbl_Inspection_Institutions");

            entity.Property(e => e.SubMissionId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ExpectedResponseDate).HasColumnType("datetime");
            entity.Property(e => e.RequestStatus).HasMaxLength(350);
            entity.Property(e => e.RequestedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Institution).WithMany(p => p.TblInspectionInstitutions)
                .HasForeignKey(d => d.InstitutionId)
                .HasConstraintName("FK_tbl_Inspection_Institutions_tbl_Inistitutions");

            entity.HasOne(d => d.ResponseStatus).WithMany(p => p.TblInspectionInstitutions)
                .HasForeignKey(d => d.ResponseStatusId)
                .HasConstraintName("FK_tbl_Inspection_Institutions_tbl_ReponseStatus");

            entity.HasOne(d => d.ReturnedByNavigation).WithMany(p => p.TblInspectionInstitutions)
                .HasForeignKey(d => d.ReturnedBy)
                .HasConstraintName("FK_tbl_Inspection_Institutions_tbl_ExternalUser");

            entity.HasOne(d => d.SubmittedByNavigation).WithMany(p => p.TblInspectionInstitutions)
                .HasForeignKey(d => d.SubmittedBy)
                .HasConstraintName("FK_tbl_Inspection_Institutions_tbl_InternalUsers");
        });

        modelBuilder.Entity<TblInspectionLaw>(entity =>
        {
            entity.HasKey(e => e.LawId);

            entity.ToTable("tbl_InspectionLaws");

            entity.Property(e => e.LawId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TblInspectionPlan>(entity =>
        {
            entity.HasKey(e => e.InspectionPlanId);

            entity.ToTable("tbl_InspectionPlans");

            entity.Property(e => e.InspectionPlanId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AssigneeTypeId).HasColumnName("AssigneeTypeID");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.ReturnDate)
                .HasColumnType("datetime")
                .HasColumnName("returnDate");
            entity.Property(e => e.ReturningRemark).HasColumnName("returningRemark");
            entity.Property(e => e.SentDate)
                .HasColumnType("datetime")
                .HasColumnName("sentDate");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.YearId).HasColumnName("YearID");

            entity.HasOne(d => d.AssigneeType).WithMany(p => p.TblInspectionPlans)
                .HasForeignKey(d => d.AssigneeTypeId)
                .HasConstraintName("FK_tbl_InspectionPlans_tbl_AssignementTypes");

            entity.HasOne(d => d.IsUpprovedbyDepartmentNavigation).WithMany(p => p.TblInspectionPlanIsUpprovedbyDepartmentNavigations)
                .HasForeignKey(d => d.IsUpprovedbyDepartment)
                .HasConstraintName("FK_tbl_InspectionPlans_tbl_DecisionStatus");

            entity.HasOne(d => d.IsUpprovedbyTeamNavigation).WithMany(p => p.TblInspectionPlanIsUpprovedbyTeamNavigations)
                .HasForeignKey(d => d.IsUpprovedbyTeam)
                .HasConstraintName("FK_tbl_InspectionPlans_tbl_DecisionStatus1");

            entity.HasOne(d => d.IsUprovedByDeputyNavigation).WithMany(p => p.TblInspectionPlanIsUprovedByDeputyNavigations)
                .HasForeignKey(d => d.IsUprovedByDeputy)
                .HasConstraintName("FK_tbl_InspectionPlans_tbl_DecisionStatus2");

            entity.HasOne(d => d.IsUserUprovedNavigation).WithMany(p => p.TblInspectionPlanIsUserUprovedNavigations)
                .HasForeignKey(d => d.IsUserUproved)
                .HasConstraintName("FK_tbl_InspectionPlans_tbl_DecisionStatus3");

            entity.HasOne(d => d.Pro).WithMany(p => p.TblInspectionPlans)
                .HasForeignKey(d => d.ProId)
                .HasConstraintName("FK_tbl_Progress_tbl_InspectionPlans");

            entity.HasOne(d => d.Team).WithMany(p => p.TblInspectionPlans)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_tbl_InspectionPlans_tbl_Teams");

            entity.HasOne(d => d.User).WithMany(p => p.TblInspectionPlans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tbl_InspectionPlans_tbl_InternalUsers");

            entity.HasOne(d => d.Year).WithMany(p => p.TblInspectionPlans)
                .HasForeignKey(d => d.YearId)
                .HasConstraintName("FK_tbl_Years_tbl_InspectionPlans");
        });

        modelBuilder.Entity<TblInspectionReplye>(entity =>
        {
            entity.HasKey(e => e.ReplyId);

            entity.ToTable("tbl_InspectionReplyes");

            entity.Property(e => e.DateCreated).HasColumnType("datetime");

            entity.HasOne(d => d.ExternalUserNavigation).WithMany(p => p.TblInspectionReplyes)
                .HasForeignKey(d => d.ExternalUser)
                .HasConstraintName("FK_tbl_InspectionReplyes_tbl_ExternalUser");

            entity.HasOne(d => d.InternalUserNavigation).WithMany(p => p.TblInspectionReplyes)
                .HasForeignKey(d => d.InternalUser)
                .HasConstraintName("FK_tbl_InspectionReplyes_tbl_InternalUsers1");

            entity.HasOne(d => d.Rec).WithMany(p => p.TblInspectionReplyes)
                .HasForeignKey(d => d.RecId)
                .HasConstraintName("FK_tbl_InspectionReplyes_tbl_SentInspections");
        });

        modelBuilder.Entity<TblInspectionStatus>(entity =>
        {
            entity.HasKey(e => e.ProId);

            entity.ToTable("tbl_InspectionStatus");

            entity.Property(e => e.ProId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TblInstotutionMonitoringReport>(entity =>
        {
            entity.ToTable("tbl_InstotutionMonitoringReports");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.AdrmoneyAmount).HasColumnName("ADRMoneyAmount");
            entity.Property(e => e.Adrnumber).HasColumnName("ADRNumber");
            entity.Property(e => e.MonthId).HasColumnName("MonthID");
            entity.Property(e => e.YearId).HasColumnName("YearID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblInstotutionMonitoringReports)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_InstotutionMonitoringReports_tbl_InternalUsers");

            entity.HasOne(d => d.Month).WithMany(p => p.TblInstotutionMonitoringReports)
                .HasForeignKey(d => d.MonthId)
                .HasConstraintName("FK_tbl_InstotutionMonitoringReports_tbl_Months");

            entity.HasOne(d => d.Year).WithMany(p => p.TblInstotutionMonitoringReports)
                .HasForeignKey(d => d.YearId)
                .HasConstraintName("FK_tbl_InstotutionMonitoringReports_tbl_Years");
        });

        modelBuilder.Entity<TblInternalUser>(entity =>
        {
            entity.HasKey(e => e.UserId);

            entity.ToTable("tbl_InternalUsers");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UserID");
            entity.Property(e => e.EmailAddress).HasMaxLength(250);
            entity.Property(e => e.FirstName).HasMaxLength(250);
            entity.Property(e => e.LastName).HasMaxLength(250);
            entity.Property(e => e.MidleName).HasMaxLength(250);
            entity.Property(e => e.Password).HasMaxLength(400);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.TeamId).HasColumnName("TeamID");
            entity.Property(e => e.UserName).HasMaxLength(250);

            entity.HasOne(d => d.Dep).WithMany(p => p.TblInternalUsers)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_tbl_InternalUsers_tbl_Department");

            entity.HasOne(d => d.Team).WithMany(p => p.TblInternalUsers)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_tbl_InternalUsers_tbl_Teams");
        });

        modelBuilder.Entity<TblLanguage>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tbl_Languages");

            entity.Property(e => e.LangId)
                .ValueGeneratedOnAdd()
                .HasColumnName("LangID");
        });

        modelBuilder.Entity<TblLegalAdviceReport>(entity =>
        {
            entity.HasKey(e => e.ReportId);

            entity.ToTable("tbl_LegalAdviceReports");

            entity.Property(e => e.ReportId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Total).HasMaxLength(50);

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.TblLegalAdviceReports)
                .HasForeignKey(d => d.Id)
                .HasConstraintName("FK_tbl_LegalAdviceReports_tbl_LegalAdviceServantTypes");

            entity.HasOne(d => d.MonthNavigation).WithMany(p => p.TblLegalAdviceReports)
                .HasForeignKey(d => d.Month)
                .HasConstraintName("FK_tbl_LegalAdviceReports_tbl_Months");

            entity.HasOne(d => d.ReportedByNavigation).WithMany(p => p.TblLegalAdviceReports)
                .HasForeignKey(d => d.ReportedBy)
                .HasConstraintName("FK_tbl_LegalAdviceReports_tbl_LegalAdviceReports");

            entity.HasOne(d => d.YearNavigation).WithMany(p => p.TblLegalAdviceReports)
                .HasForeignKey(d => d.Year)
                .HasConstraintName("FK_tbl_LegalAdviceReports_tbl_Years");
        });

        modelBuilder.Entity<TblLegalAdviceServantType>(entity =>
        {
            entity.ToTable("tbl_LegalAdviceServantTypes");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.ServantTypeName).HasMaxLength(150);
        });

        modelBuilder.Entity<TblLegalDraftingDocType>(entity =>
        {
            entity.HasKey(e => e.DocId);

            entity.ToTable("tbl_LegalDraftingDocType");

            entity.Property(e => e.DocId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("DocID");
            entity.Property(e => e.DocName).HasMaxLength(300);
        });

        modelBuilder.Entity<TblLegalDraftingQuestionType>(entity =>
        {
            entity.HasKey(e => e.QuestTypeId);

            entity.ToTable("tbl_LegalDraftingQuestionType");

            entity.Property(e => e.QuestTypeId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("QuestTypeID");
            entity.Property(e => e.QuestTypeDescription).HasMaxLength(350);
            entity.Property(e => e.QuestTypeName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblLegalStudiesActivity>(entity =>
        {
            entity.HasKey(e => e.ActivityId);

            entity.ToTable("tbl_LegalStudiesActivity");

            entity.Property(e => e.ActivityId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ActivityID");
            entity.Property(e => e.AddedDate).HasColumnType("datetime");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblLegalStudiesActivities)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_LegalStudiesActivity_tbl_InternalUsers");

            entity.HasOne(d => d.Request).WithMany(p => p.TblLegalStudiesActivities)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK_tbl_LegalStudiesActivity_tbl_LegalStudiesDrafting");
        });

        modelBuilder.Entity<TblLegalStudiesDrafting>(entity =>
        {
            entity.HasKey(e => e.RequestId);

            entity.ToTable("tbl_LegalStudiesDrafting");

            entity.Property(e => e.RequestId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("RequestID");
            entity.Property(e => e.AssignedDate).HasColumnType("datetime");
            entity.Property(e => e.CaseTypeId).HasColumnName("CaseTypeID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DocId).HasColumnName("DocID");
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.ExternalRequestStatusId).HasColumnName("ExternalRequestStatusID");
            entity.Property(e => e.QuestTypeId).HasColumnName("QuestTypeID");
            entity.Property(e => e.TopStatus).HasMaxLength(250);

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.TblLegalStudiesDraftingAssignedByNavigations)
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_InternalUsers1");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TblLegalStudiesDraftingAssignedToNavigations)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_InternalUsers");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblLegalStudiesDraftingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_InternalUsers2");

            entity.HasOne(d => d.Dep).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_Department");

            entity.HasOne(d => d.DepartmentUpprovalStatusNavigation).WithMany(p => p.TblLegalStudiesDraftingDepartmentUpprovalStatusNavigations)
                .HasForeignKey(d => d.DepartmentUpprovalStatus)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_DecisionStatus3");

            entity.HasOne(d => d.DeputyUprovalStatusNavigation).WithMany(p => p.TblLegalStudiesDraftingDeputyUprovalStatusNavigations)
                .HasForeignKey(d => d.DeputyUprovalStatus)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_DecisionStatus2");

            entity.HasOne(d => d.Doc).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.DocId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_LegalDraftingDocType");

            entity.HasOne(d => d.ExternalRequestStatus).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.ExternalRequestStatusId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_ExternalRequestStatus");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_Inistitutions");

            entity.HasOne(d => d.Priority).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.PriorityId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_Priority");

            entity.HasOne(d => d.QuestType).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.QuestTypeId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_LegalDraftingQuestionType");

            entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.RequestedBy)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_ExternalUser");

            entity.HasOne(d => d.TeamUpprovalStatusNavigation).WithMany(p => p.TblLegalStudiesDraftingTeamUpprovalStatusNavigations)
                .HasForeignKey(d => d.TeamUpprovalStatus)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_DecisionStatus");

            entity.HasOne(d => d.UserUpprovalStatusNavigation).WithMany(p => p.TblLegalStudiesDraftingUserUpprovalStatusNavigations)
                .HasForeignKey(d => d.UserUpprovalStatus)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_DecisionStatus1");
        });

        modelBuilder.Entity<TblLegalStudiesReplay>(entity =>
        {
            entity.HasKey(e => e.ReplyId);

            entity.ToTable("tbl_LegalStudiesReplays");

            entity.Property(e => e.ReplyId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.ReplyDate).HasColumnType("datetime");

            entity.HasOne(d => d.ExternalReplayedByNavigation).WithMany(p => p.TblLegalStudiesReplays)
                .HasForeignKey(d => d.ExternalReplayedBy)
                .HasConstraintName("FK_tbl_LegalStudiesReplays_tbl_ExternalUser");

            entity.HasOne(d => d.ExternalReplayedBy1).WithMany(p => p.TblLegalStudiesReplays)
                .HasForeignKey(d => d.ExternalReplayedBy)
                .HasConstraintName("FK_tbl_LegalStudiesReplays_tbl_InternalUsers");

            entity.HasOne(d => d.Request).WithMany(p => p.TblLegalStudiesReplays)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK_tbl_LegalStudiesReplays_tbl_LegalStudiesDrafting");
        });

        modelBuilder.Entity<TblMainMenu>(entity =>
        {
            entity.HasKey(e => e.MenuId);

            entity.ToTable("tbl_MainMenu");

            entity.Property(e => e.MenuId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("MenuID");
            entity.Property(e => e.ClassSvg).HasColumnName("Class_svg");
            entity.Property(e => e.MenuDescription).HasMaxLength(250);
            entity.Property(e => e.MenuName).HasMaxLength(250);
            entity.Property(e => e.MenuNameAmharic).HasMaxLength(300);
        });

        modelBuilder.Entity<TblMonth>(entity =>
        {
            entity.HasKey(e => e.MonthId);

            entity.ToTable("tbl_Months");

            entity.Property(e => e.MonthId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("MonthID");
        });

        modelBuilder.Entity<TblNotification>(entity =>
        {
            entity.HasKey(e => e.NotificationId);

            entity.ToTable("tbl_Notifications");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.NotificationDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblNotificationCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_Notifications_tbl_InternalUsers1");

            entity.HasOne(d => d.User).WithMany(p => p.TblNotificationUsers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tbl_Notifications_tbl_InternalUsers");
        });

        modelBuilder.Entity<TblPlanCatagory>(entity =>
        {
            entity.HasKey(e => e.PlanCatId);

            entity.ToTable("tbl_PlanCatagory");

            entity.HasOne(d => d.InspectionPlan).WithMany(p => p.TblPlanCatagories)
                .HasForeignKey(d => d.InspectionPlanId)
                .HasConstraintName("FK_tbl_PlanCatagory_tbl_InspectionPlans");
        });

        modelBuilder.Entity<TblPlanInistitution>(entity =>
        {
            entity.ToTable("tbl_Plan_Inistitution");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblPlanInistitutions)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_Plan_Inistitution_tbl_Inistitutions");

            entity.HasOne(d => d.SpecificPlan).WithMany(p => p.TblPlanInistitutions)
                .HasForeignKey(d => d.SpecificPlanId)
                .HasConstraintName("FK_tbl_Plan_Inistitution_tbl_SpecificPlans");
        });

        modelBuilder.Entity<TblPriority>(entity =>
        {
            entity.HasKey(e => e.PriorityId);

            entity.ToTable("tbl_Priority");

            entity.Property(e => e.PriorityId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PriorityName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblPriorityQuestion>(entity =>
        {
            entity.HasKey(e => e.PriorityQueId);

            entity.ToTable("tbl_PriorityQuestions");

            entity.Property(e => e.PriorityQueId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TblRecomendation>(entity =>
        {
            entity.HasKey(e => e.RecoId);

            entity.ToTable("tbl_Recomendation");

            entity.Property(e => e.RecoId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatinDate).HasColumnType("datetime");
            entity.Property(e => e.ModifyDate).HasColumnType("datetime");
            entity.Property(e => e.RecostatusId).HasColumnName("RecostatusID");
            entity.Property(e => e.YearId).HasColumnName("YearID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblRecomendations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_Recomendation_tbl_InternalUsers");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblRecomendations)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_Recomendation_tbl_Inistitutions");

            entity.HasOne(d => d.Law).WithMany(p => p.TblRecomendations)
                .HasForeignKey(d => d.LawId)
                .HasConstraintName("FK_tbl_Recomendation_tbl_InspectionLaws");

            entity.HasOne(d => d.Recostatus).WithMany(p => p.TblRecomendations)
                .HasForeignKey(d => d.RecostatusId)
                .HasConstraintName("FK_tbl_Recomendation_tbl_Recomendation2");

            entity.HasOne(d => d.Year).WithMany(p => p.TblRecomendations)
                .HasForeignKey(d => d.YearId)
                .HasConstraintName("FK_tbl_Recomendation_tbl_Years");
        });

        modelBuilder.Entity<TblRecomendationStatus>(entity =>
        {
            entity.HasKey(e => e.RecostatusId);

            entity.ToTable("tbl_RecomendationStatus");

            entity.Property(e => e.RecostatusId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("RecostatusID");
            entity.Property(e => e.Status).HasMaxLength(250);
        });

        modelBuilder.Entity<TblReplay>(entity =>
        {
            entity.HasKey(e => e.ReplyId);

            entity.ToTable("tbl_Replays");

            entity.Property(e => e.ReplyId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ReplyID");
            entity.Property(e => e.ReplyDate).HasColumnType("datetime");

            entity.HasOne(d => d.ExternalReplayedByNavigation).WithMany(p => p.TblReplays)
                .HasForeignKey(d => d.ExternalReplayedBy)
                .HasConstraintName("FK_tbl_Replays_tbl_ExternalUser");

            entity.HasOne(d => d.InternalReplayedByNavigation).WithMany(p => p.TblReplays)
                .HasForeignKey(d => d.InternalReplayedBy)
                .HasConstraintName("FK_tbl_Replays_tbl_InternalUsers");

            entity.HasOne(d => d.Request).WithMany(p => p.TblReplays)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Replays_tbl_Requests");
        });

        modelBuilder.Entity<TblReponseStatus>(entity =>
        {
            entity.HasKey(e => e.ResponseStatusId);

            entity.ToTable("tbl_ReponseStatus");

            entity.Property(e => e.ResponseStatusId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblReportServiceType>(entity =>
        {
            entity.ToTable("tbl_ReportServiceTypes");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
        });

        modelBuilder.Entity<TblRequest>(entity =>
        {
            entity.HasKey(e => e.RequestId);

            entity.ToTable("tbl_Requests");

            entity.Property(e => e.RequestId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("RequestID");
            entity.Property(e => e.ActingAs).HasMaxLength(300);
            entity.Property(e => e.Adrtype)
                .HasMaxLength(300)
                .HasColumnName("ADRType");
            entity.Property(e => e.AssignedDate).HasColumnType("datetime");
            entity.Property(e => e.Bench).HasMaxLength(400);
            entity.Property(e => e.CaseTypeId).HasColumnName("CaseTypeID");
            entity.Property(e => e.Claimant).HasMaxLength(300);
            entity.Property(e => e.Country).HasMaxLength(400);
            entity.Property(e => e.CourtCenter).HasMaxLength(350);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DateOfAdjournment).HasColumnType("date");
            entity.Property(e => e.DateofJudgement).HasColumnType("date");
            entity.Property(e => e.Defendent).HasMaxLength(400);
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(300);
            entity.Property(e => e.ExternalRequestStatusId).HasColumnName("ExternalRequestStatusID");
            entity.Property(e => e.FullName).HasMaxLength(300);
            entity.Property(e => e.Jursidiction).HasMaxLength(400);
            entity.Property(e => e.LitigationType).HasMaxLength(350);
            entity.Property(e => e.MoneyCurrency).HasMaxLength(50);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
            entity.Property(e => e.Plaintiful).HasMaxLength(500);
            entity.Property(e => e.QuestTypeId).HasColumnName("QuestTypeID");
            entity.Property(e => e.Reasult).HasMaxLength(300);
            entity.Property(e => e.Respondent).HasMaxLength(300);
            entity.Property(e => e.ReturningRemark).HasColumnName("returningRemark");
            entity.Property(e => e.SentDate)
                .HasColumnType("date")
                .HasColumnName("sentDate");
            entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");
            entity.Property(e => e.TopStatusId).HasColumnName("TopStatusID");

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.TblRequestAssignedByNavigations)
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK_tbl_Requests_tbl_InternalUsers1");

            entity.HasOne(d => d.CaseTypeNavigation).WithMany(p => p.TblRequests)
                .HasForeignKey(d => d.CaseTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Requests_tbl_CivilJusticeCaseType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblRequestCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_Requests_tbl_InternalUsers");

            entity.HasOne(d => d.DepartmentUpprovalStatusNavigation).WithMany(p => p.TblRequestDepartmentUpprovalStatusNavigations)
                .HasForeignKey(d => d.DepartmentUpprovalStatus)
                .HasConstraintName("FK_tbl_Requests_tbl_DecisionStatus2");

            entity.HasOne(d => d.DeputyUprovalStatusNavigation).WithMany(p => p.TblRequestDeputyUprovalStatusNavigations)
                .HasForeignKey(d => d.DeputyUprovalStatus)
                .HasConstraintName("FK_tbl_Requests_tbl_DecisionStatus3");

            entity.HasOne(d => d.DocType).WithMany(p => p.TblRequests)
                .HasForeignKey(d => d.DocTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Requests_tbl_LegalDraftingDocType");

            entity.HasOne(d => d.ExternalRequestStatus).WithMany(p => p.TblRequests)
                .HasForeignKey(d => d.ExternalRequestStatusId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Requests_tbl_ExternalRequestStatus");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblRequests)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_Requests_tbl_Inistitutions");

            entity.HasOne(d => d.Priority).WithMany(p => p.TblRequests)
                .HasForeignKey(d => d.PriorityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Requests_tbl_Priority");

            entity.HasOne(d => d.QuestType).WithMany(p => p.TblRequests)
                .HasForeignKey(d => d.QuestTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Requests_tbl_LegalDraftingQuestionType");

            entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.TblRequests)
                .HasForeignKey(d => d.RequestedBy)
                .HasConstraintName("FK_tbl_Requests_tbl_ExternalUser");

            entity.HasOne(d => d.ServiceType).WithMany(p => p.TblRequests)
                .HasForeignKey(d => d.ServiceTypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Requests_tbl_ServiceTypes");

            entity.HasOne(d => d.TeamUpprovalStatusNavigation).WithMany(p => p.TblRequestTeamUpprovalStatusNavigations)
                .HasForeignKey(d => d.TeamUpprovalStatus)
                .HasConstraintName("FK_tbl_Requests_tbl_DecisionStatus1");

            entity.HasOne(d => d.TopStatus).WithMany(p => p.TblRequests)
                .HasForeignKey(d => d.TopStatusId)
                .HasConstraintName("FK_tbl_Requests_tbl_Requests");

            entity.HasOne(d => d.Type).WithMany(p => p.TblRequests)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Requests_tbl_RequestAssignementTypes");

            entity.HasOne(d => d.UserUpprovalStatusNavigation).WithMany(p => p.TblRequestUserUpprovalStatusNavigations)
                .HasForeignKey(d => d.UserUpprovalStatus)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Requests_tbl_DecisionStatus");
        });

        modelBuilder.Entity<TblRequestAssignee>(entity =>
        {
            entity.HasKey(e => e.AssigneeRequestId);

            entity.ToTable("tbl_RequestAssignees");

            entity.Property(e => e.AssigneeRequestId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Request).WithMany(p => p.TblRequestAssignees)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_RequestAssignees_tbl_Requests");

            entity.HasOne(d => d.User).WithMany(p => p.TblRequestAssignees)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tbl_RequestAssignees_tbl_InternalUsers");
        });

        modelBuilder.Entity<TblRequestAssignementType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("tbl_RequestAssignementTypes");

            entity.Property(e => e.TypeId).HasDefaultValueSql("(newid())");
        });

        modelBuilder.Entity<TblRequestDepartmentRelation>(entity =>
        {
            entity.ToTable("TBL_RequestDepartmentRelations");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AssigneeTypeId).HasColumnName("AssigneeTypeID");
            entity.Property(e => e.DepId).HasColumnName("DepID");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.AssigneeType).WithMany(p => p.TblRequestDepartmentRelations)
                .HasForeignKey(d => d.AssigneeTypeId)
                .HasConstraintName("FK_TBL_RequestDepartmentRelations_tbl_AssignementTypes");

            entity.HasOne(d => d.Dep).WithMany(p => p.TblRequestDepartmentRelations)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_TBL_RequestDepartmentRelations_tbl_Department");

            entity.HasOne(d => d.Request).WithMany(p => p.TblRequestDepartmentRelations)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TBL_RequestDepartmentRelations_tbl_Requests");

            entity.HasOne(d => d.Team).WithMany(p => p.TblRequestDepartmentRelations)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_TBL_RequestDepartmentRelations_tbl_Teams");
        });

        modelBuilder.Entity<TblRequestPriorityQuestionsRelation>(entity =>
        {
            entity.ToTable("tbl_RequestPriorityQuestionsRelations");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");

            entity.HasOne(d => d.PriorityQue).WithMany(p => p.TblRequestPriorityQuestionsRelations)
                .HasForeignKey(d => d.PriorityQueId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_RequestPriorityQuestionsRelations_tbl_PriorityQuestions");

            entity.HasOne(d => d.Request).WithMany(p => p.TblRequestPriorityQuestionsRelations)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_RequestPriorityQuestionsRelations_tbl_Requests");
        });

        modelBuilder.Entity<TblRequestType>(entity =>
        {
            entity.HasKey(e => e.TypeId);

            entity.ToTable("tbl_RequestTypes");

            entity.Property(e => e.TypeId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("TypeID");
            entity.Property(e => e.TypeName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("tbl_Roles");

            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("RoleID");
        });

        modelBuilder.Entity<TblRound>(entity =>
        {
            entity.HasKey(e => e.RoundId);

            entity.ToTable("tbl_Rounds");

            entity.Property(e => e.RoundId).HasColumnName("RoundID");
            entity.Property(e => e.RequestIid).HasColumnName("RequestIID");

            entity.HasOne(d => d.RequestI).WithMany(p => p.TblRounds)
                .HasForeignKey(d => d.RequestIid)
                .HasConstraintName("FK_tbl_Rounds_tbl_Requests");
        });

        modelBuilder.Entity<TblSentInspection>(entity =>
        {
            entity.HasKey(e => e.RecId);

            entity.ToTable("tbl_SentInspections");

            entity.Property(e => e.RecId).HasColumnName("RecID");
            entity.Property(e => e.ExpectedReplyDate).HasColumnType("date");
            entity.Property(e => e.InstId).HasColumnName("InstID");
            entity.Property(e => e.RespondedDate).HasColumnType("datetime");
            entity.Property(e => e.SentDate).HasColumnType("datetime");

            entity.HasOne(d => d.InspectionPlan).WithMany(p => p.TblSentInspections)
                .HasForeignKey(d => d.InspectionPlanId)
                .HasConstraintName("FK_tbl_SentInspections_tbl_InspectionPlans");

            entity.HasOne(d => d.Inst).WithMany(p => p.TblSentInspections)
                .HasForeignKey(d => d.InstId)
                .HasConstraintName("FK_tbl_SentInspections_tbl_Inistitutions");

            entity.HasOne(d => d.RepliedByNavigation).WithMany(p => p.TblSentInspections)
                .HasForeignKey(d => d.RepliedBy)
                .HasConstraintName("FK_tbl_SentInspections_tbl_ExternalUser");

            entity.HasOne(d => d.SentByNavigation).WithMany(p => p.TblSentInspections)
                .HasForeignKey(d => d.SentBy)
                .HasConstraintName("FK_tbl_SentInspections_tbl_InternalUsers");
        });

        modelBuilder.Entity<TblServiceType>(entity =>
        {
            entity.HasKey(e => e.ServiceTypeId);

            entity.ToTable("tbl_ServiceTypes");

            entity.Property(e => e.ServiceTypeId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ServiceTypeID");
            entity.Property(e => e.DepId).HasColumnName("DepID");
            entity.Property(e => e.ServiceTypeName).HasMaxLength(350);

            entity.HasOne(d => d.Dep).WithMany(p => p.TblServiceTypes)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_tbl_ServiceTypes_tbl_Department");
        });

        modelBuilder.Entity<TblSpecificPlan>(entity =>
        {
            entity.HasKey(e => e.SpecificPlanId);

            entity.ToTable("tbl_SpecificPlans");

            entity.Property(e => e.SpecificPlanId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.AssigneeTypeId).HasColumnName("AssigneeTypeID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.TeamId).HasColumnName("TeamID");

            entity.HasOne(d => d.AssigneeType).WithMany(p => p.TblSpecificPlans)
                .HasForeignKey(d => d.AssigneeTypeId)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_AssignementTypes");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblSpecificPlans)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_InternalUsers");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblSpecificPlans)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_Inistitutions");

            entity.HasOne(d => d.InspectionPlan).WithMany(p => p.TblSpecificPlans)
                .HasForeignKey(d => d.InspectionPlanId)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_InspectionPlans");

            entity.HasOne(d => d.IsUpprovedbyDepartmentNavigation).WithMany(p => p.TblSpecificPlanIsUpprovedbyDepartmentNavigations)
                .HasForeignKey(d => d.IsUpprovedbyDepartment)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_DecisionStatus");

            entity.HasOne(d => d.IsUpprovedbyTeamNavigation).WithMany(p => p.TblSpecificPlanIsUpprovedbyTeamNavigations)
                .HasForeignKey(d => d.IsUpprovedbyTeam)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_DecisionStatus1");

            entity.HasOne(d => d.IsUprovedByDeputyNavigation).WithMany(p => p.TblSpecificPlanIsUprovedByDeputyNavigations)
                .HasForeignKey(d => d.IsUprovedByDeputy)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_DecisionStatus2");

            entity.HasOne(d => d.IsUserUprovedNavigation).WithMany(p => p.TblSpecificPlanIsUserUprovedNavigations)
                .HasForeignKey(d => d.IsUserUproved)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_DecisionStatus3");

            entity.HasOne(d => d.PlanCat).WithMany(p => p.TblSpecificPlans)
                .HasForeignKey(d => d.PlanCatId)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_PlanCatagory");

            entity.HasOne(d => d.Pro).WithMany(p => p.TblSpecificPlans)
                .HasForeignKey(d => d.ProId)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_InspectionStatus");

            entity.HasOne(d => d.Team).WithMany(p => p.TblSpecificPlans)
                .HasForeignKey(d => d.TeamId)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_Teams");
        });

        modelBuilder.Entity<TblStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK_tbl_Status_1");

            entity.ToTable("tbl_Status");

            entity.Property(e => e.StatusId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("StatusID");
            entity.Property(e => e.Status).HasMaxLength(300);
        });

        modelBuilder.Entity<TblSubDebatePerformance>(entity =>
        {
            entity.HasKey(e => e.SubPerformanceId);

            entity.ToTable("tbl_SubDebatePerformances");

            entity.Property(e => e.SubPerformanceId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PerformanceId).HasColumnName("PerformanceID");

            entity.HasOne(d => d.Performance).WithMany(p => p.TblSubDebatePerformances)
                .HasForeignKey(d => d.PerformanceId)
                .HasConstraintName("FK_tbl_SubDebatePerformances_tbl_DebatePerformances");
        });

        modelBuilder.Entity<TblSubmenu>(entity =>
        {
            entity.ToTable("tbl_Submenu");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ID");
            entity.Property(e => e.MenuId).HasColumnName("MenuID");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Dep).WithMany(p => p.TblSubmenus)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_tbl_Submenu_tbl_Department");

            entity.HasOne(d => d.Menu).WithMany(p => p.TblSubmenus)
                .HasForeignKey(d => d.MenuId)
                .HasConstraintName("FK_tbl_Submenu_tbl_MainMenu");

            entity.HasOne(d => d.Role).WithMany(p => p.TblSubmenus)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_tbl_Submenu_tbl_Roles");
        });

        modelBuilder.Entity<TblTeam>(entity =>
        {
            entity.HasKey(e => e.TeamId);

            entity.ToTable("tbl_Teams");

            entity.Property(e => e.TeamId)
                .ValueGeneratedNever()
                .HasColumnName("TeamID");
            entity.Property(e => e.DepId).HasColumnName("DepID");
            entity.Property(e => e.TeamLeaderId).HasColumnName("TeamLeaderID");
            entity.Property(e => e.TeamName).HasMaxLength(250);

            entity.HasOne(d => d.Dep).WithMany(p => p.TblTeams)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_tbl_Teams_tbl_Department");

            entity.HasOne(d => d.TeamLeader).WithMany(p => p.TblTeams)
                .HasForeignKey(d => d.TeamLeaderId)
                .HasConstraintName("FK_tbl_Teams_tbl_InternalUsers");
        });

        modelBuilder.Entity<TblTermsAndCondition>(entity =>
        {
            entity.HasKey(e => e.TermsId);

            entity.ToTable("tbl_TermsAndConditions");

            entity.Property(e => e.TermsId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("TermsID");
        });

        modelBuilder.Entity<TblTopStatus>(entity =>
        {
            entity.HasKey(e => e.TopStatusId);

            entity.ToTable("tbl_TopStatus");

            entity.Property(e => e.TopStatusId).HasColumnName("TopStatusID");
            entity.Property(e => e.StatusName).HasMaxLength(350);
        });

        modelBuilder.Entity<TblWitnessEvidence>(entity =>
        {
            entity.HasKey(e => e.WitnessId);

            entity.ToTable("tbl_Witness_Evidences");

            entity.Property(e => e.WitnessId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("WitnessID");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblWitnessEvidences)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_Witness_Evidences_tbl_InternalUsers");

            entity.HasOne(d => d.Request).WithMany(p => p.TblWitnessEvidences)
                .HasForeignKey(d => d.RequestId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Witness_Evidences_tbl_Requests");
        });

        modelBuilder.Entity<TblYear>(entity =>
        {
            entity.HasKey(e => e.YearId);

            entity.ToTable("tbl_Years");

            entity.Property(e => e.YearId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("YearID");
            entity.Property(e => e.Year).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
