using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblLegalDraftingDocType
{
    public Guid DocId { get; set; }

    public string? DocName { get; set; }

    public string? DocDesciption { get; set; }
}
