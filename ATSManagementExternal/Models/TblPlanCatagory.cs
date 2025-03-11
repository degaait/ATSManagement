using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblPlanCatagory
{
    public int PlanCatId { get; set; }

    public string? CatTitle { get; set; }

    public string? CatDescription { get; set; }

    public Guid? InspectionPlanId { get; set; }

    public bool DoesHaveSpecificPlan { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TblInspectionPlan? InspectionPlan { get; set; }

    public virtual ICollection<TblSpecificPlan> TblSpecificPlans { get; set; } = new List<TblSpecificPlan>();
}
