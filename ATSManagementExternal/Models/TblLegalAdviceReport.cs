using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblLegalAdviceReport
{
    public Guid ReportId { get; set; }

    public string? ServantName { get; set; }

    public string? Amount { get; set; }

    public string? Investigation { get; set; }

    public Guid? Month { get; set; }

    public Guid? Year { get; set; }

    public Guid? ReportedBy { get; set; }

    public virtual TblMonth? MonthNavigation { get; set; }

    public virtual TblInternalUser? ReportedByNavigation { get; set; }

    public virtual TblYear? YearNavigation { get; set; }
}
