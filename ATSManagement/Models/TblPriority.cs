using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblPriority
{
    public Guid PriorityId { get; set; }

    public string? PriorityName { get; set; }

    public string? PriorityNameWithColor { get; set; }

    public virtual ICollection<TblCivilJustice> TblCivilJustices { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; } = new List<TblLegalStudiesDrafting>();
}
