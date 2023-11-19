using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.ReportViewModels
{
    public class InstitutionMonitoringReport
    {
        public string? MoniteredOffice { get; set; }
        public string? ContractNumber { get; set; }
        public string? ContractMoneyAmount { get; set; }
        public string? ADRNumber { get; set; }
        public string? ADRMoneyAmount { get; set; }
        public string? DebateNumber { get; set; }
        public string? DebateMoneyAmmount { get; set; }
        public string? Investigation { get; set; }
        public Guid? YearID { get; set; }
        public IEnumerable<SelectListItem>? Years { get; set; }
        public Guid? MonthID { get; set; }
        public IEnumerable<SelectListItem>? Months { get; set; }
        public Guid? CreatedBy { get; set; }
    }
}
