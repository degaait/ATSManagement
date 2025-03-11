using System.Net.Mail;
using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.IModels;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class InispectionPlansController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        private readonly INotificationService _notificationService;

        public InispectionPlansController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mail, INotyfService notyfService, INotificationService notificationService)
        {
            _notifyService = notyfService;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true)
            {
                var atsdbContext = _context.TblInspectionPlans.Include(t => t.User).Include(T => T.Pro).Include(s => s.Year);
                return View(await atsdbContext.ToListAsync());
            }
            else if (users.IsTeamLeader == true)
            {
                var atsdbContext = _context.TblInspectionPlans.Include(t => t.User).Include(T => T.Pro).Include(s => s.Year).Include(s => s.Team).Where(s => s.TeamId == users.TeamId);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction(nameof(AssignedRequests));

            }
        }
        public async Task<IActionResult> AssignedRequests()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var user = _context.TblInternalUsers.Find(userId);
            if (user.IsSuperAdmin == true || user.IsDeputy == true || user.IsDepartmentHead == true)
            {
                IQueryable<TblAssignedYearlyPlan>? atsdbContext = _context.TblAssignedYearlyPlans.Include(t => t.AssignedToNavigation).Include(t => t.SpecificPlan).Include(p => p.Status).Include(s => s.AssignedByNavigation).Where(a => a.AssignedTo == userId);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                IQueryable<TblAssignedYearlyPlan>? atsdbContext = _context.TblAssignedYearlyPlans.Include(t => t.AssignedToNavigation).Include(t => t.SpecificPlan).Include(p => p.Status).Include(s => s.AssignedByNavigation).Where(a => a.AssignedTo == userId);
                return View(await atsdbContext.ToListAsync());
            }
        }
        public async Task<IActionResult> OngoingPlan()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy == true)
            {
                return RedirectToAction(nameof(OngoingPlanForDeputy));
            }
            else if (users.IsSuperAdmin == true || users.IsDepartmentHead == true)
            {
                IQueryable<TblAssignedYearlyPlan>? atsdbContext = _context.TblAssignedYearlyPlans.Include(t => t.AssignedToNavigation).Include(t => t.SpecificPlan).Include(p => p.Status).Include(s => s.AssignedByNavigation);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction(nameof(AssignedRequests));

            }
        }
        public async Task<IActionResult> OngoingPlanForDeputy()
        {
            List<TblAssignedYearlyPlan>? yearlyPlans = new List<TblAssignedYearlyPlan>();
            TblAssignedYearlyPlan files;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsSuperAdmin == true)
            {
                var atsdbContext =
                    _context.TblSpecificPlans
                   .Include(s => s.CreatedByNavigation)
                   .Include(T => T.Pro)
                   .Include(s => s.IsUpprovedbyDepartmentNavigation)
                   .Include(s => s.IsUpprovedbyTeamNavigation)
                   .Include(s => s.IsUprovedByDeputyNavigation)
                   .Include(s => s.IsUserUprovedNavigation).Where(s => s.Pro.ProstatusTitle != "New");
                foreach (var item in atsdbContext)
                {
                    IQueryable<TblAssignedYearlyPlan>? file = _context.TblAssignedYearlyPlans
                         .Include(t => t.AssignedToNavigation).Include(t => t.SpecificPlan)
                         .Include(p => p.Status).Include(s => s.AssignedByNavigation).Where(s => s.SpecificPlanId == item.SpecificPlanId);
                    yearlyPlans.AddRange(file);

                }
                return View(yearlyPlans);
            }
            else if (users.IsDeputy)
            {
                var atsdbContext = _context.TblSpecificPlans
                    .Include(s => s.CreatedByNavigation)
                    .Include(T => T.Pro)
                    .Include(s => s.IsUpprovedbyDepartmentNavigation)
                    .Include(s => s.IsUpprovedbyTeamNavigation)
                    .Include(s => s.IsUprovedByDeputyNavigation)
                    .Include(s => s.IsUserUprovedNavigation).Where(s => s.Pro.ProgressOrder >= 11 && s.Pro.ProgressOrder != 18);
                foreach (var item in atsdbContext)
                {
                    IQueryable<TblAssignedYearlyPlan>? file = _context.TblAssignedYearlyPlans
                         .Include(t => t.AssignedToNavigation).Include(t => t.SpecificPlan)
                         .Include(p => p.Status).Include(s => s.AssignedByNavigation).Where(s => s.SpecificPlanId == item.SpecificPlanId);
                    yearlyPlans.AddRange(file);

                }
                return View(yearlyPlans);
            }
            else
            {
                return RedirectToAction(nameof(AssignedRequests));
            }
        }
        public async Task<IActionResult> CompletedPlans()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true)
            {
                var atsdbContext = _context.TblSpecificPlans.Include(t => t.Pro).Where(s => s.Pro.ProstatusTitle == "Completed");
                return View(await atsdbContext.ToListAsync());
            }
            else if (users.IsTeamLeader == true)
            {
                var atsdbContext = _context.TblSpecificPlans.Include(T => T.Pro).Include(s => s.Team).Where(s => s.TeamId == users.TeamId && s.FinalStatus == true);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                return RedirectToAction(nameof(AssignedRequests));
            }
        }
        public async Task<IActionResult> SentPlans()
        {

            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true)
            {
                var atsdbContext = _context.TblSentInspections.Include(t => t.SentByNavigation).Include(T => T.RepliedByNavigation).Include(s => s.Inst);
                return View(await atsdbContext.ToListAsync());
            }
            else if (users.IsTeamLeader == true)
            {
                var atsdbContext = _context.TblSentInspections.Include(t => t.SentByNavigation).Include(T => T.RepliedByNavigation).Include(s => s.Inst);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                _notifyService.Error("You have no access to this page");
                return RedirectToAction(nameof(Index), nameof(HomeController));
            }
        }
        public async Task<IActionResult> OpenChatSection(int RecId)
        {
            TblSentInspection inspection = await _context.TblSentInspections.FindAsync(RecId);
            if (inspection == null)
            {
                return NotFound();

            }
            inspection.IsChatCloset = true;
            int opened = await _context.SaveChangesAsync();
            if (opened > 0)
            {
                _notifyService.Success("chat section opened");
            }
            else
            {
                _notifyService.Error("Operation isn't successfull. Please try again");
            }
            return RedirectToAction(nameof(SentPlans));
        }
        public async Task<IActionResult> CloseChatSection(int RecId)
        {
            TblSentInspection inspection = await _context.TblSentInspections.FindAsync(RecId);
            if (inspection == null)
            {
                return NotFound();

            }
            inspection.IsChatCloset = false;
            int opened = await _context.SaveChangesAsync();
            if (opened > 0)
            {
                _notifyService.Success("chat section opened");
            }
            else
            {
                _notifyService.Error("Operation isn't successfull. Please try again");
            }
            return RedirectToAction(nameof(SentPlans));
        }
        public async Task<IActionResult> TeamPlans()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();

            if (users.IsTeamLeader == true)
            {
                var assigned = (from items in _context.TblSpecificPlans
                                join assing in _context.TblPlanCatagories on items.PlanCatId equals assing.PlanCatId
                                where items.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847") && items.TeamId == users.TeamId
                                select items).ToList();
                var atsdbContext = _context.TblSpecificPlans.Include(t => t.CreatedByNavigation).Include(T => T.Pro).Include(s => s.PlanCat).Include(s => s.Team).Where(s => s.TeamId == users.TeamId && s.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847") && (s.IsAssignedToUser == false || s.IsAssignedToUser == null)).ToList();
                var filtered = atsdbContext.Intersect(assigned).ToList();
                return View(filtered);
            }
            else
            {
                return RedirectToAction(nameof(AssignedRequests));
            }
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblInspectionPlans == null)
            {
                return NotFound();
            }
            var tblInspectionPlan = await _context.TblInspectionPlans
                .Include(t => t.AssigneeType)
                .Include(s => s.Pro)
                .Include(s => s.Year)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.InspectionPlanId == id);
            if (tblInspectionPlan == null)
            {
                return NotFound();
            }

            return View(tblInspectionPlan);
        }
        public IActionResult Create()
        {
            InispectionPlan plan = new InispectionPlan();
            plan.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.InistId.ToString(),
            }).ToList();
            plan.InspectionYear = _context.TblYears.Select(a => new SelectListItem
            {
                Value = a.YearId.ToString(),
                Text = a.Year
            }).ToList();
            plan.CreationDate = DateTime.Now;

            return View(plan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InispectionPlan inispectionPlan)
        {
            try
            {
                TblInspectionPlan tible = new TblInspectionPlan();
                List<TblPlanInistitution> plan_in = new List<TblPlanInistitution>();
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));

                tible.PlanTitle = inispectionPlan.PlanTitle;
                tible.PlanDescription = inispectionPlan.PlanDescription;
                tible.CreationDate = DateTime.Now;
                tible.YearId = inispectionPlan.YearId;
                tible.UserId = userId;
                tible.IsAssignedToUser = false;
                tible.IsAssignedTeam = false;
                tible.ProId = _context.TblInspectionStatuses.Where(A => A.ProstatusTitle == "New").Select(A => A.ProId).FirstOrDefault();

                string? dbPath = null;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (inispectionPlan.Attachement != null)
                {
                    FileInfo fileInfo = new FileInfo(inispectionPlan.Attachement.FileName);
                    string fileName = Guid.NewGuid().ToString() + inispectionPlan.Attachement.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        inispectionPlan.Attachement.CopyTo(stream);
                    }
                    dbPath = "/admin/Files/" + fileName;
                    tible.Attachement = dbPath;
                    tible.ExactFileName = inispectionPlan.Attachement.FileName;
                }
                _context.TblInspectionPlans.Add(tible);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Plan is created successfully");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notifyService.Error("Plan isn't created successfully. Please try again");
                    inispectionPlan.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
                    {
                        Text = a.Name,
                        Value = a.InistId.ToString(),
                    }).ToList();
                    inispectionPlan.InspectionYear = _context.TblYears.Select(a => new SelectListItem
                    {
                        Value = a.YearId.ToString(),
                        Text = a.Year
                    }).ToList();
                    return View(inispectionPlan);
                }
            }


            catch (Exception ex)
            {
                _notifyService.Error($"Error:{ex.Message}. Please  try again");
                inispectionPlan.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
                {
                    Text = a.Name,
                    Value = a.InistId.ToString(),
                }).ToList();
                inispectionPlan.InspectionYear = _context.TblYears.Select(a => new SelectListItem
                {
                    Value = a.YearId.ToString(),
                    Text = a.Year
                }).ToList();
                return View(inispectionPlan);
            }
        }
        public Task<IActionResult> Edit(Guid? id)
        {
            InispectionPlan inispection = new InispectionPlan();
            List<Guid> InistId = new List<Guid>();
            if (id == null || _context.TblInspectionPlans == null)
            {
                return Task.FromResult<IActionResult>(NotFound());
            }

            var tblInspectionPlan = _context.TblInspectionPlans.FirstOrDefault(a => a.InspectionPlanId == id.Value);

            inispection.PlanTitle = tblInspectionPlan.PlanTitle;
            inispection.PlanDescription = tblInspectionPlan.PlanDescription;
            inispection.ModificationDate = tblInspectionPlan.ModificationDate;
            inispection.InspectionPlanId = tblInspectionPlan.InspectionPlanId;
            inispection.CreationDate = tblInspectionPlan.CreationDate;

            inispection.InspectionYear = _context.TblYears.Select(a => new SelectListItem
            {
                Value = a.YearId.ToString(),
                Text = a.Year
            }).ToList();
            inispection.YearId = tblInspectionPlan.YearId;
            if (tblInspectionPlan == null)
            {
                return Task.FromResult<IActionResult>(NotFound());
            }
            ViewData["UserId"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInspectionPlan.UserId);
            return Task.FromResult<IActionResult>(View(inispection));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(InispectionPlan model)
        {
            TblInspectionPlan tible = new TblInspectionPlan();
            List<TblPlanInistitution> plan_in = new List<TblPlanInistitution>();
            try
            {
                tible = _context.TblInspectionPlans.FirstOrDefault(x => x.InspectionPlanId == model.InspectionPlanId);

                tible.PlanTitle = model.PlanTitle;
                tible.PlanDescription = model.PlanDescription;
                tible.CreationDate = model.CreationDate;
                tible.ModificationDate = DateTime.Now;
                tible.YearId = model.YearId;

                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    _notifyService.Success("Plan is updated successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notifyService.Success("Plan isn't updated successfully. Please try again");
                    model.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
                    {
                        Text = a.Name,
                        Value = a.InistId.ToString(),

                    }).ToList();
                    model.InspectionYear = _context.TblYears.Select(a => new SelectListItem
                    {
                        Value = a.YearId.ToString(),
                        Text = a.Year
                    }).ToList();
                    return View(model);
                }

            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TblInspectionPlanExists(model.InspectionPlanId))
                {
                    return NotFound();
                }
                else
                {
                    _notifyService.Success($"Error:{ex.Message} happened. Please try again");
                    model.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
                    {
                        Text = a.Name,
                        Value = a.InistId.ToString(),

                    }).ToList();
                    model.InspectionYear = _context.TblYears.Select(a => new SelectListItem
                    {
                        Value = a.YearId.ToString(),
                        Text = a.Year
                    }).ToList();
                    return View(model);
                }
            }
        }
        public async Task<IActionResult> Accordion()
        {
            return View();
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblInspectionPlans == null)
            {
                return NotFound();
            }

            var tblInspectionPlan = await _context.TblInspectionPlans
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.InspectionPlanId == id);
            if (tblInspectionPlan == null)
            {
                return NotFound();
            }

            return View(tblInspectionPlan);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblInspectionPlans == null)
            {
                return Problem("Entity set 'AtsdbContext.TblInspectionPlans'  is null.");
            }
            var tblInspectionPlan = await _context.TblInspectionPlans.FindAsync(id);
            if (tblInspectionPlan != null)
            {
                _context.TblInspectionPlans.Remove(tblInspectionPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblInspectionPlanExists(Guid id)
        {
            return (_context.TblInspectionPlans?.Any(e => e.InspectionPlanId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Assign(Guid? id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            var isAssigned = _context.TblAssignedYearlyPlans.Where(a => a.SpecificPlanId == id).FirstOrDefault();
            InspectionAssignModel model = new InspectionAssignModel();
            var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == id).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "FLIM").ToList();
            if (isAssigned != null)
            {
                model.PlanTitle = plats.Title;
                model.AssignedBy = userId;
                if (cultur == "am")
                {
                    model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                    {
                        Value = p.StatusId.ToString(),
                        Text = p.StatusAmharic.ToString()
                    }).ToList();
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Value = s.AssigneeTypeId.ToString(),
                        Text = s.AssigneeTypeAmharic.ToString()
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                    {
                        Value = s.TeamId.ToString(),
                        Text = s.TeamNameAmharic,

                    }).ToList();
                }
                else
                {
                    model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                    {
                        Value = p.StatusId.ToString(),
                        Text = p.Status.ToString()
                    }).ToList();
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Value = s.AssigneeTypeId.ToString(),
                        Text = s.AssigneeType.ToString()
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                    {
                        Value = s.TeamId.ToString(),
                        Text = s.TeamName,
                    }).ToList();
                }
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName + " " + g.MidleName
                }).ToList();
                if (plats.TeamId != null)
                {
                    model.TeamId = plats.TeamId;
                }
                model.AssigneeTypeId = plats.AssigneeTypeId;
                model.StatusID = isAssigned.StatusId;
                model.AssignedDate = isAssigned.AssignedDate;
                model.DueDate = isAssigned.DueDate;
                model.SpecificPlanId = id;
                model.Remark = plats.AssigningRemark;
                return View(model);
            }
            else
            {
                model.PlanTitle = plats.Title;
                model.AssignedBy = userId;
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName + " " + g.MidleName
                }).ToList();
                if (cultur == "am")
                {
                    model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                    {
                        Value = p.StatusId.ToString(),
                        Text = p.StatusAmharic.ToString()
                    }).ToList();
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Value = s.AssigneeTypeId.ToString(),
                        Text = s.AssigneeTypeAmharic.ToString()
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                    {
                        Value = s.TeamId.ToString(),
                        Text = s.TeamNameAmharic,

                    }).ToList();
                }
                else
                {
                    model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                    {
                        Value = p.StatusId.ToString(),
                        Text = p.Status.ToString()
                    }).ToList();
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Value = s.AssigneeTypeId.ToString(),
                        Text = s.AssigneeType.ToString()
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                    {
                        Value = s.TeamId.ToString(),
                        Text = s.TeamName,

                    }).ToList();
                }
                model.AssignedDate = DateTime.Now;
                model.DueDate = DateTime.Now.AddDays(10);
                model.SpecificPlanId = id;
                return View(model);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(InspectionAssignModel model)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            TblAssignedYearlyPlan yearlyPlan;
            List<TblAssignedYearlyPlan> tblAssignedYearlyPlans = new List<TblAssignedYearlyPlan>();
            List<string> depHeadEmail = new List<string>();
            List<string> teamEmails = new List<string>();
            List<Guid> teamLeaderIds = new List<Guid>();
            List<string> assignees = new List<string>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.ToList().Except(loggedInUser);
            if (model.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847"))
            {
                var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
                var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "Assigned to Team").FirstOrDefault();
                plats.AssigneeTypeId = model.AssigneeTypeId;
                plats.ProId = status.ProId;
                plats.IsAssignedToTeam = true;
                var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.PlanId == model.SpecificPlanId).FirstOrDefault();
                var teamLeader = (from items in _context.TblTeams
                                  join users in _context.TblInternalUsers on items.TeamLeaderId equals users.UserId
                                  where items.TeamId == model.TeamId
                                  select new { EmailAddress= users.EmailAddress, userId=users.UserId }).ToList();
                foreach (var item in teamLeader)
                {
                    teamEmails.Add(item.EmailAddress);
                    teamLeaderIds.Add(item.userId);
                }
                plats.TeamId = model.TeamId;
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    await SendMail(teamEmails, "Inpection Plans are assignment Notification", "<h3>Inpection plans are assigned to your team. Please review the assigned plans on Task Tracking Dashboard and assign to experts</h3>");
                    _notifyService.Success("Plan is successfully assigned to team");
                    _notificationService.saveNotification(userId, teamLeaderIds, "Inpection plans are assigned to your team. Please review the assigned plans on Task Tracking Dashboard and assign to experts");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notifyService.Error("Assingment isn't successfull. Please and try again");
                    model.Users = AllUsers.Select(g => new SelectListItem
                    {
                        Value = g.UserId.ToString(),
                        Text = g.FirstName
                    }).ToList();
                    if (cultur == "am")
                    {
                        model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                        {
                            Value = p.StatusId.ToString(),
                            Text = p.StatusAmharic.ToString()
                        }).ToList();
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Value = s.AssigneeTypeId.ToString(),
                            Text = s.AssigneeTypeAmharic.ToString()
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                        {
                            Value = s.TeamId.ToString(),
                            Text = s.TeamNameAmharic,
                        }).ToList();
                    }
                    else
                    {
                        model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                        {
                            Value = p.StatusId.ToString(),
                            Text = p.Status.ToString()
                        }).ToList();
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Value = s.AssigneeTypeId.ToString(),
                            Text = s.AssigneeType.ToString()
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                        {
                            Value = s.TeamId.ToString(),
                            Text = s.TeamName,
                        }).ToList();
                    }


                    return View(model);
                }

            }
            else
            {

                var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
                var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "Assigned to expert").FirstOrDefault();
                plats.AssigneeTypeId = model.AssigneeTypeId;
                plats.IsAssignedToUser = true;
                var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
                if (ifAssigned != null)
                {
                    _context.TblAssignedYearlyPlans.Remove(ifAssigned);
                    _context.SaveChanges();
                }
                foreach (var item in model.UserId)
                {
                    teamLeaderIds.Add(item);

                    var usersTeam = _context.TblInternalUsers.Find(item);
                    assignees.Add(usersTeam.EmailAddress);
                    yearlyPlan = new TblAssignedYearlyPlan();
                    yearlyPlan.AssignedBy = userId;
                    yearlyPlan.AssignedTo = item;
                    yearlyPlan.AssignedDate = model.AssignedDate;
                    yearlyPlan.DueDate = model.DueDate;
                    yearlyPlan.Remark = model.Remark;
                    yearlyPlan.SpecificPlanId = model.SpecificPlanId;
                    plats.ProId = status.ProId;
                    plats.ModificationDate = DateTime.Now;
                    plats.AssigningRemark = model.Remark;
                    tblAssignedYearlyPlans.Add(yearlyPlan);
                }
                _context.TblAssignedYearlyPlans.AddRange(tblAssignedYearlyPlans);
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                    _notificationService.saveNotification(userId, teamLeaderIds, "Inpection plans are assigned to your team. Please review the assigned plans on Task Tracking Dashboard and assign to experts");
                    await SendMail(assignees, "Inpection Plans are assignment Notification", "<h3>Inpection plans are assigned to your team. Please review the assigned plans on Task Tracking Dashboard and assign to experts</h3>");

                    _notifyService.Success("Plan successfully assigned");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notifyService.Error("Assingment isn't successfull. Please and try again");
                    model.Users = AllUsers.Select(g => new SelectListItem
                    {
                        Value = g.UserId.ToString(),
                        Text = g.FirstName
                    }).ToList();
                    model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                    {
                        Value = p.StatusId.ToString(),
                        Text = p.Status.ToString()
                    }).ToList();
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Value = s.AssigneeTypeId.ToString(),
                        Text = s.AssigneeType.ToString()
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                    {
                        Value = s.TeamId.ToString(),
                        Text = s.TeamName,
                    }).ToList();
                    return View(model);
                }

            }
        }
        public async Task<IActionResult> AssignFromTeam(Guid? id)
        {

            var isAssigned = _context.TblAssignedYearlyPlans.FirstOrDefault(a => a.PlanId == id.Value);
            var usersIds = _context.TblAssignedYearlyPlans.Where(a => a.PlanId == id.Value).Select(S => S.AssignedTo).ToArray();
            InspectionAssignModel model = new InspectionAssignModel();

            var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == id).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "FLIM").ToList().Except(loggedInUser);
            if (isAssigned != null)
            {
                model.PlanTitle = plats.Title;
                model.AssignedBy = userId;
                model.Teams = _context.TblTeams.Where(s => s.TeamId == plats.TeamId).Select(s => new SelectListItem
                {
                    Value = s.TeamId.ToString(),
                    Text = s.TeamName
                }).ToList();
                model.TeamId = plats.TeamId;
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName
                }).ToList();
                // model.UserId = usersIds?;
                model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                {
                    Value = p.StatusId.ToString(),
                    Text = p.Status.ToString()
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Where(s => s.AssigneeTypeId == plats.AssigneeTypeId).Select(s => new SelectListItem
                {
                    Value = s.AssigneeTypeId.ToString(),
                    Text = s.AssigneeType.ToString()
                }).ToList();
                model.AssigneeTypeId = plats.AssigneeTypeId;
                model.StatusID = isAssigned.StatusId;
                model.AssignedDate = isAssigned.AssignedDate;
                model.DueDate = isAssigned.DueDate;
                model.SpecificPlanId = id;
                model.Remark = plats.AssigningRemark;
                return View(model);
            }
            else
            {
                model.PlanTitle = plats.Title;
                model.AssignedBy = userId;
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName
                }).ToList();
                model.status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "New").Select(p => new SelectListItem
                {
                    Value = p.ProId.ToString(),
                    Text = p.ProstatusTitle.ToString()
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Where(s => s.AssigneeTypeId == plats.AssigneeTypeId).Select(s => new SelectListItem
                {
                    Value = s.AssigneeTypeId.ToString(),
                    Text = s.AssigneeType
                }).ToList();
                model.AssigneeTypeId = plats.AssigneeTypeId;
                model.Teams = _context.TblTeams.Where(s => s.TeamId == plats.TeamId).Select(s => new SelectListItem
                {
                    Value = s.TeamId.ToString(),
                    Text = s.TeamName
                }).ToList();
                model.TeamId = model.TeamId;
                model.AssignedDate = DateTime.Now;
                model.DueDate = DateTime.Now.AddDays(10);
                model.SpecificPlanId = id;
                return View(model);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignFromTeam(InspectionAssignModel model)
        {

            TblAssignedYearlyPlan yearlyPlan;
            List<TblAssignedYearlyPlan> tblAssignedYearlyPlans = new List<TblAssignedYearlyPlan>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.ToList().Except(loggedInUser);
            var plats = _context.TblSpecificPlans.Where(p => p.InspectionPlanId == model.SpecificPlanId).FirstOrDefault();
            var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "Assigned to expert").FirstOrDefault();

            var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (ifAssigned != null)
                {
                    _context.TblAssignedYearlyPlans.Remove(ifAssigned);
                    _context.SaveChanges();
                }
                plats.IsAssignedToUser = true;
                foreach (var item in model.UserId)
                {
                    yearlyPlan = new TblAssignedYearlyPlan();
                    yearlyPlan.AssignedBy = model.AssignedBy;
                    yearlyPlan.AssignedTo = item;
                    yearlyPlan.AssignedDate = model.AssignedDate;
                    yearlyPlan.DueDate = model.DueDate;
                    yearlyPlan.Remark = model.Remark;
                    yearlyPlan.SpecificPlanId = model.SpecificPlanId;
                    plats.ProId = status.ProId;
                    plats.ModificationDate = DateTime.Now;
                    plats.AssigningRemark = model.Remark;
                    tblAssignedYearlyPlans.Add(yearlyPlan);
                }
                _context.TblAssignedYearlyPlans.AddRange(tblAssignedYearlyPlans);
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                    _notifyService.Success("Plan successfully assigned");
                    return RedirectToAction("Index");
                }
                else
                {
                    model.SpecificPlanId = model.SpecificPlanId;
                    model.Users = AllUsers.Select(g => new SelectListItem
                    {
                        Value = g.UserId.ToString(),
                        Text = g.FirstName
                    }).ToList();
                    model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                    {
                        Value = p.StatusId.ToString(),
                        Text = p.Status.ToString()
                    }).ToList();
                    model.AssignmentTypes = _context.TblAssignementTypes.Where(s => s.AssigneeTypeId == plats.AssigneeTypeId).Select(s => new SelectListItem
                    {
                        Value = s.AssigneeTypeId.ToString(),
                        Text = s.AssigneeType.ToString()
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.TeamId == plats.TeamId).Select(s => new SelectListItem
                    {
                        Value = s.TeamId.ToString(),
                        Text = s.TeamName
                    }).ToList();
                    model.TeamId = model.TeamId;
                    return View(model);
                }
            }
            else
            {

                model.SpecificPlanId = model.SpecificPlanId;
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName
                }).ToList();
                model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                {
                    Value = p.StatusId.ToString(),
                    Text = p.Status.ToString()
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Where(s => s.AssigneeTypeId == plats.AssigneeTypeId).Select(s => new SelectListItem
                {
                    Value = s.AssigneeTypeId.ToString(),
                    Text = s.AssigneeType.ToString()
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.TeamId == plats.TeamId).Select(s => new SelectListItem
                {
                    Value = s.TeamId.ToString(),
                    Text = s.TeamName
                }).ToList();
                model.TeamId = model.TeamId;
                return View(model);
            }


        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
        public async Task<IActionResult> UppdateDesicionStatus(Guid? id)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            var plaDetail = _context.TblSpecificPlans.Where(s => s.SpecificPlanId == id).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser user = await _context.TblInternalUsers.FindAsync(userId);
            model.SpecificPlanId = id;
            model.PlanTitle = plaDetail.Title;
            model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
            {
                Text = x.StatusName,
                Value = x.DesStatusId.ToString()
            }).ToList();
            if (user.IsDepartmentHead == true)
            {
                ViewBag.visible = "visible";
            }
            else
            {
                ViewBag.visible = "none";
            }
            model.IsDeputyApprovalNeeded = false;

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UppdateDesicionStatus(InspectionAssignModel model)
        {
            var yearlyPlan = _context.TblSpecificPlans.Find(model.SpecificPlanId);
            var waitingStatus = _context.TblDecisionStatuses.Where(s => s.StatusName == "Waiting for Upproval").FirstOrDefault();
            var status = _context.TblInspectionStatuses.Where(s => s.ProstatusTitle == "Completed").FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser user = await _context.TblInternalUsers.FindAsync(userId);
            TblTopStatus topStatus = _context.TblTopStatuses.Where(s => s.StatusName == "Completed").FirstOrDefault();
            var desicionStatus = _context.TblDecisionStatuses.Where(s => s.StatusName == "Upproved").FirstOrDefault();
            if (user.IsTeamLeader == true)
            {
                yearlyPlan.IsUpprovedbyTeam = model.DesStatusId;
                yearlyPlan.IsUprovedByDeputy = waitingStatus.DesStatusId;
            }
            else if (user.IsDepartmentHead == true)
            {
                yearlyPlan.IsUpprovedbyDepartment = model.DesStatusId;
                if (model.IsDeputyApprovalNeeded == false)
                {
                    if (model.DesStatusId == desicionStatus.DesStatusId)
                    {
                        yearlyPlan.FinalStatus = true;

                    }
                    yearlyPlan.ProId = status.ProId;
                    yearlyPlan.IsUprovedByDeputy = model.DesStatusId;
                }
                else
                {
                    yearlyPlan.IsUprovedByDeputy = waitingStatus.DesStatusId;
                }
            }
            else if (user.IsDeputy == true)
            {
                if (model.DesStatusId == desicionStatus.DesStatusId)
                {
                    yearlyPlan.FinalStatus = true;
                }
                yearlyPlan.IsUprovedByDeputy = model.DesStatusId;
                yearlyPlan.ProId = status.ProId;
            }
            else
            {
                model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.DesStatusId.ToString()
                }).ToList();
                return View(model);
            }
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Upproval status changed successfully!");
                return RedirectToAction(nameof(OngoingPlan));
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
        public async Task<IActionResult> SendToInstitutions(Guid? SpecificPlanId)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser user = await _context.TblInternalUsers.FindAsync(userId);
            model.SpecificPlanId = SpecificPlanId;
            model.ExpectedReplyDate = DateTime.Now.AddDays(20);
            model.Insititutions = _context.TblInistitutions.Select(s => new SelectListItem
            {
                Value = s.InistId.ToString(),
                Text = s.Name.ToString()
            }).ToList();
            model.status = _context.TblInspectionStatuses.Where(s => s.ProstatusTitle == "Report sent to Institution").Select(s => new SelectListItem
            {
                Text = s.ProstatusTitle,
                Value = s.ProId.ToString()
            }).ToList();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToInstitutions(InspectionAssignModel model)
        {
            TblSentInspection? yearlyPlan = new TblSentInspection();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var yearlyPla = _context.TblSpecificPlans.Find(model.SpecificPlanId);

            yearlyPlan.SentDate = DateTime.Now;
            yearlyPlan.SentBy = userId;
            yearlyPlan.ExpectedReplyDate = model.ExpectedReplyDate;
            yearlyPlan.InstId = model.InistId;
            yearlyPlan.SpecificPlanId = model.SpecificPlanId;
            yearlyPlan.SendingRemark = model.SendingRemark;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileInfo fileInfo = new FileInfo(model.SentReport.FileName);
            string fileName = Guid.NewGuid().ToString() + model.SentReport.FileName;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.SentReport.CopyTo(stream);
            }
            string dbPath = "/Files/" + fileName;
            yearlyPlan.SentReport = dbPath;
            FileInfo fileInfos = new FileInfo(model.OfficialLetter.FileName);
            string fileNames = Guid.NewGuid().ToString() + model.OfficialLetter.FileName;
            string fileNameWithPaths = Path.Combine(path, fileNames);
            using (var stream = new FileStream(fileNameWithPaths, FileMode.Create))
            {
                model.OfficialLetter.CopyTo(stream);
            }
            string dbPaths = "/admin/Files/" + fileNames;
            yearlyPlan.OfficialLetter = dbPaths;
            yearlyPla.ProId = model.StatusID;
            _context.TblSentInspections.Add(yearlyPlan);
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Reccomendation is sent successfully!");
                return RedirectToAction(nameof(OngoingPlan));
            }
            else
            {
                model.status = _context.TblInspectionStatuses.Where(s => s.ProstatusTitle == "Report sent to Institution").Select(s => new SelectListItem
                {
                    Text = s.ProstatusTitle,
                    Value = s.ProId.ToString()
                }).ToList();
                _notifyService.Error("Recomendation isn't sent. Please try again");
                return View(model);
            }
        }
        public async Task<IActionResult> Responses(int RecId)
        {
            InspectionReplyModel model = new InspectionReplyModel();
            ViewData["Replies"] = _context.TblInspectionReplyes.Include(s => s.Rec).Include(s => s.InternalUserNavigation).Include(s => s.ExternalUserNavigation).Where(s => s.RecId == RecId).OrderByDescending(s => s.ReplyId).ToList();
            model.RecId = RecId;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Responses(InspectionReplyModel replyModel)
        {
            List<string> emails = new List<string>();
            var infor = (from items in _context.TblSentInspections
                         join insts in _context.TblInspectionPlans on items.InspectionPlanId equals insts.InspectionPlanId
                         join tekuan in _context.TblInistitutions on items.InstId equals tekuan.InistId
                         join users in _context.TblExternalUsers on tekuan.InistId equals users.InistId
                         where items.RecId == replyModel.RecId
                         select new
                         {
                             users.Email
                         }).ToList();
            foreach (var item in infor)
            {
                emails.Add(item.Email);
            }
            InspectionReplyModel model = new InspectionReplyModel();
            model.RecId = replyModel.RecId;
            TblInspectionReplye replys = new TblInspectionReplye();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            replys.RecoDetail = replyModel.ResponseDetail;
            replys.DateCreated = DateTime.Now;
            replys.RecId = replyModel.RecId;
            replys.IsInternal = true;
            replys.IsExternal = false;
            replys.InternalUser = userId;
            string? dbPath = null;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (replyModel.Attachement != null)
            {
                //get file extension
                FileInfo fileInfo = new FileInfo(replyModel.Attachement.FileName);
                string fileName = Guid.NewGuid().ToString() + replyModel.Attachement.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    replyModel.Attachement.CopyTo(stream);
                }
                dbPath = "/admin/Files/" + fileName;
                replys.Attachement = dbPath;
            }
            _context.TblInspectionReplyes.Add(replys);
            int sent = _context.SaveChanges();
            if (sent > 0)
            {
                ViewData["Replies"] = _context.TblInspectionReplyes.Include(s => s.Rec).Include(s => s.InternalUserNavigation).Include(s => s.ExternalUserNavigation).Where(s => s.RecId == replyModel.RecId).OrderByDescending(s => s.ReplyId).ToList();
                await SendMail(emails, "", "");
                _notifyService.Success("Recomendation response is successfully replied");
                return View(model);
            }
            else
            {
                ViewData["Replies"] = _context.TblInspectionReplyes.Include(s => s.Rec).Include(s => s.InternalUserNavigation).Include(s => s.ExternalUserNavigation).Where(s => s.RecId == replyModel.RecId).OrderByDescending(s => s.ReplyId).ToList();
                _notifyService.Error("Recomendation response isn't succeesfully replied. Please try again");
                return View(replyModel);
            }
        }
        public async Task<IActionResult> EditResponses(int ReplyId)
        {
            var reply = _context.TblInspectionReplyes.Find(ReplyId);
            InspectionReplyModel replyModel = new InspectionReplyModel();
            replyModel.ReplyId = ReplyId;
            replyModel.RecId = reply.RecId;
            replyModel.ResponseDetail = reply.RecoDetail;

            return View(replyModel);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditResponses(InspectionReplyModel replyModel)
        {
            try
            {
                var reply = _context.TblInspectionReplyes.Find(replyModel.ReplyId);
                reply.RecoDetail = replyModel.ResponseDetail;
                string? dbPath = null;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (replyModel.Attachement != null)
                {
                    //get file extension
                    FileInfo fileInfo = new FileInfo(replyModel.Attachement.FileName);
                    string fileName = Guid.NewGuid().ToString() + replyModel.Attachement.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        replyModel.Attachement.CopyTo(stream);
                    }
                    dbPath = "/admin/Files/" + fileName;
                    reply.Attachement = dbPath;
                }
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    _notifyService.Success("Reply is uppdated successfully");
                    return RedirectToAction(nameof(Responses), new { RecId = replyModel.RecId });

                }
                _notifyService.Error("Reply ism't updated successfully. Please try again");
                return View(replyModel);
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error:{ex.Message} happened. Reply isn't updated successfully. Please try again");
                return View(replyModel);
            }

        }
        public IActionResult PDFViewerNewTab(string? filePath)
        {
            return File(System.IO.File.ReadAllBytes(filePath), "application/pdf");
        }
        public async Task<IActionResult> priveweier(string attachment)
        {
            string filename = attachment.Substring(7);
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }
        public async Task<IActionResult> ReAssign(Guid? id)
        {

            var isAssigned = _context.TblAssignedYearlyPlans.Where(a => a.SpecificPlanId == id).FirstOrDefault();
            InspectionAssignModel model = new InspectionAssignModel();
            var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == id).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));

            var AllUsers = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "FLIM").ToList();
            if (isAssigned != null)
            {
                model.PlanTitle = plats.Title;
                model.AssignedBy = userId;
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName
                }).ToList();
                model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                {
                    Value = p.StatusId.ToString(),
                    Text = p.Status.ToString()
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Value = s.AssigneeTypeId.ToString(),
                    Text = s.AssigneeType.ToString()
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                {
                    Value = s.TeamId.ToString(),
                    Text = s.TeamName,
                }).ToList();
                if (plats.TeamId != null)
                {
                    model.TeamId = plats.TeamId;
                }
                model.AssigneeTypeId = plats.AssigneeTypeId;
                model.StatusID = isAssigned.StatusId;
                model.AssignedDate = isAssigned.AssignedDate;
                model.DueDate = isAssigned.DueDate;
                model.SpecificPlanId = id;
                model.Remark = plats.AssigningRemark;
                return View(model);
            }
            else
            {
                model.PlanTitle = plats.Title;
                model.AssignedBy = userId;
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName
                }).ToList();
                model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                {
                    Value = p.StatusId.ToString(),
                    Text = p.Status.ToString()
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Value = s.AssigneeTypeId.ToString(),
                    Text = s.AssigneeType.ToString()
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                {
                    Value = s.TeamId.ToString(),
                    Text = s.TeamName,

                }).ToList();
                model.AssignedDate = DateTime.Now;
                model.DueDate = DateTime.Now.AddDays(10);
                model.SpecificPlanId = id;
                return View(model);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReAssign(InspectionAssignModel model)
        {

            TblAssignedYearlyPlan yearlyPlan;
            List<TblAssignedYearlyPlan> tblAssignedYearlyPlans = new List<TblAssignedYearlyPlan>();
            List<String> depHeadEmail = new List<string>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.ToList().Except(loggedInUser);
            if (model.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847"))
            {
                var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
                var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "Assigned to Team").FirstOrDefault();
                plats.AssigneeTypeId = model.AssigneeTypeId;
                plats.ProId = status.ProId;
                plats.IsAssignedToTeam = true;
                var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.PlanId == model.SpecificPlanId).FirstOrDefault();
                var teamLeader = (from items in _context.TblTeams
                                  join users in _context.TblInternalUsers on items.TeamLeaderId equals users.UserId
                                  where items.TeamId == model.TeamId
                                  select users.EmailAddress).ToList();
                plats.TeamId = model.TeamId;
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    await SendMail(teamLeader, "Inpection Plans are assignment Notification", "<h3>Inpection plans are assigned to your team. Please review the assigned plans on Task Tracking Dashboard and assign to experts</h3>");
                    _notifyService.Success("Plan is successfully assigned to team");
                    return RedirectToAction(nameof(Index));
                }
                _notifyService.Error("Assingment isn't successfull. Please and try again");
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName
                }).ToList();
                model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                {
                    Value = p.StatusId.ToString(),
                    Text = p.Status.ToString()
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Value = s.AssigneeTypeId.ToString(),
                    Text = s.AssigneeType.ToString()
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                {
                    Value = s.TeamId.ToString(),
                    Text = s.TeamName,
                }).ToList();
                return View(model);
            }
            else
            {

                var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
                var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "Assigned to expert").FirstOrDefault();
                plats.AssigneeTypeId = model.AssigneeTypeId;
                plats.IsAssignedToUser = true;
                var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
                if (ifAssigned != null)
                {
                    _context.TblAssignedYearlyPlans.Remove(ifAssigned);
                    _context.SaveChanges();
                }
                foreach (var item in model.UserId)
                {
                    var usersTeam = _context.TblInternalUsers.Find(item);
                    yearlyPlan = new TblAssignedYearlyPlan();
                    yearlyPlan.AssignedBy = userId;
                    yearlyPlan.AssignedTo = item;
                    yearlyPlan.AssignedDate = model.AssignedDate;
                    yearlyPlan.DueDate = model.DueDate;
                    yearlyPlan.Remark = model.Remark;
                    yearlyPlan.SpecificPlanId = model.SpecificPlanId;
                    plats.ProId = status.ProId;
                    plats.ModificationDate = DateTime.Now;
                    plats.AssigningRemark = model.Remark;
                    tblAssignedYearlyPlans.Add(yearlyPlan);
                }
                _context.TblAssignedYearlyPlans.AddRange(tblAssignedYearlyPlans);
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                    _notifyService.Success("Plan successfully assigned");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notifyService.Error("Assingment isn't successfull. Please and try again");
                    model.Users = AllUsers.Select(g => new SelectListItem
                    {
                        Value = g.UserId.ToString(),
                        Text = g.FirstName
                    }).ToList();
                    model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                    {
                        Value = p.StatusId.ToString(),
                        Text = p.Status.ToString()
                    }).ToList();
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Value = s.AssigneeTypeId.ToString(),
                        Text = s.AssigneeType.ToString()
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                    {
                        Value = s.TeamId.ToString(),
                        Text = s.TeamName,
                    }).ToList();
                    return View(model);
                }

            }
        }
        public IActionResult AddCheckList(Guid? id)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);
            model.Id = assignedTasks.Id;
            model.AssignedBy = assignedTasks.AssignedBy;
            model.PlanTitle = yearlyPlan.Title;
            model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
            model.status = _context.TblInspectionStatuses.Where(s => s.ProstatusTitle == "Evaluation Criteria").Select(s => new SelectListItem
            {
                Text = s.ProstatusTitle,
                Value = s.ProId.ToString()
            }).ToList();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCheckList(InspectionAssignModel? model)
        {
            var emails = _context.TblInternalUsers.Where(s => s.UserId == model.AssignedBy).Select(s => s.EmailAddress).ToList();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
            var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);
            assignedTasks.EvaluationCheckLists = model.EvaluationCheckLists;
            if (model.EvaluationCheckListsAttachmet != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                FileInfo fileInfo = new FileInfo(model.EvaluationCheckListsAttachmet.FileName);
                string fileName = Guid.NewGuid().ToString() + model.EvaluationCheckListsAttachmet.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.EvaluationCheckListsAttachmet.CopyTo(stream);
                }
                string dbPath = "/admin/Files/" + fileName;
                assignedTasks.EvaluationCheckListsAttachmet = dbPath;
            }
            yearlyPlan.ProId = model.StatusID;
            int added = _context.SaveChanges();
            if (added > 0)
            {
                await SendMail(emails, "Notification from Task Tracking Dashboard", "Expert is added Evaluation criteria for his assigned task. Please review on system.");
                _notifyService.Success("Check lists are successfully added");
                return RedirectToAction("Index");
            }
            else
            {
                _notifyService.Error("Check Lists aren't successfully created. Please try again");
                return View(model);
            }
        }
        public IActionResult AddEngagementLetter(Guid? id)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);
            model.Id = assignedTasks.Id;
            model.AssignedBy = assignedTasks.AssignedBy;
            model.PlanTitle = yearlyPlan.Title;
            model.status = _context.TblInspectionStatuses.Where(s => s.ProstatusTitle == "Engagement Letter").Select(s => new SelectListItem
            {
                Text = s.ProstatusTitle,
                Value = s.ProId.ToString()
            }).ToList();

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddEngagementLetter(InspectionAssignModel? model)
        {
            try
            {
                var emails = _context.TblInternalUsers.Where(s => s.UserId == model.AssignedBy).Select(s => s.EmailAddress).ToList();

                TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
                var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);

                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(model.EngagementLetter.FileName);
                string fileName = Guid.NewGuid().ToString() + model.EngagementLetter.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.EngagementLetter.CopyTo(stream);
                }
                string dbPath = "/admin/Files/" + fileName;
                assignedTasks.EngagementLetter = dbPath;
                assignedTasks.Remark = model.Remark;

                yearlyPlan.ProId = model.StatusID;
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    await SendMail(emails, "Notification from Task Tracking Dashboard", "Expert added Engagement letter for his assigned task. Please review on system.");
                    _notifyService.Success("Engagement letter is successfully added.");
                    return RedirectToAction(nameof(AssignedRequests));
                }
                else
                {
                    _notifyService.Error("Engagement letter isn't added successfully. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                _notifyService.Error($"Error:{ex.Message} happened. Engagement letter isn't added successfully. Please try again");
                return View(model);
            }

        }
        public IActionResult AddTORAttachement(Guid? id)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);
            model.Id = assignedTasks.Id;
            model.status = _context.TblInspectionStatuses.Where(s => s.ProstatusTitle == "TOR Attached").Select(s => new SelectListItem
            {
                Text = s.ProstatusTitle,
                Value = s.ProId.ToString()
            }).ToList();

            if (yearlyPlan != null)
            {
                model.PlanTitle = yearlyPlan.Title;
            }

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult AddTORAttachement(InspectionAssignModel? model)
        {
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
            var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            FileInfo fileInfo = new FileInfo(model.TORAttachment.FileName);
            string fileName = Guid.NewGuid().ToString() + model.TORAttachment.FileName;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.TORAttachment.CopyTo(stream);
            }
            string dbPath = "/admin/Files/" + fileName;
            assignedTasks.Torattachment = dbPath;
            assignedTasks.Remark = model.Remark;

            yearlyPlan.ProId = model.StatusID;
            int updated = _context.SaveChanges();
            if (updated > 0)
            {
                _notifyService.Success("TOR is successfully uppdated");
                return RedirectToAction(nameof(AssignedRequests));
            }
            else
            {
                return View(model);
            }
        }
        public IActionResult UpdateProgressStatus(Guid? id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var user = _context.TblInternalUsers.Find(userId);
            List<TblInspectionStatus> statuces = new List<TblInspectionStatus>();
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);
            model.AssignedBy = assignedTasks.AssignedBy;
            model.Id = assignedTasks.Id;
            model.PlanTitle = yearlyPlan.Title;
            model.Remark = assignedTasks.Remark;
            model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
            if (user.IsDeputy == true || user.IsDepartmentHead == true)
            {
                statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder >= 12).OrderBy(s => s.ProgressOrder).ToList();
            }
            else if (user.IsDefaultUser == true)
            {
                statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder <= 11).OrderBy(s => s.ProgressOrder).ToList();
            }
            model.status = statuces.Select(p => new SelectListItem
            {
                Value = p.ProId.ToString(),
                Text = p.ProstatusTitle,
            }).ToList();
            model.StatusID = yearlyPlan.ProId;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProgressStatus(InspectionAssignModel? model)
        {
            try
            {
                var status = _context.TblInspectionStatuses.Find(model.StatusID);
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var user = _context.TblInternalUsers.Find(userId);
                List<TblInspectionStatus> statuces = new List<TblInspectionStatus>();
                var emails = _context.TblInternalUsers.Where(s => s.UserId == model.AssignedBy).Select(s => s.EmailAddress).ToList();
                var desicionStatus = _context.TblDecisionStatuses.Where(s => s.StatusName == "Waiting for Upproval").FirstOrDefault();
                TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
                var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);
                yearlyPlan.ProId = model.StatusID;
                if (status.ProstatusTitle == "TOR Attached")
                {
                    if (model.TORAttachment != null)
                    {

                        string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        FileInfo fileInfo = new FileInfo(model.TORAttachment.FileName);
                        string fileName = Guid.NewGuid().ToString() + model.TORAttachment.FileName;
                        string fileNameWithPath = Path.Combine(path, fileName);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            model.TORAttachment.CopyTo(stream);
                        }
                        string dbPath = "/admin/Files/" + fileName;
                        assignedTasks.Torattachment = dbPath;
                    }
                        //divTORAttachment
                }
                else if (status.ProstatusTitle == "Engagement Letter")
                {
                    //engagement
                    if (model.EngagementLetter!=null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                        //create folder if not exist
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        //get file extension
                        FileInfo fileInfo = new FileInfo(model.EngagementLetter.FileName);
                        string fileName = Guid.NewGuid().ToString() + model.EngagementLetter.FileName;
                        string fileNameWithPath = Path.Combine(path, fileName);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            model.EngagementLetter.CopyTo(stream);
                        }
                        string dbPath = "/admin/Files/" + fileName;
                        assignedTasks.EngagementLetter = dbPath;
                    }
                  
                }
                else if (status.ProstatusTitle == "Evaluation Criteria")
                {
                    if (model.EvaluationCheckListsAttachmet!=null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        FileInfo fileInfo = new FileInfo(model.EvaluationCheckListsAttachmet.FileName);
                        string fileName = Guid.NewGuid().ToString() + model.EvaluationCheckListsAttachmet.FileName;
                        string fileNameWithPath = Path.Combine(path, fileName);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            model.EvaluationCheckListsAttachmet.CopyTo(stream);
                        }
                        string dbPath = "/admin/Files/" + fileName;
                        assignedTasks.EvaluationCheckListsAttachmet = dbPath;
                    }
                    else
                    {
                        assignedTasks.EvaluationCheckLists = model.EvaluationCheckLists;
                    }
                }
                else if (status.ProstatusTitle == "First Draft Completed")
                {
                    //2dc5bc83-0330-4b60-b142-44c1f7296e7c
                    TblInspectionReportFile reportFile = new TblInspectionReportFile();

                    if (model.FinalReport!=null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                        //create folder if not exist
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        //get file extension
                        FileInfo fileInfo = new FileInfo(model.FinalReport.FileName);
                        string fileName = Guid.NewGuid().ToString() + model.FinalReport.FileName;
                        string fileNameWithPath = Path.Combine(path, fileName);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            model.FinalReport.CopyTo(stream);
                        }
                        string dbPath = "/admin/Files/" + fileName;
                        assignedTasks.FinalReport = dbPath;
                        reportFile.ReportFiles = dbPath;
                    }     
                    reportFile.CreatedDate = DateTime.Now;
                    reportFile.CreatedBy = userId;
                    reportFile.Feedback ="First draft report is added. Please review on the system";
                    reportFile.Id = model.Id;
                    reportFile.FileName = model.FinalReport.FileName;
                    reportFile.SpecificPlanId = assignedTasks.SpecificPlanId;                   
                    if (user.IsDefaultUser == true)
                    {
                        reportFile.ForDepartment = true;
                    }
                    else if (user.IsDeputy == true)
                    {
                        reportFile.ForDepartment = true;
                    }
                    else if (user.IsDepartmentHead == true)
                    {
                        reportFile.ForDepartment = true;
                    }
                    _context.TblInspectionReportFiles.Add(reportFile);
                }
                else if (status.ProstatusTitle == "Exit Confrence")
                {
                    if (model.ExitConfirence != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        FileInfo fileInfo = new FileInfo(model.ExitConfirence.FileName);
                        string fileName = Guid.NewGuid().ToString() + model.ExitConfirence.FileName;
                        string fileNameWithPath = Path.Combine(path, fileName);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            model.ExitConfirence.CopyTo(stream);
                        }
                        string dbPath = "/admin/Files/" + fileName;
                        assignedTasks.ExitConfrence = dbPath;
                    }
                }
                else if (status.ProstatusTitle == "Department Review")
                {
                    if (model.DepartmentReview != null)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        FileInfo fileInfo = new FileInfo(model.DepartmentReview.FileName);
                        string fileName = Guid.NewGuid().ToString() + model.DepartmentReview.FileName;
                        string fileNameWithPath = Path.Combine(path, fileName);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            model.DepartmentReview.CopyTo(stream);
                        }
                        string dbPath = "/admin/Files/" + fileName;
                        assignedTasks.DepartmentReview = dbPath;
                    }
                }
                assignedTasks.Remark = model.Remark;
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    await SendMail(emails, "Notification from Task Tracking Dashboard", "Expert uppdated progress status for his assigned task. Please review on system.");
                    _notifyService.Success("Progress is successfully updated.");
                    return RedirectToAction(nameof(AssignedRequests));
                }
                else
                {
                    if (user.IsDeputy == true || user.IsDepartmentHead == true)
                    {
                        statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder >= 12).OrderBy(s => s.ProgressOrder).ToList();
                    }
                    else if (user.IsDefaultUser == true)
                    {
                        statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder <= 11).OrderBy(s => s.ProgressOrder).ToList();
                    }
                    model.status = statuces.Select(p => new SelectListItem
                    {
                        Value = p.ProId.ToString(),
                        Text = p.ProstatusTitle,
                    }).ToList();
                    ViewBag.StatusId = new SelectList(statuces, "ProId", "ProstatusTitle", assignedTasks.StatusId);
                    _notifyService.Error("Progress isn't successfully uppdated. Please try again");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var user = _context.TblInternalUsers.Find(userId);
                List<TblInspectionStatus> statuces = new List<TblInspectionStatus>();
                if (user.IsDeputy == true || user.IsDepartmentHead == true)
                {
                    statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder >= 12).OrderBy(s => s.ProgressOrder).ToList();

                }
                else if (user.IsDefaultUser == true)
                {
                    statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder <= 11).OrderBy(s => s.ProgressOrder).ToList();

                }
                model.status = statuces.Select(p => new SelectListItem
                {
                    Value = p.ProId.ToString(),
                    Text = p.ProstatusTitle,

                }).ToList();
                ViewBag.StatusId = new SelectList(statuces, "ProId", "ProstatusTitle", assignedTasks.StatusId);
                _notifyService.Error($"Error:{ex.Message} happened. Progress isn't successfully uppdated. Please try again");
                return View(model);
            }
        }

        public IActionResult UpdateProgressStatusbyDeapHeade(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var user = _context.TblInternalUsers.Find(userId);
            List<TblInspectionStatus> statuces = new List<TblInspectionStatus>();
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);
            model.AssignedBy = assignedTasks.AssignedBy;
            model.Id = assignedTasks.Id;
            model.PlanTitle = yearlyPlan.Title;
            model.Remark = assignedTasks.Remark;
            model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
            if (user.IsDeputy == true || user.IsDepartmentHead == true)
            {
                statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder >= 12).OrderBy(s => s.ProgressOrder).ToList();
            }
            else if (user.IsDefaultUser == true)
            {
                statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder <= 11).OrderBy(s => s.ProgressOrder).ToList();

            }
            model.status = statuces.Select(p => new SelectListItem
            {
                Value = p.ProId.ToString(),
                Text = p.ProstatusTitle,
            }).ToList();
            ViewBag.StatusId = new SelectList(statuces, "ProId", "ProstatusTitle", yearlyPlan.ProId);

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProgressStatusbyDeapHeade(InspectionAssignModel? model)
        {
            try
            {
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var user = _context.TblInternalUsers.Find(userId);
                var userEmail = _context.TblAssignedYearlyPlans.Include(s => s.AssignedToNavigation).Where(s => s.SpecificPlanId == model.SpecificPlanId).Select(s => s.AssignedToNavigation.EmailAddress).ToList();
                List<TblInspectionStatus> statuces = new List<TblInspectionStatus>();
                TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
                var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);
                //2dc5bc83-0330-4b60-b142-44c1f7296e7c
                TblInspectionReportFile reportFile = new TblInspectionReportFile();

                if (model.FinalReport != null)
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    //get file extension
                    FileInfo fileInfo = new FileInfo(model.FinalReport.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.FinalReport.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.FinalReport.CopyTo(stream);
                    }
                    string dbPath = "/admin/Files/" + fileName;
                    assignedTasks.FinalReport = dbPath;
                    reportFile.ReportFiles = dbPath;
                    reportFile.CreatedDate = DateTime.Now;
                    reportFile.CreatedBy = userId;
                    reportFile.Feedback = model.Remark;
                    reportFile.FileName = model.FinalReport.FileName;
                    reportFile.Id = yearlyPlan.SpecificPlanId;
                    reportFile.SpecificPlanId = assignedTasks.SpecificPlanId;
                    reportFile.ForDepartment = true;
                    _context.TblInspectionReportFiles.Add(reportFile);
                }
               
                yearlyPlan.IsUserUproved = _context.TblDecisionStatuses.Where(s => s.StatusName == "Upproved").Select(s => s.DesStatusId).FirstOrDefault();
                
                yearlyPlan.ProId = model.StatusID;
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    await SendMail(userEmail, "Notification from Task Tracking Dashboard", "Expert uppdated progress status for his assigned task. Please review on system.");
                    _notifyService.Success("Progress is successfully updated.");
                    return RedirectToAction(nameof(AssignedRequests));
                }
                else
                {
                    _notifyService.Error("Progress isn't successfully uppdated. Please try again");

                    model.AssignedBy = assignedTasks.AssignedBy;
                    model.Id = assignedTasks.Id;
                    model.PlanTitle = yearlyPlan.Title;
                    model.Remark = assignedTasks.Remark;
                    model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
                    if (user.IsDeputy == true || user.IsDepartmentHead == true)
                    {
                        statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder >= 12).OrderBy(s => s.ProgressOrder).ToList();
                    }
                    else if (user.IsDefaultUser == true)
                    {
                        statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder <= 11).OrderBy(s => s.ProgressOrder).ToList();

                    }
                    model.status = statuces.Select(p => new SelectListItem
                    {
                        Value = p.ProId.ToString(),
                        Text = p.ProstatusTitle,
                    }).ToList();
                    ViewBag.StatusId = new SelectList(statuces, "ProId", "ProstatusTitle", yearlyPlan.ProId);

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);

                var user = _context.TblInternalUsers.Find(userId);
                List<TblInspectionStatus> statuces = new List<TblInspectionStatus>();
                model.AssignedBy = assignedTasks.AssignedBy;
                model.Id = assignedTasks.Id;
                model.PlanTitle = yearlyPlan.Title;
                model.Remark = assignedTasks.Remark;
                model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
                if (user.IsDeputy == true || user.IsDepartmentHead == true)
                {
                    statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder >= 12).OrderBy(s => s.ProgressOrder).ToList();
                }
                else if (user.IsDefaultUser == true)
                {
                    statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder <= 11).OrderBy(s => s.ProgressOrder).ToList();

                }
                model.status = statuces.Select(p => new SelectListItem
                {
                    Value = p.ProId.ToString(),
                    Text = p.ProstatusTitle,
                }).ToList();
                ViewBag.StatusId = new SelectList(statuces, "ProId", "ProstatusTitle", yearlyPlan.ProId);

                return View(model); _notifyService.Error($"Error:{ex.Message} happened. Progress isn't successfully uppdated. Please try again");
                return View(model);
            }
        }

        public IActionResult UpdateProgressStatusbyDeputy(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var user = _context.TblInternalUsers.Find(userId);
            List<TblInspectionStatus> statuces = new List<TblInspectionStatus>();
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);
            model.AssignedBy = assignedTasks.AssignedBy;
            model.Id = assignedTasks.Id;
            model.PlanTitle = yearlyPlan.Title;
            model.Remark = assignedTasks.Remark;
            model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
            if (user.IsDeputy == true || user.IsDepartmentHead == true)
            {
                statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder >= 12).OrderBy(s => s.ProgressOrder).ToList();
            }
            else if (user.IsDefaultUser == true)
            {
                statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder <= 11).OrderBy(s => s.ProgressOrder).ToList();

            }
            model.status = statuces.Select(p => new SelectListItem
            {
                Value = p.ProId.ToString(),
                Text = p.ProstatusTitle,
            }).ToList();
            ViewBag.StatusId = new SelectList(statuces, "ProId", "ProstatusTitle", yearlyPlan.ProId);

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProgressStatusbyDeputy(InspectionAssignModel? model)
        {
            try
            {
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var user = _context.TblInternalUsers.Find(userId);
                var userEmail = _context.TblAssignedYearlyPlans.Include(s => s.AssignedToNavigation).Where(s => s.SpecificPlanId == model.SpecificPlanId).Select(s => s.AssignedToNavigation.EmailAddress).ToList();
                List<TblInspectionStatus> statuces = new List<TblInspectionStatus>();
                TblSpecificPlan assignedTasks = await _context.TblSpecificPlans.FindAsync(model.Id);
                var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);

                yearlyPlan.IsUserUproved = _context.TblDecisionStatuses.Where(s => s.StatusName == "Upproved").Select(s => s.DesStatusId).FirstOrDefault();
                yearlyPlan.ProId = model.StatusID;
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    await SendMail(userEmail, "Notification from Task Tracking Dashboard", "Expert uppdated progress status for his assigned task. Please review on system.");
                    _notifyService.Success("Progress is successfully updated.");
                    return RedirectToAction(nameof(AssignedRequests));
                }
                else
                {
                    if (user.IsDepartmentHead == true)
                    {
                        statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder >= 12).OrderBy(s => s.ProgressOrder).ToList();

                    }
                    model.status = statuces.Select(p => new SelectListItem
                    {
                        Value = p.ProId.ToString(),
                        Text = p.ProstatusTitle,

                    }).ToList();
                    ViewBag.StatusId = new SelectList(statuces, "ProId", "ProstatusTitle", model.StatusID);

                    _notifyService.Error("Progress isn't successfully uppdated. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var user = _context.TblInternalUsers.Find(userId);
                List<TblInspectionStatus> statuces = new List<TblInspectionStatus>();
                if (user.IsDeputy == true || user.IsDepartmentHead == true)
                {
                    statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder >= 12).OrderBy(s => s.ProgressOrder).ToList();

                }
                else if (user.IsDefaultUser == true)
                {
                    statuces = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team" && a.ProgressOrder <= 11).OrderBy(s => s.ProgressOrder).ToList();
                }
                model.status = statuces.Select(p => new SelectListItem
                {
                    Value = p.ProId.ToString(),
                    Text = p.ProstatusTitle,

                }).ToList();
                ViewBag.StatusId = new SelectList(statuces, "ProId", "ProstatusTitle", model.StatusID);
                _notifyService.Error($"Error:{ex.Message} happened. Progress isn't successfully uppdated. Please try again");
                return View(model);
            }
        }
        public IActionResult AddFinalReport(Guid? id)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);

            var yearlyPlan = _context.TblAssignedYearlyPlans.Include(s => s.SpecificPlan).Where(s => s.Id == assignedTasks.Id).FirstOrDefault();
            model.Id = assignedTasks.Id;
            model.PlanTitle = yearlyPlan.SpecificPlan.Title;
            model.Remark = assignedTasks.Remark;
            model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
            model.status = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team").Select(p => new SelectListItem
            {
                Value = p.ProId.ToString(),
                Text = p.ProstatusTitle,
            }).ToList();
            model.StatusID = yearlyPlan.StatusId;
            ViewBag.StatusId = new SelectList(_context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team").ToList(), "ProId", "ProstatusTitle", yearlyPlan.StatusId);
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddFinalReport(InspectionAssignModel? model)
        {
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
            var yearlyPlan = _context.TblSpecificPlans.Find(assignedTasks.SpecificPlanId);
            try
            {
                var emails = _context.TblInternalUsers.Where(s => s.UserId == model.AssignedBy).Select(s => s.EmailAddress).ToList();
                if (assignedTasks.EngagementLetter == null || assignedTasks.EvaluationCheckLists == null)
                {
                    _notifyService.Error("Before Upploading final report you should add evaluation criteria or Engagement letter");
                    return View(model);
                }
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(model.FinalReport.FileName);
                string fileName = Guid.NewGuid().ToString() + model.FinalReport.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.FinalReport.CopyTo(stream);
                }
                string dbPath = "/admin/Files/" + fileName;
                assignedTasks.FinalReport = dbPath;
                yearlyPlan.FinalReport = dbPath;
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    await SendMail(emails, "Notification from Task Tracking Dashboard", "Expert is added Engagement letter for his assigned task. Please review on system.");
                    _notifyService.Success("Final report is successfully upploaded");
                    return RedirectToAction(nameof(AssignedRequests));
                }
                else
                {
                    model.status = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team").Select(p => new SelectListItem
                    {
                        Value = p.ProId.ToString(),
                        Text = p.ProstatusTitle,

                    }).ToList();
                    ViewBag.StatusId = new SelectList(_context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team").ToList(), "ProId", "ProstatusTitle", yearlyPlan.ProId);
                    _notifyService.Error("Report isn't successfully upploaded");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                model.status = _context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team").Select(p => new SelectListItem
                {
                    Value = p.ProId.ToString(),
                    Text = p.ProstatusTitle,

                }).ToList();
                ViewBag.StatusId = new SelectList(_context.TblInspectionStatuses.Where(a => a.ProstatusTitle != "New" && a.ProstatusTitle != "Assigned to expert" && a.ProstatusTitle != "Assigned to Team").ToList(), "ProId", "ProstatusTitle", yearlyPlan.ProId);
                _notifyService.Error($"Error:{ex.Message} happened. Report isn't successfully upploaded");
                return View(model);
            }

        }
        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string path)
        {
            string filename = path.Substring(7);
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }
        public async Task<IActionResult> PlanCategoryDetails(int? PlanCatId)
        {
            if (PlanCatId == null || _context.TblPlanCatagories == null)
            {
                return NotFound();
            }
            var tblPlanCatagory = await _context.TblPlanCatagories
                .Include(t => t.InspectionPlan)
                .FirstOrDefaultAsync(m => m.PlanCatId == PlanCatId);
            if (tblPlanCatagory == null)
            {
                return NotFound();
            }

            return View(tblPlanCatagory);
        }
        public async Task<IActionResult> SpecificDetails(Guid? SpecificPlanId)
        {
            if (SpecificPlanId == null || _context.TblSpecificPlans == null)
            {
                return NotFound();
            }
            var tblSpecificPlan = await _context.TblSpecificPlans
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.InspectionPlan)
                .Include(t => t.PlanCat)
                .FirstOrDefaultAsync(m => m.SpecificPlanId == SpecificPlanId);
            if (tblSpecificPlan == null)
            {
                return NotFound();
            }
            ViewBag.InspectionPlanId = tblSpecificPlan.InspectionPlanId;
            return View(tblSpecificPlan);
        }
        public async Task<IActionResult> SpecificEdit(Guid? id)
        {
            AnnualSpecificPlan model = new AnnualSpecificPlan();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (id == null || _context.TblSpecificPlans == null)
            {
                return NotFound();
            }

            var tblSpecificPlan = await _context.TblSpecificPlans.FindAsync(id);
            if (tblSpecificPlan == null)
            {
                return NotFound();
            }
            model.SpecificPlanId = tblSpecificPlan.SpecificPlanId;
            model.PlanCatId = tblSpecificPlan.PlanCatId;
            model.Title = tblSpecificPlan.Title;
            model.Description = tblSpecificPlan.Description;
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = userId;
            ViewBag.PlanCatId = tblSpecificPlan.PlanCatId.ToString();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SpecificEdit(AnnualSpecificPlan model)
        {
            TblSpecificPlan specific = await _context.TblSpecificPlans.FindAsync(model.SpecificPlanId);
            if (specific == null)
            {
                return NotFound();
            }
            try
            {
                specific.ModificationDate = DateTime.Now;
                specific.Title = model.Title;
                specific.Description = model.Description;
                specific.PlanCatId = model.PlanCatId;
                specific.CreatedBy = model.CreatedBy;
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    _notifyService.Success("Specific plan successfully updated!.");
                    return RedirectToAction(nameof(Index), new { PlanCatId = model.PlanCatId });
                }
                else
                {
                    ViewBag.PlanCatId = model.PlanCatId.ToString();
                    return View(model);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TblSpecificPlanExists(model.SpecificPlanId))
                {
                    return NotFound();
                }
                else
                {
                    _notifyService.Error(ex.Message + " happened. Please try again");
                    ViewBag.PlanCatId = model.PlanCatId.ToString();
                    return View(model);
                }
            }
        }
        public async Task<IActionResult> SpecificDelete(Guid? id)
        {
            if (id == null || _context.TblSpecificPlans == null)
            {
                return NotFound();
            }

            var tblSpecificPlan = await _context.TblSpecificPlans
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.InspectionPlan)
                .FirstOrDefaultAsync(m => m.SpecificPlanId == id);
            if (tblSpecificPlan == null)
            {
                return NotFound();
            }
            ViewBag.PlanCatId = tblSpecificPlan.PlanCatId.ToString();
            return View(tblSpecificPlan);
        }

        [HttpPost, ActionName("SpecificDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SpecificDeleteConfirmed(Guid id)
        {
            try
            {
                if (_context.TblSpecificPlans == null)
                {
                    return Problem("Entity set 'AtsdbContext.TblSpecificPlans'  is null.");
                }
                var tblSpecificPlan = await _context.TblSpecificPlans.FindAsync(id);
                if (tblSpecificPlan != null)
                {
                    _context.TblSpecificPlans.Remove(tblSpecificPlan);
                }

                int deleted = await _context.SaveChangesAsync();
                if (deleted > 0)
                {
                    _notifyService.Success("Deleted Successfully.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var tblSpecificPland = await _context.TblSpecificPlans
               .Include(t => t.CreatedByNavigation)
               .Include(t => t.InspectionPlan)
               .FirstOrDefaultAsync(m => m.SpecificPlanId == id);
                    _notifyService.Error("Not successful. Please try again");
                    return View(tblSpecificPland);
                }

            }
            catch (Exception ex)
            {
                var tblSpecificPlan = await _context.TblSpecificPlans
               .Include(t => t.CreatedByNavigation)
               .Include(t => t.InspectionPlan)
               .FirstOrDefaultAsync(m => m.SpecificPlanId == id);
                _notifyService.Warning("This Specific plan is in use. Before deleting this please try to delete specific plans under this category. See detail " + ex.Message);
                return View(tblSpecificPlan);
            }

        }
        private bool TblSpecificPlanExists(Guid id)
        {
            return (_context.TblSpecificPlans?.Any(e => e.SpecificPlanId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> PlanCategoryEdit(int PlanCatId)
        {
            TblPlanCatagory Categories = _context.TblPlanCatagories.Find(PlanCatId);
            AnnualPlanCatagory category = new AnnualPlanCatagory();
            category.PlanCatId = PlanCatId;
            category.InspectionPlanId = Categories.InspectionPlanId;
            category.CatTitle = Categories.CatTitle;
            category.CatDescription = Categories.CatDescription;
            category.DoesHaveSpecificPlan = Categories.DoesHaveSpecificPlan;
            if (Categories == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlanCategoryEdit(AnnualPlanCatagory model)
        {
            if (model.PlanCatId == null)
            {
                return NotFound();
            }
            try
            {
                TblPlanCatagory Categories = _context.TblPlanCatagories.Find(model.PlanCatId);
                Categories.CatTitle = model.CatTitle;
                Categories.CatDescription = model.CatDescription;
                Categories.DoesHaveSpecificPlan = model.DoesHaveSpecificPlan;
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    if (model.DoesHaveSpecificPlan == false)
                    {
                        var specific = _context.TblSpecificPlans.Where(s => s.PlanCatId == model.PlanCatId).ToList();
                        if (specific != null)
                        {
                            _context.TblSpecificPlans.RemoveRange(specific);
                            _context.SaveChanges();
                        }
                    }
                    _notifyService.Success("Plan category is successfully.");
                    return RedirectToAction(nameof(Index), new { InspectionPlanId = model.InspectionPlanId });
                }
                else
                {
                    _notifyService.Error("Annual plan category isn't successfully uppdated.");
                    return View(model);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!TblPlanCatagoryExists(model.PlanCatId))
                {
                    return NotFound();
                }
                else
                {
                    _notifyService.Error($"Error:{ex.Message}. Annual plan category isn't successfully uppdated.");
                    return View(model);
                }
            }
        }
        public async Task<IActionResult> PlanCategoryDelete(int? PlanCatId)
        {
            if (PlanCatId == null || _context.TblPlanCatagories == null)
            {
                return NotFound();
            }
            var tblPlanCatagory = await _context.TblPlanCatagories
                .Include(t => t.InspectionPlan)
                .FirstOrDefaultAsync(m => m.PlanCatId == PlanCatId);
            if (tblPlanCatagory == null)
            {
                return NotFound();
            }
            return View(tblPlanCatagory);
        }
        [HttpPost, ActionName("PlanCategoryDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlanCategoryDeleteConfirmed(int id)
        {
            try
            {
                if (_context.TblPlanCatagories == null)
                {
                    return Problem("Entity set 'AtsdbContext.TblPlanCatagories'  is null.");
                }
                var tblPlanCatagory = await _context.TblPlanCatagories.FindAsync(id);
                if (tblPlanCatagory != null)
                {
                    _context.TblPlanCatagories.Remove(tblPlanCatagory);
                }

                int deleted = await _context.SaveChangesAsync();
                if (deleted > 0)
                {
                    _notifyService.Success("Successfully Deleted");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var tblPlanCatagorys = await _context.TblPlanCatagories
             .Include(t => t.InspectionPlan)
             .FirstOrDefaultAsync(m => m.PlanCatId == id);
                    _notifyService.Error("Not successfull. Please try again");
                    return View(tblPlanCatagorys);
                }
            }
            catch (Exception ex)
            {
                var tblPlanCatagorydele = await _context.TblPlanCatagories
             .Include(t => t.InspectionPlan)
             .FirstOrDefaultAsync(m => m.PlanCatId == id);
                _notifyService.Warning("This Category is in use. Before deleting this Please try to delete specific plans under this category. See detail " + ex.Message);
                return View(tblPlanCatagorydele);
            }
        }
        private bool TblPlanCatagoryExists(int id)
        {
            return (_context.TblPlanCatagories?.Any(e => e.PlanCatId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> AddActivity(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            InspectionActivityModel model = new InspectionActivityModel();
            model.SpecificPlanId = id;
            model.AddedDate = DateTime.Now;
            ViewBag.SpecificPlanId = model.SpecificPlanId;
            model.CreatedBy = userId;
            ViewData["Activities"] = _context.TblInpectionActivites
                 .Include(x => x.SpecificPlan)
                 .Include(x => x.CreatedByNavigation)
                 .Where(x => x.SpecificPlanId == id).ToList();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActivity(InspectionActivityModel model)
        {
            InspectionActivityModel newModel = new InspectionActivityModel();
            newModel.SpecificPlanId = model.SpecificPlanId;
            newModel.AddedDate = DateTime.Now;
            ViewBag.SpecificPlanId = model.SpecificPlanId;
            newModel.CreatedBy = model.CreatedBy;

            List<string> assignedBody = new List<string>();
            var request = _context.TblAssignedYearlyPlans.Where(x => x.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
            assignedBody = (from items in _context.TblInternalUsers where items.UserId == request.AssignedBy select items.EmailAddress).ToList();
            TblInpectionActivite activity = new TblInpectionActivite();
            activity.SpecificPlanId = model.SpecificPlanId;
            activity.AddedDate = DateTime.Now;
            activity.ActivityDetail = model.ActivityDetail;
            activity.TimeTakenTocomplete = model.TimeTakenTocomplete;
            activity.Remark = model.Remark;
            activity.CreatedBy = model.CreatedBy;
            _context.TblInpectionActivites.Add(activity);
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                newModel.ActivityDetail = null;
                await SendMail(assignedBody, "Adding activities notifications.", "<h3>Assigned body is adding activities via <strong> Task tacking Dashboard</strong>. Please check on the system and followup!.</h3>");
                ViewData["Activities"] = _context.TblInpectionActivites
                    .Include(x => x.SpecificPlan)
                    .Include(x => x.CreatedByNavigation)
                    .Where(x => x.SpecificPlanId == model.SpecificPlanId).ToList();
                _notifyService.Success("Activity added successfully!");
                return View(newModel);
            }
            else
            {
                ViewData["Activities"] = _context.TblInpectionActivites
                    .Include(x => x.SpecificPlan)
                    .Include(x => x.CreatedByNavigation)
                    .Where(x => x.SpecificPlanId == model.SpecificPlanId).ToList();
                _notifyService.Error("Activity isn't added successfully Please try again");
                return View(model);
            }

        }
        public async Task<IActionResult> EditActivity(int ActivityId)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInpectionActivite? activity = await _context.TblInpectionActivites.FindAsync(ActivityId);
            InspectionActivityModel model = new InspectionActivityModel();
            model.ActivityId = ActivityId;
            model.Remark = activity.Remark;
            model.TimeTakenTocomplete = activity.TimeTakenTocomplete;
            model.SpecificPlanId = activity.SpecificPlanId;
            model.ActivityDetail = activity.ActivityDetail;
            model.AddedDate = activity.AddedDate;
            ViewData["Activities"] = _context.TblInpectionActivites
                 .Include(x => x.SpecificPlan)
                 .Include(x => x.CreatedByNavigation)
                 .Where(x => x.SpecificPlanId == model.SpecificPlanId).ToList();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditActivity(InspectionActivityModel model)
        {
            TblInpectionActivite? activity = await _context.TblInpectionActivites.FindAsync(model.ActivityId);
            InspectionActivityModel newModel = new InspectionActivityModel();
            activity.SpecificPlanId = model.SpecificPlanId;
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
                return RedirectToAction(nameof(AddActivity), new { id = activity.SpecificPlanId });
            }
            else
            {
                _notifyService.Error("Data isn't updated. Please try again");
                return View(model);
            }
        }
        public async Task<IActionResult> DeleteActivity(int? id)
        {
            if (id == null || _context.TblInpectionActivites == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblInpectionActivites
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.SpecificPlan)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (tblWitnessEvidence == null)
            {
                return NotFound();
            }

            return View(tblWitnessEvidence);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteActivity(int id)
        {
            var tblWitnessEvidences = await _context.TblInpectionActivites
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.SpecificPlan)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            var tblWitnessEvidence = await _context.TblInpectionActivites.FindAsync(id);
            if (tblWitnessEvidence != null)
            {
                _context.TblInpectionActivites.Remove(tblWitnessEvidence);
            }

            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Successfully removed");
                return RedirectToAction(nameof(AddActivity), new { id = tblWitnessEvidence.SpecificPlanId });
            }
            else
            {
                _notifyService.Error("Not successfull");
                return View(tblWitnessEvidences);
            }
        }
        public async Task<IActionResult> DailyActivities(Guid? id)
        {
            var atsdbContext = _context.TblInpectionActivites.Include(t => t.CreatedByNavigation).Include(t => t.SpecificPlan).Where(s => s.SpecificPlanId == id);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> ReplyResponseWithExpert(Guid? id)
        {
            var yearlyAssigned = _context.TblAssignedYearlyPlans.Find(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
          
           
            if (yearlyAssigned == null)
            {
                return NotFound();

            }
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            ReplyResponseModel model = new ReplyResponseModel();

            model.CreatedBy = userId;
            model.SpecificPlanId = yearlyAssigned.SpecificPlanId;
            model.Id = id;
            model.CreatedDate = DateTime.Now;
            model.forDepartment = false;
            model.ForDeputy = false;
            model.ForExpert = false;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyResponseWithExpert(ReplyResponseModel model)
        {
            TblInspectionReportFile reportFile = new TblInspectionReportFile();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var user = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            string fullname = user.FirstName + " " + user.MidleName;
            var depHeade = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "LSDC" && s.IsDepartmentHead == true).Select(s => s.EmailAddress).ToList();
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileInfo fileInfo = new FileInfo(model.ReportFiles.FileName);
            string fileName = Guid.NewGuid().ToString() + model.ReportFiles.FileName;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.ReportFiles.CopyTo(stream);
            }
            string dbPath = "/admin/Files/" + fileName;
            reportFile.CreatedDate = model.CreatedDate;
            reportFile.CreatedBy = model.CreatedBy;
            reportFile.Feedback = model.Feedback;
            reportFile.Id = model.Id;
            reportFile.FileName = model.ReportFiles.FileName;
            reportFile.SpecificPlanId = model.SpecificPlanId;
            reportFile.ReportFiles = dbPath;
            if (user.IsDefaultUser == true)
            {
                reportFile.ForDepartment = true;
            }
            else if (user.IsDeputy == true)
            {
                reportFile.ForDepartment = true;
            }
            else if (user.IsDepartmentHead == true)
            {
                reportFile.ForDepartment = true;
            }
            _context.TblInspectionReportFiles.Add(reportFile);
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                await SendMail(depHeade, "Activity notification from Task tracking system", fullname + " uploaded document. Please review on the system ");
                _notifyService.Success("Successfully uppdated");
                return View(model);
            }
            else
            {
                _notifyService.Error("Not uppdated. Please try again");
                return View();
            }
        }
        public async Task<IActionResult> ReplyResponseWithStateMinister(Guid? id)
        {
            var yearlyAssigned = _context.TblAssignedYearlyPlans.Find(id);
            if (yearlyAssigned == null)
            {
                return NotFound();
            }
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            ReplyResponseModel model = new ReplyResponseModel();
            model.CreatedBy = userId;
            model.SpecificPlanId = yearlyAssigned.SpecificPlanId;
            model.Id = id;
            model.CreatedDate = DateTime.Now;
            model.forDepartment = false;
            model.ForDeputy = false;
            model.ForExpert = false;
            return View(model);

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReplyResponseWithStateMinister(ReplyResponseModel model)
        {
            TblInspectionReportFile reportFile = new TblInspectionReportFile();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var assigneee = _context.TblAssignedYearlyPlans.Include(s => s.AssignedToNavigation).Where(s => s.Id == model.Id).Select(S => S.AssignedToNavigation.EmailAddress).ToList();
            var user = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            List<string> emails = new List<string>();
            if (user.IsDeputy == true)
            {
                emails = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "LSDC" && s.IsDepartmentHead == true).Select(s => s.EmailAddress).ToList();
            }
            else if (user.IsDepartmentHead == true)
            {
                emails = assigneee;
            }
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            FileInfo fileInfo = new FileInfo(model.ReportFiles.FileName);
            string fullname = user.FirstName + " " + user.LastName;
            string fileName = Guid.NewGuid().ToString() + model.ReportFiles.FileName;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.ReportFiles.CopyTo(stream);
            }
            string dbPath = "/admin/Files/" + fileName;
            reportFile.CreatedDate = model.CreatedDate;
            reportFile.CreatedBy = model.CreatedBy;
            reportFile.Feedback = model.Feedback;
            reportFile.Id = model.Id;
            reportFile.SpecificPlanId = model.SpecificPlanId;
            reportFile.ReportFiles = dbPath;
            if (user.IsDeputy == true)
            {
                reportFile.ForDepartment = true;
                reportFile.ForExpert = model.ForExpert;
            }
            if (user.IsDepartmentHead == true)
            {
                if (model.ForDeputy == true)
                {
                    reportFile.ForDeputy = true;
                }
                if (model.ForExpert == true)
                {
                    reportFile.ForExpert = model.ForExpert;
                }
            }
            _context.TblInspectionReportFiles.Add(reportFile);
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                await SendMail(emails, "Activity notification from Task tracking system", fullname + " uploaded document. Please review on the system ");
                _notifyService.Success("Successfully uppdated");
                return View(model);
            }
            else
            {
                _notifyService.Error("Not uppdated. Please try again");
                return View(model);
            }
        }
        public async Task<IActionResult> AllActivities()
        {
            var atsdbContext = _context.TblInpectionActivites.Include(t => t.CreatedByNavigation).Include(t => t.SpecificPlan);
            return View(await atsdbContext.ToListAsync());

        }

        public async Task<IActionResult> DetailsAssigned(Guid? id)
        {
            if (id == null || _context.TblSpecificPlans == null)
            {
                return NotFound();
            }

            var tblSpecificPlan = await _context.TblSpecificPlans
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.InspectionPlan)
                .Include(s => s.PlanCat)
                .FirstOrDefaultAsync(m => m.SpecificPlanId == id);
            if (tblSpecificPlan == null)
            {
                return NotFound();
            }
            ViewBag.InspectionPlanId = tblSpecificPlan.InspectionPlanId;
            return View(tblSpecificPlan);
        }
        public async Task<IActionResult> DetailsOngoing(Guid? id)
        {
            if (id == null || _context.TblSpecificPlans == null)
            {
                return NotFound();
            }

            var tblSpecificPlan = await _context.TblSpecificPlans
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.InspectionPlan)
                .Include(s => s.PlanCat)
                .FirstOrDefaultAsync(m => m.SpecificPlanId == id);
            if (tblSpecificPlan == null)
            {
                return NotFound();
            }
            ViewBag.InspectionPlanId = tblSpecificPlan.InspectionPlanId;
            return View(tblSpecificPlan);
        }
        public async Task<IActionResult> DetailsCompleted(Guid? id)
        {
            if (id == null || _context.TblSpecificPlans == null)
            {
                return NotFound();
            }

            var tblSpecificPlan = await _context.TblSpecificPlans
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.InspectionPlan)
                .Include(s => s.PlanCat)
                .FirstOrDefaultAsync(m => m.SpecificPlanId == id);
            if (tblSpecificPlan == null)
            {
                return NotFound();
            }
            ViewBag.InspectionPlanId = tblSpecificPlan.InspectionPlanId;
            return View(tblSpecificPlan);
        }
        public async Task<IActionResult> RequestChats(Guid? id, string actionMethod, string controller, string type)
        {

            InspectionChatModel model = new InspectionChatModel();

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
            model.SpecificPlanId = id;
            ViewBag.actionMethod = actionMethod;
            ViewBag.controller = controller;
            _contextAccessor.HttpContext.Session.SetString("actionMethod", actionMethod);
            _contextAccessor.HttpContext.Session.SetString("controller", controller);
            return View(model);




        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestChats(InspectionChatModel chatModel)
        {
            try
            {
                var depHeadEmail = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "FLIM" &&s.IsDepartmentHead==true).Select(s => s.EmailAddress).ToList();
                string actionMethod = _contextAccessor.HttpContext.Session.GetString("actionMethod");
                string controller = _contextAccessor.HttpContext.Session.GetString("controller");
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var user = _context.TblInternalUsers.Find(userId);
                TblInspectionChat chat = new TblInspectionChat();
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
                chat.SpecificPlanId = chatModel.SpecificPlanId;
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
                _context.TblInspectionChats.Add(chat);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    if (user.IsDefaultUser == true)
                    {
                        await SendMail(depHeadEmail, "Request update", "<h3>Expert dropped message on dashboard please check it.</h3>");

                    }
                    _notifyService.Success("Sent");
                    return RedirectToAction(nameof(RequestChats), new { id = chatModel.SpecificPlanId, actionMethod = actionMethod, controller = controller });
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
        public async Task<IActionResult> ViewEvaluation(Guid id, string actionMethod)
        {
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            ViewBag.ActionMethod=actionMethod;
            return View(assignedTasks);
        }
        public async Task<IActionResult> DocumentHistory(Guid? id, string action)
        {
            ViewBag.action = action;
            var docs = await _context.TblInspectionReportFiles
                .Include(s => s.SpecificPlan)
                .Include(s => s.IdNavigation)
                .Include(s => s.CreatedByNavigation)
                .Include(s => s.IdNavigation).Where(s => s.SpecificPlanId == id).OrderByDescending(s => s.RepId).ToListAsync();
            return View(docs);
        }
        public async Task<IActionResult> DeleteDocument(int? RepId)
        {
            var docs = _context.TblInspectionReportFiles.Find(RepId);
            Guid? specificId = docs.SpecificPlanId;
            try
            {                
                _context.TblInspectionReportFiles.Remove(docs);
                int deleted = await _context.SaveChangesAsync();
                if (deleted > 0)
                {
                    _notifyService.Success("Successfully deleted");
                    return RedirectToAction(nameof(DocumentHistory), new { id = specificId });
                }
                else
                {
                    _notifyService.Error("Not successfull. Please try again");
                    return RedirectToAction(nameof(DocumentHistory), new { id = specificId });
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message + " happened. Please try again latter");
                return RedirectToAction(nameof(DocumentHistory), new { id = specificId });
            }
        }

    }
}
