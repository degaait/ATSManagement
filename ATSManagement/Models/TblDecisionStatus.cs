using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblDecisionStatus
{
    public Guid DesStatusId { get; set; }

    public string? StatusName { get; set; }

    public string? StatusDescription { get; set; }

    public string? StatusWithColor { get; set; }

    public virtual ICollection<TblCivilJustice> TblCivilJusticeDepartmentUpprovalStatusNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeDeputyUprovalStatusNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeTeamUpprovalStatusNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblCivilJustice> TblCivilJusticeUserUpprovalStatusNavigations { get; set; } = new List<TblCivilJustice>();

    public virtual ICollection<TblInspectionPlan> TblInspectionPlanIsUpprovedbyDepartmentNavigations { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblInspectionPlan> TblInspectionPlanIsUpprovedbyTeamNavigations { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblInspectionPlan> TblInspectionPlanIsUprovedByDeputyNavigations { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblInspectionPlan> TblInspectionPlanIsUserUprovedNavigations { get; set; } = new List<TblInspectionPlan>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingDepartmentUpprovalStatusNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingDeputyUprovalStatusNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingTeamUpprovalStatusNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftingUserUpprovalStatusNavigations { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblRequest> TblRequestDepartmentUpprovalStatusNavigations { get; set; } = new List<TblRequest>();

    public virtual ICollection<TblRequest> TblRequestDeputyUprovalStatusNavigations { get; set; } = new List<TblRequest>();

    public virtual ICollection<TblRequest> TblRequestTeamUpprovalStatusNavigations { get; set; } = new List<TblRequest>();

    public virtual ICollection<TblRequest> TblRequestUserUpprovalStatusNavigations { get; set; } = new List<TblRequest>();
}
