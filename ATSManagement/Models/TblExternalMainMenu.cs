using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblExternalMainMenu
{
    public Guid MenuId { get; set; }

    public string? MenuName { get; set; }

    public string? MenuDescription { get; set; }

    public int? DisplayOrder { get; set; }

    public string? ClassSvg { get; set; }

    public string? MenuNameAmharic { get; set; }

    public virtual ICollection<TblExternalSubmenu> TblExternalSubmenus { get; set; } = new List<TblExternalSubmenu>();
}
