using ATSManagementExternal.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagementExternal.ViewModels
{
    public class CollectionModel
    {
        internal List<TblRecomendation>? tblRecomendations { get; set; }
        public Guid RecostatusId { get; set; }
        public IEnumerable<SelectListItem>? tblRecomendationsStatus { get; set; }
        public Guid InistId { get; set; }
        public IEnumerable<SelectListItem>? tblInistitutions { get; set; }
        public Guid YearId { get; set; }
        public IEnumerable<SelectListItem>? tblYears { get; set; }
        public Guid LawId { get; set; }
        public IEnumerable<SelectListItem>? tblInspectionLaws { get; set; }

        public Guid RecoId { get; set; }
        public IEnumerable<SelectListItem>? tblRecomendings { get; set; }

    }
}
