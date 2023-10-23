using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblRequestAssignee
{
    public Guid AssigneeRequestId { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? UserId { get; set; }

    public virtual TblRequest? Request { get; set; }

    public virtual TblInternalUser? User { get; set; }
}
