using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblYearlyPlan
{
    public Guid PlanId { get; set; }

    public string? PlanTitle { get; set; }

    public string? PlanDescription { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? ModifyBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? PlanYear { get; set; }

    public Guid? DepId { get; set; }

    public string? DirectorFeedback { get; set; }

    public Guid? PlanStatusId { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual TblInternalUser? ModifyByNavigation { get; set; }
}
