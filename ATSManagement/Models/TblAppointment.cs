using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblAppointment
{
    public Guid AppointmentId { get; set; }

    public string? AppointmentDetail { get; set; }

    public Guid? InistId { get; set; }

    public Guid? RequestedBy { get; set; }

    public string? DescusionFinalComeup { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblExternalUser? RequestedByNavigation { get; set; }

    public virtual ICollection<TblAppointmentParticipant> TblAppointmentParticipants { get; set; } = new List<TblAppointmentParticipant>();
}
