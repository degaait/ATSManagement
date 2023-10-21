using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblWitnessEvidence
{
    public Guid WitnessId { get; set; }

    public string? WitnessesName { get; set; }

    public string? EvidenceFiles { get; set; }

    public string? EvidenceVideos { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblRequest? Request { get; set; }
}
