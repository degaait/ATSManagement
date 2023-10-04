using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblLegalStudiesReplay
{
    public Guid ReplyId { get; set; }

    public string? ReplayDetail { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? InternalReplayedBy { get; set; }

    public DateTime? ReplyDate { get; set; }

    public Guid? ExternalReplayedBy { get; set; }

    public virtual TblInternalUser? ExternalReplayedBy1 { get; set; }

    public virtual TblExternalUser? ExternalReplayedByNavigation { get; set; }

    public virtual TblLegalStudiesDrafting? Request { get; set; }
}
