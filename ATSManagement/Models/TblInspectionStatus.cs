using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInspectionStatus
{
    public Guid ProId { get; set; }

    public string? ProstatusTitle { get; set; }

    public int? ProgressOrder { get; set; }
}
