namespace ATSManagement.Controllers
{
    using NToastNotify;
    using System.Numerics;
    using ATSManagement.Models;
    using ATSManagement.IModels;
    using ATSManagement.Filters;
    using ATSManagement.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using AspNetCoreHero.ToastNotification.Abstractions;

    [CheckSessionIsAvailable]
    public class InispectionPlansController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        public InispectionPlansController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mail, INotyfService notyfService)
        {
            _notifyService = notyfService;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
        }

        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true)
            {
                var atsdbContext = _context.TblInspectionPlans.Include(t => t.User).Include(T => T.Status).Include(s => s.Year);
                return View(await atsdbContext.ToListAsync());
            }
            else if (users.IsTeamLeader==true)
            {
                var atsdbContext = _context.TblInspectionPlans.Include(t => t.User).Include(T => T.Status).Include(s => s.Year).Include(s=>s.Team).Where(s=>s.TeamId==users.TeamId);
                return View(await atsdbContext.ToListAsync());           
            }
            else
            {
                var atsdbContext = _context.TblInspectionPlans.Include(t => t.User).Include(T => T.Status).Include(s=>s.TblAssignedYearlyPlans).Include(s => s.Year);
                return View(await atsdbContext.ToListAsync());

            }          
        }
        public async Task<IActionResult> TeamPlans()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true)
            {
                var assigned=(from items in _context.TblInspectionPlans
                              join assing in _context.TblAssignedYearlyPlans on items.InspectionPlanId equals assing.PlanId where items.AssigneeTypeId==Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847")  select items).ToList();

                var atsdbContext = _context.TblInspectionPlans.Include(t => t.User).Include(T => T.Status).Include(s => s.Year).Where(s=>s.TeamId!=null&&s.AssigneeTypeId==Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847")).ToList();
                var filtered=atsdbContext.Intersect(assigned).ToList();
                return View(filtered);
            }
           else if (users.IsTeamLeader == true)
            {
                var assigned = (from items in _context.TblInspectionPlans
                                join assing in _context.TblAssignedYearlyPlans on items.InspectionPlanId equals assing.PlanId
                                where items.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847")
                                select items).ToList();

                var atsdbContext = _context.TblInspectionPlans.Include(t => t.User).Include(T => T.Status).Include(s => s.Year).Include(s => s.Team).Where(s => s.TeamId == users.TeamId && s.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847")&&(s.IsAssignedToUser==false||s.IsAssignedToUser==null)).ToList();
                var filtered = atsdbContext.Intersect(assigned).ToList();

                return View(atsdbContext);
            }
            else
            {
                _notifyService.Information("You have no access to this page");
                return RedirectToAction(nameof(Index));
            }
        }
        public async Task<IActionResult> Details(Guid? id)
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
        public IActionResult Create()
        {
            InispectionPlan plan = new InispectionPlan();
            plan.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.InistId.ToString(),
            }).ToList();
            plan.InspectionYear = _context.TblYears.Select(a=> new SelectListItem
            {
                Value= a.YearId.ToString(),
                Text=a.Year
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

                if (ModelState.IsValid)
                {
                    tible.PlanTitle = inispectionPlan.PlanTitle;
                    tible.PlanDescription = inispectionPlan.PlanDescription;
                    tible.CreationDate = DateTime.Now;
                    tible.YearId = inispectionPlan.YearId;
                    tible.UserId = userId;
                    tible.StatusId = _context.TblStatuses.Where(A => A.Status == "New").Select(A => A.StatusId).FirstOrDefault();
                    if (inispectionPlan.InistId.Length > 0)
                    {
                        plan_in = new List<TblPlanInistitution>();
                        foreach (var item in inispectionPlan.InistId)
                        {
                            plan_in.Add(new TblPlanInistitution { InistId = item, PlanId = inispectionPlan.InspectionPlanId });
                        }
                        tible.TblPlanInistitutions = plan_in;
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
                else
                {
                    _notifyService.Error("Please fill all neccessary field and try again");
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

            var tblInspectionPlan = _context.TblInspectionPlans.Include("TblPlanInistitutions").FirstOrDefault(a => a.InspectionPlanId == id.Value);
            tblInspectionPlan.TblPlanInistitutions.ToList().ForEach(x => InistId.Add((Guid)x.InistId));
            inispection.PlanTitle = tblInspectionPlan.PlanTitle;
            inispection.PlanDescription = tblInspectionPlan.PlanDescription;           
            inispection.ModificationDate = tblInspectionPlan.ModificationDate;
            inispection.InspectionPlanId = tblInspectionPlan.InspectionPlanId;
            inispection.CreationDate = tblInspectionPlan.CreationDate;
            inispection.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.InistId.ToString(),

            }).ToList();
            inispection.InspectionYear = _context.TblYears.Select(a => new SelectListItem
            {
                Value = a.YearId.ToString(),
                Text = a.Year
            }).ToList();
            inispection.YearId = tblInspectionPlan.YearId;
            inispection.InistId = InistId.ToArray();
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
            if (ModelState.IsValid)
            {
                try
                {
                    tible = _context.TblInspectionPlans.Include("TblPlanInistitutions").FirstOrDefault(x => x.InspectionPlanId == model.InspectionPlanId);
                    tible.TblPlanInistitutions.ToList().ForEach(result => plan_in.Add(result));
                    _context.TblPlanInistitutions.RemoveRange(plan_in);
                    await _context.SaveChangesAsync();
                    tible.PlanTitle = model.PlanTitle;
                    tible.PlanDescription = model.PlanDescription;
                    tible.CreationDate = model.CreationDate;
                    tible.ModificationDate = DateTime.Now;
                    tible.YearId = model.YearId;
                    if (model.InistId.Length > 0)
                    {
                        plan_in = new List<TblPlanInistitution>();
                        foreach (var item in model.InistId)
                        {
                            plan_in.Add(new TblPlanInistitution { InistId = item, PlanId = model.InspectionPlanId });
                        }
                        tible.TblPlanInistitutions = plan_in;
                    }
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
            else
            {
                _notifyService.Success("Please fill all neccessary fields and try again");
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

            var isAssigned = _context.TblAssignedYearlyPlans.FirstOrDefault(a => a.PlanId == id.Value);
            InspectionAssignModel model = new InspectionAssignModel();
            var plats = _context.TblInspectionPlans.Where(p => p.InspectionPlanId == id).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.Where(s=>s.Dep.DepCode== "FLIM").ToList().Except(loggedInUser);
            if (isAssigned != null)
            {
                model.PlanTitle = plats.PlanTitle;
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
                if (plats.TeamId!=null)
                {
                    model.TeamId = plats.TeamId;
                }               
                model.AssigneeTypeId=plats.AssigneeTypeId;
                model.StatusID = isAssigned.StatusId;
                model.AssignedDate = isAssigned.AssignedDate;
                model.DueDate = isAssigned.DueDate;
                model.PlanId = id;
                model.Remark = plats.AssigningRemark;
                return View(model);
            }
            else
            {
                model.PlanTitle = plats.PlanTitle;
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
                model.AssignmentTypes=_context.TblAssignementTypes.Select(s=> new SelectListItem
                {
                    Value=s.AssigneeTypeId.ToString(),
                    Text = s.AssigneeType.ToString()
                }).ToList() ;
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "FLIM").Select(s => new SelectListItem
                {
                    Value = s.TeamId.ToString(),
                    Text = s.TeamName,

                }).ToList();
                model.AssignedDate = DateTime.Now;
                model.DueDate = DateTime.Now.AddDays(10);
                model.PlanId = id;
                return View(model);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(InspectionAssignModel model)
        {

            TblAssignedYearlyPlan yearlyPlan;
            List<TblAssignedYearlyPlan> tblAssignedYearlyPlans = new List<TblAssignedYearlyPlan>();
            List<String> depHeadEmail = new List<string>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.ToList().Except(loggedInUser);
            if (model.AssigneeTypeId==Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847"))
            {
                var plats = _context.TblInspectionPlans.Where(p => p.InspectionPlanId == model.PlanId).FirstOrDefault();
                var status = _context.TblStatuses.Where(p => p.Status == "Assigned to Team").FirstOrDefault();
                plats.AssigneeTypeId = model.AssigneeTypeId;
                plats.StatusId = status.StatusId;
                var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.PlanId == model.PlanId).FirstOrDefault();
                var teamLeader=(from items in _context.TblTeams 
                                join users in _context.TblInternalUsers on items.TeamLeaderId equals users.UserId where items.TeamId==model.TeamId
                                select users.EmailAddress).ToList();
                plats.TeamId=model.TeamId;
                int saved= await _context.SaveChangesAsync();
                if (saved>0)
                {
                    await SendMail(teamLeader,"Inpection Plans are assignment Notification","<h3>Inpection plans are assigned to your team. Please review the assigned plans on Task Tracking Dashboard and assign to experts</h3>");
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
                var plats = _context.TblInspectionPlans.Where(p => p.InspectionPlanId == model.PlanId).FirstOrDefault();
                var status = _context.TblStatuses.Where(p => p.Status == "Assigned to user").FirstOrDefault();
                plats.AssigneeTypeId = model.AssigneeTypeId;
                plats.IsAssignedToUser = true;
                var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.PlanId == model.PlanId).FirstOrDefault();
                if (ModelState.IsValid)
                {
                    if (ifAssigned != null)
                    {
                        _context.TblAssignedYearlyPlans.Remove(ifAssigned);
                        _context.SaveChanges();
                    }
                    foreach (var item in model.UserId)
                    {
                        yearlyPlan = new TblAssignedYearlyPlan();
                        yearlyPlan.AssignedBy = model.AssignedBy;
                        yearlyPlan.AssignedTo = item;
                        yearlyPlan.ProgressStatus = status.Status;
                        yearlyPlan.AssignedDate = model.AssignedDate;
                        yearlyPlan.DueDate = model.DueDate;
                        yearlyPlan.Remark = model.Remark;
                        yearlyPlan.PlanId = model.PlanId;
                        plats.StatusId = status.StatusId;
                        plats.ModificationDate = DateTime.Now;
                        plats.AssigningRemark = model.Remark;
                        tblAssignedYearlyPlans.Add(yearlyPlan);
                    }
                    _context.TblAssignedYearlyPlans.AddRange(tblAssignedYearlyPlans);
                   int saved= _context.SaveChanges();
                    if (saved>0)
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
           
            var plats = _context.TblInspectionPlans.Where(p => p.InspectionPlanId == id).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "FLIM").ToList().Except(loggedInUser);
            if (isAssigned != null)
            {
                model.PlanTitle = plats.PlanTitle;
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
                model.AssignmentTypes = _context.TblAssignementTypes.Where(s=>s.AssigneeTypeId==plats.AssigneeTypeId).Select(s => new SelectListItem
                {
                    Value = s.AssigneeTypeId.ToString(),
                    Text = s.AssigneeType.ToString()
                }).ToList();
                model.AssigneeTypeId = plats.AssigneeTypeId;
                model.StatusID = isAssigned.StatusId;
                model.AssignedDate = isAssigned.AssignedDate;
                model.DueDate = isAssigned.DueDate;
                model.PlanId = id;
                model.Remark = plats.AssigningRemark;
                return View(model);
            }
            else
            {
                model.PlanTitle = plats.PlanTitle;
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
                model.PlanId = id;
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
            var plats = _context.TblInspectionPlans.Where(p => p.InspectionPlanId == model.PlanId).FirstOrDefault();
            var status = _context.TblStatuses.Where(p => p.Status == "Assigned to user").FirstOrDefault();

            var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.PlanId == model.PlanId).FirstOrDefault();
          
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
                        yearlyPlan.ProgressStatus = status.Status;
                        yearlyPlan.AssignedDate = model.AssignedDate;
                        yearlyPlan.DueDate = model.DueDate;
                        yearlyPlan.Remark = model.Remark;
                        yearlyPlan.PlanId = model.PlanId;
                        plats.StatusId = status.StatusId;
                        plats.ModificationDate = DateTime.Now;
                        plats.AssigningRemark = model.Remark;
                        tblAssignedYearlyPlans.Add(yearlyPlan);
                    }
                    _context.TblAssignedYearlyPlans.AddRange(tblAssignedYearlyPlans);
                    int saved=_context.SaveChanges();
                    if (saved>0)
                    {
                        _notifyService.Success("Plan successfully assigned");
                        return RedirectToAction("Index");
                    }
                    else
                    {
                    model.PlanId = model.PlanId;
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
                    
                    model.PlanId = model.PlanId;
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
                    model.Teams=_context.TblTeams.Where(s=>s.TeamId==plats.TeamId).Select(s=> new SelectListItem
                    {
                        Value=s.TeamId.ToString(),
                        Text=s.TeamName
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
    }
}
