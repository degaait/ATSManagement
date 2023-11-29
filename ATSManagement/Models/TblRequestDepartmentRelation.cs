using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblRequestDepartmentRelation
{
    public int Id { get; set; }

    public Guid? DepId { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? AssigneeTypeId { get; set; }

    public Guid? TeamId { get; set; }

    public bool? IsAssingedToUser { get; set; }

    public virtual TblAssignementType? AssigneeType { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual TblRequest? Request { get; set; }

    public virtual TblTeam? Team { get; set; }
}
