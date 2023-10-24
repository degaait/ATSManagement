using ATSManagementExternal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagementExternal.ViewModels
{
    public class CivilJusticeExternalRequestModel
    {
        public Guid RequestId { get; set; }
        [Required(ErrorMessage = "*")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Request Detail")]
        public string? RequestDetail { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Request Detail")]
        public string? AppointmentDetail { get; set; }

        public Guid? TypeId { get; set; }

        [Display(Name = "Request types")]
        public List<SelectListItem>? RequestTypes { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Request Date")]
        [DataType(DataType.DateTime)]

        public DateTime? RequestedDate { get; set; }

        [Display(Name = "Request Date")]
        [DataType(DataType.DateTime)]

        public DateTime? AppointmentDate { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Request Detail")]
        public Guid? ExterUserId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Request Detail")]
        public IEnumerable<TblExternalUser>? ExterUser { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Request Detail")]
        public Guid? IntId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Request Detail")]
        public IEnumerable<TblInistitution>? Intitutions { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? DepId { get; set; }

        [Display(Name = "Departments")]
        public List<SelectListItem>? Deparments { get; set; }

        public Guid? DocId { get; set; }
        public List<SelectListItem>? LegalStadiesCasetypes { get; set; }
        [Display(Name = "Question type")]
        public Guid? QuestTypeId { get; set; }
        public List<SelectListItem>? LegalStadiesQuestiontypes { get; set; }
        public Guid? CaseTypeId { get; set; }

        [Display(Name = "Case types")]
        public List<SelectListItem>? CaseTypes { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        public Guid? ServiceTypeID { get; set; }

        [Display(Name = "Service types")]
        public List<SelectListItem>? ServiceTypes { get; set; }


        [Display(Name = "Document File")]
        public IFormFile DocumentFile { get; set; }

        public Guid? PriorityId { get; set; }
        [Display(Name = "Priority")]
        public List<SelectListItem>? Priorities { get; set; }
    }
}
