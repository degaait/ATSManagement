using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.ViewModels
{
    public class SubmenuModel
    {
        public Guid Id { get; set; }
        public Guid SubMenuId { get; set; }
        public string? SubMenuName { get; set; }
        public string? SubmenuAmharic { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public IEnumerable<SelectListItem>? MainMenus { get; set; }
        public Guid? MainMenuId { get; set; }
        public Guid? DepId { get; set; }
        public IEnumerable<SelectListItem>? Departments { get; set; }
        public bool forDeputy { get; set; }
        public bool forDepHead { get; set; }
        public bool forDefaulUser { get; set; }
        public bool forTeamLeader { get; set; }
        public bool forSuperAdmin { get; set; }
        public bool forBranchOfficer {  get; set; }
        public bool forSecretary {  get; set; }
        public bool forInternalUser { get; set; }
    }



}
