using ATSManagement.Models;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class UserModel
    {
        public Guid UserId { get; set; }

        [Display(Name = "User Name")]
        [Required(ErrorMessage = "*")]
        public string? UserName { get; set; }
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
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
        [Display(Name = "Is Superadmin?")]
        public bool IsSuperAdmin { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Is Team leader?")]
        public SpecialRoles specialRoles { get; set; }

        [Display(Name = "Department")]
        public Guid? DepId { get; set; }
        public IEnumerable<SelectListItem>? Departments { get; set; }
        [Display(Name ="Team")]
        public Guid? TeamID { get; set; }
        public IEnumerable<SelectListItem>? Teams { get; set; }


    }

    public enum SpecialRoles
    {
        [DataMember(Name = "Is Deputy?")]
        IsDeputy,
        [DataMember(Name ="Is Department head")]
        IsDepartmentHead,
        [DataMember(Name = "Is Team leader?")]
        IsTeamLeader,
        [DataMember(Name = "Is Default User?")]
        DefaultUser,
        [DataMember(Name = "Is Secretary?")]
        IsSecretary,
    }

}
