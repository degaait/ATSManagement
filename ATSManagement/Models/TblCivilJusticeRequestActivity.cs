using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblCivilJusticeRequestActivity
{
    public Guid ActivityId { get; set; }

    public string? ActivityDetail { get; set; }

    public DateTime? AddedDate { get; set; }

    public Guid? RequestId { get; set; }
}
