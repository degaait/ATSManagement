using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblSpecificPlan
{
    public Guid SpecificPlanId { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedDate { get; set; }

    public DateTime? ModificationDate { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? InspectionPlanId { get; set; }

    public int? PlanCatId { get; set; }

    public bool? IsAssignedToUser { get; set; }

    public bool? IsAssignedToTeam { get; set; }

    public string? AssigningRemark { get; set; }

    public Guid? TeamId { get; set; }

    public Guid? AssigneeTypeId { get; set; }

    public Guid? InistId { get; set; }

    public Guid? ProId { get; set; }

    public string? FinalReport { get; set; }

    public Guid? IsUprovedByDeputy { get; set; }

    public Guid? IsUpprovedbyTeam { get; set; }

    public Guid? IsUpprovedbyDepartment { get; set; }

    public bool? FinalStatus { get; set; }

    public Guid? IsUserUproved { get; set; }

    public virtual TblAssignementType? AssigneeType { get; set; }

    public virtual TblInternalUser? CreatedByNavigation { get; set; }

    public virtual TblInistitution? Inist { get; set; }

    public virtual TblInspectionPlan? InspectionPlan { get; set; }

    public virtual TblDecisionStatus? IsUpprovedbyDepartmentNavigation { get; set; }

    public virtual TblDecisionStatus? IsUpprovedbyTeamNavigation { get; set; }

    public virtual TblDecisionStatus? IsUprovedByDeputyNavigation { get; set; }

    public virtual TblDecisionStatus? IsUserUprovedNavigation { get; set; }

    public virtual TblPlanCatagory? PlanCat { get; set; }

    public virtual TblInspectionStatus? Pro { get; set; }

    public virtual ICollection<TblAssignedYearlyPlan> TblAssignedYearlyPlans { get; set; } = new List<TblAssignedYearlyPlan>();

    public virtual ICollection<TblInpectionActivite> TblInpectionActivites { get; set; } = new List<TblInpectionActivite>();

    public virtual ICollection<TblInspectionChat> TblInspectionChats { get; set; } = new List<TblInspectionChat>();

    public virtual ICollection<TblInspectionReportFile> TblInspectionReportFiles { get; set; } = new List<TblInspectionReportFile>();

    public virtual ICollection<TblPlanInistitution> TblPlanInistitutions { get; set; } = new List<TblPlanInistitution>();

    public virtual ICollection<TblReplyResponseWithExpert> TblReplyResponseWithExperts { get; set; } = new List<TblReplyResponseWithExpert>();

    public virtual ICollection<TblReplyResponseWithStateMinster> TblReplyResponseWithStateMinsters { get; set; } = new List<TblReplyResponseWithStateMinster>();

    public virtual ICollection<TblSentInspection> TblSentInspections { get; set; } = new List<TblSentInspection>();

    public virtual TblTeam? Team { get; set; }
}
