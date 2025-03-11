namespace ATSManagement.ViewModels
{
    public class RequestChatModel
    {
        public int ChatId { get; set; }

        public string? ChatContent { get; set; }

        public bool? IsInternal { get; set; }

        public Guid? UserId { get; set; }

        public DateTime? Datetime { get; set; }

        public bool? IsDephead { get; set; }

        public bool? IsExpert { get; set; }

        public Guid? RequestId { get; set; }
        public Guid? ExpertId { get; set; }
        public Guid? DepHeadId { get; set; }


        public Guid? SendBy { get; set; }

        public Guid? SendTo { get; set; }

        public IFormFile? FilePath { get; set; }

        public bool? ForStateMinister { get; set; } = false;

    }
}
