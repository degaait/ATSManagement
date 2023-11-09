using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class ArchiveModel
    {
        [Display(Name = "Request Detail")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.MultilineText)]
        public string? RequestDetail { get; set; }
        public Guid? RequestId { get; set; }


    }
}
