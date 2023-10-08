using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ATSManagement.Models;

public partial class AtsdbContext : DbContext
{
    public AtsdbContext()
    {
    }

    public AtsdbContext(DbContextOptions<AtsdbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; }

    public virtual DbSet<TblCivilJustice> TblCivilJustices { get; set; }

    public virtual DbSet<TblCivilJusticeCaseType> TblCivilJusticeCaseTypes { get; set; }

    public virtual DbSet<TblCivilJusticeRequestActivity> TblCivilJusticeRequestActivities { get; set; }

    public virtual DbSet<TblCivilJusticeRequestReply> TblCivilJusticeRequestReplys { get; set; }

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblExternalMainMenu> TblExternalMainMenus { get; set; }

    public virtual DbSet<TblExternalRequest> TblExternalRequests { get; set; }

    public virtual DbSet<TblExternalRequestStatus> TblExternalRequestStatuses { get; set; }

    public virtual DbSet<TblExternalSubmenu> TblExternalSubmenus { get; set; }

    public virtual DbSet<TblExternalUser> TblExternalUsers { get; set; }

    public virtual DbSet<TblInistitution> TblInistitutions { get; set; }

    public virtual DbSet<TblInspectionInstitution> TblInspectionInstitutions { get; set; }

    public virtual DbSet<TblInspectionPlan> TblInspectionPlans { get; set; }

    public virtual DbSet<TblInspectionStatus> TblInspectionStatuses { get; set; }

    public virtual DbSet<TblInternalUser> TblInternalUsers { get; set; }

    public virtual DbSet<TblLegalDraftingDocType> TblLegalDraftingDocTypes { get; set; }

    public virtual DbSet<TblLegalDraftingQuestionType> TblLegalDraftingQuestionTypes { get; set; }

    public virtual DbSet<TblLegalStudiesActivity> TblLegalStudiesActivities { get; set; }

    public virtual DbSet<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; }

    public virtual DbSet<TblLegalStudiesReplay> TblLegalStudiesReplays { get; set; }

    public virtual DbSet<TblMainMenu> TblMainMenus { get; set; }

    public virtual DbSet<TblPlanInistitution> TblPlanInistitutions { get; set; }

    public virtual DbSet<TblPriority> TblPriorities { get; set; }

    public virtual DbSet<TblRecomendation> TblRecomendations { get; set; }

    public virtual DbSet<TblRecomendationStatus> TblRecomendationStatuses { get; set; }

    public virtual DbSet<TblReponseStatus> TblReponseStatuses { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblSpecificPlan> TblSpecificPlans { get; set; }

    public virtual DbSet<TblStatus> TblStatuses { get; set; }

    public virtual DbSet<TblSubmenu> TblSubmenus { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-12IJ13A;Database=ATSDB;User ID=sa;Password=pass;Integrated Security=True; Trusted_Connection=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

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

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TblAssignedYearlyPlans)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK_tbl_AssignedYearlyPlans_tbl_InternalUsers");

            entity.HasOne(d => d.Plan).WithMany(p => p.TblAssignedYearlyPlans)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_AssignedYearlyPlans_tbl_InspectionPlans");

            entity.HasOne(d => d.Status).WithMany(p => p.TblAssignedYearlyPlans)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_tbl_AssignedYearlyPlans_tbl_Status");
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
                .HasConstraintName("FK_tbl_CivilJustice_tbl_InternalUsers");

            entity.HasOne(d => d.CaseType).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.CaseTypeId)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_CivilJusticeCaseType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblCivilJusticeCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_InternalUsers2");

            entity.HasOne(d => d.Dep).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_Department");

            entity.HasOne(d => d.ExternalRequestStatus).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.ExternalRequestStatusId)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_ExternalRequestStatus");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_Inistitutions");

            entity.HasOne(d => d.Priority).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.PriorityId)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_Priority");

            entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.TblCivilJustices)
                .HasForeignKey(d => d.RequestedBy)
                .HasConstraintName("FK_tbl_CivilJustice_tbl_ExternalUser");
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
                .HasConstraintName("FK_tbl_CivilJusticeRequestReplys_tbl_CivilJustice");
        });

        modelBuilder.Entity<TblDepartment>(entity =>
        {
            entity.HasKey(e => e.DepId);

            entity.ToTable("tbl_Department");

            entity.Property(e => e.DepId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DepCode).HasMaxLength(50);
            entity.Property(e => e.DepName).HasMaxLength(300);
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

        modelBuilder.Entity<TblInistitution>(entity =>
        {
            entity.HasKey(e => e.InistId);

            entity.ToTable("tbl_Inistitutions");

            entity.Property(e => e.InistId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.Description).HasMaxLength(350);
            entity.Property(e => e.Name).HasMaxLength(250);
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

        modelBuilder.Entity<TblInspectionPlan>(entity =>
        {
            entity.HasKey(e => e.InspectionPlanId);

            entity.ToTable("tbl_InspectionPlans");

            entity.Property(e => e.InspectionPlanId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreationDate).HasColumnType("datetime");
            entity.Property(e => e.InspectionYear).HasColumnType("date");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Status).WithMany(p => p.TblInspectionPlans)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("FK_tbl_InspectionPlans_tbl_Status");

            entity.HasOne(d => d.User).WithMany(p => p.TblInspectionPlans)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_tbl_InspectionPlans_tbl_InternalUsers");
        });

        modelBuilder.Entity<TblInspectionStatus>(entity =>
        {
            entity.HasKey(e => e.ProId);

            entity.ToTable("tbl_InspectionStatus");

            entity.Property(e => e.ProId).HasDefaultValueSql("(newid())");
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
            entity.Property(e => e.UserName).HasMaxLength(250);

            entity.HasOne(d => d.Dep).WithMany(p => p.TblInternalUsers)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_tbl_InternalUsers_tbl_Department");
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
            entity.Property(e => e.DueDate).HasColumnType("datetime");
            entity.Property(e => e.ExternalRequestStatusId).HasColumnName("ExternalRequestStatusID");
            entity.Property(e => e.TopStatus).HasMaxLength(250);

            entity.HasOne(d => d.AssignedByNavigation).WithMany(p => p.TblLegalStudiesDraftingAssignedByNavigations)
                .HasForeignKey(d => d.AssignedBy)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_InternalUsers1");

            entity.HasOne(d => d.AssignedToNavigation).WithMany(p => p.TblLegalStudiesDraftingAssignedToNavigations)
                .HasForeignKey(d => d.AssignedTo)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_InternalUsers");

            entity.HasOne(d => d.CaseType).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.CaseTypeId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_CivilJusticeCaseType");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblLegalStudiesDraftingCreatedByNavigations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_InternalUsers2");

            entity.HasOne(d => d.Dep).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.DepId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_Department");

            entity.HasOne(d => d.ExternalRequestStatus).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.ExternalRequestStatusId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_ExternalRequestStatus");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_Inistitutions");

            entity.HasOne(d => d.Priority).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.PriorityId)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_Priority");

            entity.HasOne(d => d.RequestedByNavigation).WithMany(p => p.TblLegalStudiesDraftings)
                .HasForeignKey(d => d.RequestedBy)
                .HasConstraintName("FK_tbl_LegalStudiesDrafting_tbl_ExternalUser");
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

            entity.HasOne(d => d.Plan).WithMany(p => p.TblPlanInistitutions)
                .HasForeignKey(d => d.PlanId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_tbl_Plan_Inistitution_tbl_YearlyPlans");
        });

        modelBuilder.Entity<TblPriority>(entity =>
        {
            entity.HasKey(e => e.PriorityId);

            entity.ToTable("tbl_Priority");

            entity.Property(e => e.PriorityId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.PriorityName).HasMaxLength(250);
        });

        modelBuilder.Entity<TblRecomendation>(entity =>
        {
            entity.HasKey(e => e.RecoId);

            entity.ToTable("tbl_Recomendation");

            entity.Property(e => e.RecoId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatinDate).HasColumnType("datetime");
            entity.Property(e => e.EvaluationYear).HasColumnType("date");
            entity.Property(e => e.ModifyDate).HasColumnType("datetime");
            entity.Property(e => e.RecostatusId).HasColumnName("RecostatusID");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblRecomendations)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_Recomendation_tbl_InternalUsers");

            entity.HasOne(d => d.Inist).WithMany(p => p.TblRecomendations)
                .HasForeignKey(d => d.InistId)
                .HasConstraintName("FK_tbl_Recomendation_tbl_Inistitutions");

            entity.HasOne(d => d.Recostatus).WithMany(p => p.TblRecomendations)
                .HasForeignKey(d => d.RecostatusId)
                .HasConstraintName("FK_tbl_Recomendation_tbl_Recomendation2");
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

        modelBuilder.Entity<TblReponseStatus>(entity =>
        {
            entity.HasKey(e => e.ResponseStatusId);

            entity.ToTable("tbl_ReponseStatus");

            entity.Property(e => e.ResponseStatusId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("tbl_Roles");

            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("RoleID");
        });

        modelBuilder.Entity<TblSpecificPlan>(entity =>
        {
            entity.HasKey(e => e.SpecificPlanId);

            entity.ToTable("tbl_SpecificPlans");

            entity.Property(e => e.SpecificPlanId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ModificationDate).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.TblSpecificPlans)
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_InternalUsers");

            entity.HasOne(d => d.InspectionPlan).WithMany(p => p.TblSpecificPlans)
                .HasForeignKey(d => d.InspectionPlanId)
                .HasConstraintName("FK_tbl_SpecificPlans_tbl_InspectionPlans");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
