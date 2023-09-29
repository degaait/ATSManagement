using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblReponseStatus
{
    public Guid ResponseStatusId { get; set; }

    public string? StatusName { get; set; }

    public string? StatusWithColor { get; set; }

    public virtual ICollection<TblInspectionInstitution> TblInspectionInstitutions { get; set; } = new List<TblInspectionInstitution>();
}
