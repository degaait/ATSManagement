using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblRequestAssignementType
{
    public Guid TypeId { get; set; }

    public string? TypeName { get; set; }

    public virtual ICollection<TblRequest> TblRequests { get; set; } = new List<TblRequest>();
}
