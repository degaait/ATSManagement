using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblTermsAndCondition
{
    public Guid TermsId { get; set; }

    public string? Details { get; set; }

    public bool? IsActive { get; set; }
}
