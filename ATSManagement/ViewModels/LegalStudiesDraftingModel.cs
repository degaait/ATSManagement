using ATSManagement.Models;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class LegalStudiesDraftingModel
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
    }
}
