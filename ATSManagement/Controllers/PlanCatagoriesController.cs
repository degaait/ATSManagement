using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.Services;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    public class PlanCatagoriesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        private LanguageService _localization;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationService _notificationService;
        public PlanCatagoriesController(IHttpContextAccessor contextAccessor,ILogger<HomeController> logger, IMailService mail, AtsdbContext atsdbContext, INotyfService notyfService, LanguageService localization, INotificationService notificationService)
        {
            _notifyService = notyfService;
            _logger = logger;
            _mail = mail;
            _context = atsdbContext;
            _localization = localization;
            _notificationService = notificationService;
            _contextAccessor = contextAccessor;
        }

        public async Task<IActionResult> Index(Guid? InspectionPlanId)
        {
            ViewBag.InspectionPlanId = InspectionPlanId;
            var atsdbContext = _context.TblPlanCatagories.Include(t => t.InspectionPlan).Where(s => s.InspectionPlanId == InspectionPlanId);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: PlanCatagories/Details/5
        public async Task<IActionResult> Details(int? PlanCatId)
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

        // GET: PlanCatagories/Create
        public IActionResult Create(Guid? InspectionPlanId)
        {
            AnnualPlanCatagory model = new AnnualPlanCatagory();
            model.InspectionPlanId = InspectionPlanId;
            model.DoesHaveSpecificPlan = true;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnualPlanCatagory model)
        {
            try
            {
                TblPlanCatagory item = new TblPlanCatagory();
                TblSpecificPlan specificPlan = new TblSpecificPlan();
                item.InspectionPlanId = model.InspectionPlanId;
                item.CatTitle = model.CatTitle;
                item.CatDescription = model.CatDescription;
                item.CreatedDate=DateTime.Now;
                item.DoesHaveSpecificPlan = model.DoesHaveSpecificPlan;
                _context.TblPlanCatagories.Add(item);
                int save = _context.SaveChanges();
                if (save > 0)
                {
                    if (model.DoesHaveSpecificPlan == false)
                    {
                        specificPlan.PlanCatId = item.PlanCatId;
                        specificPlan.Description = model.CatDescription;
                        specificPlan.Title = model.CatTitle;
                        specificPlan.InspectionPlanId = item.InspectionPlanId;
                        _context.TblSpecificPlans.Add(specificPlan);
                        int saved = _context.SaveChanges();
                    }
                    _notifyService.Success("Annual plan category successfully added");
                    return RedirectToAction(nameof(Index), new { InspectionPlanId = model.InspectionPlanId });
                }
                else
                {
                    _notifyService.Error("Annual plan category isn't successfully Added.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error:{ex.Message} happened. Annual plan category isn't successfully Added.");
                return View(model);
            }
        }

        public async Task<IActionResult> Assign(Guid? id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            var isAssigned = _context.TblAssignedYearlyPlans.Where(a => a.SpecificPlanId == id).FirstOrDefault();
            InspectionAssignModel model = new InspectionAssignModel();
            var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == id).FirstOrDefault();
            var category = _context.TblPlanCatagories.Where(s => s.PlanCatId == plats.PlanCatId).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "FLIM").ToList();
            ViewBag.InspectionPlanId = category.InspectionPlanId;
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
                model.PlanCatId=plats.PlanCatId;
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
                model.PlanCatId=plats.PlanCatId;
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
                                  select new { EmailAddress = users.EmailAddress, userId = users.UserId }).ToList();
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
                    return RedirectToAction(nameof(Index), new
                    {
                        InspectionPlanId = plats.InspectionPlanId
                    });
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
                    var usersTeam = _context.TblInternalUsers.Find(item);
                    assignees.Add(usersTeam.EmailAddress);
                    teamLeaderIds.Add(usersTeam.UserId);
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
                    await SendMail(assignees, "Inpection Plans are assignment Notification", "<h3>Inpection plans are assigned for you. Please review the assigned plans on Task Tracking Dashboard and assign to experts</h3>");
                    _notifyService.Success("Plan successfully assigned");
                    return RedirectToAction(nameof(Index), new
                    {
                        InspectionPlanId = plats.InspectionPlanId
                    });
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


        public async Task<IActionResult> Edit(int PlanCatId)
        {
            TblPlanCatagory Categories = _context.TblPlanCatagories.Find(PlanCatId);
            AnnualPlanCatagory category = new AnnualPlanCatagory();
            category.PlanCatId = PlanCatId;
            category.InspectionPlanId = Categories.InspectionPlanId;
            category.CatTitle = Categories.CatTitle;
            category.CatDescription = Categories.CatDescription;
            category.DoesHaveSpecificPlan = Categories.DoesHaveSpecificPlan;
            ViewBag.InspectionPlanId = Categories.InspectionPlanId;
            if (Categories == null)
            {
                return NotFound();
            }
            return View(category);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AnnualPlanCatagory model)
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
                ViewBag.InspectionPlanId = Categories.InspectionPlanId;
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblPlanCatagories == null)
            {
                return NotFound();
            }
            var tblPlanCatagory = await _context.TblPlanCatagories
                .Include(t => t.InspectionPlan)
                .FirstOrDefaultAsync(m => m.PlanCatId == id);
            if (tblPlanCatagory == null)
            {
                return NotFound();
            }
            return View(tblPlanCatagory);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
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
                    _notifyService.Success("Successfully deleted");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notifyService.Error("Not successfull. Please try again");
                    return View(tblPlanCatagory);
                }
            }
            catch (Exception ex)
            {
                var tblPlanCatagory = await _context.TblPlanCatagories.FindAsync(id);
                _notifyService.Warning("This Category is in use. Before deleting this Please try to delete specific plans under this category. See detail " + ex.Message);
                return View(tblPlanCatagory);
            }


        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
        private bool TblPlanCatagoryExists(int id)
        {
            return (_context.TblPlanCatagories?.Any(e => e.PlanCatId == id)).GetValueOrDefault();
        }
    }
}
