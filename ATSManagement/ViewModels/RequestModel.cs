using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class RequestModel
    {
        public Guid? RequestId { get; set; }

        public string? RequestDetail { get; set; }

        public Guid? InistId { get; set; }

        public Guid? RequestedBy { get; set; }
        public IEnumerable<SelectListItem>? RequestedUsers { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? DepId { get; set; }

        public Guid? CaseTypeId { get; set; }

        public Guid? ServiceTypeID { get; set; }

        [Display(Name = "Service types")]
        public List<SelectListItem>? ServiceTypes { get; set; }

        [Display(Name = "Addional Questions")]
        public List<SelectListItem>? PrioritiesQues { get; set; }
        public Guid? IntId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Request Detail")]
        public IEnumerable<SelectListItem>? Intitutions { get; set; }

        [Display(Name = "Departments")]
        public List<SelectListItem>? Deparments { get; set; }

    }
}
