using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblAppointmentChat
{
    public int ChatId { get; set; }

    public string? ChatContent { get; set; }

    public Guid? AppointmentId { get; set; }

    public bool? IsEnternal { get; set; }

    public bool? IsInternal { get; set; }

    public Guid? UserId { get; set; }

    public Guid? ExterUserId { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual TblAppointment? Appointment { get; set; }

    public virtual TblExternalUser? ExterUser { get; set; }

    public virtual TblInternalUser? User { get; set; }
}
