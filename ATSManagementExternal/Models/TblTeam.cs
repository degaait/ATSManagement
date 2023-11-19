using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblTeam
{
    public Guid TeamId { get; set; }

    public string? TeamName { get; set; }

    public Guid? DepId { get; set; }

    public Guid? TeamLeaderId { get; set; }

    public string? TeamDescription { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual ICollection<TblInternalUser> TblInternalUsers { get; set; } = new List<TblInternalUser>();

    public virtual TblInternalUser? TeamLeader { get; set; }
}
