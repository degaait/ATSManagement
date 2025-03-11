using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.ViewModels
{
    public class WitnessEvidenceModel
    {
        public Guid WitnessId { get; set; }
        public string? WitnessesName { get; set; }
        public string? Address { get; set; }
        public IFormFile? EvidenceFiles { get; set; }
        public IFormFile? EvidenceVideos { get; set; }
        public Guid? RequestId { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? TypeId { get; set; }
        public IEnumerable<SelectListItem>? EvidenceTypes { get; set; }
    }
}
