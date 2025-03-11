using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.ViewModels
{
    public class ReplyResponseModel
    {

        public int RepId { get; set; }
        [Display(Name = "Commented File")]
        public IFormFile? ReportFiles { get; set; }
        [Display(Name = "Created by")]
        public Guid CreatedBy { get; set; }
        [Display(Name = "Created date")]
        public DateTime? CreatedDate { get; set; }

        public Guid? Id { get; set; }
        [Display(Name = "Feedback(Highlight)")]
        public string? Feedback { get; set; }
        [Display(Name = "Specific Plan")]
        public Guid? SpecificPlanId { get; set; }
        public bool ForDeputy { get; set; }
        public bool ForExpert { get; set; }
        public bool forDepartment { get; set; }

        public IEnumerable<SelectListItem>? status { get; set; }

        public Guid? StatusID { get; set; }
    }
}
