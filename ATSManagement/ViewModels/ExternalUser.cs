using ATSManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class ExternalUser
    {
        public Guid ExterUserId { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "*")]
        public string? UserName { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "*")]
        public string? Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("Password", ErrorMessage = "New password and Confirmation password must match.")]
        public string? ConfirmPassword { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Middle Name")]
        public string? MiddleName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "User Email")]
        public string? EmailAddress { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Is Active?")]
        public bool IsActive { get; set; }


        [Required(ErrorMessage = "*")]
        [Display(Name = "Inistitution")]
        public Guid? InistId { get; set; }
        public IEnumerable<TblInistitution>? Inistitutions { get; set; }
    }
}
