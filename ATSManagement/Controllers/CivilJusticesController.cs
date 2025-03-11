using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.IModels;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.Extensions;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class CivilJusticesController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        private readonly INotificationService _notificationService;
        public CivilJusticesController(AtsdbContext context, INotyfService notyfService, IHttpContextAccessor contextAccessor, IMailService mailService, INotificationService notificationService)
        {
            _notifyService = notyfService;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
            _notificationService = notificationService;

        }
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var userinfor = _context.TblInternalUsers.Include(c => c.Dep).Where(x => x.UserId == userId).FirstOrDefault();
            if (userinfor.IsDeputy == true || (userinfor.Dep.DepCode == "CVA" && userinfor.IsDepartmentHead == true) || userinfor.IsSuperAdmin == true)
            {
                var result = _context.CivilJusticeNewRequestViewModels.FromSqlRaw($"EXEC GetCivilJusticeNewRequests").ToList();

                return View(result);
            }
            else
            {
                return RedirectToAction(nameof(AssignedRequests));
            }
        }
        public async Task<IActionResult> NewRequests()
        {

            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var userinfor = _context.TblInternalUsers.Include(c => c.Dep).Where(x => x.UserId == userId).FirstOrDefault();
            if (userinfor.IsDeputy == true || (userinfor.Dep.DepCode == "CVA" && userinfor.IsDepartmentHead == true) || userinfor.IsSuperAdmin == true)
            {
                var result = _context.CivilJusticeNewRequestViewModels.FromSqlRaw($"EXEC GetCiviJustiesUnassignedRequests").ToList();

                return View(result);
            }
            else
            {
                return RedirectToAction(nameof(AssignedRequests));
            }
        }
        public async Task<IActionResult> TeamRequests()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var user = _context.TblInternalUsers.Find(userId);
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest tblRequest;
            var moreDeps = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "CVA" && x.TeamId == user.TeamId && x.IsAssingedToUser == false).Select(a => a.RequestId).ToList();
            foreach (var item in moreDeps)
            {
                tblRequest = _context.TblRequests
                    .Include(t => t.AssignedByNavigation)
                    .Include(t => t.Inist)
                    .Include(t => t.ServiceType)
                    .Include(s => s.DocType)
                    .Include(t => t.RequestedByNavigation)
                    .Include(t => t.CreatedByNavigation)
                    .Include(x => x.ExternalRequestStatus)
                    .Include(x => x.DepartmentUpprovalStatusNavigation)
                    .Include(x => x.DeputyUprovalStatusNavigation)
                    .Include(y => y.TeamUpprovalStatusNavigation)
                    .Include(t => t.Priority).Where(x => x.RequestId == item).FirstOrDefault();
                atsdbContext.Add(tblRequest);
            }
            var sortedLists = atsdbContext.OrderByDescending(s => s.OrderId);
            return View(sortedLists);
        }
        public async Task<IActionResult> AddActivity(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            ActivityModel model = new ActivityModel();
            model.RequestId = id;
            model.AddedDate = DateTime.Now;
            ViewBag.RequestId = model.RequestId;
            model.CreatedBy = userId;
            ViewData["Activities"] = _context.TblActivities
                 .Include(x => x.Request)
                 .Include(x => x.CreatedByNavigation)
                 .Where(x => x.RequestId == id).ToList();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActivity(ActivityModel model)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            List<Guid> cretatedTos = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.IsDepartmentHead == true && s.Dep.DepCode == "CVA").Select(S => S.UserId).ToList();

            ActivityModel newModel = new ActivityModel();
            newModel.RequestId = model.RequestId;
            newModel.AddedDate = DateTime.Now;
            ViewBag.RequestId = model.RequestId;
            newModel.CreatedBy = model.CreatedBy;

            List<string> assignedBody = new List<string>();
            var request = _context.TblRequests.Where(x => x.RequestId == model.RequestId).FirstOrDefault();
            assignedBody = (from items in _context.TblInternalUsers where items.UserId == request.AssignedBy select items.EmailAddress).ToList();
            TblActivity activity = new TblActivity();
            activity.RequestId = model.RequestId;
            activity.AddedDate = DateTime.Now;
            activity.ActivityDetail = model.ActivityDetail;
            activity.TimeTakenTocomplete = model.TimeTakenTocomplete;
            activity.Remark = model.Remark;
            activity.CreatedBy = model.CreatedBy;
            _context.TblActivities.Add(activity);
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                model.ActivityDetail = null;
                _notificationService.saveNotification(userId, cretatedTos, "Daily activities added by expert");
                await SendMail(assignedBody, "Adding activities notifications.", "<h3>Assigned body is adding activities via <strong> Task tacking Dashboard</strong>. Please check on the system and followup!.</h3>");
                ViewData["Activities"] = _context.TblActivities
                    .Include(x => x.Request)
                    .Include(x => x.CreatedByNavigation)
                    .Where(x => x.RequestId == model.RequestId).ToList();
                _notifyService.Success("Activity added successfully!");
                return View(newModel);
            }
            else
            {
                ViewData["Activities"] = _context.TblActivities
                    .Include(x => x.Request)
                    .Include(x => x.CreatedByNavigation)
                    .Where(x => x.RequestId == model.RequestId).ToList();
                _notifyService.Error("Activity isn't added successfully Please try again");
                return View(model);
            }

        }
        public async Task<IActionResult> EditActivity(Guid? ActivityId)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblActivity? activity = await _context.TblActivities.FindAsync(ActivityId);
            ActivityModel model = new ActivityModel();
            model.ActivityId = ActivityId;
            model.Remark = activity.Remark;
            model.TimeTakenTocomplete = activity.TimeTakenTocomplete;
            model.RequestId = activity.RequestId;
            model.ActivityDetail = activity.ActivityDetail;
            model.AddedDate = activity.AddedDate;
            ViewData["Activities"] = _context.TblActivities
                 .Include(x => x.Request)
                 .Include(x => x.CreatedByNavigation)
                 .Where(x => x.RequestId == model.RequestId).ToList();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActivity(ActivityModel model)
        {
            TblActivity? activity = await _context.TblActivities.FindAsync(model.ActivityId);
            ActivityModel newModel = new ActivityModel();
            activity.RequestId = model.RequestId;
            activity.AddedDate = DateTime.Now;
            activity.ActivityDetail = model.ActivityDetail;
            activity.TimeTakenTocomplete = model.TimeTakenTocomplete;
            activity.Remark = model.Remark;
            activity.CreatedBy = model.CreatedBy;
            activity.AddedDate = model.AddedDate;
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Data updated successfully");
                return RedirectToAction(nameof(AddActivity), new { id = activity.RequestId });
            }
            else
            {
                _notifyService.Error("Data isn't updated. Please try again");
                return View(model);
            }

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
        public async Task<IActionResult> DetailsNewRequest(Guid? id)
        {
            if (id == null || _context.TblRequests == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> DetailsPendingRequest(Guid? id)
        {
            if (id == null || _context.TblRequests == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> DetailsCompleted(Guid? id)
        {
            if (id == null || _context.TblRequests == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> DetailsAssigned(Guid? id)
        {
            if (id == null || _context.TblRequests == null)
            {
                return NotFound();
            }

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
        public async Task<IActionResult> DetailsTeamRequests(Guid? id)
        {
            if (id == null || _context.TblRequests == null)
            {
                return NotFound();
            }

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
        public IActionResult Create()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
            {
                Value = x.InistId.ToString(),
                Text = x.Name
            }).ToList();
            model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
            {
                Value = x.DepId.ToString(),
                Text = x.DepName
            }).ToList();
            model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
            {
                Text = x.PriorityName,
                Value = x.PriorityId.ToString()
            }).ToList();
            model.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
            {
                Value = s.ServiceTypeId.ToString(),
                Text = s.ServiceTypeName
            }).ToList();
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = userId;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CivilJusticeExternalRequestModel model)
        {
            try
            {
                TblRequest tblCivilJustice = new TblRequest();
                var decision = _context.TblDecisionStatuses.Where(x => x.StatusName == "Not set").FirstOrDefault();
                Guid statusiD = (from id in _context.TblExternalRequestStatuses where id.StatusName == "New" select id.ExternalRequestStatusId).FirstOrDefault();
                tblCivilJustice.RequestDetail = model.RequestDetail;
                tblCivilJustice.CreatedBy = model.CreatedBy;
                tblCivilJustice.CreatedDate = DateTime.Now;
                tblCivilJustice.CaseTypeId = model.ServiceTypeId;
                tblCivilJustice.InistId = model.InistId;
                tblCivilJustice.PriorityId = model.PriorityId;
                tblCivilJustice.DepartmentUpprovalStatus = decision.DesStatusId;
                tblCivilJustice.TeamUpprovalStatus = decision.DesStatusId;
                tblCivilJustice.DeputyUprovalStatus = decision.DesStatusId;
                tblCivilJustice.UserUpprovalStatus = decision.DesStatusId;
                tblCivilJustice.ExternalRequestStatusId = statusiD;
                _context.TblRequests.Add(tblCivilJustice);
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
                    {
                        Value = x.InistId.ToString(),
                        Text = x.Name
                    }).ToList();
                    model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                    {
                        Value = x.DepId.ToString(),
                        Text = x.DepName
                    }).ToList();
                    model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                    {
                        Text = x.PriorityName,
                        Value = x.PriorityId.ToString()
                    }).ToList();
                    model.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
                    {
                        Value = s.ServiceTypeId.ToString(),
                        Text = s.ServiceTypeName
                    }).ToList();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Value = x.InistId.ToString(),
                    Text = x.Name
                }).ToList();
                model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                {
                    Value = x.DepId.ToString(),
                    Text = x.DepName
                }).ToList();
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Text = x.PriorityName,
                    Value = x.PriorityId.ToString()
                }).ToList();
                model.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                return View(model);
            }

        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            if (id == null || _context.TblCivilJustices == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblRequests.FindAsync(id);
            if (tblCivilJustice == null)
            {
                return NotFound();
            }
            model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
            {
                Value = x.InistId.ToString(),
                Text = x.Name
            }).ToList();
            model.InistId = tblCivilJustice.InistId;
            model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
            {
                Value = x.DepId.ToString(),
                Text = x.DepName
            }).ToList();
            model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
            {
                Text = x.PriorityName,
                Value = x.PriorityId.ToString()
            }).ToList();
            model.PriorityId = tblCivilJustice.PriorityId;
            model.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
            {
                Value = s.ServiceTypeId.ToString(),
                Text = s.ServiceTypeName
            }).ToList();
            model.ServiceTypeId = tblCivilJustice.CaseTypeId;
            model.RequestDetail = tblCivilJustice.RequestDetail;
            model.RequestId = tblCivilJustice.RequestId;
            model.CreatedDate = tblCivilJustice.CreatedDate;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CivilJusticeExternalRequestModel model)
        {
            try
            {
                if (model.RequestId == Guid.Empty)
                {
                    return NotFound();
                }
                if (!TblCivilJusticeExists(model.RequestId))
                {
                    return NotFound();
                }
                TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(model.RequestId);
                tblCivilJustice.RequestDetail = model.RequestDetail;
                tblCivilJustice.PriorityId = model.PriorityId;
                tblCivilJustice.CaseTypeId = model.ServiceTypeId;
                tblCivilJustice.InistId = model.InistId;
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    return RedirectToAction("Index");

                }
                else
                {
                    model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
                    {
                        Value = x.InistId.ToString(),
                        Text = x.Name
                    }).ToList();
                    model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                    {
                        Value = x.DepId.ToString(),
                        Text = x.DepName
                    }).ToList();
                    model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                    {
                        Text = x.PriorityName,
                        Value = x.PriorityId.ToString()
                    }).ToList();
                    model.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
                    {
                        Value = s.ServiceTypeId.ToString(),
                        Text = s.ServiceTypeName
                    }).ToList();
                    return View(model);

                }
            }
            catch (Exception EX)
            {

                model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Value = x.InistId.ToString(),
                    Text = x.Name
                }).ToList();
                model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                {
                    Value = x.DepId.ToString(),
                    Text = x.DepName
                }).ToList();
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Text = x.PriorityName,
                    Value = x.PriorityId.ToString()
                }).ToList();
                model.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                return View(model);
            }

        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblCivilJustices == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblRequests
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.CaseType)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(t => t.ExternalRequestStatus)
                .Include(x => x.DepartmentUpprovalStatusNavigation)
                .Include(x => x.DeputyUprovalStatusNavigation)
                .Include(y => y.TeamUpprovalStatusNavigation)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblCivilJustice == null)
            {
                return NotFound();
            }

            return View(tblCivilJustice);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblRequests == null)
            {
                return Problem("Entity set 'AtsdbContext.TblCivilJustices'  is null.");
            }
            var tblCivilJustice = await _context.TblRequests.FindAsync(id);
            if (tblCivilJustice != null)
            {
                _context.TblRequests.Remove(tblCivilJustice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblCivilJusticeExists(Guid id)
        {
            return (_context.TblRequests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> AssignToUser(Guid id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblRequest drafting = await _context.TblRequests.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            model.RequestDetail = drafting.RequestDetail;
            model.RequestId = id;
            model.AssignedBy = userId;
            model.AssignedDate = DateTime.Now;
            model.CreatedBy = drafting.CreatedBy;
            if (cultur == "am")
            {
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeNameAmharic
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeTypeAmharic.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Text = s.TeamNameAmharic,
                    Value = s.TeamId.ToString(),
                }).ToList();
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityNameAmharic.ToString()

                }).ToList();
            }
            else
            {
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeType.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Text = s.TeamName,
                    Value = s.TeamId.ToString(),
                }).ToList();
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityName.ToString()

                }).ToList();
            }

            model.ServiceTypeId = drafting.ServiceTypeId;
            model.AssignedTos = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "CVA").Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(),
                Text = x.FirstName.ToString() + " " + x.MidleName

            }).ToList();
            model.DueDate = DateTime.Now.AddDays(10);

            model.PriorityId = drafting.PriorityId;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToUser(CivilJusticeExternalRequestModel model)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            List<Guid> cretatedTos = new List<Guid>();
            List<string>? emails = new List<string>();
            List<TblRequestAssignee> assignees;
            if (model.RequestId == null)
            {
                return NotFound();
            }
            TblRequest drafting = await _context.TblRequests.FindAsync(model.RequestId);
            TblRequestDepartmentRelation relation = await _context.TblRequestDepartmentRelations.Where(s => s.RequestId == model.RequestId).FirstOrDefaultAsync();
            if (drafting == null)
            {
                return NotFound();
            }
            try
            {
                var topstatus = (from tops in _context.TblTopStatuses where tops.StatusName == "In Progress" select tops).FirstOrDefault();
                if (model.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847"))
                {
                    if (model.TeamId == null)
                    {
                        _notifyService.Error("Since assignment type is team , You should select one the team");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeId = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);
                    }
                    
                    TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "Assigned to team" select items).FirstOrDefault();
                    var TeamEmail = _context.TblInternalUsers.Where(x => x.TeamId == model.TeamId && x.IsTeamLeader == true).Select(s => s.EmailAddress).ToList();
                    relation.AssigneeTypeId = model.AssigneeTypeId;
                    relation.TeamId = model.TeamId;
                    relation.IsAssingedToUser = false;
                    drafting.AssingmentRemark = model.AssingmentRemark;
                    drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                    drafting.TopStatusId = topstatus.TopStatusId;
                    drafting.IsAssignedTodepartment = true;
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Request is successfully assigned to team");
                        await SendMail(TeamEmail, "Task is assign notification", "<h3>Some tasks are assigned to your team via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Assignment isn't successfull. Please try again");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeId = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);
                    }
                }
                else
                {
                    TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "Assigned to user" select items).FirstOrDefault();
                    foreach (var item in model.AssignedTo)
                    {
                        var userEmails = (from user in _context.TblInternalUsers where user.UserId == item select user.EmailAddress).FirstOrDefault();
                        emails.Add(userEmails);
                    }
                    if (model.AssignedTo.Length == 0)
                    {
                        _notifyService.Error("Since assignment type is Expert , You should select one the experts");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeId = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);
                    }
                    relation.IsAssingedToUser = true;                    
                    drafting.DueDate = model.DueDate;
                    drafting.AssignedDate = model.AssignedDate;
                    drafting.PriorityId = model.PriorityId;
                    drafting.AssignedBy = model.AssignedBy;
                    drafting.TopStatusId = topstatus.TopStatusId;
                    drafting.AssingmentRemark = model.AssingmentRemark;
                    drafting.CreatedBy = model.CreatedBy;                    
                    drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                    if (model.AssignedTo.Length > 0)
                    {
                        assignees = new List<TblRequestAssignee>();
                        foreach (var item in model.AssignedTo)
                        {
                            var ifExists = _context.TblRequestAssignees.Where(x => x.RequestId == model.RequestId && x.UserId == item).FirstOrDefault();
                            if (ifExists == null)
                            {
                                cretatedTos.Add(item);
                                assignees.Add(new TblRequestAssignee { UserId = item, RequestId = model.RequestId });
                            }
                        }
                        drafting.TblRequestAssignees = assignees;
                    }
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Request is successfully assigned to team");

                        _notificationService.saveNotification(userId, cretatedTos, "New request assigned.");
                        await SendMail(emails, "Task is assign notification", "<h3>Some tasks are assigned to you via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {

                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeId = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);

                    }
                }
            }
            catch (Exception)
            {
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                model.ServiceTypeId = drafting.ServiceTypeId;
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeType.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Text = s.TeamName,
                    Value = s.TeamId.ToString(),
                }).ToList();
                model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                {
                    Value = x.UserId.ToString(),
                    Text = x.FirstName.ToString() + " " + x.MidleName

                }).ToList();
                model.DueDate = DateTime.Now.AddDays(10);
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityName.ToString()

                }).ToList();
                model.PriorityId = drafting.PriorityId;
                return View(model);
            }
        }
        public async Task<IActionResult> SendReply(Guid? ReplyId)
        {
            TblReplay replay = _context.TblReplays.Find(ReplyId);
            try
            {
                Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                replay.IsSent = true;
                replay.InternalReplayedBy = userId;
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply successfully sent.");
                    return RedirectToAction("Replies", new { id = replay.RequestId });
                }
                else
                {
                    _notifyService.Error("Reply isn't sent. Please try again");
                    return RedirectToAction("Replies", new { id = replay.RequestId });
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again.");
                return RedirectToAction("Replies", new { id = replay.RequestId });
            }

        }
        public async Task<IActionResult> Replies(Guid? id)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var replays = await _context.TblReplays.Where(a => a.RequestId == id).ToListAsync();
            RepliesModel replies = new RepliesModel();
            replies.RequestId = id;
            replies.ReplyDate = DateTime.UtcNow;
            replies.InternalReplayedBy = userId;
            replies.IsSent = false;
            ViewData["Replies"] = _context.TblReplays
                .Include(x => x.InternalReplayedByNavigation)
                .Include(x => x.ExternalReplayedByNavigation)
                .Include(x => x.Request)
                .Where(_context => _context.RequestId == id).ToList();
            return View(replies);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Replies(RepliesModel model)
        {
            try
            {
                TblReplay replay = new TblReplay();
                replay.ReplyDate = DateTime.Now;
                replay.InternalReplayedBy = model.InternalReplayedBy;
                replay.RequestId = model.RequestId;
                replay.ReplayDetail = model.ReplayDetail;
                replay.IsExternal = false;
                replay.IsInternal = true;
                if (model.IsSent == true)
                {
                    replay.IsSent = true;
                }
                else
                {
                    replay.IsSent = false;
                }
                _context.TblReplays.Add(replay);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply successfully added.");
                    return RedirectToAction("Replies", new { id = model.RequestId });
                }
                else
                {
                    _notifyService.Error("Reply isn't added. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again.");
                return View(model);
            }
        }
        public async Task<IActionResult> EditReplies(Guid? ReplyId)
        {
            var reply = _context.TblReplays.Where(s => s.ReplyId == ReplyId).FirstOrDefault();
            RepliesModel repliesModel = new RepliesModel();
            repliesModel.ReplyId = reply.ReplyId;
            repliesModel.RequestId = reply.RequestId;
            if (reply.IsSent == null || reply.IsSent == false)
            {
                repliesModel.IsSent = false;
            }
            else
            {
                repliesModel.IsSent = true;
            }
            repliesModel.ReplayDetail = reply.ReplayDetail;
            return View(repliesModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> EditReplies(RepliesModel model)
        {
            try
            {
                var DepheadId = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "CVA" && s.IsDepartmentHead == true).FirstOrDefault();
                TblReplay replay = _context.TblReplays.Find(model.ReplyId);
                replay.ReplayDetail = model.ReplayDetail;
                replay.InternalReplayedBy = DepheadId.UserId;
                if (model.IsSent == true)
                {
                    replay.IsSent = true;
                }
                else
                {
                    replay.IsSent = false;
                }
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply successfully added.");
                    return RedirectToAction("Replies", new { id = model.RequestId });
                }
                else
                {
                    _notifyService.Error("Reply isn't added. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again.");
                return View(model);
            }

        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
        public async Task<IActionResult> CompletedRequests()
        {
            TblTopStatus tblTopStatus = _context.TblTopStatuses.Where(x => x.StatusName == "Completed").FirstOrDefault();

            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest tblRequest;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
            var moreDeps = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "CVA").Select(a => a.RequestId).ToList();
            if (tblInternalUser.IsDeputy == true || tblInternalUser.IsSuperAdmin == true || tblInternalUser.IsDepartmentHead == true)
            {
                foreach (var item in moreDeps)
                {
                    tblRequest = _context.TblRequests
                                                    .Include(t => t.AssignedByNavigation)
                                                    .Include(t => t.Inist)
                                                    .Include(t => t.RequestedByNavigation)
                                                    .Include(t => t.CreatedByNavigation)
                                                    .Include(t => t.TopStatus)
                                                    .Include(t => t.ServiceType)
                                                    .Include(x => x.ExternalRequestStatus)
                                                    .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                    .Include(x => x.DeputyUprovalStatusNavigation)
                                                    .Include(y => y.TeamUpprovalStatusNavigation)
                                                    .Include(t => t.Priority).Where(x => x.TopStatusId == tblTopStatus.TopStatusId && x.RequestId == item).FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }

                }
            }
            else
            {
                var assignedReq = _context.TblRequestAssignees.Where(x => x.UserId == userId).ToList();
                foreach (var item in assignedReq)
                {
                    tblRequest = _context.TblRequests
                                                                       .Include(t => t.AssignedByNavigation)
                                                                       .Include(t => t.ServiceType)
                                                                       .Include(t => t.Inist)
                                                                       .Include(t => t.TopStatus)
                                                                       .Include(t => t.RequestedByNavigation)
                                                                       .Include(t => t.CreatedByNavigation)
                                                                       .Include(x => x.ExternalRequestStatus)
                                                                       .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                                       .Include(x => x.DeputyUprovalStatusNavigation)
                                                                       .Include(y => y.TeamUpprovalStatusNavigation)
                                                                       .Include(t => t.Priority).Where(x => x.RequestId == item.RequestId && x.TopStatusId == tblTopStatus.TopStatusId).FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }


                }
            }
            var sortedLists = atsdbContext.OrderByDescending(s => s.OrderId);
            return View(sortedLists);
        }
        public async Task<IActionResult> PendingRequests()
        {
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest tblRequest;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
            var moreDeps = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "CVA"&&x.IsAssingedToUser == true).Select(a => a.RequestId).ToList();
            if (tblInternalUser.IsDeputy == true || tblInternalUser.IsSuperAdmin == true || tblInternalUser.IsDepartmentHead == true)
            {
                foreach (var item in moreDeps)
                {
                    tblRequest = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.DocType)
                                                        .Include(x => x.QuestType)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.TopStatus)
                                                        .Include(t => t.ServiceType)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.RequestId == item && x.TopStatus.StatusName == "In Progress").FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }
                }
            }
            else
            {

                var assignedReq = _context.TblRequestAssignees.Where(x => x.UserId == userId).ToList();
                foreach (var item in assignedReq)
                {
                    tblRequest = _context.TblRequests
                                                      .Include(t => t.AssignedByNavigation)
                                                      .Include(t => t.DocType)
                                                      .Include(x => x.QuestType)
                                                      .Include(t => t.ServiceType)
                                                      .Include(t => t.Inist)
                                                      .Include(t => t.TopStatus)
                                                      .Include(t => t.RequestedByNavigation)
                                                      .Include(t => t.CreatedByNavigation)
                                                      .Include(x => x.ExternalRequestStatus)
                                                      .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                      .Include(x => x.DeputyUprovalStatusNavigation)
                                                      .Include(y => y.TeamUpprovalStatusNavigation)
                                                      .Include(t => t.Priority).Where(x => x.RequestId == item.RequestId && x.ExternalRequestStatus.StatusName != "New" && x.ExternalRequestStatus.StatusName != "Completed" && x.AssignedTo == userId).FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }
                }
            }
            var sortedLists = atsdbContext.OrderByDescending(s => s.OrderId);
            return View(sortedLists);
        }
        public async Task<IActionResult> AssignedRequests()
        {
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest Request;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
            var assignedReq = _context.TblRequestAssignees.Where(x => x.UserId == userId).ToList();
            foreach (var item in assignedReq)
            {
                Request = new TblRequest();
                Request = _context.TblRequests
                                 .Include(t => t.AssignedByNavigation)
                                 .Include(t => t.Inist)
                                 .Include(t => t.ServiceType)
                                 .Include(s => s.TopStatus)
                                 .Include(s => s.DocType)
                                 .Include(t => t.RequestedByNavigation)
                                 .Include(t => t.CreatedByNavigation)
                                 .Include(x => x.ExternalRequestStatus)
                                 .Include(x => x.DepartmentUpprovalStatusNavigation)
                                 .Include(x => x.DeputyUprovalStatusNavigation)
                                 .Include(y => y.TeamUpprovalStatusNavigation)
                                 .Include(t => t.Priority).Where(a => a.RequestId == item.RequestId).FirstOrDefault();
                if (Request != null)
                {
                    atsdbContext.Add(Request);
                }
            }
            var sortedLists = atsdbContext.OrderByDescending(s => s.OrderId);
            return View(sortedLists);
        }
        public async Task<IActionResult> UpploadFinalReport(Guid id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            var detail = _context.TblRequests.FindAsync(id).Result;
            model.RequestId = id;
            model.CreatedDate = DateTime.UtcNow;
            model.RequestDetail = detail.RequestDetail;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> UpploadFinalReport(CivilJusticeExternalRequestModel model)
        {
            TblRequest civilJustice = await _context.TblRequests.FindAsync(model.RequestId);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //get file extension
            FileInfo fileInfo = new FileInfo(model.finalReport.FileName);
            string fileName = Guid.NewGuid().ToString() + model.finalReport.FileName;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.finalReport.CopyTo(stream);
            }
            string dbPath = "/admin/Files/" + fileName;
            civilJustice.FinalReport = dbPath;
            civilJustice.FinalReportSummary = model.FinalReportSummary;
            int updated = _context.SaveChanges();
            if (updated > 0)
            {
                _notifyService.Success("Report successfully upploaded");
                return RedirectToAction(nameof(AssignedRequests));
            }
            else
            {
                _notifyService.Error("Report isn't successfully uploaded. Please try again");
                return View(model);
            }
        }
        public async Task<IActionResult> AddAdjornyDates(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var userInfo = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            ViewData["Adjornies"] = _context.TblAdjornments.Include(X => X.Request).Where(x => x.RequestId == id).ToList();
            AjornyDateModel model = new AjornyDateModel();
            model.RequestId = id;
            model.CreatedDate = DateTime.UtcNow;
            model.ExpertHanlingCase = userInfo.FirstName + " " + userInfo.MidleName + " " + userInfo.LastName;
            ViewBag.id = id;
            _contextAccessor.HttpContext.Session.SetString("id", id.ToString());
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddAdjornyDates(AjornyDateModel ajornyDateModel)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var userInfo = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();

            List<Guid> cretatedTos = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.IsDepartmentHead == true && s.Dep.DepCode == "CVA").Select(S => S.UserId).ToList();
            ViewBag.id = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("id"));
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            List<string> user = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "CVA" && s.IsDepartmentHead == true).Select(s => s.EmailAddress).ToList();
            AjornyDateModel model = new AjornyDateModel();
            model.RequestId = ajornyDateModel.RequestId;
            model.AdjorneyDate = DateTime.Now;
            TblAdjornment tblAdjornment = new TblAdjornment();
            tblAdjornment.AdjorneyDate = ajornyDateModel.AdjorneyDate;
            tblAdjornment.RequestId = ajornyDateModel.RequestId;
            tblAdjornment.CreatedDate = DateTime.UtcNow;
            tblAdjornment.WhatIsDone = ajornyDateModel.WhatIsDone;
            tblAdjornment.AppointmentReason = ajornyDateModel.AppointmentReason;
            tblAdjornment.PlaintiffDefendant = ajornyDateModel.Plaintiff_Defendant;
            tblAdjornment.TheCourtCaseHanled = ajornyDateModel.TheCourtCaseHanled;
            tblAdjornment.ExpertHanlingCase = userInfo.FirstName + " " + userInfo.MidleName + " " + userInfo.LastName;
            tblAdjornment.CreatedBy = userId;
            tblAdjornment.DefendantInfo = ajornyDateModel.Defendant_info;
            tblAdjornment.CaseNumber = ajornyDateModel.CaseNumber;
            _context.TblAdjornments.Add(tblAdjornment);
            int saved = _context.SaveChanges();
            ViewData["Adjornies"] = _context.TblAdjornments.Include(X => X.Request).Where(x => x.RequestId == ajornyDateModel.RequestId).ToList();
            if (saved > 0)
            {
                _notificationService.saveNotification(userId, cretatedTos, "Adjournment date added by Expert");

                if (cultur == "am")
                {
                    await SendMail(user, "የቀጠሮ ቀንን ስለማሳወቅ", "በባለሙያ የገባ ቀጠሮ አለ።");
                }
                else
                {
                    await SendMail(user, "Appointment alert from expert", "Appontment is added by expert. Please review on Task tracking Dashboard");
                }
                return View(model);
            }
            return View(ajornyDateModel);
        }
        public IActionResult EditAdjornyDate(Guid AdjoryId)
        {
            AjornyDateModel ajornyDateModel = new AjornyDateModel();
            TblAdjornment tblAdjornment = _context.TblAdjornments.Find(AdjoryId);
            ajornyDateModel.AdjoryId = AdjoryId;
            ajornyDateModel.AdjorneyDate = tblAdjornment.AdjorneyDate;
            ajornyDateModel.TheCourtCaseHanled = tblAdjornment.TheCourtCaseHanled;
            ajornyDateModel.AppointmentReason = tblAdjornment.AppointmentReason;
            ajornyDateModel.RequestId = tblAdjornment.RequestId;
            ajornyDateModel.WhatIsDone = tblAdjornment.WhatIsDone;
            ajornyDateModel.Plaintiff_Defendant = tblAdjornment.PlaintiffDefendant;
            ajornyDateModel.ExpertHanlingCase = tblAdjornment.ExpertHanlingCase;
            ajornyDateModel.CreatedBy = tblAdjornment.CreatedBy;
            ajornyDateModel.Defendant_info = tblAdjornment.DefendantInfo;
            ajornyDateModel.CaseNumber = tblAdjornment.CaseNumber;
            return View(ajornyDateModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult EditAdjornyDate(AjornyDateModel ajornyDateModel)
        {
            try
            {
                TblAdjornment tblAdjornment = _context.TblAdjornments.Find(ajornyDateModel.AdjoryId);
                if (tblAdjornment == null)
                {
                    return NotFound();
                }
                else
                {
                    tblAdjornment.AdjoryId = ajornyDateModel.AdjoryId;
                    tblAdjornment.AdjorneyDate = tblAdjornment.AdjorneyDate;
                    tblAdjornment.TheCourtCaseHanled = tblAdjornment.TheCourtCaseHanled;
                    tblAdjornment.AppointmentReason = tblAdjornment.AppointmentReason;
                    tblAdjornment.RequestId = tblAdjornment.RequestId;
                    tblAdjornment.WhatIsDone = tblAdjornment.WhatIsDone;
                    tblAdjornment.PlaintiffDefendant = tblAdjornment.PlaintiffDefendant;
                    tblAdjornment.ExpertHanlingCase = tblAdjornment.ExpertHanlingCase;
                    tblAdjornment.CreatedBy = tblAdjornment.CreatedBy;
                    tblAdjornment.DefendantInfo = ajornyDateModel.Defendant_info;
                    tblAdjornment.CaseNumber = ajornyDateModel.CaseNumber;
                    int updated = _context.SaveChanges();
                    if (updated > 0)
                    {
                        _notifyService.Success("Data Updated");
                        return RedirectToAction("AddAdjornyDates", new { id = ajornyDateModel.RequestId });
                    }
                    else
                    {
                        _notifyService.Error("Data isn't updated");
                        return View(ajornyDateModel);
                    }
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message);
                return View(ajornyDateModel);
            }
        }
        public async Task<IActionResult> AddEvidencesAndWitnesses(Guid? id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            WitnessEvidenceModel model = new WitnessEvidenceModel();
            List<EvidenceType> types;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            model.RequestId = id;
            model.CreatedDate = DateTime.UtcNow;
            model.CreatedBy = userId;
            if (cultur == "am")
            {
                types = new List<EvidenceType>
                {
                    new EvidenceType
                    {
                        TypeId=1,
                        Name="የምስክር ማስረጃ"
                    },
                    new EvidenceType
                    {
                        TypeId=2,
                        Name="የሰነድ ማስረጃ"
                    }
                };
            }
            else
            {
                types = new List<EvidenceType>
                {
                    new EvidenceType
                    {
                        TypeId=1,
                        Name="Witness"
                    },
                    new EvidenceType
                    {
                        TypeId=2,
                        Name="Document/File"
                    }
                };
            }
            model.EvidenceTypes = types.Select(s => new SelectListItem
            {
                Value = s.TypeId.ToString(),
                Text = s.Name.ToString()
            }).ToList();
            ViewData["evidences"] = _context.TblWitnessEvidences.Include(X => X.Request).Where(x => x.RequestId == id).ToList();
            if (id == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddEvidencesAndWitnesses(WitnessEvidenceModel evidenceModel)
        {
            List<EvidenceType> types;
            WitnessEvidenceModel model = new WitnessEvidenceModel();
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            if (cultur == "am")
            {
                types = new List<EvidenceType>
                {
                    new EvidenceType
                    {
                        TypeId=1,
                        Name="የምስክር ማስረጃ"
                    },
                    new EvidenceType
                    {
                        TypeId=2, Name="የሰነድ ማስረጃ"
                    }
                };
            }
            else
            {
                types = new List<EvidenceType>
                {
                    new EvidenceType
                    {
                        TypeId=1,
                        Name="Witness"
                    },
                    new EvidenceType
                    {
                        TypeId=2, Name="Document/File"
                    }
                };
            }
            TblWitnessEvidence evidence = new TblWitnessEvidence();
            List<Guid> cretatedTos = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.IsDepartmentHead == true && s.Dep.DepCode == "CVA").Select(S => S.UserId).ToList();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            string dbPath = null, dbVideoPath = null;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //get file extension
            if (evidenceModel.EvidenceFiles != null)
            {
                FileInfo fileInfo = new FileInfo(evidenceModel.EvidenceFiles.FileName);
                string fileName = Guid.NewGuid().ToString() + evidenceModel.EvidenceFiles.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    evidenceModel.EvidenceFiles.CopyTo(stream);
                }
               dbPath = "/admin/Files/" + fileName;                
            }
            if (evidenceModel.EvidenceVideos != null)
            {
                FileInfo fileInfo = new FileInfo(evidenceModel.EvidenceVideos.FileName);
                string fileName = Guid.NewGuid().ToString() + evidenceModel.EvidenceVideos.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    evidenceModel.EvidenceVideos.CopyTo(stream);
                }
                dbVideoPath = "/admin/Files/" + fileName;
            }
            evidence.RequestId = evidenceModel.RequestId;
            evidence.CreatedDate = DateTime.UtcNow;
            evidence.Address = evidenceModel.Address;
            evidence.CreatedBy = userId;
            evidence.WitnessesName = evidenceModel.WitnessesName;
            evidence.EvidenceVideos = dbVideoPath;
            evidence.EvidenceFiles = dbPath;
            _context.TblWitnessEvidences.Add(evidence);
            int saved = _context.SaveChanges();
            ViewData["evidences"] = _context.TblWitnessEvidences.Include(X => X.Request).Where(x => x.RequestId == evidenceModel.RequestId).ToList();
            if (saved > 0)
            {
                _notificationService.saveNotification(userId, cretatedTos, "Expert added evidences and witness to assigned requests");
                model.EvidenceTypes = types.Select(s => new SelectListItem
                {
                    Value = s.TypeId.ToString(),
                    Text = s.Name.ToString()
                }).ToList();
                model.RequestId = evidenceModel.RequestId;
                model.CreatedDate = DateTime.UtcNow;
                model.CreatedBy = userId;
                return View(model);
            }
            else
            {
                evidenceModel.EvidenceTypes = types.Select(s => new SelectListItem
                {
                    Value = s.TypeId.ToString(),
                    Text = s.Name.ToString()
                }).ToList();
                return View(evidenceModel);
            }
        }
        [HttpGet]
        public async Task<IActionResult> DownloadEvidenceFile(string path,string methodController, string method, Guid? id)
        {
            string filename = path.Substring(13);
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Files", filename);

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }
            if (!System.IO.File.Exists(filepath))
            {
                return RedirectToAction(nameof(FileNotFound), new { path = filepath, methodController = methodController, method = method, id=id });
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));


        }

        public async Task<IActionResult>? FileNotFound(string path, string? methodController, string? method, Guid? id)
        {
            ViewBag.controller = methodController;
            ViewBag.action = method;
            ViewBag.id = id;
            return View();
        }

        public async Task<IActionResult> DeleteActivity(Guid? id)
        {
            if (id == null || _context.TblActivities == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblActivities
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (tblWitnessEvidence == null)
            {
                return NotFound();
            }

            return View(tblWitnessEvidence);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            var tblWitnessEvidences = await _context.TblActivities
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            var tblWitnessEvidence = await _context.TblActivities.FindAsync(id);
            if (tblWitnessEvidence != null)
            {
                _context.TblActivities.Remove(tblWitnessEvidence);
            }

            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Successfully removed");
                return RedirectToAction(nameof(AddActivity), new { id = tblWitnessEvidence.RequestId });
            }
            else
            {
                _notifyService.Error("Not successfull");
                return View(tblWitnessEvidences);
            }
        }
        public async Task<IActionResult> DeleteWitness(Guid? id)
        {
            if (id == null || _context.TblWitnessEvidences == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblWitnessEvidences
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.WitnessId == id);
            if (tblWitnessEvidence == null)
            {
                return NotFound();
            }

            return View(tblWitnessEvidence);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteWitness(Guid id)
        {
            if (_context.TblWitnessEvidences == null)
            {
                return Problem("Entity set 'AtsdbContext.TblWitnessEvidences'  is null.");
            }
            var tblWitnessEvidence = await _context.TblWitnessEvidences.FindAsync(id);
            if (tblWitnessEvidence != null)
            {
                _context.TblWitnessEvidences.Remove(tblWitnessEvidence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AddEvidencesAndWitnesses), new { id = tblWitnessEvidence.RequestId });
        }
        public async Task<IActionResult> DeleteAdjorny(Guid? id)
        {
            if (id == null || _context.TblAdjornments == null)
            {
                return NotFound();
            }

            var tblAdjornment = await _context.TblAdjornments
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.AdjoryId == id);
            if (tblAdjornment == null)
            {
                return NotFound();
            }
            return View(tblAdjornment);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdjorny(Guid id)
        {
            if (_context.TblAdjornments == null)
            {
                return Problem("Entity set 'AtsdbContext.TblAdjornments'  is null.");
            }
            var tblAdjornment = await _context.TblAdjornments.FindAsync(id);
            if (tblAdjornment != null)
            {
                _context.TblAdjornments.Remove(tblAdjornment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AddAdjornyDates), new { id = tblAdjornment.RequestId });
        }
        public async Task<IActionResult> UppdateProgressStatus(Guid? id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(id);
            model.RequestDetail = tblCivilJustice.RequestDetail;
            model.RequestId = tblCivilJustice.RequestId;
            if (cultur == "am")
            {
                model.ExternalStatus = _context.TblExternalRequestStatuses.Where(x => x.StatusName != "New" && x.StatusName != "Assigned to user" 
                && x.StatusName != "Assigned to team"&&x.StatusName!= "In Progress").Select(x => new SelectListItem
                {
                    Text = x.StatusNameAmharic,
                    Value = x.ExternalRequestStatusId.ToString()
                }).ToList();
            }
            else
            {
                model.ExternalStatus = _context.TblExternalRequestStatuses.Where(x => x.StatusName != "New" && x.StatusName != "Assigned to user"
                && x.StatusName != "Assigned to team" && x.StatusName != "In Progress").Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.ExternalRequestStatusId.ToString()
                }).ToList();
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UppdateProgressStatus(CivilJusticeExternalRequestModel model)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            List<Guid> cretatedTos = new List<Guid>();
            cretatedTos = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "LSDC").Select(s => s.UserId).ToList();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));

            TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(model.RequestId);
            TblDecisionStatus status = _context.TblDecisionStatuses.Where(x => x.StatusName == "Waiting for Upproval").FirstOrDefault();
            if (model.ExternalRequestStatusID == Guid.Parse("2521c2b7-a886-439b-b4ba-6c0167d74940") && tblCivilJustice.FinalReport == null)
            {
                _notifyService.Error("Before you make complete status. Please uppload final report");
                if (cultur == "am")
                {
                    model.ExternalStatus = _context.TblExternalRequestStatuses.Where(x => x.StatusName != "New" && x.StatusName != "Assigned to user"
               && x.StatusName != "Assigned to team" && x.StatusName != "In Progress").Select(x => new SelectListItem
               {
                   Text = x.StatusNameAmharic,
                        Value = x.ExternalRequestStatusId.ToString()
                    }).ToList();
                }
                else
                {
                    model.ExternalStatus = _context.TblExternalRequestStatuses.Where(x => x.StatusName != "New" && x.StatusName != "Assigned to user"
               && x.StatusName != "Assigned to team" && x.StatusName != "In Progress").Select(x => new SelectListItem
               {
                   Text = x.StatusName,
                        Value = x.ExternalRequestStatusId.ToString()
                    }).ToList();
                }
                model.RequestDetail = tblCivilJustice.RequestDetail;
                return View(model);
            }
            tblCivilJustice.ExternalRequestStatusId = model.ExternalRequestStatusID;
            tblCivilJustice.TeamUpprovalStatus = status.DesStatusId;
            tblCivilJustice.DepartmentUpprovalStatus = status.DesStatusId;
            tblCivilJustice.DeputyUprovalStatus = status.DesStatusId;
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Status successfully updated");
                _notificationService.saveNotification(userId, cretatedTos, "New request is assigned. Please check");

                return RedirectToAction(nameof(AssignedRequests));
            }
            return View(model);
        }
        public async Task<IActionResult> UppdateDesicionStatus(Guid? id)
        {
           
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser user = await _context.TblInternalUsers.FindAsync(userId);
            if (user.IsDepartmentHead == true)
            {
                ViewBag.visible = "visible";
            }
            else
            {
                ViewBag.visible = "none";
            }
            model.IsDeputyApprovalNeeded = false;
            model.RequestDetail = tblCivilJustice.RequestDetail;
            model.RequestId = tblCivilJustice.RequestId;
            if (cultur == "am")
            {
                model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                {
                    Text = x.StatusNameAmharic,
                    Value = x.DesStatusId.ToString()
                }).ToList();
            }
            else
            {
                model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.DesStatusId.ToString()
                }).ToList();
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UppdateDesicionStatus(CivilJusticeExternalRequestModel model)
        {
            TblCivilJusticeChat chat = new TblCivilJusticeChat();
            List<Guid> ids = new List<Guid>();
            List<string> emails = new List<string>();
            var exterStatu = _context.TblExternalRequestStatuses.Where(s => s.StatusName == "Reviewed by Department Head").FirstOrDefault();

            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            TblRequest? tblCivilJustice = await _context.TblRequests.FindAsync(model.RequestId);
            var users = (from items in _context.TblInternalUsers
                         join assigne in _context.TblRequestAssignees on items.UserId equals assigne.UserId 
                         where assigne.RequestId == model.RequestId 
                         select new { UserId=items.UserId, email=items.EmailAddress}).ToList();
            var Chatusers = (from items in _context.TblInternalUsers
                         join assigne in _context.TblRequestAssignees on items.UserId equals assigne.UserId
                         where assigne.RequestId == model.RequestId
                         select new { UserId = items.UserId, email = items.EmailAddress }).FirstOrDefault();
            foreach (var item in users)
            {
                ids.Add(item.UserId);
                emails.Add(item.email);
            }
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser? user = await _context.TblInternalUsers.FindAsync(userId);
            TblTopStatus? topStatus = _context.TblTopStatuses.Where(s => s.StatusName == "Completed").FirstOrDefault();
            TblDecisionStatus? decisionStatus = await _context.TblDecisionStatuses.FindAsync(model.DesStatusId);
            if (user.IsTeamLeader == true)
            {
                tblCivilJustice.TeamUpprovalStatus = model.DesStatusId;
                tblCivilJustice.TeamDesicionRemark = model.DescissionRemark;
            }
            else if (user.IsDefaultUser == true)
            {
                tblCivilJustice.UserUpprovalStatus = model.DesStatusId;
            }
            else if (user.IsDepartmentHead == true)
            {
                tblCivilJustice.DepartmentUpprovalStatus = model.DesStatusId;
                tblCivilJustice.DepartmentDesicionRemark = model.DescissionRemark;
                if (model.IsDeputyApprovalNeeded == true)
                {
                    tblCivilJustice.TopStatusId = topStatus.TopStatusId;
                    tblCivilJustice.DeputyUprovalStatus = model.DesStatusId;
                    
                }
                if (decisionStatus.StatusName == "Upproved")
                {
                    tblCivilJustice.TeamUpprovalStatus = model.DesStatusId;
                }
            }
            else if (user.IsDeputy == true)
            {
                tblCivilJustice.DeputyUprovalStatus = model.DesStatusId;
                tblCivilJustice.TopStatusId = topStatus.TopStatusId;
                tblCivilJustice.DeputyDesicionRemark = model.DescissionRemark;
                if (decisionStatus.StatusName == "Upproved")
                {
                    tblCivilJustice.TeamUpprovalStatus = model.DesStatusId;
                    tblCivilJustice.DepartmentUpprovalStatus = model.DesStatusId;
                }
            }
            else
            {
                if (cultur == "am")
                {
                    model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                    {
                        Text = x.StatusNameAmharic,
                        Value = x.DesStatusId.ToString()
                    }).ToList();
                }
                else
                {
                    model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                    {
                        Text = x.StatusName,
                        Value = x.DesStatusId.ToString()
                    }).ToList();
                }
                _notifyService.Error("Opperation isn't successfull. Please try again");
                return View(model);
            }
           
            if (model.finalReport!=null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(model.finalReport.FileName);
                string fileName = Guid.NewGuid().ToString() + model.finalReport.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.finalReport.CopyTo(stream);
                }
                string dbPath = "/admin/Files/" + fileName;
                tblCivilJustice.FinalReport = dbPath;
            }
            tblCivilJustice.ExternalRequestStatusId = exterStatu.ExternalRequestStatusId;
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Upproval status changed successfully!");
                chat.UserId = userId;
                chat.RequestId = model.RequestId;
                chat.ChatContent = model.DescissionRemark;
                chat.Datetime = DateTime.Now;
                chat.IsDephead = true;
                chat.SendBy = userId;
                chat.SendTo = Chatusers.UserId;
                _context.TblCivilJusticeChats.Add(chat);
                _context.SaveChanges();
                await SendMail(emails, "Request update status", "Upproval status changed successfully!. You can check detail on request chat section");
                _notificationService.saveNotification(userId, ids, "Your request status is changed by Department head");
                return RedirectToAction(nameof(PendingRequests));
            }
            else
            {
                if (user.IsDepartmentHead == true)
                {
                    ViewBag.visible = true;
                }
                else
                {
                    ViewBag.visible = false;
                }
                _notifyService.Error("Upproval status isn't updated. Please try again");
                model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.DesStatusId.ToString()
                }).ToList();
                return View(model);
            }
        }
        public async Task<IActionResult> RequestActivities(Guid? id)
        {
            var atsdbContext = _context.TblActivities.Include(t => t.CreatedByNavigation).Include(t => t.Request).Where(s => s.RequestId == id);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> RequestAdjornies(Guid? id)
        {
            var atsdbContext = _context.TblAdjornments.Include(t => t.CreatedByNavigation).Include(t => t.Request);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> WitnessesandEvidences(Guid? id)
        {
            var atsdbContext = _context.TblWitnessEvidences.Include(t => t.CreatedByNavigation).Include(t => t.Request);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> AddHistory(Guid? id)
        {
            DocumentHistoryModel model = await historyModel(id);

            return View(model);
        }
        private async Task<DocumentHistoryModel> historyModel(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblDocumentHistory tblDocument = _context.TblDocumentHistories.Where(x => x.RequestId == id).OrderBy(x => x.Round).Last();
            DocumentHistoryModel model = new DocumentHistoryModel();
            ViewData["histories"] = _context.TblDocumentHistories.Where(x => x.RequestId == id).ToList();
            model.RequestId = id;
            model.InternalReplyId = userId;
            if (tblDocument.Round == null)
            {
                model.Round = 1;
            }
            else
            {
                model.Round = Convert.ToInt32(tblDocument.Round) + 1;
            }
            return model;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddHistory(DocumentHistoryModel? model)
        {
            List<string> emails = new List<string>();
            var users = (from user in _context.TblRequestAssignees
                         join assignees in _context.TblInternalUsers on user.UserId equals assignees.UserId
                         where (assignees.IsDepartmentHead == true || assignees.IsDeputy == true) && user.RequestId == model.RequestId
                         select assignees.EmailAddress).ToList();
            if (users.Count != 0)
            {
                emails = users;
            }
            else
            {
                emails = (from assignees in _context.TblInternalUsers
                          where assignees.IsDepartmentHead == true || assignees.IsDeputy == true
                          select assignees.EmailAddress).ToList();
            }
            var institutionName = (from items in _context.TblRequests where items.RequestId == model.RequestId select items.Inist.Name).FirstOrDefault();
            TblDocumentHistory history = new TblDocumentHistory();
            history.InternalReplyId = model.InternalReplyId;
            history.Round = model.Round;
            history.Description = model.Description;
            history.FileDescription = model.FileDescription;
            history.RequestId = model.RequestId;
            history.CreatedDate = DateTime.Now;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (model.DocPath != null)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                FileInfo fileInfo = new FileInfo(model.DocPath.FileName);
                string fileName = Guid.NewGuid().ToString() + model.DocPath.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.DocPath.CopyTo(stream);
                }
                string dbPath = "/Files/" + fileName;
                history.DocPath = dbPath;
            }
            _context.TblDocumentHistories.Add(history);
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                await SendMail(users, "Request notifications from " + institutionName, "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");
                _notifyService.Success("Document added successfully!");
                return RedirectToAction(nameof(AddHistory));
            }
            else
            {
                _notifyService.Error("Document is not successfully added. Please try again");
                return View(model);
            }
        }
        public async Task<IActionResult> AssignFromTeam(Guid id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();

            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblRequest drafting = await _context.TblRequests.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            model.RequestDetail = drafting.RequestDetail;
            model.RequestId = id;
            model.AssignedBy = userId;
            model.AssignedDate = DateTime.Now;
            model.CreatedBy = drafting.CreatedBy;
            if (cultur == "am")
            {
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeNameAmharic
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeTypeAmharic.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityNameAmharic.ToString()

                }).ToList();
            }
            else
            {
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeType.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityName.ToString()

                }).ToList();

            }
            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
            {
                Text = s.TeamName,
                Value = s.TeamId.ToString(),
            }).ToList();
            model.ServiceTypeId = drafting.ServiceTypeId;
            model.AssignedTos = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "CVA").Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(),
                Text = x.FirstName.ToString() + " " + x.MidleName

            }).ToList();
            model.DueDate = DateTime.Now.AddDays(10);

            model.PriorityId = drafting.PriorityId;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignFromTeam(CivilJusticeExternalRequestModel model)
        {
            List<string>? emails = new List<string>();
            List<TblRequestAssignee> assignees;
            foreach (var item in model.AssignedTo)
            {
                var userEmails = (from user in _context.TblInternalUsers where user.UserId == item select user.EmailAddress).FirstOrDefault();
                emails.Add(userEmails);
            }
            TblRequestDepartmentRelation departmentRelation = await _context.TblRequestDepartmentRelations.Where(s => s.RequestId == model.RequestId).FirstOrDefaultAsync();
            TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "Assigned to user" select items).FirstOrDefault();
            if (model.RequestId == null)
            {
                return NotFound();
            }
            TblRequest drafting = await _context.TblRequests.FindAsync(model.RequestId);
            if (drafting == null)
            {
                return NotFound();
            }
            try
            {
                var topstatus = (from tops in _context.TblTopStatuses where tops.StatusName == "In Progress" select tops).FirstOrDefault();

                departmentRelation.IsAssingedToUser = true;
                drafting.DueDate = model.DueDate;
                drafting.AssignedDate = model.AssignedDate;
                drafting.PriorityId = model.PriorityId;
                drafting.AssignedBy = model.AssignedBy;
                drafting.AssingmentRemark = model.AssingmentRemark;
                drafting.CreatedBy = model.CreatedBy;
                drafting.TopStatusId = topstatus.TopStatusId;
                drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                drafting.IsAssignedTodepartment = true;
                if (model.AssignedTo.Length > 0)
                {
                    assignees = new List<TblRequestAssignee>();
                    foreach (var item in model.AssignedTo)
                    {
                        assignees.Add(new TblRequestAssignee { UserId = item, RequestId = model.RequestId });
                    }
                    drafting.TblRequestAssignees = assignees;
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Task is successfully assigned");
                        await SendMail(emails, "Task is assign notification", "<h3>Some tasks are assigned to you via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                        return RedirectToAction(nameof(TeamRequests));
                    }

                    _notifyService.Error("Task Assignation isn't successfull. Please try again");
                    model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                    {
                        Value = s.ServiceTypeId.ToString(),
                        Text = s.ServiceTypeName
                    }).ToList();
                    model.ServiceTypeId = drafting.ServiceTypeId;
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Text = s.AssigneeType.ToString(),
                        Value = s.AssigneeTypeId.ToString(),
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                    {
                        Text = s.TeamName,
                        Value = s.TeamId.ToString(),
                    }).ToList();
                    model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                    {
                        Value = x.UserId.ToString(),
                        Text = x.FirstName.ToString() + " " + x.MidleName

                    }).ToList();
                    model.DueDate = DateTime.Now.AddDays(10);
                    model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                    {
                        Value = x.PriorityId.ToString(),
                        Text = x.PriorityName.ToString()

                    }).ToList();
                    model.PriorityId = drafting.PriorityId;
                    return View(model);

                }
                else
                {
                    _notifyService.Error("Task Assignation isn't successfull. Please try again");
                    model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                    {
                        Value = s.ServiceTypeId.ToString(),
                        Text = s.ServiceTypeName
                    }).ToList();
                    model.ServiceTypeId = drafting.ServiceTypeId;
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Text = s.AssigneeType.ToString(),
                        Value = s.AssigneeTypeId.ToString(),
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                    {
                        Text = s.TeamName,
                        Value = s.TeamId.ToString(),
                    }).ToList();
                    model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                    {
                        Value = x.UserId.ToString(),
                        Text = x.FirstName.ToString() + " " + x.MidleName

                    }).ToList();
                    model.DueDate = DateTime.Now.AddDays(10);
                    model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                    {
                        Value = x.PriorityId.ToString(),
                        Text = x.PriorityName.ToString()

                    }).ToList();
                    model.PriorityId = drafting.PriorityId;
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again");
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                model.ServiceTypeId = drafting.ServiceTypeId;
                model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                {
                    Value = x.DepId.ToString(),
                    Text = x.DepName
                }).ToList();
                model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                {
                    Value = x.UserId.ToString(),
                    Text = x.FirstName.ToString() + " " + x.MidleName

                }).ToList();
                model.DueDate = DateTime.Now.AddDays(10);
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityName.ToString()
                }).ToList();
                model.PriorityId = drafting.PriorityId;
                return View(model);
            }
        }
        public IActionResult SendToInstitution(Guid? RequestId)
        {
            SendModel model = new SendModel();
            model.RequestId = RequestId;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToInstitution(SendModel sendModel)
        {
            try
            {
                string dbPath;
                string FinallySentReport;
                TblRequest? request = _context.TblRequests.Where(s => s.RequestId == sendModel.RequestId).FirstOrDefault(); ;
                List<string?> intEmail = _context.TblExternalUsers.Where(s => s.InistId == request.InistId).Select(s => s.Email).ToList();
                request.SendingRemark = sendModel.SendingRemark;
                request.IsSenttoInst = true;
                request.SentDate = DateTime.Now;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (sendModel.ApprovalLetter != null)
                {
                    string file = Guid.NewGuid().ToString() + sendModel.ApprovalLetter.FileName;
                    string fileWithPath = Path.Combine(path, file);
                    using (var stream = new FileStream(fileWithPath, FileMode.Create))
                    {
                        sendModel.ApprovalLetter.CopyTo(stream);
                    }
                    dbPath = "/admin/Files/" + file;
                    request.LetterofUpproval = dbPath;
                }
                if (sendModel.FinalReport != null)
                {
                    string file = Guid.NewGuid().ToString() + sendModel.FinalReport.FileName;
                    string fileWithPath = Path.Combine(path, file);
                    using (var stream = new FileStream(fileWithPath, FileMode.Create))
                    {
                        sendModel.FinalReport.CopyTo(stream);
                    }
                    FinallySentReport = "/admin/Files/" + file;
                    request.SentReport = FinallySentReport;
                }
                int sent = await _context.SaveChangesAsync();
                if (sent > 0)
                {
                    _notifyService.Success("Final report is sent successfully!");
                    await SendMail(intEmail, "Completed request notification", "Your request is completed. Please check detail information on Task Tracking Dashboard.");
                    return RedirectToAction(nameof(SentBackRequests));
                }
                else
                {
                    _notifyService.Error("Final report isn't sent successfully. Please try again ");
                    return View(sendModel);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again");
                return View(sendModel);
            }

        }
        public async Task<IActionResult> SentBackRequests()
        {
            TblTopStatus tblTopStatus = _context.TblTopStatuses.Where(x => x.StatusName == "Completed").FirstOrDefault();
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest tblRequest;
            var moreDeps = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "CVA").Select(a => a.RequestId).ToList();
            foreach (var item in moreDeps)
            {
                tblRequest = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.Inist)
                                                        .Include(s => s.DocType)
                                                         .Include(t => t.ServiceType)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.TopStatusId == tblTopStatus.TopStatusId && x.IsSenttoInst == true && x.RequestId == item).FirstOrDefault();
                if (tblRequest != null)
                {
                    atsdbContext.Add(tblRequest);
                }
            }
            var sortedLists = atsdbContext.OrderByDescending(s => s.OrderId);
            return View(sortedLists);
        }
        public async Task<IActionResult> ReAssignToUser(Guid id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            LegalStudiesDraftingModel model = new LegalStudiesDraftingModel();
            TblRequest drafting = await _context.TblRequests.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (drafting.RequestDetail != null)
            {
                model.RequestDetail = drafting.RequestDetail;
            }
            model.RequestId = id;
            model.AssignedBy = userId;
            model.AssignedDate = DateTime.Now;
            model.DocId = drafting.DocTypeId;
            model.ServiceTypeID = drafting.ServiceTypeId;
            if (cultur == "am")
            {
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeTypeAmharic.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeNameAmharic
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Text = s.TeamNameAmharic,
                    Value = s.TeamId.ToString(),
                }).ToList();

                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityNameAmharic.ToString()

                }).ToList();
                model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
                {
                    Value = x.DocId.ToString(),
                    Text = x.DocNameAmharic
                }).ToList();
            }
            else
            {
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeType.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeName
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Text = s.TeamName,
                    Value = s.TeamId.ToString(),
                }).ToList();

                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityName.ToString()

                }).ToList();
                model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
                {
                    Value = x.DocId.ToString(),
                    Text = x.DocName
                }).ToList();
            }
            model.AssignedTos = _context.TblInternalUsers.Where(x => x.Dep.DepCode == "CVA").Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(),
                Text = x.FirstName.ToString() + " " + x.MidleName

            }).ToList();
            model.DueDate = DateTime.Now.AddDays(10);
            model.PriorityId = drafting.PriorityId;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReAssignToUser(LegalStudiesDraftingModel model)
        {
            List<string>? emails = new List<string>();
            List<TblRequestAssignee> assignees;
            if (model.RequestId == null)
            {
                return NotFound();
            }
            TblRequest drafting = await _context.TblRequests.FindAsync(model.RequestId);
            TblRequestDepartmentRelation relation = await _context.TblRequestDepartmentRelations.Where(s => s.RequestId == model.RequestId).FirstOrDefaultAsync();
            if (drafting == null)
            {
                return NotFound();
            }
            try
            {

                if (model.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847"))
                {
                    if (model.TeamId == null)
                    {
                        _notifyService.Error("Since assignment type is team , You should select one the team");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);
                    }
                    TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "Assigned to team" select items).FirstOrDefault();
                    var TeamEmail = _context.TblInternalUsers.Where(x => x.TeamId == model.TeamId && x.IsTeamLeader == true).Select(s => s.EmailAddress).ToList();
                    relation.AssigneeTypeId = model.AssigneeTypeId;
                    relation.TeamId = model.TeamId;
                    relation.IsAssingedToUser = false;

                    drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Request is successfully assigned to team");
                        await SendMail(TeamEmail, "Task is assign notification", "<h3>Some tasks are assigned to your team via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Assignment isn't successfull. Please try again");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);
                    }
                }
                else
                {
                    TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "Assigned to user" select items).FirstOrDefault();
                    foreach (var item in model.AssignedTo)
                    {
                        var userEmails = (from user in _context.TblInternalUsers where user.UserId == item select user.EmailAddress).FirstOrDefault();
                        emails.Add(userEmails);
                    }
                    if (model.AssignedTo.Length == 0)
                    {
                        _notifyService.Error("Since assignment type is Expert , You should select one the experts");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);
                    }
                    relation.IsAssingedToUser = true;
                    drafting.DueDate = model.DueDate;
                    drafting.AssignedDate = model.AssignedDate;
                    drafting.PriorityId = model.PriorityId;
                    drafting.AssignedBy = model.AssignedBy;
                    drafting.AssingmentRemark = model.AssingmentRemark;
                    drafting.CreatedBy = model.CreatedBy;
                    drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                    if (model.AssignedTo.Length > 0)
                    {
                        assignees = new List<TblRequestAssignee>();
                        foreach (var item in model.AssignedTo)
                        {
                            var ifExists = _context.TblRequestAssignees.Where(x => x.RequestId == model.RequestId && x.UserId == item).FirstOrDefault();
                            if (ifExists == null)
                            {
                                assignees.Add(new TblRequestAssignee { UserId = item, RequestId = model.RequestId });
                            }
                        }
                        drafting.TblRequestAssignees = assignees;
                    }
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        await SendMail(emails, "Task is assign notification", "<h3>Some tasks are assigned to you via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {

                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);

                    }
                }
            }
            catch (Exception)
            {
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                model.ServiceTypeID = drafting.ServiceTypeId;
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeType.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                {
                    Text = s.TeamName,
                    Value = s.TeamId.ToString(),
                }).ToList();
                model.AssignedTos = _context.TblInternalUsers.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
                {
                    Value = x.UserId.ToString(),
                    Text = x.FirstName.ToString() + " " + x.MidleName

                }).ToList();
                model.DueDate = DateTime.Now.AddDays(10);
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityName.ToString()

                }).ToList();
                model.PriorityId = drafting.PriorityId;
                return View(model);
            }
        }
        public async Task<IActionResult> ArchivedRequests()
        {
            ArchiveFilterModel model = new ArchiveFilterModel();
            List<CurrencyModel> currencyModels;
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            if (cultur == "am")
            {
                model.ServiceType = _context.TblServiceTypes.Where(x => x.DepId == Guid.Parse("159f57e9-bc26-4b6e-859e-c577ce8a86a8")).Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeNameAmharic
                }).ToList();
                model.Institution = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Text = x.NameAmharic,
                    Value = x.InistId.ToString()

                }).ToList();
                model.DocumentType = _context.TblLegalDraftingDocTypes.Select(d => new SelectListItem
                {
                    Value = d.DocId.ToString(),
                    Text = d.DocNameAmharic
                }).ToList();
                model.Year = _context.TblYears.Select(s => new SelectListItem
                {
                    Value = s.YearId.ToString(),
                    Text = s.Year

                }).ToList();
                currencyModels = new List<CurrencyModel>()
            {
                new CurrencyModel
                {
                    CurrencyId="USD",
                    CurrencyName="DOLLAR"
                },
                new CurrencyModel
                {
                    CurrencyId="ETB",
                    CurrencyName="ETB"
                },
                new CurrencyModel
                {
                    CurrencyName="Euro",
                    CurrencyId="Euro"
                },
                new CurrencyModel
                {
                    CurrencyId="Pound",
                    CurrencyName="Pound"
                },

            };
                model.Currencies = currencyModels.Select(s => new SelectListItem
                {
                    Value = s.CurrencyId.ToString(),
                    Text = s.CurrencyName
                }).ToList();

            }
            else
            {
                model.ServiceType = _context.TblServiceTypes.Where(x => x.DepId == Guid.Parse("159f57e9-bc26-4b6e-859e-c577ce8a86a8")).Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeName
                }).ToList();
                model.Institution = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.InistId.ToString()

                }).ToList();
                model.DocumentType = _context.TblLegalDraftingDocTypes.Select(d => new SelectListItem
                {
                    Value = d.DocId.ToString(),
                    Text = d.DocName
                }).ToList();
                model.Year = _context.TblYears.Select(s => new SelectListItem
                {
                    Value = s.YearId.ToString(),
                    Text = s.Year

                }).ToList();
                currencyModels = new List<CurrencyModel>()
            {
                new CurrencyModel
                {
                    CurrencyId="USD",
                    CurrencyName="DOLLAR"
                },
                new CurrencyModel
                {
                    CurrencyId="ETB",
                    CurrencyName="ETB"
                },
                new CurrencyModel
                {
                    CurrencyName="Euro",
                    CurrencyId="Euro"
                },
                new CurrencyModel
                {
                    CurrencyId="Pound",
                    CurrencyName="Pound"
                },

            };
                model.Currencies = currencyModels.Select(s => new SelectListItem
                {
                    Value = s.CurrencyId.ToString(),
                    Text = s.CurrencyName
                }).ToList();

            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ArchivedRequests(ArchiveFilterModel filterModel)
        {
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            List<CurrencyModel> currencyModels;
            ArchiveFilterModel model = new ArchiveFilterModel();
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();

            var departmental = _context.TblRequestDepartmentRelations
                                                                  .Include(s => s.Request)
                                                                  .Where(s => s.Request.IsArchived == true && s.Dep.DepCode == "CVA").ToList();
            foreach (var item in departmental)
            {

            }
            ViewBag.Requests = departmental;
            if (cultur == "am")
            {
                model.ServiceType = _context.TblServiceTypes.Where(x => x.DepId == Guid.Parse("159f57e9-bc26-4b6e-859e-c577ce8a86a8")).Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeNameAmharic
                }).ToList();
                model.Institution = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Text = x.NameAmharic,
                    Value = x.InistId.ToString()

                }).ToList();
                model.DocumentType = _context.TblLegalDraftingDocTypes.Select(d => new SelectListItem
                {
                    Value = d.DocId.ToString(),
                    Text = d.DocNameAmharic
                }).ToList();
                model.Year = _context.TblYears.Select(s => new SelectListItem
                {
                    Value = s.YearId.ToString(),
                    Text = s.Year

                }).ToList();
                currencyModels = new List<CurrencyModel>()
            {
                new CurrencyModel
                {
                    CurrencyId="USD",
                    CurrencyName="DOLLAR"
                },
                new CurrencyModel
                {
                    CurrencyId="ETB",
                    CurrencyName="ETB"
                },
                new CurrencyModel
                {
                    CurrencyName="Euro",
                    CurrencyId="Euro"
                },
                new CurrencyModel
                {
                    CurrencyId="Pound",
                    CurrencyName="Pound"
                },

            };
                model.Currencies = currencyModels.Select(s => new SelectListItem
                {
                    Value = s.CurrencyId.ToString(),
                    Text = s.CurrencyName
                }).ToList();
            }
            else
            {
                model.ServiceType = _context.TblServiceTypes.Where(x => x.DepId == Guid.Parse("159f57e9-bc26-4b6e-859e-c577ce8a86a8")).Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeName
                }).ToList();
                model.Institution = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.InistId.ToString()

                }).ToList();
                model.DocumentType = _context.TblLegalDraftingDocTypes.Select(d => new SelectListItem
                {
                    Value = d.DocId.ToString(),
                    Text = d.DocName
                }).ToList();
                model.Year = _context.TblYears.Select(s => new SelectListItem
                {
                    Value = s.YearId.ToString(),
                    Text = s.Year

                }).ToList();
                currencyModels = new List<CurrencyModel>()
            {
                new CurrencyModel
                {
                    CurrencyId="USD",
                    CurrencyName="DOLLAR"
                },
                new CurrencyModel
                {
                    CurrencyId="ETB",
                    CurrencyName="ETB"
                },
                new CurrencyModel
                {
                    CurrencyName="Euro",
                    CurrencyId="Euro"
                },
                new CurrencyModel
                {
                    CurrencyId="Pound",
                    CurrencyName="Pound"
                },

            };
                model.Currencies = currencyModels.Select(s => new SelectListItem
                {
                    Value = s.CurrencyId.ToString(),
                    Text = s.CurrencyName
                }).ToList();
            }


            return View(filterModel);

        }
        public async Task<IActionResult> Archive(Guid? id)
        {
            ArchiveModel model = new ArchiveModel();
            var request = _context.TblRequests.FindAsync(id).Result;
            model.RequestId = id;
            model.RequestDetail = request.RequestDetail;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(ArchiveModel? model)
        {
            TblRequest? tblRequest = await _context.TblRequests.FindAsync(model.RequestId);
            tblRequest.IsArchived = true;
            int updated = await _context.SaveChangesAsync();
            if (updated > 0)
            {
                _notifyService.Success("Request is successfully Archived");
                return RedirectToAction(nameof(CompletedRequests));
            }
            else
            {
                _notifyService.Error("Request isn't successfully archived. Please try again");
                return View(model);
            }
        }
        public async Task<IActionResult> RequestChats(Guid? id, string actionMethod, string controller, string type)
        {
            RequestChatModel model = new RequestChatModel();
            if (type != null)
            {
                if (type == "minister")
                {
                    model.ForStateMinister = true;
                }
                else if (type == "expert")
                {
                    model.ForStateMinister = false;
                }
            }
            var appointment = _context.TblSpecificPlans.Find(id);
            model.RequestId = id;
            ViewBag.actionMethod = actionMethod;
            ViewBag.controller = controller;
            _contextAccessor.HttpContext.Session.SetString("actionMethod", actionMethod);
            _contextAccessor.HttpContext.Session.SetString("controller", controller);
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestChats(RequestChatModel chatModel)
        {
            try
            {
                var depHeadEmail = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "CVA"&&s.IsDepartmentHead==true).Select(s => s.EmailAddress).ToList();
                string actionMethod = _contextAccessor.HttpContext.Session.GetString("actionMethod");
                string controller = _contextAccessor.HttpContext.Session.GetString("controller");
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var user = _context.TblInternalUsers.Find(userId);
                TblCivilJusticeChat chat = new TblCivilJusticeChat();
                if (user.IsDepartmentHead == true)
                {
                    chat.SendBy = user.UserId;
                    chat.IsDephead = true;
                    if (chatModel.ForStateMinister == true)
                    {
                        chat.ForStateMinister = true;
                        chat.IsExpert = false;
                    }
                    else
                    {
                        chat.ForStateMinister = false;
                        chat.IsExpert = true;
                    }
                }
                else if (user.IsDeputy == true)
                {
                    chat.ForStateMinister = true;
                    chat.IsExpert = true;
                    chat.IsDephead = true;
                }
                else
                {
                    chat.ForStateMinister = false;
                    chat.SendTo = user.UserId;
                    chat.IsDephead = true;
                    chat.IsExpert = true;
                }
                chat.RequestId = chatModel.RequestId;
                chat.ChatContent = chatModel.ChatContent;
                chat.Datetime = DateTime.Now;
                chat.UserId = userId;
                chat.IsSeen = false;
                string? dbPath = null;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (chatModel.FilePath != null)
                {
                    FileInfo fileInfo = new FileInfo(chatModel.FilePath.FileName);
                    string fileName = Guid.NewGuid().ToString() + chatModel.FilePath.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        chatModel.FilePath.CopyTo(stream);
                    }
                    dbPath = "/admin/Files/" + fileName;
                    chat.FilePath = dbPath;
                }
                _context.TblCivilJusticeChats.Add(chat);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    if (user.IsDefaultUser == true)
                    {
                        await SendMail(depHeadEmail, "Request update", "<h3>Expert dropped message on dashboard please check it.</h3>");

                    }
                    _notifyService.Success("Sent");
                    return RedirectToAction(nameof(RequestChats), new { id = chatModel.RequestId, actionMethod = actionMethod, controller = controller });
                }
                else
                {
                    _notifyService.Error("Not sent. Please try again");
                    return View(chatModel);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message + " happened. Please try again");
                return View(chatModel);
            }
        }
        public async Task<IActionResult> TransferBackToOtherDepartment(Guid? id)

        {
            if (id == null)
            {
                return NotFound();
            }
            return View(getModels(id));
        
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TransferBackToOtherDepartment(RequestModel requestModel)
        {
            List<Guid> cretatedTos = new List<Guid>();
            Guid depId = _context.TblDepartments.Where(s => s.DepCode == "CVA").Select(s => s.DepId).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblRequest request = await _context.TblRequests.FindAsync(requestModel.RequestId);
            TblTopStatus topStatus = _context.TblTopStatuses.Where(s => s.StatusName == "In Progress").FirstOrDefault();
            List<TblRequestDepartmentRelation> departmentRelations;
            List<String> depHeadEmail = new List<string>();
            List<Guid> departmentIds = new List<Guid>();
            request.ServiceTypeId = requestModel.ServiceTypeID;
            request.IsAssignedTodepartment = true;
            request.DeputyRemark = requestModel.DeputyRemark;
            var existingRelation=_context.TblRequestDepartmentRelations.Where(s=>s.RequestId == requestModel.RequestId&&s.DepId==depId).ToList();
            request.TopStatusId = topStatus.TopStatusId;
            if (requestModel.DepId.Length > 0)
            {
                departmentRelations = new List<TblRequestDepartmentRelation>();
                foreach (var item in requestModel.DepId)
                {
                    departmentRelations.Add(new TblRequestDepartmentRelation { DepId = item, RequestId = requestModel.RequestId, IsAssingedToUser = false, TeamId = null });
                }
                _context.TblRequestDepartmentRelations.RemoveRange(existingRelation);
                request.TblRequestDepartmentRelations = departmentRelations;
            }
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                foreach (var item in requestModel.DepId)
                {
                    var users = _context.TblInternalUsers.Where(x => x.DepId == item&&x.IsDepartmentHead==true).Select(s => s.UserId).ToList();
                    var DepartmentHeade = _context.TblInternalUsers.Where(x => x.Dep.DepId == item).Select(s => s.EmailAddress).ToList();
                    depHeadEmail.AddRange(DepartmentHeade);
                    departmentIds.AddRange(users);
                }
                _notifyService.Success("Request is transfered from Civil Justice Administration");
                await SendMail(depHeadEmail, "Request transfer notification from Civil Justice Administration", "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");
                _notificationService.saveNotification(userId, departmentIds, "Request transfer notification from from Civil Justice Administration");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notifyService.Error("Request isn't transfered. Please try again");
                return View(getModels(requestModel.RequestId));
            }
        }
        public RequestModel getModels(Guid? id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            var reques = _context.TblRequests.Find(id);
            RequestModel requestModel = new RequestModel();
            requestModel.RequestId = id;
            requestModel.DeputyRemark = null;
            requestModel.CreatedDate = reques.CreatedDate;
            requestModel.RequestDetail = reques.RequestDetail;
            requestModel.ServiceTypeID = reques.ServiceTypeId;
            requestModel.DepId = _context.TblDepartments.Where(S => S.DepCode == "LSDC").Select(s => s.DepId).ToArray();
            if (cultur == "am")
            {
                requestModel.Deparments = _context.TblDepartments.Where(S=>S.DepCode== "LSDC").Select(x => new SelectListItem
                {
                    Text = x.DepNameAmharic,
                    Value = x.DepId.ToString(),

                }).ToList();
                requestModel.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
                {
                    Text = s.ServiceTypeNameAmharic,
                    Value = s.ServiceTypeId.ToString()
                }).ToList();

            }
            else
            {
                requestModel.Deparments = _context.TblDepartments.Where(S => S.DepCode == "LSDC").Select(x => new SelectListItem
                {
                    Text = x.DepName,
                    Value = x.DepId.ToString()
                }).ToList();
                requestModel.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
                {
                    Text = s.ServiceTypeName,
                    Value = s.ServiceTypeId.ToString()
                }).ToList();

            }
            requestModel.Intitutions = _context.TblInistitutions.Where(s => s.InistId == reques.InistId).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.InistId.ToString()
            }).ToList();
            requestModel.InistId = reques.InistId;
            requestModel.RequestedUsers = _context.TblExternalUsers.Where(x => x.ExterUserId == reques.RequestedBy).Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.MiddleName,
                Value = x.ExterUserId.ToString()
            }).ToList();
            requestModel.RequestedBy = reques.RequestedBy;
            return requestModel;
        }
    }
}
