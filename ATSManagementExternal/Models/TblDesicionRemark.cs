using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblDesicionRemark
{
    public int DesicionId { get; set; }

    public string? DesicionRemark { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblRequest? Request { get; set; }
}
