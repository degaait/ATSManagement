using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblDocumentHistory
{
    public Guid HistoryId { get; set; }

    public Guid? RequestId { get; set; }

    public string? DocPath { get; set; }

    public int? Round { get; set; }

    public string? Description { get; set; }

    public Guid? InternalReplyId { get; set; }

    public Guid? ExternalRepliedBy { get; set; }

    public string? FileDescription { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TblExternalUser? ExternalRepliedByNavigation { get; set; }

    public virtual TblInternalUser? InternalReply { get; set; }

    public virtual TblRequest? Request { get; set; }
}
