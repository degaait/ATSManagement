﻿using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblDepartment
{
    public Guid DepId { get; set; }

    public string? DepName { get; set; }

    public virtual ICollection<TblInternalUser> TblInternalUsers { get; set; } = new List<TblInternalUser>();

    public virtual ICollection<TblSubmenu> TblSubmenus { get; set; } = new List<TblSubmenu>();
}