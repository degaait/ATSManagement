using ATSManagement.IModels;
using ATSManagement.Models;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ATSManagement.Controllers
{
    public class InispectionPlansController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly IToastNotification _toastNotification;
        public InispectionPlansController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mail, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
        }

        // GET: InispectionPlans
        public async Task<IActionResult> Index()
        {

            var atsdbContext = _context.TblInspectionPlans.Include(t => t.User).Include(T => T.Status);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: InispectionPlans/Details/5
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

        // GET: InispectionPlans/Create
        public IActionResult Create()
        {
            InispectionPlan plan = new InispectionPlan();
            plan.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.InistId.ToString(),
            }).ToList();

            plan.CreationDate = DateTime.Now;
            return View(plan);
        }

        // POST: InispectionPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InispectionPlan inispectionPlan)
        {
            TblInspectionPlan tible = new TblInspectionPlan();
            List<TblPlanInistitution> plan_in = new List<TblPlanInistitution>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (ModelState.IsValid)
            {
                tible.PlanTitle = inispectionPlan.PlanTitle;
                tible.PlanDescription = inispectionPlan.PlanDescription;
                tible.CreationDate = DateTime.Now;
                tible.InspectionYear = inispectionPlan.InspectionYear;
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
                return RedirectToAction(nameof(Index));
            }
            return View(inispectionPlan);
        }

        // GET: InispectionPlans/Edit/5
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
            inispection.InspectionYear = tblInspectionPlan.InspectionYear;
            inispection.ModificationDate = tblInspectionPlan.ModificationDate;
            inispection.InspectionPlanId = tblInspectionPlan.InspectionPlanId;
            inispection.CreationDate = tblInspectionPlan.CreationDate;
            inispection.Inistitutions = _context.TblInistitutions.Select(a => new SelectListItem
            {
                Text = a.Name,
                Value = a.InistId.ToString(),

            }).ToList();
            inispection.InistId = InistId.ToArray();
            if (tblInspectionPlan == null)
            {
                return Task.FromResult<IActionResult>(NotFound());
            }
            ViewData["UserId"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInspectionPlan.UserId);
            return Task.FromResult<IActionResult>(View(inispection));
        }

        // POST: InispectionPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                    tible.InspectionYear = model.InspectionYear;
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
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View(model);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblInspectionPlanExists(model.InspectionPlanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: InispectionPlans/Delete/5
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

        // POST: InispectionPlans/Delete/5
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
            var AllUsers = _context.TblInternalUsers.ToList().Except(loggedInUser);

            if (isAssigned != null)
            {
                model.PlanTitle = plats.PlanTitle;
                model.AssignedBy = userId;
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName
                }).ToList();
                model.UserId = isAssigned.AssignedTo;
                model.status = _context.TblStatuses.Where(p => p.Status == "New").Select(p => new SelectListItem
                {
                    Value = p.StatusId.ToString(),
                    Text = p.Status.ToString()
                }).ToList();
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
                model.AssignedDate = DateTime.Now;
                model.DueDate = DateTime.Now.AddDays(10);
                model.PlanId = id;
                model.UserId = userId;
                return View(model);
            }
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign(InspectionAssignModel model)
        {

            TblAssignedYearlyPlan yearlyPlan = new TblAssignedYearlyPlan();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var loggedInUser = _context.TblInternalUsers.Where(p => p.UserId == userId).ToList();
            var AllUsers = _context.TblInternalUsers.ToList().Except(loggedInUser);

            if (ModelState.IsValid)
            {
                var plats = _context.TblInspectionPlans.Where(p => p.InspectionPlanId == model.PlanId).FirstOrDefault();
                var status = _context.TblStatuses.Where(p => p.Status == "Assigned to user").FirstOrDefault();
                var ifAssigned = _context.TblAssignedYearlyPlans.Where(p => p.PlanId == model.PlanId).FirstOrDefault();
                if (ifAssigned != null)
                {
                    _context.TblAssignedYearlyPlans.Remove(ifAssigned);
                    _context.SaveChanges();
                }
                yearlyPlan.AssignedBy = model.AssignedBy;
                yearlyPlan.AssignedTo = model.UserId;
                yearlyPlan.ProgressStatus = status.Status;
                yearlyPlan.AssignedDate = model.AssignedDate;
                yearlyPlan.DueDate = model.DueDate;
                yearlyPlan.Remark = model.Remark;
                yearlyPlan.PlanId = model.PlanId;
                plats.StatusId = status.StatusId;
                plats.ModificationDate = DateTime.Now;
                plats.AssigningRemark = model.Remark;
                _context.TblAssignedYearlyPlans.Add(yearlyPlan);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                model.Users = AllUsers.Select(g => new SelectListItem
                {
                    Value = g.UserId.ToString(),
                    Text = g.FirstName
                }).ToList();
                model.PlanId = model.PlanId;
                ViewBag.AssignedTo = new SelectList(_context.TblInternalUsers, "UserId", "FirstName");
                return View(model);
            }
        }
    }
}
