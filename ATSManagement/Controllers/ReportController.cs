using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore;
using ATSManagement.Filters;
using EthiopianCalendar;
using Microsoft.AspNetCore.Http.Extensions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class ReportController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        private readonly INotificationService _notificationService;

        public ReportController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mail, INotyfService notifyService, INotificationService notificationService)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
            _notifyService = notifyService;
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> ArchivedRequests()
        {
            ReportsModel model = new ReportsModel();
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            if (cultur == "am")
            {
                model.ServiceType = _context.TblServiceTypes.Include(s=>s.Dep).Where(x => x.Dep.DepCode == "CVA").Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeNameAmharic
                }).ToList();
                model.Institution = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Text = x.NameAmharic,
                    Value = x.InistId.ToString()
                }).ToList();
                model.Year = _context.TblYears.Select(s => new SelectListItem
                {
                    Value = s.YearId.ToString(),
                    Text = s.Year
                }).ToList();
            }
            else
            {
                model.ServiceType = _context.TblServiceTypes.Include(s => s.Dep).Where(x => x.Dep.DepCode == "CVA").Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeName
                }).ToList();
                model.Institution = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.InistId.ToString()

                }).ToList();
                model.Year = _context.TblYears.Select(s => new SelectListItem
                {
                    Value = s.YearId.ToString(),
                    Text = s.Year

                }).ToList();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ArchivedRequests(ReportsModel Model)
        {
            ReportsModel model = new ReportsModel();
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            var depCode = _context.TblDepartments.Where(s => s.DepCode == "CVA").FirstOrDefault();
            List<TblRequest> atsdbContext = new List<TblRequest>();
            var yeaId = Model.YearID;
            if (Model.YearID != null)
            {
                var year = _context.TblYears.Find(yeaId);
                int currentYear = int.Parse(year.Year)+8;
                DateTime startdate = DateTime.Parse(new DateTime(currentYear, 1, 1).ToString("dd-MMM-yy"));
                DateTime endDate = startdate.AddYears(1);
                if ( Model.InstId == null && Model.ServiceTypeId == null)
                {
                    atsdbContext = (from item in _context.TblRequests
                                    join serviceType in _context.TblServiceTypes on item.ServiceTypeId equals serviceType.ServiceTypeId
                                    join insts in _context.TblRequestDepartmentRelations on item.RequestId equals insts.RequestId
                                    
                                    where  item.CreatedDate<=endDate &&item.CreatedDate>=startdate && insts.DepId== depCode.DepId
                                    select item
                                 ).ToList();
                }
                else if ( Model.InstId != null && Model.ServiceTypeId == null)
                {
                    atsdbContext = (from item in _context.TblRequests
                                    join serviceType in _context.TblServiceTypes on item.ServiceTypeId equals serviceType.ServiceTypeId
                                    join insts in _context.TblRequestDepartmentRelations on item.RequestId equals insts.RequestId
                                    where  item.InistId == Model.InstId &&
                                    item.CreatedDate >= startdate && item.CreatedDate <= endDate && insts.DepId == depCode.DepId
                                    select item
                       ).ToList();
                }
                else if ( Model.InstId != null && Model.ServiceTypeId != null)
                {
                    atsdbContext = (from item in _context.TblRequests
                                    join serviceType in _context.TblServiceTypes on item.ServiceTypeId equals serviceType.ServiceTypeId
                                    join insts in _context.TblRequestDepartmentRelations on item.RequestId equals insts.RequestId
                                    where  item.InistId == Model.InstId &&
                                    item.ServiceTypeId == Model.ServiceTypeId && item.CreatedDate >= startdate 
                                    && item.CreatedDate <= endDate && insts.DepId == depCode.DepId
                                    select item
                                     ).ToList();
                }
                else if ( Model.InstId == null && Model.ServiceTypeId != null)
                {
                    atsdbContext = (from item in _context.TblRequests
                                    join serviceType in _context.TblServiceTypes on item.ServiceTypeId equals serviceType.ServiceTypeId
                                    join insts in _context.TblRequestDepartmentRelations on item.RequestId equals insts.RequestId
                                    where  item.ServiceTypeId == Model.ServiceTypeId &&
                                    item.CreatedDate >= startdate && item.CreatedDate <= endDate && insts.DepId == depCode.DepId
                                    select item
        ).ToList();
                }
                else
                {
                    atsdbContext = (from item in _context.TblRequests
                                    join serviceType in _context.TblServiceTypes on item.ServiceTypeId equals serviceType.ServiceTypeId
                                    join insts in _context.TblRequestDepartmentRelations on item.RequestId equals insts.RequestId
                                    where item.CreatedDate >= startdate && item.CreatedDate <= endDate && insts.DepId == depCode.DepId
                                    select item).ToList();
                }
            }
            else
            {
                if ( Model.InstId == null && Model.ServiceTypeId == null)
                {
                    atsdbContext = (from item in _context.TblRequests
                                    join serviceType in _context.TblServiceTypes on item.ServiceTypeId equals serviceType.ServiceTypeId
                                    join insts in _context.TblRequestDepartmentRelations on item.RequestId equals insts.RequestId
                                   where insts.DepId == depCode.DepId
                                    select item
                                 ).ToList();
                }
                else if ( Model.InstId != null && Model.ServiceTypeId == null)
                {
                    atsdbContext = (from item in _context.TblRequests
                                    join serviceType in _context.TblServiceTypes on item.ServiceTypeId equals serviceType.ServiceTypeId
                                    join insts in _context.TblRequestDepartmentRelations on item.RequestId equals insts.RequestId
                                    where item.InistId == Model.InstId && insts.DepId == depCode.DepId

                                    select item
                       ).ToList();
                }
                else if ( Model.InstId != null && Model.ServiceTypeId != null)
                {
                    atsdbContext = (from item in _context.TblRequests
                                    join serviceType in _context.TblServiceTypes on item.ServiceTypeId equals serviceType.ServiceTypeId
                                    join insts in _context.TblRequestDepartmentRelations on item.RequestId equals insts.RequestId
                                    where  item.InistId == Model.InstId &&
                                    item.ServiceTypeId == Model.ServiceTypeId && insts.DepId == depCode.DepId
                                    select item
              ).ToList();
                }
                else if ( Model.InstId == null && Model.ServiceTypeId != null)
                {
                    atsdbContext = (from item in _context.TblRequests
                                    join priority in _context.TblPriorities on item.PriorityId equals priority.PriorityId
                                    join serviceType in _context.TblServiceTypes on item.ServiceTypeId equals serviceType.ServiceTypeId
                                    join insts in _context.TblRequestDepartmentRelations on item.RequestId equals insts.RequestId
                                    where  item.ServiceTypeId == Model.ServiceTypeId && insts.DepId == depCode.DepId

                                    select item
        ).ToList();
                }
                else
                {
                    atsdbContext = (from item in _context.TblRequests
                                    join serviceType in _context.TblServiceTypes on item.ServiceTypeId equals serviceType.ServiceTypeId
                                    join insts in _context.TblRequestDepartmentRelations on item.RequestId equals insts.RequestId
                                   where insts.DepId == depCode.DepId
                                    select item).ToList();
                }

            }
            if (cultur == "am")
            {
                model.ServiceType = _context.TblServiceTypes.Include(s => s.Dep).Where(x => x.Dep.DepCode == "CVA").Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeNameAmharic
                }).ToList();
                model.Institution = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Text = x.NameAmharic,
                    Value = x.InistId.ToString()

                }).ToList();
            }
            else
            {
                model.ServiceType = _context.TblServiceTypes.Include(s => s.Dep).Where(x => x.Dep.DepCode == "CVA").Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeName
                }).ToList();
                model.Institution = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.InistId.ToString()

                }).ToList();
            }
            model.requests = atsdbContext;

            model.Year = _context.TblYears.Select(s => new SelectListItem
            {
                Value = s.YearId.ToString(),
                Text = s.Year

            }).ToList();
            return View(model);

        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblRequests == null)
            {
                return NotFound();
            }
            ViewBag.backUrl = HttpContext.Request.GetEncodedUrl();

            var tblRequest = await _context.TblRequests
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.DepartmentUpprovalStatusNavigation)
                .Include(t => t.DeputyUprovalStatusNavigation)
                .Include(t => t.DocType)
                .Include(t => t.ExternalRequestStatus)
                .Include(t => t.Inist)
                .Include(t => t.Priority)
                .Include(t => t.QuestType)
                .Include(t => t.RequestedByNavigation)
                .Include(t => t.ServiceType)
                .Include(t => t.TeamUpprovalStatusNavigation)
                .Include(t => t.UserUpprovalStatusNavigation)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblRequest == null)
            {
                return NotFound();
            }

            return View(tblRequest);
        }
    }
}
