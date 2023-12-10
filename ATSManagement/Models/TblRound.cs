using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblRound
{
    public int RoundId { get; set; }

    public Guid? RequestIid { get; set; }

    public int? RoundNumber { get; set; }

    public virtual TblRequest? RequestI { get; set; }
}
