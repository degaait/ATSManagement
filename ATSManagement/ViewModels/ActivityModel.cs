using System.ComponentModel.DataAnnotations;

namespace ATSManagement.ViewModels
{
    public class ActivityModel
    {
        public Guid ActivityId { get; set; }
        [DataType(DataType.MultilineText)]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public string? ActivityDetail { get; set; }

        public DateTime? AddedDate { get; set; }

        public Guid? RequestId { get; set; }

        public Guid? CreatedBy { get; set; }
    }
}
