using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblExternalUser
{
    public Guid ExterUserId { get; set; }

    public string? FirstName { get; set; }

    public string? MiddleName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public Guid? InistId { get; set; }

    public bool? IsActive { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual ICollection<TblCivilJustice> TblCivilJustices { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblExternalRequest> TblExternalRequests { get; set; } = new List<TblExternalRequest>();

    public virtual ICollection<TblInspectionInstitution> TblInspectionInstitutions { get; set; } = new List<TblInspectionInstitution>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; } = new List<TblLegalStudiesDrafting>();
}
