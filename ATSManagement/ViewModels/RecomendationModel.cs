using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class RecomendationModel
    {
        public Guid RecoId { get; set; }

        [Display(Name = "Recomendation")]
        [Required(ErrorMessage = "*")]
        public string? Title { get; set; }
        [Display(Name = "Recomendation")]
        [Required(ErrorMessage = "*")]
        [DisplayFormat(HtmlEncode = true)]
        public string? Recomendation { get; set; }
        [Display(Name = "Inistitutions")]
        public List<SelectListItem>? Inistitutions { get; set; }
        [Required(ErrorMessage = "Required")]
        public Guid? InistId { get; set; }
        [Display(Name = "Status")]
        public List<SelectListItem>? Status { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? RecostatusID { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? YearId { get; set; }
        [Display(Name = "Evaluation year")]
       
        public IEnumerable<SelectListItem>? Years { get; set; }

        [Display(Name = "Created by")]
        [Required(ErrorMessage = "*")]
        public Guid? CreatedBy { get; set; }

        [Display(Name = "Creation date")]
        public DateTime? CreatinDate { get; set; }

        [Display(Name = "Modification date")]
        public DateTime? ModifyDate { get; set; }

        [Display(Name = "IsActive?")]
        [Required(ErrorMessage = "*")]
        public bool IsActive { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? LawId { get; set; }
        public IEnumerable<SelectListItem>? Laws { get; set; }
     
        public IFormFile? FinalReport { get; set; }
    }
}
