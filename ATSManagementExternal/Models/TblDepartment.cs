﻿using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblDepartment
{
    public Guid DepId { get; set; }

    public string? DepName { get; set; }

    public string? DepCode { get; set; }

    public virtual ICollection<TblCivilJustice> TblCivilJustices { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblInternalUser> TblInternalUsers { get; set; } = new List<TblInternalUser>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblSubmenu> TblSubmenus { get; set; } = new List<TblSubmenu>();
}