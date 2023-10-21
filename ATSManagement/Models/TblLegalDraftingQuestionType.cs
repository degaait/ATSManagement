namespace ATSManagement.Models;

public partial class TblLegalDraftingQuestionType
{
    public Guid QuestTypeId { get; set; }

    public string? QuestTypeName { get; set; }

    public string? QuestTypeDescription { get; set; }


    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; } = new List<TblLegalStudiesDrafting>();

}
