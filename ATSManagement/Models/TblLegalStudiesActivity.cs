using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblLegalStudiesActivity
{
    public Guid ActivityId { get; set; }

    public string? ActivityDetail { get; set; }

    public DateTime? AddedDate { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? CreatedBy { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblLegalStudiesDrafting? Request { get; set; }
}
