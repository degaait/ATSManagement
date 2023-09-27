using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblReponseStatus
{
    public Guid ResponseStatusId { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<TblInspectionInstitution> TblInspectionInstitutions { get; set; } = new List<TblInspectionInstitution>();
}
