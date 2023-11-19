using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ReportViewModels
{
    public class DebateWorkPerformanceReport
    {
        public Guid? WorkPerformID { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Womens { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Childrens { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? WomeElders { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? HIVPositives { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? Mens { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? OtherServants { get; set; }
        public string? TotalServants { get; set; }
        public string? OutofContact { get; set; }
        public string? Family { get; set; }
        public string? Property { get; set; }
        public string? WorkDebate { get; set; }
        public string? OtherCaseTypes { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? JudgementMoneyAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? JudgementVerifiedAmmount { get; set; }

        public Guid? ID { get; set; }
        public IEnumerable<SelectListItem>? PerformanceEventTypes { get; set; }
        public Guid? YearID { get; set; }
        public IEnumerable<SelectListItem>? Years { get; set; }
        public Guid? MonthID { get; set; }
        public IEnumerable<SelectListItem>? Months { get; set; }
        public Guid? CreatedBy { get; set; }

    }
}
