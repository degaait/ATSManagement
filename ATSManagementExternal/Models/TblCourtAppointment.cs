using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblCourtAppointment
{
    public int ChatId { get; set; }

    public Guid? UserId { get; set; }

    public DateTime? Datetime { get; set; }

    public string? ChatContent { get; set; }

    public string? AppointmentId { get; set; }

    public bool? IsDephead { get; set; }

    public bool? IsExpert { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? SendBy { get; set; }

    public Guid? SendTo { get; set; }
}
