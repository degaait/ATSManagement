﻿using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblCivilJusticeRequestActivity
{
    public Guid ActivityId { get; set; }

    public string? ActivityDetail { get; set; }

    public DateTime? AddedDate { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblCivilJustice? Request { get; set; }
}
