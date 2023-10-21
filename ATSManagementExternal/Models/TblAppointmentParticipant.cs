using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblAppointmentParticipant
{
    public Guid Id { get; set; }

    public Guid? AppointmentId { get; set; }

    public Guid? UserId { get; set; }

    public virtual TblAppointment? Appointment { get; set; }

    public virtual TblInternalUser? User { get; set; }
}
