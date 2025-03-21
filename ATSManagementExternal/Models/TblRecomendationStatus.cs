﻿using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblRecomendationStatus
{
    public Guid RecostatusId { get; set; }

    public string? Status { get; set; }

    public string? StatusColour { get; set; }

    public string? StatusAmharic { get; set; }

    public string? StatusColourAmharic { get; set; }

    public virtual ICollection<TblInspectionReport> TblInspectionReports { get; set; } = new List<TblInspectionReport>();

    public virtual ICollection<TblRecomendation> TblRecomendations { get; set; } = new List<TblRecomendation>();
}
