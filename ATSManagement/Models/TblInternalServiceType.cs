using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInternalServiceType
{
    public Guid ServiceTypeId { get; set; }

    public string? ServiceTypeName { get; set; }

    public string? ServiceTypeNameAmharic { get; set; }

    public string? ServiceOrderOrder { get; set; }

    public string? ServiceResultEnglish { get; set; }

    public string? ServiceResultAmharic { get; set; }

    public virtual ICollection<TblInternalRequest> TblInternalRequests { get; set; } = new List<TblInternalRequest>();
}
