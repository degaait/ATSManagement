﻿using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblPlanInistitution
{
    public Guid Id { get; set; }

    public Guid? PlanId { get; set; }

    public Guid? InistId { get; set; }

    public Guid? SpecificPlanId { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblInspectionPlan? Plan { get; set; }

    public virtual TblSpecificPlan? SpecificPlan { get; set; }
}
