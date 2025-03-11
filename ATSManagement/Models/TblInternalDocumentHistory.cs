using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInternalDocumentHistory
{
    public Guid HistoryId { get; set; }

    public Guid? RequestId { get; set; }

    public string? DocPath { get; set; }

    public int? Round { get; set; }

    public string? Description { get; set; }

    public Guid? CreatedBy { get; set; }

    public string? FileDescription { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? ExactFileName { get; set; }

    public string? FileTitle { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblInternalRequest? Request { get; set; }
}
