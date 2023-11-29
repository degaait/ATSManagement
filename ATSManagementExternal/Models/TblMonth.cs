using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblMonth
{
    public Guid MonthId { get; set; }

    public string? MonthName { get; set; }

    public virtual ICollection<TblAdractivitiesReport> TblAdractivitiesReports { get; set; } = new List<TblAdractivitiesReport>();

    public virtual ICollection<TblDebateWorkPerformanceReport> TblDebateWorkPerformanceReports { get; set; } = new List<TblDebateWorkPerformanceReport>();

    public virtual ICollection<TblDraftContractExaminationReport> TblDraftContractExaminationReports { get; set; } = new List<TblDraftContractExaminationReport>();

    public virtual ICollection<TblInstotutionMonitoringReport> TblInstotutionMonitoringReports { get; set; } = new List<TblInstotutionMonitoringReport>();

    public virtual ICollection<TblLegalAdviceReport> TblLegalAdviceReports { get; set; } = new List<TblLegalAdviceReport>();
}
