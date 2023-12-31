﻿using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblDepartment
{
    public Guid DepId { get; set; }

    public string? DepName { get; set; }

    public string? DepCode { get; set; }

    public string? DepNameAmharic { get; set; }

    public virtual ICollection<TblCivilJustice> TblCivilJustices { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblExternalRequest> TblExternalRequests { get; set; } = new List<TblExternalRequest>();

    public virtual ICollection<TblExternalSubmenu> TblExternalSubmenus { get; set; } = new List<TblExternalSubmenu>();

    public virtual ICollection<TblInternalUser> TblInternalUsers { get; set; } = new List<TblInternalUser>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblRequestDepartmentRelation> TblRequestDepartmentRelations { get; set; } = new List<TblRequestDepartmentRelation>();

    public virtual ICollection<TblServiceType> TblServiceTypes { get; set; } = new List<TblServiceType>();

    public virtual ICollection<TblSubmenu> TblSubmenus { get; set; } = new List<TblSubmenu>();

    public virtual ICollection<TblTeam> TblTeams { get; set; } = new List<TblTeam>();
}
