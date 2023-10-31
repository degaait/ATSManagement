using ATSManagementExternal.Models;
using System.ComponentModel.DataAnnotations;

namespace ATSManagementExternal.ViewModels
{
    public class AppointmentModel
    {
        public Guid? AppointmentId { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true)]
        [Display(Name = "Request Detail")]
        public string? AppointmentDetail { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Institutions")]
        public Guid? IntId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Institutions")]
        public IEnumerable<TblInistitution>? Intitutions { get; set; }
        [Required(ErrorMessage = "*")]
        public Guid? ExterUserId { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "External User")]
        public IEnumerable<TblExternalUser>? ExterUser { get; set; }
        [Display(Name = "Expected Appointment Date")]
        public DateTime? AppointmentDate { get; set; }
    }
}
