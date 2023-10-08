using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblDecisionStatus
{
    public Guid DesStatusId { get; set; }

    public string? StatusName { get; set; }

    public string? StatusDescription { get; set; }

    public string? StatusWithColor { get; set; }

    public virtual ICollection<TblCivilJustice> TblCivilJusticeDepartmentUpprovalStatusNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeDeputyUprovalStatusNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeTeamUpprovalStatusNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeUserUpprovalStatusNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingDeputyUprovalStatusNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingTeamUpprovalStatusNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingUserUpprovalStatusNavigations { get; set; } = new List<TblLegalStudiesDrafting>();
}
