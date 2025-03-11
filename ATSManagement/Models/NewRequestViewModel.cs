using System;
using System.Collections.Generic;

namespace ATSManagement.Models;

public partial class NewRequestViewModel
{
    public int? OrderId { get; set; }

    public Guid? RequestId { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? RequestDetail { get; set; }

    public Guid? PriorityId { get; set; }

    public string? ServiceTypeName { get; set; }

    public string? ServiceTypeNameAmharic { get; set; }

    public string? PriorityNameWithColor { get; set; }

    public string? PriorityNameWithColorAmharic { get; set; }

    public string? StatusName { get; set; }

    public string? StatusNameAmharic { get; set; }
    public string? OtherServiceType { get; set; }
    public string? SecretaryFullName {  get; set; }
    public string? AssingmentRemark { get; set; }

   // public string? FirstName { get; set; }

    //public string? MiddleName { get; set; }

    public string? Name { get; set; }
}
