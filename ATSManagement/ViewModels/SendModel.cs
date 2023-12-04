namespace ATSManagement.ViewModels
{
    public class SendModel
    {
        public Guid? RequestId { get; set; }
        public IFormFile? ApprovalLetter { get; set; }
        public IFormFile? FinalReport { get; set; }
        public DateTime SentDate { get; set; }
        public string? SendingRemark { get;set; }
        public string? returningRemark {  get; set; }
    }
}
