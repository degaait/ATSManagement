using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblRole
{
    public Guid RoleId { get; set; }

    public string? RoleName { get; set; }

    public string? RoleDescription { get; set; }

    public virtual ICollection<TblSubmenu> TblSubmenus { get; set; } = new List<TblSubmenu>();
}
