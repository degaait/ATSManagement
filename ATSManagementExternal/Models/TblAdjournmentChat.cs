using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblAdjournmentChat
{
    public int ChatId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? ChatContent { get; set; }

    public bool? IsDephead { get; set; }

    public bool? IsExpert { get; set; }

    public Guid? SendBy { get; set; }

    public Guid? SendTo { get; set; }

    public Guid? AdjoryId { get; set; }

    public virtual TblAdjornment? Adjory { get; set; }

    public virtual TblInternalUser? SendByNavigation { get; set; }

    public virtual TblInternalUser? SendToNavigation { get; set; }

    public virtual TblInternalUser? User { get; set; }
}
