using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInstotutionMonitoringReport
{
    public Guid Id { get; set; }

    public string? MoniteredOffice { get; set; }

    public string? ContractNumber { get; set; }

    public string? ContractMoneyAmount { get; set; }

    public string? Adrnumber { get; set; }

    public string? AdrmoneyAmount { get; set; }

    public string? DebateNumber { get; set; }

    public string? DebateMoneyAmmount { get; set; }

    public string? Investigation { get; set; }

    public Guid? YearId { get; set; }

    public Guid? MonthId { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblMonth? Month { get; set; }

    public virtual TblYear? Year { get; set; }
}
