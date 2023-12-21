using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblRequestType
{
    public Guid TypeId { get; set; }

    public string? TypeName { get; set; }

    public string? TypeNameAmharic { get; set; }
}
