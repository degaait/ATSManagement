using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblServiceType
{
    public Guid ServiceTypeId { get; set; }

    public string? ServiceTypeName { get; set; }

    public virtual ICollection<TblRequest> TblRequests { get; set; } = new List<TblRequest>();
}
