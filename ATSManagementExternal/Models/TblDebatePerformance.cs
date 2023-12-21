using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblDebatePerformance
{
    public Guid PerformanceId { get; set; }

    public string? PreformanceName { get; set; }

    public string? PerformanceNameEnglish { get; set; }

    public virtual ICollection<TblSubDebatePerformance> TblSubDebatePerformances { get; set; } = new List<TblSubDebatePerformance>();
}
