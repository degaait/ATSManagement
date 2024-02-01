using System.ComponentModel.DataAnnotations;

namespace ATSManagementExternal.ViewModels
{
    public class LoginModels
    {
        public Guid UserId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "User name")]
        public string? UserName { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }
        public Guid? DepId { get; set; }
        public string? DepName { get; set; }
        public string? UserFullName { get; set; }
        public string? ModuleName { get; set; }
        public Guid? Id { get; set; }


        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Confirmation Password is required.")]
        [Compare("NewPassword", ErrorMessage = "New password and Confirmation password must match.")]
        public string? ConfirmPassword { get; set; }
    }
}
