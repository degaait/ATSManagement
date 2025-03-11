namespace ATSManagementExternal.ViewModels
{
    public class AppointmentChatModel
    {
        public int ChatId { get; set; }

        public string? ChatContent { get; set; }

        public Guid? AppointmentId { get; set; }

        public bool? IsEnternal { get; set; }

        public bool? IsInternal { get; set; }

        public Guid? UserId { get; set; }

        public Guid? ExterUserId { get; set; }

        public DateTime? Datetime { get; set; }
        public Guid? RequestedBy { get; set; }
    }
}
