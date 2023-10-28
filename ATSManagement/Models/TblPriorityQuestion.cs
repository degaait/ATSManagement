using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblPriorityQuestion
{
    public Guid PriorityQueId { get; set; }

    public string? QuestionName { get; set; }

    public string? QuestDescription { get; set; }

    public virtual ICollection<TblRequestPriorityQuestionsRelation> TblRequestPriorityQuestionsRelations { get; set; } = new List<TblRequestPriorityQuestionsRelation>();
}
