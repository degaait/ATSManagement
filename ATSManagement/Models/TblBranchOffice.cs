using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblBranchOffice
{
    public int BranchId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<TblInternalUser> TblInternalUsers { get; set; } = new List<TblInternalUser>();
}
