namespace ATSManagement.Models;

public partial class TblLegalDraftingDocType
{
    public Guid DocId { get; set; }

    public string? DocName { get; set; }

    public string? DocDesciption { get; set; }


    public virtual ICollection<TblLegalStudiesDrafting> TblLegalStudiesDraftings { get; set; } = new List<TblLegalStudiesDrafting>();

}
