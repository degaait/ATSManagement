using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class ActivityModel
    {
        public Guid? ActivityId { get; set; }
        [DataType(DataType.MultilineText)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string? ActivityDetail { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime? AddedDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string? TimeTakenTocomplete { get; set; }
        [Required(ErrorMessage = "*")]
        public string? Remark { get; set; }
        public Guid? RequestId { get; set; }

        public Guid? CreatedBy { get; set; }
    }

    public class InspectionActivityModel
    {
        public int ActivityId { get; set; }
        [DataType(DataType.MultilineText)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string? ActivityDetail { get; set; }
        [Required(ErrorMessage = "*")]
        public DateTime? AddedDate { get; set; }
        [Required(ErrorMessage = "*")]
        public string? TimeTakenTocomplete { get; set; }
        [Required(ErrorMessage = "*")]
        public string? Remark { get; set; }

        public Guid? CreatedBy { get; set; }
        public Guid? SpecificPlanId { get; set; }
    }
}
