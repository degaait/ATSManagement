using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblRequestAssignee
{
    public Guid AssigneeRequestId { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? UserId { get; set; }
}
