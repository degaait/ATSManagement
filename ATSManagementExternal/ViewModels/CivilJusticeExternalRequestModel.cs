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
        [Required(ErrorMessage = "*")]
        [Display(Name = "Request Date")]
        [DataType(DataType.DateTime)]

        public DateTime? RequestedDate { get; set; }

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
    }
}
