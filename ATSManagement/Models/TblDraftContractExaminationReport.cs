using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblDraftContractExaminationReport
{
    public Guid Id { get; set; }

    public string? QuestionSubmitter { get; set; }

    public string? CaseType { get; set; }

    public string? FullMoneyAmmount { get; set; }

    public string? Investigation { get; set; }

    public Guid? YearId { get; set; }

    public Guid? MonthId { get; set; }

    public Guid? SubmittedBy { get; set; }

    public virtual TblMonth? Month { get; set; }

    public virtual TblInternalUser? SubmittedByNavigation { get; set; }

    public virtual TblYear? Year { get; set; }
}
