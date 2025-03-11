using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblNotification
{
    public int NotificationId { get; set; }

    public string? NotificationDetail { get; set; }

    public DateTime? NotificationDate { get; set; }

    public Guid? UserId { get; set; }

    public Guid? CreatedBy { get; set; }

    public bool? IsChecked { get; set; }

    public string? Icon { get; set; }

    public Guid? ExterUserId { get; set; }

    public bool? FromExternal { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblExternalUser? ExterUser { get; set; }

    public virtual TblInternalUser? User { get; set; }
}
