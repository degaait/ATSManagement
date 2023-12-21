using System;
using System.Collections.Generic;

namespace ATSManagementExternal.Models;

public partial class TblPriorityQuestion
{
    public Guid PriorityQueId { get; set; }

    public string? QuestionName { get; set; }

    public string? QuestDescription { get; set; }

    public string? QuestionsNameAmharic { get; set; }

    public virtual ICollection<TblRequestPriorityQuestionsRelation> TblRequestPriorityQuestionsRelations { get; set; } = new List<TblRequestPriorityQuestionsRelation>();
}
