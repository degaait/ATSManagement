using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblYear
{
    public Guid YearId { get; set; }

    public string? Year { get; set; }

    public virtual ICollection<TblAdractivitiesReport> TblAdractivitiesReports { get; set; } = new List<TblAdractivitiesReport>();

    public virtual ICollection<TblDebateWorkPerformanceReport> TblDebateWorkPerformanceReports { get; set; } = new List<TblDebateWorkPerformanceReport>();

    public virtual ICollection<TblDraftContractExaminationReport> TblDraftContractExaminationReports { get; set; } = new List<TblDraftContractExaminationReport>();

    public virtual ICollection<TblInspectionPlan> TblInspectionPlans { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblInspectionReport> TblInspectionReports { get; set; } = new List<TblInspectionReport>();

    public virtual ICollection<TblInstotutionMonitoringReport> TblInstotutionMonitoringReports { get; set; } = new List<TblInstotutionMonitoringReport>();

    public virtual ICollection<TblLegalAdviceReport> TblLegalAdviceReports { get; set; } = new List<TblLegalAdviceReport>();

    public virtual ICollection<TblRecomendation> TblRecomendations { get; set; } = new List<TblRecomendation>();
}
