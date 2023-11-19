using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblDebateWorkPerformanceReport
{
    public Guid WorkPerformId { get; set; }

    public string? Womens { get; set; }

    public string? Childrens { get; set; }

    public string? WomenElders { get; set; }

    public string? Hivpositives { get; set; }

    public string? Mens { get; set; }

    public string? OtherServants { get; set; }

    public string? TotalServant { get; set; }

    public string? OutofContact { get; set; }

    public string? Family { get; set; }

    public string? Property { get; set; }

    public string? WorkDebate { get; set; }

    public string? OtherCaseTypes { get; set; }

    public string? JudgementMoneyAmmount { get; set; }

    public string? JudgementVerifiedAmmount { get; set; }

    public Guid? Id { get; set; }

    public Guid? YearId { get; set; }

    public Guid? MonthId { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? SubPerformanceId { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblDebatePerformanceEventType? IdNavigation { get; set; }

    public virtual TblMonth? Month { get; set; }

    public virtual TblSubDebatePerformance? SubPerformance { get; set; }

    public virtual TblYear? Year { get; set; }
}
