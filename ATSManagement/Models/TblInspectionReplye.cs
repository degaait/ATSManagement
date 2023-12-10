using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInspectionReplye
{
    public int ReplyId { get; set; }

    public int? RecId { get; set; }

    public string? RecoDetail { get; set; }

    public bool? IsExternal { get; set; }

    public bool? IsInternal { get; set; }

    public Guid? InternalUser { get; set; }

    public Guid? ExternalUser { get; set; }

    public DateTime? DateCreated { get; set; }

    public virtual TblExternalUser? ExternalUserNavigation { get; set; }

    public virtual TblInternalUser? InternalUserNavigation { get; set; }

    public virtual TblSentInspection? Rec { get; set; }
}
