using System;
using System.Linq;
using ATSManagement.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ATSManagement.ViewModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.Controllers
{
    public class SpecificPlansController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public SpecificPlansController(AtsdbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // GET: SpecificPlans
        public async Task<IActionResult> Index(Guid? InspectionPlanId)
        {
            ViewBag.InspectionPlanId = InspectionPlanId;
            var atsdbContext = _context.TblSpecificPlans.Include(t => t.CreatedByNavigation).Include(t => t.InspectionPlan).Where(x=>x.InspectionPlanId==InspectionPlanId);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: SpecificPlans/Details/5
        public async Task<IActionResult> Details(Guid? id)
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

            return View(tblSpecificPlan);
        }

        // GET: SpecificPlans/Create
        public IActionResult Create(Guid? InspectionPlanId)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            AnnualSpecificPlan annualSpecificPlan = new AnnualSpecificPlan();
            annualSpecificPlan.InspectionPlanId = InspectionPlanId;
            annualSpecificPlan.CreatedBy = userId;
            annualSpecificPlan.CreatedDate= DateTime.Now;

            return View();
        }

        // POST: SpecificPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnualSpecificPlan model)
        {
            if (ModelState.IsValid)
            {
                TblSpecificPlan tblSpecificPlan = new TblSpecificPlan();
                tblSpecificPlan.Title = model.Title;
                tblSpecificPlan.Description = model.Description;
                tblSpecificPlan.InspectionPlanId = model.InspectionPlanId;
                tblSpecificPlan.CreatedBy = model.CreatedBy;
                tblSpecificPlan.CreatedDate = DateTime.Now;
                _context.Add(tblSpecificPlan);
               int saved= await _context.SaveChangesAsync();
                if (saved>0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
               
            }
            else
            {
                return View(model);
            }
        
        }

        // GET: SpecificPlans/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
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
            model.InspectionPlanId= tblSpecificPlan.InspectionPlanId;
            model.Title= tblSpecificPlan.Title;
            model.Description= tblSpecificPlan.Description;
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = userId;
            return View(tblSpecificPlan);
        }

        // POST: SpecificPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SpecificPlanId,Title,Description,CreatedDate,ModificationDate,CreatedBy,InspectionPlanId")] TblSpecificPlan tblSpecificPlan)
        {
            if (id != tblSpecificPlan.SpecificPlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblSpecificPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblSpecificPlanExists(tblSpecificPlan.SpecificPlanId))
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
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblSpecificPlan.CreatedBy);
            ViewData["InspectionPlanId"] = new SelectList(_context.TblInspectionPlans, "InspectionPlanId", "InspectionPlanId", tblSpecificPlan.InspectionPlanId);
            return View(tblSpecificPlan);
        }

        // GET: SpecificPlans/Delete/5
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

            return View(tblSpecificPlan);
        }

        // POST: SpecificPlans/Delete/5
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
            return RedirectToAction(nameof(Index));
        }

        private bool TblSpecificPlanExists(Guid id)
        {
          return (_context.TblSpecificPlans?.Any(e => e.SpecificPlanId == id)).GetValueOrDefault();
        }
    }
}
