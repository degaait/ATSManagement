using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblPlanStatus
{
    public Guid PlanStatusId { get; set; }

    public string? StatusTitle { get; set; }
}
