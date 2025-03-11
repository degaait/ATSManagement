using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInspectionReportFile
{
    public int RepId { get; set; }

    public string? ReportFiles { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public Guid? Id { get; set; }

    public string? Feedback { get; set; }

    public Guid? SpecificPlanId { get; set; }

    public bool? ForDeputy { get; set; }

    public bool? ForExpert { get; set; }

    public bool? ForDepartment { get; set; }

    public string? FileName { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblAssignedYearlyPlan? IdNavigation { get; set; }

    public virtual TblSpecificPlan? SpecificPlan { get; set; }
}
