using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblInspectionStatus
{
    public Guid ProId { get; set; }

    public string? ProstatusTitle { get; set; }

    public int? ProgressOrder { get; set; }

    public string? StatusWithColor { get; set; }

    public virtual ICollection<TblInspectionPlan> TblInspectionPlans { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblSpecificPlan> TblSpecificPlans { get; set; } = new List<TblSpecificPlan>();
}
