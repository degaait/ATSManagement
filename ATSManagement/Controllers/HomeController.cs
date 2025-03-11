using System.Diagnostics;
using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.IModels;
using ATSManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        private LanguageService _localization;
        private readonly IHttpContextAccessor _contextAccessor;
        public HomeController(ILogger<HomeController> logger, IMailService mail, AtsdbContext atsdbContext, INotyfService notyfService, LanguageService localization, IHttpContextAccessor contextAccessor)
        {
            _notifyService = notyfService;
            _logger = logger;
            _mail = mail;
            _context = atsdbContext;
            _localization = localization;
            _contextAccessor = contextAccessor;
        }
        public async Task<IActionResult> Index(string? type, string? message)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var user=_context.TblInternalUsers.Include(s=>s.Dep).Where(s=>s.UserId == userId).FirstOrDefault();
            var assignedReq = _context.TblRequestAssignees.Where(x => x.UserId == userId).ToList();
            var assignedInternal = _context.TblInternalRequestAssignees.Where(s => s.UserId == userId).ToList();
            if (type == "0")
            {
                _notifyService.Error(message);
            }
            ViewBag.internalUser = _context.TblInternalUsers.ToList().Count;
            ViewBag.ExternalUser = _context.TblExternalUsers.ToList().Count;
            ViewBag.Insititutions = _context.TblInistitutions.ToList().Count;
            ViewBag.Inspects = _context.TblInspectionPlans.ToList().Count;
            if (user.Dep!=null)
            {
                if (user.Dep.DepCode == "FLIM")
                {
                    var assigned = (from items in _context.TblSpecificPlans
                                    join assing in _context.TblPlanCatagories on items.PlanCatId equals assing.PlanCatId
                                    where items.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847") && items.TeamId == user.TeamId
                                    select items).ToList();
                    var atsdbContext = _context.TblSpecificPlans.Include(t => t.CreatedByNavigation).Include(T => T.Pro).Include(s => s.PlanCat).Include(s => s.Team).Where(s => s.TeamId == user.TeamId && s.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847") && (s.IsAssignedToUser == false || s.IsAssignedToUser == null)).ToList();
                    var filtered = atsdbContext.Intersect(assigned).ToList();
                    ViewBag.TeamPlans = filtered.Count;
                    ViewBag.AssignedInspections = _context.TblAssignedYearlyPlans.Include(t => t.AssignedToNavigation).Include(t => t.SpecificPlan).Include(p => p.Status).Include(s => s.AssignedByNavigation).Where(a => a.AssignedTo == userId).ToList().Count;
                }
            }
            ViewBag.NewInternalRequestsAssigned = assignedInternal.Count;
            ViewBag.NewInternalRequest= _context.TblInternalRequests.Where(x => x.IsAssignedToexpert == null || x.IsAssignedToexpert == false).ToList().Count;

            ViewBag.NewRequests = _context.TblRequests.Where(x => x.IsAssignedTodepartment == null || x.IsAssignedTodepartment == false).ToList().Count;
            ViewBag.ExpertRequest = assignedReq.Count;
            ViewBag.HighPriorityRequests = _context.TblRequests.Where(x => x.PriorityId == Guid.Parse("12fba758-fa2a-406a-ae64-0a561d0f5e73")).ToList().Count;
            ViewBag.RequestsFromeCJAD = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "CVA"&&x.IsAssingedToUser==false&&x.TeamId==null).Select(a => a.RequestId).ToList().Count;
            ViewBag.RequestFromLegalStudies = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "LSDC" && x.IsAssingedToUser == false && x.TeamId == null).ToList().Count;
            ViewBag.ReturnedRequests = _context.TblRequests.Where(s => s.IsSenttoInst == true).ToList().Count;
            return View();
        }

        private async Task SendMail()
        {
            List<string> to = new List<string>();
            to.Add("degaait@gmail.com");
            MailData data = new MailData(to, "Test Email", "<h1>H1 tag response</h1>", "degaait@gmail.com");
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<object> InstChart()
        {
            List<object> data = new List<object>();
            List<string> instaName = new List<string>();
            List<int> instNumber = new List<int>();
            var genderGroups = (from items in _context.TblRequests
                                join ints in _context.TblInistitutions on items.InistId equals ints.InistId

                                group items by items.Inist.Name into g
                                select new
                                {
                                    InstName = g.Key,
                                    InstNumber = g.Count()
                                }).ToList();

            foreach (var item in genderGroups)
            {
                instaName.Add(item.InstName);
                instNumber.Add(item.InstNumber);
            }
            data.Add(instaName);
            data.Add(instNumber);


            return data;
        }
        public List<object> StatusChart()
        {
            List<object> data = new List<object>();
            List<string> statusName = new List<string>();
            List<int> statusNumber = new List<int>();

            var genderGroups = (from items in _context.TblRequests
                                join ints in _context.TblTopStatuses on items.TopStatusId equals ints.TopStatusId

                                group items by items.TopStatus.StatusName into g
                                select new
                                {
                                    StatusName = g.Key,
                                    StatusNumber = g.Count()
                                }).ToList();
            foreach (var item in genderGroups)
            {
                statusName.Add(item.StatusName);
                statusNumber.Add(item.StatusNumber);
            }
            data.Add(statusName);
            data.Add(statusNumber);
            return data;
        }

    }
}