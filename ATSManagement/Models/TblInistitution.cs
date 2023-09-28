using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInistitution
{
    public Guid InistId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TblCivilJustice> TblCivilJustices { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblExternalRequest> TblExternalRequests { get; set; } = new List<TblExternalRequest>();

    public virtual ICollection<TblExternalUser> TblExternalUsers { get; set; } = new List<TblExternalUser>();

    public virtual ICollection<TblInspectionInstitution> TblInspectionInstitutions { get; set; } = new List<TblInspectionInstitution>();

    public virtual ICollection<TblPlanInistitution> TblPlanInistitutions { get; set; } = new List<TblPlanInistitution>();

    public virtual ICollection<TblRecomendation> TblRecomendations { get; set; } = new List<TblRecomendation>();
}
