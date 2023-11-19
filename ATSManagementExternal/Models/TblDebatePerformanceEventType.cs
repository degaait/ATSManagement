using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblDebatePerformanceEventType
{
    public Guid Id { get; set; }

    public string? WorkPerformanceEventType { get; set; }

    public Guid? SubPerformanceId { get; set; }

    public virtual TblSubDebatePerformance? SubPerformance { get; set; }

    public virtual ICollection<TblDebateWorkPerformanceReport> TblDebateWorkPerformanceReports { get; set; } = new List<TblDebateWorkPerformanceReport>();
}
