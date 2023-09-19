using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblMainMenu
{
    public Guid MenuId { get; set; }

    public string? MenuName { get; set; }

    public string? MenuDescription { get; set; }

    public virtual ICollection<TblSubmenu> TblSubmenus { get; set; } = new List<TblSubmenu>();
}
