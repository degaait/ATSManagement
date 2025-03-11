using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblInternalRequestStatus
{
    public Guid RequestStatusId { get; set; }

    public string? StatusName { get; set; }

    public string? StatusWithColor { get; set; }

    public string? StatusNameAmharic { get; set; }

    public string? StatusWithColorAmharic { get; set; }

    public int? OrderId { get; set; }

    public bool? ForDephead { get; set; }

    public bool? ForExpert { get; set; }

    public virtual ICollection<TblInternalRequest> TblInternalRequests { get; set; } = new List<TblInternalRequest>();
}
