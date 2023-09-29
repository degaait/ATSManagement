using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblRecomendationStatus
{
    public Guid RecostatusId { get; set; }

    public string? Status { get; set; }

    public string? StatusColour { get; set; }

    public virtual ICollection<TblRecomendation> TblRecomendations { get; set; } = new List<TblRecomendation>();
}
