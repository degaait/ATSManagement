using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblStatus
{
    public Guid StatusId { get; set; }

    public string Status { get; set; } = null!;

    public string? StatusWithColor { get; set; }

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblInspectionPlan> TblInspectionPlans { get; set; } = new List<TblInspectionPlan>();
}
