using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblSpecificPlan
{
    public Guid SpecificPlanId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? InspectionPlanId { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblInspectionPlan? InspectionPlan { get; set; }
}
