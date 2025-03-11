using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblInistitution
{
    public Guid InistId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? NameAmharic { get; set; }

    public virtual ICollection<TblAppointment> TblAppointments { get; set; } = new List<TblAppointment>();

    public virtual ICollection<TblCivilJustice> TblCivilJustices { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblExternalRequest> TblExternalRequests { get; set; } = new List<TblExternalRequest>();

    public virtual ICollection<TblExternalUser> TblExternalUsers { get; set; } = new List<TblExternalUser>();

    public virtual ICollection<TblFollowup> TblFollowups { get; set; } = new List<TblFollowup>();

    public virtual ICollection<TblInspectionInstitution> TblInspectionInstitutions { get; set; } = new List<TblInspectionInstitution>();

    public virtual ICollection<TblInspectionReport> TblInspectionReports { get; set; } = new List<TblInspectionReport>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblPlanInistitution> TblPlanInistitutions { get; set; } = new List<TblPlanInistitution>();

    public virtual ICollection<TblRecomendation> TblRecomendations { get; set; } = new List<TblRecomendation>();

    public virtual ICollection<TblRequest> TblRequests { get; set; } = new List<TblRequest>();

    public virtual ICollection<TblSentInspection> TblSentInspections { get; set; } = new List<TblSentInspection>();

    public virtual ICollection<TblSpecificPlan> TblSpecificPlans { get; set; } = new List<TblSpecificPlan>();
}
