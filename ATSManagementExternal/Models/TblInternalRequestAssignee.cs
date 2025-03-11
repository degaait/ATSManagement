using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblInternalRequestAssignee
{
    public Guid AssigneeRequestId { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? UserId { get; set; }

    public virtual TblInternalRequest? Request { get; set; }

    public virtual TblInternalUser? User { get; set; }
}
