using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblSentInspection
{
    public int RecId { get; set; }

    public string? SentReport { get; set; }

    public string? OfficialLetter { get; set; }

    public Guid? InstId { get; set; }

    public string? SendingRemark { get; set; }

    public DateTime? SentDate { get; set; }

    public DateTime? ExpectedReplyDate { get; set; }

    public string? ResponseDetail { get; set; }

    public Guid? RepliedBy { get; set; }

    public DateTime? RespondedDate { get; set; }

    public Guid? SentBy { get; set; }

    public Guid? InspectionPlanId { get; set; }

    public virtual TblInspectionPlan? InspectionPlan { get; set; }

    public virtual TblInistitution? Inst { get; set; }

    public virtual TblExternalUser? RepliedByNavigation { get; set; }

    public virtual TblInternalUser? SentByNavigation { get; set; }

    public virtual ICollection<TblInspectionReplye> TblInspectionReplyes { get; set; } = new List<TblInspectionReplye>();
}
