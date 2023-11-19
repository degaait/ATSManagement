using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblEvent
{
    public Guid EventId { get; set; }

    public string? EventName { get; set; }

    public string? EventDescription { get; set; }

    public Guid? SubPerformanceId { get; set; }

    public virtual TblSubDebatePerformance? SubPerformance { get; set; }
}
