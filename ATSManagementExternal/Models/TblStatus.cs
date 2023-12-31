﻿using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblStatus
{
    public Guid StatusId { get; set; }

    public string Status { get; set; } = null!;

    public string? StatusWithColor { get; set; }

    public string? StatusAmharic { get; set; }

    public string? StatusWithColorAmharic { get; set; }

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; } = new List<TblAssignedYearlyPlan>();
}
