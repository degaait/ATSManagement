using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblAdjornment
{
    public Guid AdjoryId { get; set; }

    public Guid? RequestId { get; set; }

    public DateTime? AdjorneyDate { get; set; }

    public string? WhatIsDone { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ExpertHanlingCase { get; set; }

    public string? PlaintiffDefendant { get; set; }

    public string? TheCourtCaseHanled { get; set; }

    public string? AppointmentReason { get; set; }

    public string? DefendantInfo { get; set; }

    public string? CaseNumber { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblRequest? Request { get; set; }

    public virtual ICollection<TblAdjournmentChat> TblAdjournmentChats { get; set; } = new List<TblAdjournmentChat>();
}
