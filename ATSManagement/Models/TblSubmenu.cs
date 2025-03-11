using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblSubmenu
{
    public Guid Id { get; set; }

    public string? Submenu { get; set; }

    public string? Controller { get; set; }

    public string? Action { get; set; }

    public Guid? RoleId { get; set; }

    public Guid? MenuId { get; set; }

    public bool IsActive { get; set; }

    public bool IsDeleted { get; set; }

    public Guid? DepId { get; set; }

    public string? SubmenuAmharic { get; set; }

    public bool? ForDeputy { get; set; }

    public bool? ForDepHead { get; set; }

    public bool? ForDefaulUser { get; set; }

    public bool? ForTeamLeader { get; set; }

    public bool? ForSuperAdmin { get; set; }

    public bool? ForSecretary { get; set; }

    public bool? ForBranchOfficer { get; set; }

    public bool? ForInternalUser { get; set; }

    public int? SortOrder { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual TblMainMenu? Menu { get; set; }

    public virtual TblRole? Role { get; set; }
}
