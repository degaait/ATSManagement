using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblDocumentHistory
{
    public Guid HistoryId { get; set; }

    public Guid? RequestId { get; set; }

    public string? DocPath { get; set; }

    public int? Round { get; set; }

    public virtual TblRequest? Request { get; set; }
}
