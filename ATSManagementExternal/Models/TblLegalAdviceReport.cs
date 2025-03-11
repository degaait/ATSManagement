using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblLegalAdviceReport
{
    public Guid ReportId { get; set; }

    public Guid? Id { get; set; }

    public string? Amount { get; set; }

    public string? Investigation { get; set; }

    public Guid? Month { get; set; }

    public Guid? Year { get; set; }

    public Guid? ReportedBy { get; set; }

    public int? Total { get; set; }

    public int? Mens { get; set; }

    public int? Womens { get; set; }

    public int? Disablities { get; set; }

    public int? Hivpositive { get; set; }

    public int? GovernmentInstitutes { get; set; }

    public int? Other { get; set; }

    public int? Childrens { get; set; }

    public int? Elders { get; set; }

    public int? PersecutionReturnies { get; set; }

    public int? MensInvestigations { get; set; }

    public int? WomensInvestigations { get; set; }

    public int? DisablitiesInvestigation { get; set; }

    public int? HivpositiveInvestigation { get; set; }

    public int? GovernmentInstitutesInvestigations { get; set; }

    public int? OtherInvestigations { get; set; }

    public int? ChildrensInvestigations { get; set; }

    public int? EldersInvestigations { get; set; }

    public int? PersecutionReturniesInvestigations { get; set; }

    public int? TotalInvestigations { get; set; }

    public virtual TblLegalAdviceServantType? IdNavigation { get; set; }

    public virtual TblMonth? MonthNavigation { get; set; }

    public virtual TblInternalUser? ReportedByNavigation { get; set; }

    public virtual TblYear? YearNavigation { get; set; }
}
