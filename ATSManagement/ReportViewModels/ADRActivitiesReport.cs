using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.ReportViewModels
{
    public class ADRActivitiesReport
    {
        public Guid? ID { get; set; }
        public string? Womens { get; set; }
        public string? Childrens { get; set; }
        public string? WomeElders { get; set; }
        public string? HIVPositives { get; set; }
        public string? Mens { get; set; }
        public string? OtherServantType { get;}
        public string? OutofResponsibilty { get; set; }
        public string? Family { get; set; }
        public string? Property { get; set; }
        public string? AsserinaSerategna { get; set; }
        public string? LelochGudayAyine { get;set; }
        public string? YediridirGenzebMeten { get; set; }
        public Guid? YearID {  get; set; }
        public IEnumerable<SelectListItem>? Years { get; set; }
        public Guid? MonthID { get; set; }
        public IEnumerable<SelectListItem>? Months { get; set; }
        public Guid? CreatedBy { get;set; }
        public Guid? TypeID { get; set; }
        public IEnumerable<SelectListItem>? ADREventType { get; set; }

    }
}
