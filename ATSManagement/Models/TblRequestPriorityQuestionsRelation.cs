using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class TblRequestPriorityQuestionsRelation
{
    public Guid Id { get; set; }

    public Guid? RequestId { get; set; }

    public Guid? PriorityQueId { get; set; }

    public virtual TblPriorityQuestion? PriorityQue { get; set; }

    public virtual TblRequest? Request { get; set; }
}
