using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblRequestDepartmentRelation
{
    public int Id { get; set; }

    public Guid? DepId { get; set; }

    public Guid? RequestId { get; set; }

    public virtual TblDepartment? Dep { get; set; }

    public virtual TblRequest? Request { get; set; }
}
