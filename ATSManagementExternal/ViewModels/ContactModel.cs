using System.ComponentModel.DataAnnotations;

namespace ATSManagementExternal.ViewModels
{
    public class ContactModel
    {
        [Required(ErrorMessage = "*")]
        [Display(Name = "Message")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string? ContactDetaiMessage { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string? ContactEmail { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string? ContactPhoneNumber { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Country")]
        public string? ContactCountry { get; set; }
        [Display(Name = "File")]
        public IFormFile? formFile { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "*")]
        public string? FullName { get; set; }
    }
}
