using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblInspectionLaw
{
    public Guid LawId { get; set; }

    public string? LawDescription { get; set; }

    public string? ReferenceArticle { get; set; }

    public virtual ICollection<TblRecomendation> TblRecomendations { get; set; } = new List<TblRecomendation>();
}
