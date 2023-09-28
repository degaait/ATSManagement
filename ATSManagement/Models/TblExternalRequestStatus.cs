using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblExternalRequestStatus
{
    public Guid ExternalRequestStatusId { get; set; }

    public string? StatusName { get; set; }

    public string? StatusWithColor { get; set; }

    public virtual ICollection<TblExternalRequest> TblExternalRequests { get; set; } = new List<TblExternalRequest>();
}
