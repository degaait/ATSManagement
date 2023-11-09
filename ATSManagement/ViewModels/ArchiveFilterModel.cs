using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.ViewModels
{
    public class ArchiveFilterModel
    {
        public string Year { get; set; }
        public Guid? DoctypeId { get; set; }
        public IEnumerable<SelectListItem> DocumentType { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public IEnumerable<SelectListItem> ServiceType { get; set; }
        public Guid? InstId { get; set; }
        public IEnumerable<SelectListItem> Institution { get; set; }

    }
}
