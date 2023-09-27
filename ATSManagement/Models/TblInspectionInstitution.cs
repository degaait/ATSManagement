using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblInspectionInstitution
{
    public Guid SubMissionId { get; set; }

    public string? RequestStatus { get; set; }

    public string? RecomendationDetail { get; set; }

    public string? RecomendationFeedBack { get; set; }

    public DateTime? RequestedDate { get; set; }

    public DateTime? ExpectedResponseDate { get; set; }

    public Guid? InstitutionId { get; set; }

    public Guid? ResponseStatusId { get; set; }

    public string? ReComendationFile { get; set; }

    public Guid? SubmittedBy { get; set; }

    public Guid? ReturnedBy { get; set; }

    public virtual TblInistitution? Institution { get; set; }

    public virtual TblReponseStatus? ResponseStatus { get; set; }

    public virtual TblExternalUser? ReturnedByNavigation { get; set; }

    public virtual TblInternalUser? SubmittedByNavigation { get; set; }
}
