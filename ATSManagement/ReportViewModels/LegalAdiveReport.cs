using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ReportViewModels
{
    public class LegalAdiveReport
    {
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage ="*")]
        public string? WomensAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? WomensInvestigation { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? ChildrensAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? ChildrensInvestigation { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? EldersAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? EldersInvestigation { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? DisablesAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? DisablesInvestigation { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? HIVsAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? HIVsInvestigation { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? PersecutionReturnsAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? PersecutionReturnsInvestigation { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? MensAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? MensInvestigation { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? GovernmentInstitutionsAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? GovernmentInstitutionsInvestigation { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? OthersAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? OthersInvestigation { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? TotalAmmount { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "*")]
        public string? TotalInvestigation { get; set; }
        public Guid? YearID { get; set; }
        public IEnumerable<SelectListItem>? Years { get; set; }
        public Guid? MonthID { get; set; }
        public IEnumerable<SelectListItem>? Months { get; set; }
    }
}
