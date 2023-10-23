using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class AppointmentModel
    {

        [Required(ErrorMessage = "*")]
        public Guid? AppointmentID { get; set; }
        public String? AppointmentDetail { get; set; }
        [Display(Name = "Requested Date")]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Institution")]
        public String? Institution { get; set; }
        public Guid[]? UserId { get; set; }
        [Display(Name = "Users")]
        public IEnumerable<SelectListItem>? Users { get; set; }
    }
}
