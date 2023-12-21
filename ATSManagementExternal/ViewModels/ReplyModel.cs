namespace ATSManagementExternal.ViewModels
{
    public class ReplyModel
    {
        public int? RecId { get; set; }
        public string? SentReport { get; set; }
        public string? OfficialLetter { get; set; }
        public Guid? InstId { get; set; }
        public string? SendingRemark { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? ExpectedReplyDate { get; set; }
        public string? ResponseDetail { get; set; }
        public Guid? RepliedBy { get; set; }
        public DateTime? RespondedDate { get; set; }
        public Guid? SentBy { get; set; }
        public int ReplyId { get; set; }

        public IFormFile? Attachement { get; set; }
    }
}
