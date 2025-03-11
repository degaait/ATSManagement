using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.ViewModels
{
    public class ArchiveFilterModel
    {
        public Guid? DoctypeId { get; set; }
        public IEnumerable<SelectListItem>? DocumentType { get; set; }
        public Guid? ServiceTypeId { get; set; }
        public IEnumerable<SelectListItem>? ServiceType { get; set; }
        public Guid? InstId { get; set; }
        public IEnumerable<SelectListItem>? Institution { get; set; }

        public Guid? YearID { get; set; }
        public IEnumerable<SelectListItem>? Year { get; set; }

        public string? CurrencyId { get; set; }
        public IEnumerable<SelectListItem>? Currencies { get; set; }

        public List<TblRequest>? requests { get; set; }= new List<TblRequest>();

    }
}
