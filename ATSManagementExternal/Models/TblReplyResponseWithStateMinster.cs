using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblReplyResponseWithStateMinster
{
    public int RepId { get; set; }

    public string? ReportFiles { get; set; }

    public Guid CreatedBy { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public Guid? Id { get; set; }

    public string? Feedback { get; set; }

    public Guid? SpecificPlanId { get; set; }

    public virtual TblInternalUser CreatedByNavigation { get; set; } = null!;

    public virtual TblAssignedYearlyPlan? IdNavigation { get; set; }

    public virtual TblSpecificPlan? SpecificPlan { get; set; }
}
