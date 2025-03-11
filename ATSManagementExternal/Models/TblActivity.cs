using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblActivity
{
    public Guid ActivityId { get; set; }

    public string? ActivityDetail { get; set; }

    public DateTime? AddedDate { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? CreatedBy { get; set; }

    public string? TimeTakenTocomplete { get; set; }

    public string? Remark { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblRequest? Request { get; set; }
}
