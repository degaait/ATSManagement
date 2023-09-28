using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInternalUser
{
    public Guid UserId { get; set; }

    public string? FirstName { get; set; }

    public string? MidleName { get; set; }

    public string? LastName { get; set; }

    public string? EmailAddress { get; set; }

    public bool IsSuperAdmin { get; set; }

    public bool IsTeamLeader { get; set; }

    public Guid? DepId { get; set; }

    public string? PhoneNumber { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public bool IsDeputy { get; set; }

    public bool IsDepartmentHead { get; set; }

    public bool IsActive { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeAssignedByNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeAssignedToNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblInspectionInstitution> TblInspectionInstitutions { get; set; } = new List<TblInspectionInstitution>();

    public virtual ICollection<TblInspectionPlan> TblInspectionPlans { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblRecomendation> TblRecomendations { get; set; } = new List<TblRecomendation>();

    public virtual ICollection<TblSpecificPlan> TblSpecificPlans { get; set; } = new List<TblSpecificPlan>();
}
