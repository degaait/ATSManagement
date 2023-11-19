using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblReportServiceType
{
    public Guid Id { get; set; }

    public string? ReportSeriviceName { get; set; }

    public string? Description { get; set; }
}
