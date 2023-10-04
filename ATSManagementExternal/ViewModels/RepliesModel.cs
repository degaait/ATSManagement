using System.ComponentModel.DataAnnotations;

namespace ATSManagementExternal.ViewModels
{
    public class RepliesModel
    {
        public Guid ReplyId { get; set; }
        [Display(Name = "Reply")]
        [Required(ErrorMessage = "*")]
        [DataType(DataType.MultilineText)]
        public string? ReplayDetail { get; set; }

        public Guid? RequestId { get; set; }

        public Guid? InternalReplayedBy { get; set; }
        [Display(Name = "Date time")]
        public DateTime? ReplyDate { get; set; }

        public Guid? ExternalReplayedBy { get; set; }

    }
}
