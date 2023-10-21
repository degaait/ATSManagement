namespace ATSManagement.ViewModels
{
    public class WitnessEvidenceModel
    {
        public Guid WitnessId { get; set; }

        public string? WitnessesName { get; set; }

        public IFormFile? EvidenceFiles { get; set; }

        public IFormFile? EvidenceVideos { get; set; }

        public Guid? RequestId { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
