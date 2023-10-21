using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblAssignee
{
    public Guid Id { get; set; }

    public Guid? UserId { get; set; }

    public Guid? RequestId { get; set; }

    public virtual TblInternalUser? User { get; set; }
}
