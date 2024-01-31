using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblPlanInistitution
{
    public Guid Id { get; set; }

    public Guid? InistId { get; set; }

    public Guid? SpecificPlanId { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblSpecificPlan? SpecificPlan { get; set; }
}
