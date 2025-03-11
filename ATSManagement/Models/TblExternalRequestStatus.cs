using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblExternalRequestStatus
{
    public Guid ExternalRequestStatusId { get; set; }

    public string? StatusName { get; set; }

    public string? StatusWithColor { get; set; }

    public string? StatusNameAmharic { get; set; }

    public string? StatusWithColorAmharic { get; set; }

    public int? OrderId { get; set; }

    public virtual ICollection<TblCivilJustice> TblCivilJustices { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblExternalRequest> TblExternalRequests { get; set; } = new List<TblExternalRequest>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblRequest> TblRequests { get; set; } = new List<TblRequest>();
}
