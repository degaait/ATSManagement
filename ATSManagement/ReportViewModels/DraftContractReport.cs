using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ReportViewModels
{
    public class DraftContractReport
    {
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "*")]
        public string? QuestionSubmitter { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "*")]
        public string? CaseType { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? FullMoneyAmmount { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "*")]
        public string? Investigation { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? YearID { get; set; }
        public IEnumerable<SelectListItem>? Years { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? MonthID { get; set; }
        public IEnumerable<SelectListItem>? Months { get; set; }
    }
}
