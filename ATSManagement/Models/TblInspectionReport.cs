using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInspectionReport
{
    public Guid ReportId { get; set; }

    public string? ReportTitle { get; set; }

    public Guid? InistId { get; set; }

    public Guid? RecostatusId { get; set; }

    public Guid? YearId { get; set; }

    public Guid? CreatedBy { get; set; }

    public DateTime? CreatinDate { get; set; }

    public Guid? LawId { get; set; }

    public string? ReportPath { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblInspectionLaw? Law { get; set; }

    public virtual TblRecomendationStatus? Recostatus { get; set; }

    public virtual TblYear? Year { get; set; }
}
