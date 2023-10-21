using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblLegalDraftingQuestionType
{
    public Guid QuestTypeId { get; set; }

    public string? QuestTypeName { get; set; }

    public string? QuestTypeDescription { get; set; }

    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; } = new List<TblLegalStudiesDrafting>();

    public virtual ICollection<TblRequest> TblRequests { get; set; } = new List<TblRequest>();
}
