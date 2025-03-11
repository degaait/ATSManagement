using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblServiceType
{
    public Guid ServiceTypeId { get; set; }

    public string? ServiceTypeName { get; set; }

    public Guid? DepId { get; set; }

    public string? ServiceTypeNameAmharic { get; set; }

    public int ServiceOrderOrder { get; set; }

    public string? ServiceResultEnglish { get; set; }

    public string? ServiceResultAmharic { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual ICollection<TblRequest> TblRequests { get; set; } = new List<TblRequest>();
}
