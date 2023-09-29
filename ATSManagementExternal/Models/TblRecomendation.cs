using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblRecomendation
{
    public Guid RecoId { get; set; }

    public string? Recomendation { get; set; }

    public Guid? InistId { get; set; }

    public Guid? RecostatusId { get; set; }

    public DateTime? EvaluationYear { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatinDate { get; set; }

    public DateTime? ModifyDate { get; set; }

    public bool IsActive { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblRecomendationStatus? Recostatus { get; set; }
}
