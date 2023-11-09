using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblTeam
{
    public Guid TeamId { get; set; }

    public string? TeamName { get; set; }

    public Guid? DepId { get; set; }

    public Guid? TeamLeaderId { get; set; }

    public string? TeamDescription { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual TblInternalUser? TeamLeader { get; set; }
}
