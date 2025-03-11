using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class InspectionAssignModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Assigned to")]
        public List<SelectListItem>? Users { get; set; }
        [Display(Name = "Assign to")]
        [Required(ErrorMessage = "*")]
        public Guid[]? UserId { get; set; }
        public Guid? AssignedBy { get; set; }
        [Display(Name = "Inspection title")]
        [Required(ErrorMessage = "*")]
        public string? PlanTitle { get; set; }
        //public Guid? PlanId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Assigned Date")]
        public DateTime? AssignedDate { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Delivery Date")]
        public DateTime? DueDate { get; set; }
        [Display(Name = "Evaluation checklists")]
        [DisplayFormat(HtmlEncode = true)]
        public string? EvaluationCheckLists { get; set; }

        [Display(Name = "Evaluation checklists(Attachement)")]
        public IFormFile? EvaluationCheckListsAttachmet { get; set; }

        [Display(Name = "Engagement Letter")]
        public IFormFile? EngagementLetter { get; set; }

        [Display(Name = "Meeting Letter")]
        public IFormFile? MeetingLetter { get; set; }
        [Display(Name = "Remark")]
        [DataType(DataType.MultilineText)]
        [MaxLength(255)]
        public string? Remark { get; set; }
        [Display(Name = "Plan status")]
        public string? ProgressStatus { get; set; }

        [Display(Name = "Status")]
        public List<SelectListItem>? status { get; set; }
        [Required(ErrorMessage = "Required")]
        public Guid? StatusID { get; set; }

        [Display(Name = "Final Report")]
        public IFormFile? FinalReport { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? AssigneeTypeId { get; set; }
        public IEnumerable<SelectListItem>? AssignmentTypes { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? TeamId { get; set; }
        public IEnumerable<SelectListItem?>? Teams { get; set; }

        [Display(Name = "Plan status")]
        public Guid? DesStatusId { get; set; }

        [Display(Name = "Status")]
        public List<SelectListItem>? DesicionStatus { get; set; }
        public bool IsDeputyApprovalNeeded { get; set; }

        [Display(Name = "Final of final Letter")]
        public IFormFile? SentReport { get; set; }
        [Display(Name = "Official letter")]
        public IFormFile? OfficialLetter { get; set; }
        public string? SendingRemark { get; set; }
        public Guid? InistId { get; set; }
        public IEnumerable<SelectListItem>? Insititutions { get; set; }
        public DateTime? ExpectedReplyDate { get;  set; }

        public Guid? SpecificPlanId {  get; set; }
        public int? PlanCatId { get; set; }

        [Display(Name = "TOR Attachment")]
        public IFormFile? TORAttachment { get; set; }
        [Display(Name = "Department Review")]
        public IFormFile? DepartmentReview { get; set; }
        public IFormFile? ExitConfirence { get; set; }

    }
}
