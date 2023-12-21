using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblTopStatus
{
    public int TopStatusId { get; set; }

    public string? StatusName { get; set; }

    public string? StatusNameAmharic { get; set; }

    public virtual ICollection<TblRequest> TblRequests { get; set; } = new List<TblRequest>();
}
