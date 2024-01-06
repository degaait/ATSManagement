namespace ATSManagement.ViewModels
{
    public class AnnualPlanCatagory
    {
        public int PlanCatId { get; set; }
        public string? CatTitle { get; set; }
        public string? CatDescription { get; set; }
        public Guid? InspectionPlanId { get; set; }
    }
}
