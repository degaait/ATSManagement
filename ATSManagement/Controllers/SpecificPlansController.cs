using ATSManagement.Models;
using ATSManagement.Filters;
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
    [CheckSessionIsAvailable]
    public class SpecificPlansController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotificationService _notificationService;
        private LanguageService _localization;
        public SpecificPlansController(ILogger<HomeController> logger, IMailService mail, AtsdbContext atsdbContext, INotyfService notyfService, LanguageService localization, IHttpContextAccessor contextAccessor, INotificationService notificationService)
        {
            _notifyService = notyfService;
            _logger = logger;
            _mail = mail;
            _context = atsdbContext;
            _localization = localization;
            _contextAccessor = contextAccessor;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index(int PlanCatId, Guid? InspectionPlanId)
        {
            var InspecId = _context.TblPlanCatagories.Find(PlanCatId);
          
            ViewBag.InspectionPlanId = InspecId.InspectionPlanId;
            ViewBag.PlanCatId = PlanCatId;
            var atsdbContext = _context.TblSpecificPlans.Include(s => s.Pro).Include(t => t.CreatedByNavigation).Include(t => t.PlanCat).Where(x => x.PlanCatId == PlanCatId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> Details(Guid? id, int PlanCatId, Guid? InspectionPlanId)
        {
            ViewBag.InspectionPlanId = InspectionPlanId;
            ViewBag.PlanCatId = PlanCatId;
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
            return View(tblSpecificPlan);
        }

        public IActionResult Create(int? PlanCatId, Guid? InspectionPlanId)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            AnnualSpecificPlan annualSpecificPlan = new AnnualSpecificPlan();
            annualSpecificPlan.PlanCatId = PlanCatId;
            annualSpecificPlan.CreatedBy = userId;
            annualSpecificPlan.InspectionPlanId=InspectionPlanId;
            annualSpecificPlan.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.InistId.ToString(),
            }).ToList();
            annualSpecificPlan.CreatedDate = DateTime.Now;
            ViewBag.InspectionPlanId = InspectionPlanId;
            ViewBag.PlanCatId = PlanCatId;
            return View(annualSpecificPlan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnualSpecificPlan model)
        {
            try
            {
                List<TblPlanInistitution> plan_in = new List<TblPlanInistitution>();
                var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "New").FirstOrDefault();
                ViewBag.InspectionPlanId = model.InspectionPlanId;
                ViewBag.PlanCatId = model.PlanCatId;
                TblSpecificPlan tblSpecificPlan = new TblSpecificPlan();
                tblSpecificPlan.Title = model.Title;
                tblSpecificPlan.Description = model.Description;
                tblSpecificPlan.PlanCatId = model.PlanCatId;
                tblSpecificPlan.CreatedBy = model.CreatedBy;
                tblSpecificPlan.CreatedDate = DateTime.Now;
                tblSpecificPlan.IsAssignedToTeam = false;
                tblSpecificPlan.ProId = status.ProId;
                tblSpecificPlan.IsAssignedToUser = false;
                if (model.InistId != null)
                {
                    plan_in = new List<TblPlanInistitution>();
                    foreach (var item in model.InistId)
                    {
                        plan_in.Add(new TblPlanInistitution { InistId = item, SpecificPlanId = model.SpecificPlanId });
                    }
                    tblSpecificPlan.TblPlanInistitutions = plan_in;
                }
                _context.TblSpecificPlans.Add(tblSpecificPlan);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Specific plan successfully added");
                    return RedirectToAction(nameof(Index), new { PlanCatId = model.PlanCatId });
                }
                else
                {
                    model.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
                    {
                        Text = a.Name,
                        Value = a.InistId.ToString(),
                    }).ToList();
                    _notifyService.Error("Specific plan isn't successfull added. Please try again");
                    ViewBag.PlanCatId = model.PlanCatId.ToString();
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error:{ex.Message} happened. Specific plan isn't successfull added. Please try again");
                ViewBag.PlanCatId = model.PlanCatId.ToString();
                return View(model);
            }
        }
        public async Task<IActionResult> Edit(Guid? id, int PlanCatId, Guid? InspectionPlanId)
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
            ViewBag.PlanCatId = PlanCatId;
            ViewBag.InspectionPlanId = InspectionPlanId;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AnnualSpecificPlan model)
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
        public async Task<IActionResult> Delete(Guid? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
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

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index), new { PlanCatId = tblSpecificPlan.PlanCatId });
        }

        private bool TblSpecificPlanExists(Guid id)
        {
            return (_context.TblSpecificPlans?.Any(e => e.SpecificPlanId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Assign(Guid? SpecificPlanId)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();

            var isAssigned = _context.TblAssignedYearlyPlans.FirstOrDefault(a => a.SpecificPlanId == SpecificPlanId.Value);
            InspectionAssignModel model = new InspectionAssignModel();
            var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == SpecificPlanId).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "FLIM").ToList();
            if (isAssigned != null)
            {
                model.PlanTitle = plats.Title;
                model.AssignedBy = userId;
                model.PlanCatId = plats.PlanCatId;
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName +" "+g.MidleName
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


                if (plats.TeamId != null)
                {
                    model.TeamId = plats.TeamId;
                }
                model.AssigneeTypeId = plats.AssigneeTypeId;
                model.StatusID = isAssigned.StatusId;
                model.AssignedDate = isAssigned.AssignedDate;
                model.DueDate = isAssigned.DueDate;
                model.SpecificPlanId = SpecificPlanId;
                model.Remark = plats.AssigningRemark;
                return View(model);
            }
            else
            {
                model.PlanTitle = plats.Title;
                model.AssignedBy = userId;
                model.PlanCatId = plats.PlanCatId;
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
                model.SpecificPlanId = SpecificPlanId;
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
            List<Guid> teamLeaderIds = new List<Guid>();
            List<string> teamEmails = new List<string>();
            List<string> assignees = new List<string>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.ToList();
            try
            {
                if (model.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847"))
                {
                    var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
                    var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "Assigned to Team").FirstOrDefault();
                    plats.AssigneeTypeId = model.AssigneeTypeId;
                    plats.ProId = status.ProId;
                    plats.IsAssignedToTeam = true;
                    //  var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.PlanId == model.PlanId).FirstOrDefault();
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
                        _notificationService.saveNotification(userId, teamLeaderIds, "Inpection plans are assigned to your team. Please review the assigned plans on Task Tracking Dashboard and assign to experts");

                        await SendMail(teamEmails, "Inpection Plans are assignment Notification", "<h3>Inpection plans are assigned to your team. Please review the assigned plans on Task Tracking Dashboard and assign to experts</h3>");
                        _notifyService.Success("Plan is successfully assigned to team");
                        return RedirectToAction(nameof(Index), new { PlanCatId = model.PlanCatId });
                    }
                    _notifyService.Error("Assingment isn't successfull. Please and try again");
                    model.Users = AllUsers.Select(g => new SelectListItem
                    {
                        Value = g.UserId.ToString(),
                        Text = g.FirstName + " " + g.MidleName,
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

                    return RedirectToAction(nameof(Assign), new { SpecificPlanId = model.SpecificPlanId });
                }
                else
                {
                    var yearlyStatus = _context.TblStatuses.Where(p => p.Status == "Assigned to user").FirstOrDefault();

                    var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
                    var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "Assigned to expert").FirstOrDefault();
                    plats.AssigneeTypeId = model.AssigneeTypeId;
                    plats.IsAssignedToUser = true;
                    var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).ToList();

                    if (ifAssigned.Count != 0)
                    {
                        _context.TblAssignedYearlyPlans.RemoveRange(ifAssigned);
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
                        yearlyPlan.StatusId = yearlyStatus.StatusId;
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

                        _notifyService.Success("Specific Plan successfully assigned");
                        return RedirectToAction(nameof(Index), new { PlanCatId = model.PlanCatId });
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

                        return RedirectToAction(nameof(Assign), new { SpecificPlanId = model.SpecificPlanId });
                    }

                }
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message + " Assingment isn't successfull. Please and try again");

                return RedirectToAction(nameof(Assign), new { SpecificPlanId = model.SpecificPlanId });

            }

        }

        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }


    }
}
