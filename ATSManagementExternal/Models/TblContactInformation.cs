using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblContactInformation
{
    public string? ContactDetaiMessage { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactPhoneNumber { get; set; }

    public string? ContactCountry { get; set; }

    public string? FileUplaod { get; set; }

    public int ContactId { get; set; }

    public string? FullName { get; set; }
}
