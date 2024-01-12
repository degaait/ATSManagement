using System.IO;
using NToastNotify;
using System.Numerics;
using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.Filters;
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
                var atsdbContext = _context.TblInspectionPlans.Include(t => t.User).Include(T => T.Pro).Include(s => s.Year);
                return View(await atsdbContext.ToListAsync());
            }
            else if (users.IsTeamLeader==true)
            {
                var atsdbContext = _context.TblInspectionPlans.Include(t => t.User).Include(T => T.Pro).Include(s => s.Year).Include(s=>s.Team).Where(s=>s.TeamId==users.TeamId);
                return View(await atsdbContext.ToListAsync());           
            }
            else
            {
                _notifyService.Error("You have no access to this page");
                return RedirectToAction(nameof(Index), nameof(AssignedYearlyPlansController));

            }          
        }
        public async Task<IActionResult> OngoingPlan(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true|| users.IsTeamLeader == true)
            {
                var atsdbContext = _context.TblSpecificPlans
                    .Include(s=>s.CreatedByNavigation)
                    .Include(T => T.Pro)
                    .Include(s=>s.IsUpprovedbyDepartmentNavigation)
                    .Include(s=>s.IsUpprovedbyTeamNavigation)
                    .Include(s => s.IsUprovedByDeputyNavigation)
                    .Include(s=>s.IsUserUprovedNavigation).Where(s =>  s.Pro.ProstatusTitle != "New"&&(s.FinalStatus == false ||s.FinalStatus==null));
                return View(await atsdbContext.ToListAsync());
            }
            else if (users.IsTeamLeader == true)
            {
                var atsdbContext = _context.TblSpecificPlans
                    .Include(s => s.CreatedByNavigation)
                    .Include(T => T.Pro)
                    .Include(s => s.Team)
                    .Include(s => s.IsUpprovedbyDepartmentNavigation)
                    .Include(s => s.IsUpprovedbyTeamNavigation)
                    .Include(s => s.IsUprovedByDeputyNavigation)
                    .Include(s => s.IsUserUprovedNavigation).Where(s => s.TeamId == users.TeamId&& s.Pro.ProstatusTitle != "Completed" && s.Pro.ProstatusTitle != "New");
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                _notifyService.Error("You have no access to this page");
                return RedirectToAction(nameof(Index), nameof(AssignedYearlyPlansController));

            }
        }
        public async Task<IActionResult> CompletedPlans(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true)
            {
                var atsdbContext = _context.TblSpecificPlans.Include(t => t.Pro).Where(s => s.FinalStatus == true);
                return View(await atsdbContext.ToListAsync());
            }
            else if (users.IsTeamLeader == true)
            {
                var atsdbContext = _context.TblSpecificPlans.Include(T => T.Pro).Include(s => s.Team).Where(s => s.TeamId == users.TeamId&& s.FinalStatus == true);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                _notifyService.Error("You have no access to this page");
                return RedirectToAction(nameof(Index), nameof(AssignedYearlyPlansController));

            }
        }
        public async Task<IActionResult> SentPlans(Guid? id)
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
                return RedirectToAction(nameof(Index),nameof(HomeController));
            }
        }
        public async Task<IActionResult> TeamPlans()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
          var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
          
          if(users.IsTeamLeader == true)
            {
               

                var assigned = (from items in _context.TblSpecificPlans
                                join assing in _context.TblPlanCatagories on items.PlanCatId equals assing.PlanCatId
                                where items.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847") && items.TeamId==users.TeamId
                                select items).ToList();
                var atsdbContext = _context.TblSpecificPlans.Include(t => t.CreatedByNavigation).Include(T => T.Pro).Include(s => s.PlanCat).Include(s => s.Team).Where(s => s.TeamId == users.TeamId && s.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847")&&(s.IsAssignedToUser==false||s.IsAssignedToUser==null)).ToList();
                var filtered = atsdbContext.Intersect(assigned).ToList();
                return View(filtered);
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
                        dbPath = "/Files/" + fileName;
                        tible.Attachement = dbPath;
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
            var plats = _context.TblSpecificPlans.Where(p => p.InspectionPlanId == id).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.Where(s=>s.Dep.DepCode== "FLIM").ToList().Except(loggedInUser);
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
                if (plats.TeamId!=null)
                {
                    model.TeamId = plats.TeamId;
                }               
                model.AssigneeTypeId=plats.AssigneeTypeId;
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
                model.SpecificPlanId = id;
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
                var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
                var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "Assigned to Team").FirstOrDefault();
                plats.AssigneeTypeId = model.AssigneeTypeId;
                plats.ProId = status.ProId;
                plats.IsAssignedToTeam = true;
                var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.PlanId == model.SpecificPlanId).FirstOrDefault();
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
              
                var plats = _context.TblSpecificPlans.Where(p => p.SpecificPlanId == model.SpecificPlanId).FirstOrDefault();
                var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "Assigned to user").FirstOrDefault();
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
                model.AssignmentTypes = _context.TblAssignementTypes.Where(s=>s.AssigneeTypeId==plats.AssigneeTypeId).Select(s => new SelectListItem
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
            var status = _context.TblInspectionStatuses.Where(p => p.ProstatusTitle == "Assigned to user").FirstOrDefault();

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
                    int saved=_context.SaveChanges();
                    if (saved>0)
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
        public async Task<IActionResult> UppdateDesicionStatus(Guid? id)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            var plaDetail=_context.TblSpecificPlans.Where(s=>s.SpecificPlanId==id).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser user = await _context.TblInternalUsers.FindAsync(userId);
            model.SpecificPlanId=id;
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
                if (model.IsDeputyApprovalNeeded == true)
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
        public async Task<IActionResult> SendToInstitutions(Guid? InspectionPlanId)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            var plaDetail=_context.TblInspectionPlans.Where(s=>s.InspectionPlanId== InspectionPlanId).FirstOrDefault();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser user = await _context.TblInternalUsers.FindAsync(userId);
            model.SpecificPlanId =InspectionPlanId;
            model.ExpectedReplyDate=DateTime.Now.AddDays(20);
            model.Insititutions = _context.TblInistitutions.Select(s => new SelectListItem
            {
                Value = s.InistId.ToString(),
                Text = s.Name.ToString()
            }).ToList();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToInstitutions(InspectionAssignModel model)
        {
            TblSentInspection? yearlyPlan =  new TblSentInspection();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            
            yearlyPlan.SentDate = DateTime.Now;
            yearlyPlan.SentBy = userId;
            yearlyPlan.ExpectedReplyDate = model.ExpectedReplyDate;
            yearlyPlan.InstId = model.InistId;
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
            string dbPaths = "/Files/" + fileNames;
            yearlyPlan.OfficialLetter=dbPaths;
            _context.TblSentInspections.Add(yearlyPlan);
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Reccomendation is sent successfully!");
                return RedirectToAction(nameof(OngoingPlan));
            }
            else
            {                
                _notifyService.Error("Recomendation isn't sent. Please try again");               
                return View(model);
            }
        }
        public async Task<IActionResult> Responses(int RecId)
        {
            InspectionReplyModel model= new InspectionReplyModel();
            ViewData["Replies"] = _context.TblInspectionReplyes.Include(s => s.Rec).Include(s=>s.InternalUserNavigation).Include(s=>s.ExternalUserNavigation).Where(s => s.RecId == RecId).OrderByDescending(s=>s.ReplyId).ToList();
            model.RecId = RecId;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Responses(InspectionReplyModel replyModel)
        {
            List<string> emails = new List<string>();
            var infor=(from items in _context.TblSentInspections 
                                             join insts in _context.TblInspectionPlans on items.InspectionPlanId equals insts.InspectionPlanId
                                             join tekuan in _context.TblInistitutions on items.InstId equals tekuan.InistId 
                                             join users in _context.TblExternalUsers on tekuan.InistId equals users.InistId
                                             where items.RecId==replyModel.RecId
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
            replys.RecId= replyModel.RecId;
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
                dbPath = "/Files/" + fileName;
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
            else{
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
                    dbPath = "/Files/" + fileName;
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
    }
}
