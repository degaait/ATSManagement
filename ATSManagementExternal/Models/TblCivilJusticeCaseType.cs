using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblCivilJusticeCaseType
{
    public Guid CaseTypeId { get; set; }

    public string? CaseTypeName { get; set; }

    public virtual ICollection<TblCivilJustice> TblCivilJustices { get; set; } = new List<TblCivilJustice>();
}
