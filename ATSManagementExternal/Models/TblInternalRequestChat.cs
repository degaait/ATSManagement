using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblInternalRequestChat
{
    public int ChatId { get; set; }

    public string? ChatContent { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public bool? IsDephead { get; set; }

    public bool? IsExpert { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? SendBy { get; set; }

    public Guid? SendTo { get; set; }

    public string? FilePath { get; set; }

    public bool? ForExpert { get; set; }

    public bool? IsSeen { get; set; }

    public virtual TblInternalUser? SendByNavigation { get; set; }

    public virtual TblInternalUser? SendToNavigation { get; set; }

    public virtual TblInternalUser? User { get; set; }
}
