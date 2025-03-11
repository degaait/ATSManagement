using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInternalRequestReplay
{
    public Guid ReplyId { get; set; }

    public string? ReplayDetail { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? InternalReplayedBy { get; set; }

    public DateTime? ReplyDate { get; set; }

    public bool? IsSent { get; set; }

    public bool? IsInternal { get; set; }

    public bool? IsInternalUser { get; set; }

    public string? Attachment { get; set; }

    public virtual TblInternalUser? InternalReplayedByNavigation { get; set; }

    public virtual TblInternalRequest? Request { get; set; }
}
