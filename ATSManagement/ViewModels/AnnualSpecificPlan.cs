using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class AnnualSpecificPlan
    {
        public Guid SpecificPlanId { get; set; }
        [Display(Name = "Specific plan Title")]
        [Required(ErrorMessage = "*")]
        public string? Title { get; set; }
        [Display(Name = "Description")]

        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string? Description { get; set; }
        [Display(Name = "Created date")]
        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Modification date")]
        public DateTime? ModificationDate { get; set; }
        [Display(Name = "Created by")]
        public Guid? CreatedBy { get; set; }
        [Display(Name = "Annual Plan")]
        public Guid? InspectionPlanId { get; set; }
        public int? PlanCatId { get;  set; }

        [Display(Name = "Inistitutions")]
        public List<SelectListItem>? Inistitutions { get; set; }
       
        public Guid[]? InistId { get; set; }
    }
}
