namespace ATSManagementExternal.ViewModels
{
    public class DocumentHistoryModel
    {
        public Guid HistoryId { get; set; }

        public Guid? RequestId { get; set; }

        public string? DocPath { get; set; }

        public int? Round { get; set; }

        public string? Description { get; set; }

        public Guid? InternalReplyId { get; set; }

        public Guid? ExternalRepliedBy { get; set; }

        public string? FileDescription { get; set; }

    }
}
