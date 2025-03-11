using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
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
        [Display(Name = "Document type")]
        public Guid? DocId { get; set; }
        public List<SelectListItem>? LegalStadiesDocumenttypes { get; set; }
        public bool IsDeputyApprovalNeeded { get; set; }
        [Display(Name = "Status")]
        [Required(ErrorMessage = "*")]
        public Guid? ExternalRequestStatusID { get; set; }
        [Required(ErrorMessage = "*")]
        public List<SelectListItem>? ExternalStatus { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? AssigneeTypeId { get; set; }
        public IEnumerable<SelectListItem>? AssignmentTypes { get; set; }
        public DateTime? DueDate { get; set; }

        [Required(ErrorMessage = "*")]
        public Guid[]? AssignedTo { get; set; }

        [Display(Name = "Assigned to")]
        public List<SelectListItem>? AssignedTos { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? ServiceTypeId { get; set; }

        [Display(Name = "Service types")]
        public List<SelectListItem>? ServiceTypes { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? TeamId { get; set; }
        public IEnumerable<SelectListItem?>? Teams { get; set; }
        public DateTime? AssignedDate { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? AssignedBy { get; set; }
        [Display(Name = "Assigned by")]
        public List<SelectListItem>? AssignedBys { get; set; }
        [Display(Name = "Created By")]
        public Guid? CreatedBy { get; set; }
        [Display(Name = "Final Report")]
        public IFormFile? finalReport { get; set; }
        [Display(Name = "Summary")]
        public string? FinalReportSummary { get; set; }

        [Required(ErrorMessage = "*")]
        [Display(Name = "Assignement remark")]
        [DataType(DataType.MultilineText)]
        public string? AssingmentRemark { get; set; }
        [Display(Name = "Desicion status")]
        [Required(ErrorMessage = "*")]
        public Guid DesStatusId { get; set; }
        [Required(ErrorMessage = "*")]
        public List<SelectListItem>? DesicionStatus { get; set; }
        [Display(Name = "Desicion Remark")]
        public string? DescissionRemark { get; set; }

        [Required(ErrorMessage = "*")]
        public Guid? InistId { get; set; }
        [Display(Name = "Institutions")]
        public List<SelectListItem>? Intitutions { get; set; }
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
        public Guid[]? DepId { get; set; }
        [Display(Name = "Departments")]
        public List<SelectListItem>? Deparments { get; set; }

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


        [Display(Name = "Document File")]
        public IFormFile? DocumentFile { get; set; }
        [Display(Name = "Document File")]
        public IEnumerable<IFormFile>? MultipleFiles { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? PriorityId { get; set; }
        [Display(Name = "Priority")]
        public List<SelectListItem>? Priorities { get; set; }


        [Display(Name = "Addional Questions")]
        public List<CheckBoxItem>? PrioritiesQues { get; set; }

        [Display(Name = "Full Name")]
        public string? FullName { get; set; }
        [RegularExpression(@"^([0-9a-zA-Z]([\+\-_\.][0-9a-zA-Z]+)*)+@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,3})$", ErrorMessage = "Your email address is not in a valid format. Example of correct format: joe.example@example.org")]
        [DataType(DataType.EmailAddress)]
        public string? EmailAddress { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Numebr")]
        [RegularExpression(@"^(\d{10})$", ErrorMessage = "Wrong mobile")]
        public string? PhoneNumber { get; set; }
        public string? TermsAndCondionts { get; set; }

        public Guid? CompleteRequestID { get; set; }
        public List<SelectListItem>? CompletedRequests { get; set; }
        public List<SelectListItem>? RoundTypes { get; set; }
        [Required(ErrorMessage = "*")]
        public int RoundTypeId { get; set; }
        [DataType(DataType.PhoneNumber, ErrorMessage = "Only Numbers")]
        public string? MoneyAmount { get; set; }
        public string? CurrencyId { get; set; }
        public List<SelectListItem>? Currency { get; set; }



        //New Rows
        [Display(Name = "ADR Type")]
        public List<SelectListItem>? Adrtypes { get; set; }
        public String? ADRTypeId { get; set; }

        public List<SelectListItem>? AdrStatus { get; set; }
        public string? AdrStatusId { get; set; }
        public List<SelectListItem>? AdrResults { get; set; }
        public string? AdrResultID { get; set; }
        public List<SelectListItem>? ContractNegotiations { get; set; }
        public string? ContractNegotiationId { get; set; }
        public string? ContractNegotiationsOther { get; set; }

        public List<SelectListItem>? ContractNegotiationResult { get; set; }
        public string? ContractNegotiationResultId { get; set; }



        public List<SelectListItem>? ContractNegotiationsStatus { get; set; }
        public string? ContractNegotiationStatusId { get; set; }

        public List<SelectListItem>? InternationCaseResult { get; set; }
        public string? InternationalCaseResultID { get; set; }

        public List<SelectListItem>? InternationCaseStatus { get; set; }
        public string? InternationalCaseStatusID { get; set; }

        public List<SelectListItem>? LegalAdviceStatus { get; set; }
        public string? LegalAdviceStatusID { get; set; }

        public List<SelectListItem>? LegalAdviceResults { get; set; }
        public string? LegalAdviceResultID { get; set; }


        [Display(Name = "Claimant")]
        public string? Claimant { get; set; }
        [Display(Name = "Acting As")]
        public List<SelectListItem>? ActingAs { get; set; }
        public string? ActingAsId { get; set; }

        [Display(Name = "Respondent")]
        public string? Respondent { get; set; }
        [Display(Name = "Reasult")]
        public List<SelectListItem>? Reasults { get; set; }
        public String? ResultId { get; set; }
        [Display(Name = "Result Description")]
        public string? ResultDescription { get; set; }

        [Display(Name = "Case Type")]
        public List<SelectListItem>? CaseType { get; set; }
        public string? CaseTypeID { get; set; }

        [Display(Name = "Specialization")]
        public List<SelectListItem>? Specializations { get; set; }
        public string? SpecializationId { get; set; }

        [Display(Name = "Country")]
        public int? CountryId { get; set; }
        public List<SelectListItem>? Countries { get; set; }
        [Display(Name = "Court Center")]
        public string? CourtCenter { get; set; }
        [Display(Name = "Date Of Adjournment")]
        public DateTime? DateOfAdjournment { get; set; }
        [Display(Name = "Remark")]
        public string? Remark { get; set; }
        [Display(Name = "Litigation Type")]
        public List<SelectListItem>? LitigationTypes { get; set; }
        public string? LitigationtypeId { get; set; }

        public List<SelectListItem>? LitigationStatus { get; set; }
        public string? LitigationStatusID { get; set; }
        public List<SelectListItem>? LitigationResults { get; set; }
        public string? LitigationResultID { get; set; }

        [Display(Name = "Jursidiction")]
        public List<SelectListItem>? Jursidictions { get; set; }
        public string? JuristrictionId { get; set; }

        [Display(Name = "Bench")]
        public string? Bench { get; set; }
        [Display(Name = "Plaintiful")]
        public string? Plaintiful { get; set; }
        [Display(Name = "Defendent")]
        public string? Defendent { get; set; }
        [Display(Name = "Date of Judgement")]
        public DateTime? DateofJudgement { get; set; }
        [Required(ErrorMessage = "*")]
        public string? OtherServiceType { get; set; }
        [Required(ErrorMessage = "*")]
        public string? OtherDocumentType { get; set; }
        [Display(Name = "ምድብ")]
        public string? midib { get; set; }


    }
    public class RoundModel
    {
        [Required(ErrorMessage = "*")]
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
    //Litiigation
    public class Litigationtype
    {
        public string? LitigationtypeId { get; set; }
        public string? LitigationtypeName { get; set; }
    }
    public class Juristriction
    {
        public string? JuristrictionId { get; set; }
        public string? JuristrictionName { get; set; }
    }
    public class StatusReason
    {
        public string? StatusReasonId { get; set; }
        public string? StatusReasonName { get; set; }
    }
    public class Result
    {
        public String? ResultId { get; set; }
        public String? ResultName { get; set; }
    }
    //Adr Activities
    public class ADRType
    {
        public String? ADRTypeId { get; set; }
        public String? ADRTypeName { get; set; }
    }
    public class ActingAs
    {
        public string? ActingAsId { get; set; }
        public string? ActingAsName { get; set; }
    }
    public class ContractNegotiation
    {
        public string? ContractNegotiationId { get; set; }
        public string? ContractNegotiationName { get; set; }

    }
    public class ContractNegotiationStatus
    {
        public string? ContractNegotiationStatusId { get; set; }
        public string? ContractNegotiationStatusName { get; set; }

    }
    public class ContractNegotiationResult
    {
        public string? ContractNegotiationResultId { get; set; }
        public string? ContractNegotiationResultName { get; set; }

    }
    public class AdrStatus
    {
        public string? AdrStatusId { get; set; }
        public string? AdrStatusName { get; set; }
    }
    public class AdrResult
    {
        public string? AdrResultID { get; set; }
        public string? AdrResultName { get; set; }

    }
    public class LegalAdviceStatus
    {
        public string? LegalAdviceStatusID { get; set; }
        public string? LegalAdviceStatusName { get; set; }
    }
    public class LegalAdviceResult
    {
        public string? LegalAdviceResultID { get; set; }
        public string? LegalAdviceResultName { get; set; }
    }
    public class InternationalCaseStatus
    {
        public string? InternationalCaseStatusID { get; set; }
        public string? InternationalCaseStatusName { get; set; }
    }
    public class InternationalCaseResult
    {
        public string? InternationalCaseResultID { get; set; }
        public string? InternationalCaseResultName { get; set; }
    }
    public class LitigationStatus
    {
        public string? LitigationStatusID { get; set; }
        public string? LitigationStatusName { get; set; }
    }
    public class LitigationResult
    {
        public string? LitigationResultID { get; set; }
        public string? LitigationResultName { get; set; }
    }
    public class ServiceType
    {
        public string? ServiceTypeId { get; set; }
        public string? ServiceTypeName { get; set; }
    }
    public class Specializations
    {
        public string? SpecializationId { get; set; }
        public string? SpecializationName { get; set; }
    }
    public class CaseType
    {
        public string? CaseTypeID { get; set; }
        public string? CaseTypeName { get; set; }

    }

}
