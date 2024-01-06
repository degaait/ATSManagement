using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class InispectionPlan
    {
        public Guid InspectionPlanId { get; set; }
        [Display(Name = "Title")]
        [Required(ErrorMessage ="*")]
        public string? PlanTitle { get; set; }
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? PlanDescription { get; set; }
        [Display(Name = "Creation Date")]
        [DataType(DataType.DateTime)]
        public DateTime? CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public Guid? UserId { get; set; }
        [Display(Name = "Inipection Year")]
        [Required(ErrorMessage = "*")]
        public Guid? YearId { get; set; }
        [Display(Name = "Inipection Year")]       
        public IEnumerable<SelectListItem>? InspectionYear { get; set; }

        [Display(Name = "Inistitutions")]
        public List<SelectListItem>? Inistitutions { get; set; }
        [Required(ErrorMessage = "Required")]
        public Guid[] InistId { get; set; }
        public IFormFile? Attachement { get; set; }

      


    }
}
