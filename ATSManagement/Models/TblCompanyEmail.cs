using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblCompanyEmail
{
    public Guid EmailId { get; set; }

    public string? EmailAdress { get; set; }

    public bool? IsActive { get; set; }
}
