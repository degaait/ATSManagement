using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblAdjornment
{
    public Guid AdjoryId { get; set; }

    public Guid? RequestId { get; set; }

    public DateTime? AdjorneyDate { get; set; }

    public string? WhatIsDone { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblRequest? Request { get; set; }
}
