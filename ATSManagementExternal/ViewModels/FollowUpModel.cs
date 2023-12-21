namespace ATSManagementExternal.ViewModels
{
    public class FollowUpModel
    {
        public int? FollowUpId { get; set; }

        public string? Message { get; set; }

        public Guid? RequestId { get; set; }

        public Guid? InistId { get; set; }

        public Guid? UserId { get; set; }

        public Guid? ExternalUserId { get; set; }
        public IFormFile? Attachement { get; set; }
    }
}
