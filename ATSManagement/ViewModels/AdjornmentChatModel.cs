namespace ATSManagement.ViewModels
{
    public class AdjornmentChatModel
    {
        public int ChatId { get; set; }
        public string? ChatContent { get; set; }
        public Guid? AdjoryId { get; set; }
        public Guid? UserId { get; set; }
        public DateTime? Datetime { get; set; }
        public Guid? RequestedBy { get; set; }
        public Guid? ExpertId { get; set; }
        public Guid? DepHeadId { get; set; }
        public bool? IsDephead { get; set; }

        public bool? IsExpert { get; set; }
    }
}
