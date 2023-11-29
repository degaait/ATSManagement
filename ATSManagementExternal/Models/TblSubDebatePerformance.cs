using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblSubDebatePerformance
{
    public Guid SubPerformanceId { get; set; }

    public string? SubPerformanceName { get; set; }

    public Guid? PerformanceId { get; set; }

    public virtual TblDebatePerformance? Performance { get; set; }

    public virtual ICollection<TblDebatePerformanceEventType> TblDebatePerformanceEventTypes { get; set; } = new List<TblDebatePerformanceEventType>();

    public virtual ICollection<TblDebateWorkPerformanceReport> TblDebateWorkPerformanceReports { get; set; } = new List<TblDebateWorkPerformanceReport>();

    public virtual ICollection<TblEvent> TblEvents { get; set; } = new List<TblEvent>();
}
