using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblMainMenu
{
    public Guid MenuId { get; set; }

    public string? MenuName { get; set; }

    public string? MenuDescription { get; set; }

    public int? DisplayOrder { get; set; }

    public string? ClassSvg { get; set; }

    public string? MenuNameAmharic { get; set; }

    public virtual ICollection<TblSubmenu> TblSubmenus { get; set; } = new List<TblSubmenu>();
}
