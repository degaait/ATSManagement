namespace ATSManagement.ViewModels
{
    public class WitnessEvidenceModel
    {
        public Guid WitnessId { get; set; }

        public string? WitnessesName { get; set; }

        public string? EvidenceFiles { get; set; }

        public string? EvidenceVideos { get; set; }

        public Guid? RequestId { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
