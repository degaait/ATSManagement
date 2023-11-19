using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblAdreventType
{
    public Guid TypeId { get; set; }

    public string? TypeNames { get; set; }

    public virtual ICollection<TblAdractivitiesReport> TblAdractivitiesReports { get; set; } = new List<TblAdractivitiesReport>();
}
