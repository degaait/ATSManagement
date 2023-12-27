using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class CivilJusticeExternalRequestModel
    {
        public Guid RequestId { get; set; }
        [Required(ErrorMessage = "*")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Request Detail")]
        public string? RequestDetail { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Creation Date")]
        [DataType(DataType.DateTime)]
        public DateTime? CreatedDate { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? InistId { get; set; }
        [Display(Name = "Institutions")]
        public List<SelectListItem>? Intitutions { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? ExterUserId { get; set; }
        [Display(Name = "Requested by")]
        public List<SelectListItem>? RequestedBys { get; set; }
        [Display(Name = "Created By")]
        public Guid? CreatedBy { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? DepId { get; set; }


        [Required(ErrorMessage = "*")]
        public Guid? AssignedBy { get; set; }
        [Display(Name = "Assigned by")]
        public List<SelectListItem>? AssignedBys { get; set; }

        public DateTime? AssignedDate { get; set; }

        public DateTime? DueDate { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Assignement remark")]
        [DataType(DataType.MultilineText)]
        public string? AssingmentRemark { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid[] AssignedTo { get; set; }

        [Display(Name = "Assigned to")]
        public List<SelectListItem>? AssignedTos { get; set; }

        [Display(Name = "Is Upproved by User")]
        public bool? IsUpprovedByUser { get; set; }
        [Display(Name = "Is Upproved by Team")]
        public bool? IsUprovedByTeam { get; set; }
        [Display(Name = "Is Upproved by Deputy?")]
        public bool? IsUprovedByDeputy { get; set; }
        [Display(Name = "Assigned to")]

        public string? TopStatus { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? PriorityId { get; set; }
        [Display(Name = "Priority")]
        public List<SelectListItem>? Priorities { get; set; }

        [Required(ErrorMessage = "*")]
        public Guid? ServiceTypeId { get; set; }

        [Display(Name = "Service types")]
        public List<SelectListItem>? ServiceTypes { get; set; }
        [Display(Name = "Departments")]
        public List<SelectListItem>? Deparments { get; set; }

        [Display(Name = "Requested Date")]
        public DateTime? RequestedDate { get; set; }
        [Display(Name = "Document type")]
        public Guid? DocId { get; set; }
        public List<SelectListItem>? LegalStadiesDocumenttypes { get; set; }
        [Display(Name = "Question type")]

        public Guid? QuestTypeId { get; set; }
        public List<SelectListItem>? LegalStadiesQuestiontypes { get; set; }
        [Display(Name = "Final Report")]
        public IFormFile finalReport { get; set; }
        [Display(Name = "Request letter")]
        public IFormFile RequestLetter { get; set; }
        [Display(Name = "Adjorney Date")]
        [DataType(DataType.DateTime)]
        public DateTime? AdjorneyDate { get; set; }
        [Display(Name = "Status")]
        [Required(ErrorMessage = "*")]
        public Guid? ExternalRequestStatusID { get; set; }
        [Required(ErrorMessage = "*")]
        public List<SelectListItem>? ExternalStatus { get; set; }

        [Display(Name = "Desicion status")]
        [Required(ErrorMessage = "*")]
        public Guid DesStatusId { get; set; }
        [Required(ErrorMessage = "*")]
        public List<SelectListItem>? DesicionStatus { get; set; }

        //Additional information for requester
        [Display(Name ="Full Name")]
        public string? FullName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Email id is required")]
        public string? EmailAddress { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name ="Phone Numebr")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "*")]
        public Guid? AssigneeTypeId { get; set; }
        public IEnumerable<SelectListItem>? AssignmentTypes { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? TeamId { get; set; }
        public IEnumerable<SelectListItem?>? Teams { get; set; }
        public bool IsDeputyApprovalNeeded {  get; set; }
        [Display(Name = "Round")]
        public int? Round;
        public Guid? TypeId { get; set; }

        [Display(Name = "Request types")]
        public List<SelectListItem>? RequestTypes { get; set; }
        [Display(Name = "Request Date")]
        [DataType(DataType.DateTime)]
        public DateTime? AppointmentDate { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "External User")]
        public IEnumerable<TblExternalUser>? ExterUser { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Institutions")]
        public Guid? IntId { get; set; }
        public List<SelectListItem>? LegalStadiesCasetypes { get; set; }
        public Guid? CaseTypeId { get; set; }

        [Display(Name = "Case types")]
        public List<SelectListItem>? CaseTypes { get; set; }
        
        [Required(ErrorMessage = "*")]
        public Guid? ServiceTypeID { get; set; }
        [Display(Name = "Document File")]
        public IFormFile DocumentFile { get; set; }
        [Display(Name = "Addional Questions")]
        public List<CheckBoxItem>? PrioritiesQues { get; set; }
        public string? TermsAndCondionts { get; set; }
        public Guid? CompleteRequestID { get; set; }
        public List<SelectListItem>? CompletedRequests { get; set; }
        public List<SelectListItem>? RoundTypes { get; set; }
        public int RoundTypeId { get; set; }
    }
    public class RoundModel
    {
        public int RoundTypeId { get; set; }
        public string? Name { get; set; }

    }
    public class CompletedRequests
    {
        public Guid CompleteRequestID { get; set; }
        public string? RequestDetail { get; set; }
    }



}
