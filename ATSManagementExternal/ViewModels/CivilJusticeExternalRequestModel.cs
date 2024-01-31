using ATSManagementExternal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagementExternal.ViewModels
{
    public class CivilJusticeExternalRequestModel
    {

        [Display(Name = "Round")]
        public int? Round;
        public Guid RequestId { get; set; }
        [Required(ErrorMessage = "*")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Request Detail")]
        public string? RequestDetail { get; set; }
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
        public Guid? ExterUserId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "External User")]
        public IEnumerable<TblExternalUser>? ExterUser { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Institutions")]
        public Guid? IntId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Institutions")]
        public IEnumerable<TblInistitution>? Intitutions { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? DepId { get; set; }

        [Display(Name = "Departments")]
        public List<SelectListItem>? Deparments { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? DocId { get; set; }
        public List<SelectListItem>? LegalStadiesCasetypes { get; set; }
        [Display(Name = "Question type")]
        public Guid? QuestTypeId { get; set; }
        public List<SelectListItem>? LegalStadiesQuestiontypes { get; set; }
        public Guid? CaseTypeId { get; set; }

        [Display(Name = "Case types")]
        public List<SelectListItem>? CaseTypes { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "*")]
        public Guid? ServiceTypeID { get; set; }

        [Display(Name = "Service types")]
        public List<SelectListItem>? ServiceTypes { get; set; }


        [Display(Name = "Document File")]
        public IFormFile DocumentFile { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? PriorityId { get; set; }
        [Display(Name = "Priority")]
        public List<SelectListItem>? Priorities { get; set; }


        [Display(Name = "Addional Questions")]
        public List<CheckBoxItem>? PrioritiesQues { get; set; }

        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "*")]
        public string? FullName { get; set; }
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Your email address is not in a valid format. Example of correct format: joe.example@example.org")]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Numebr")]
        [Required(ErrorMessage = "*")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string? PhoneNumber { get; set; }
        public string? TermsAndCondionts { get; set; }

        public Guid? CompleteRequestID { get; set; }
        public List<SelectListItem>? CompletedRequests { get; set; }
        public List<SelectListItem>? RoundTypes { get; set; }
        public int RoundTypeId { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Only Numbers")]
        public String? MoneyAmount { get; set; }
        public string? CurrencyId { get; set; }
        public List<SelectListItem>? Currency { get; set; }



        //
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
    public class CurrencyModel
    {
        public string? CurrencyId { get; set; }
        public string? CurrencyName { get; set; }
    }
}
