using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblCompanyEmail
{
    public Guid EmailId { get; set; }

    public string? EmailAdress { get; set; }

    public bool? IsActive { get; set; }
}
