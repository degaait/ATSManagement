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
        public IEnumerable<SelectListItem>? Intitutions { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? ExterUserId { get; set; }
        [Display(Name = "Requested by")]
        public IEnumerable<SelectListItem>? RequestedBys { get; set; }
        [Display(Name = "Created By")]
        public Guid? CreatedBy { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? DepId { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? CaseTypeId { get; set; }

        [Required(ErrorMessage = "*")]
        public Guid? AssignedBy { get; set; }
        [Display(Name = "Assigned by")]
        public IEnumerable<SelectListItem>? AssignedBys { get; set; }

        public DateTime? AssignedDate { get; set; }

        public DateTime? DueDate { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Assignement remark")]
        [DataType(DataType.MultilineText)]
        public string? AssingmentRemark { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? AssignedTo { get; set; }

        [Display(Name = "Assigned to")]
        public IEnumerable<SelectListItem>? AssignedTos { get; set; }

        public string? ProgressStatus { get; set; }
        [Display(Name = "Is Upproved by User")]

        public bool? IsUpprovedByUser { get; set; }
        [Display(Name = "Is Upproved by Team")]
        public bool? IsUprovedByTeam { get; set; }
        [Display(Name = "Is Upproved by Deputy?")]
        public bool? IsUprovedByDeputy { get; set; }
        [Display(Name = "Assigned to")]

        public string? TopStatus { get; set; }

        public Guid? PriorityId { get; set; }
        [Display(Name = "Priority")]
        public IEnumerable<SelectListItem>? Priorities { get; set; }

        [Display(Name = "Case types")]
        public IEnumerable<SelectListItem>? CaseTypes { get; set; }
        [Display(Name = "Departments")]
        public IEnumerable<SelectListItem>? Deparments { get; set; }

        [Display(Name = "Requested Date")]
        public DateTime? RequestedDate { get; set; }





    }
}
