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

    public virtual DbSet<TblDepartment> TblDepartments { get; set; }

    public virtual DbSet<TblExternalUser> TblExternalUsers { get; set; }

    public virtual DbSet<TblInistitution> TblInistitutions { get; set; }

    public virtual DbSet<TblInspectionPlan> TblInspectionPlans { get; set; }

    public virtual DbSet<TblInternalUser> TblInternalUsers { get; set; }

    public virtual DbSet<TblMainMenu> TblMainMenus { get; set; }

    public virtual DbSet<TblPlanInistitution> TblPlanInistitutions { get; set; }

    public virtual DbSet<TblRecomendation> TblRecomendations { get; set; }

    public virtual DbSet<TblRecomendationStatus> TblRecomendationStatuses { get; set; }

    public virtual DbSet<TblRole> TblRoles { get; set; }

    public virtual DbSet<TblStatus> TblStatuses { get; set; }

    public virtual DbSet<TblSubmenu> TblSubmenus { get; set; }

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

        modelBuilder.Entity<TblDepartment>(entity =>
        {
            entity.HasKey(e => e.DepId);

            entity.ToTable("tbl_Department");

            entity.Property(e => e.DepId).HasDefaultValueSql("(newid())");
            entity.Property(e => e.DepName).HasMaxLength(300);
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

        modelBuilder.Entity<TblRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("tbl_Roles");

            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("RoleID");
        });

        modelBuilder.Entity<TblStatus>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PK_tbl_Status_1");

            entity.ToTable("tbl_Status");

            entity.Property(e => e.StatusId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("StatusID");
            entity.Property(e => e.Status).HasMaxLength(50);
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
