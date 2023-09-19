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

    public virtual TblDepartment? Dep { get; set; }

    public virtual TblMainMenu? Menu { get; set; }

    public virtual TblRole? Role { get; set; }
}
