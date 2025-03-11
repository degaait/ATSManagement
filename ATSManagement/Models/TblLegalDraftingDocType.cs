using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblLegalDraftingDocType
{
    public Guid DocId { get; set; }

    public string? DocName { get; set; }

    public string? DocDesciption { get; set; }

    public string? DocNameAmharic { get; set; }

    public int DocmentOrder { get; set; }

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblRequest> TblRequests { get; set; } = new List<TblRequest>();
}
