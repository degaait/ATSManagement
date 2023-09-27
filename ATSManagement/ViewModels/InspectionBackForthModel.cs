using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class InspectionBackForthModel
    {
        public Guid SubMissionId { get; set; }
        [Display(Name = "Recommendation Title")]
        [Required(ErrorMessage = "*")]
        public IFormFile? ReComendationFile { get; set; }
        [Display(Name = "Request status")]
        public string? RequestStatus { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Recommendation Summary")]
        public string? RecomendationDetail { get; set; }
        [Display(Name = "Recommendation Feedback")]
        [Required(ErrorMessage = "*")]
        public string? RecomendationFeedBack { get; set; }
        [Display(Name = "Requested Date")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime? RequestedDate { get; set; }
        [Display(Name = "Expected response Date")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Date)]
        public DateTime? ExpectedResponseDate { get; set; }
        [Display(Name = "Institutions")]
        [Required(ErrorMessage = "*")]
        public Guid? InstitutionId { get; set; }

        [Display(Name = "Institutions")]
        public List<SelectListItem>? Institutions { get; set; }

        [Display(Name = "Response status")]
        public string? ResponseStatus { get; set; }

        [Display(Name = "Submitted by")]
        public Guid? SubmittedBy { get; set; }
        [Display(Name = "Responded by")]
        public Guid? ReturnedBy { get; set; }
    }
}
