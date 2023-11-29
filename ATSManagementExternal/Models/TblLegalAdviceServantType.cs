using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblLegalAdviceServantType
{
    public Guid Id { get; set; }

    public string? ServantTypeName { get; set; }

    public virtual ICollection<TblLegalAdviceReport> TblLegalAdviceReports { get; set; } = new List<TblLegalAdviceReport>();
}
