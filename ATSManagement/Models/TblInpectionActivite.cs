using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInpectionActivite
{
    public int ActivityId { get; set; }

    public string? ActivityDetail { get; set; }

    public DateTime? AddedDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public string? TimeTakenTocomplete { get; set; }

    public string? Remark { get; set; }

    public Guid? SpecificPlanId { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblSpecificPlan? SpecificPlan { get; set; }
}
