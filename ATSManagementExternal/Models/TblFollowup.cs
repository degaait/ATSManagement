using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblFollowup
{
    public int FollowUpId { get; set; }

    public string? Message { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? InistId { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ExternalUserId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public bool? FromExternal { get; set; }

    public bool? FromInternal { get; set; }

    public string? Attachment { get; set; }

    public virtual TblExternalUser? ExternalUser { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblRequest? Request { get; set; }

    public virtual TblInternalUser? User { get; set; }
}
