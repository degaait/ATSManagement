using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class InternalRequestModel
    {
        public Guid RequestId { get; set; }
        [Required(ErrorMessage = "*")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Request Detail")]
        public string? RequestDetail { get; set; }

        public Guid? RequestedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public bool? IsAssignedToexpert { get; set; }

        public Guid? AssignedBy { get; set; }

        public DateTime? AssignedDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string? AssingmentRemark { get; set; }

        public Guid? RequestStatusId { get; set; }

        public Guid? UserUpprovalStatus { get; set; }

        public Guid? TeamUpprovalStatus { get; set; }

        public Guid? DepartmentUpprovalStatus { get; set; }

        public string? FinalReport { get; set; }       
        [Required(ErrorMessage = "*")]
        public Guid? ServiceTypeId { get; set; }

        [Display(Name = "Service types")]
        public List<SelectListItem>? ServiceTypes { get; set; }

        public bool? IsArchived { get; set; }

        public string? SentReport { get; set; }
        [Display(Name = "Document File")]
        public IEnumerable<IFormFile>? MultipleFiles { get; set; }
        public DateTime? SentDate { get; set; }

        public string? Remark { get; set; }

        public string? StatusDescription { get; set; }

        public string? TeamDesicionRemark { get; set; }

        public string? DepartmentDesicionRemark { get; set; }

        public string? FinalReportSummary { get; set; }
        [Display(Name = "Created By")]
        public Guid? CreatedBy { get; set; }
        [Display(Name = "Final Report")]
        public IFormFile? finalReport { get; set; }
        public int OrderId { get; set; }

        public string? MoneyAmount { get; set; }
        public string? CurrencyId { get; set; }
        public List<SelectListItem>? Currency { get; set; }


        [Required(ErrorMessage = "*")]
        public Guid? AssigneeTypeId { get; set; }
        public IEnumerable<SelectListItem>? AssignmentTypes { get; set; }

        [Required(ErrorMessage = "*")]
        public Guid[]? AssignedTo { get; set; }

        [Display(Name = "Assigned to")]
        public List<SelectListItem>? AssignedTos { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? TeamId { get; set; }
        public IEnumerable<SelectListItem?>? Teams { get; set; }

    }
}
