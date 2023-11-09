using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblContactInformation
{
    public Guid ContactId { get; set; }

    public string? ContactDetaiMessage { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhoneNumber { get; set; }

    public string? ContactCountry { get; set; }
}
