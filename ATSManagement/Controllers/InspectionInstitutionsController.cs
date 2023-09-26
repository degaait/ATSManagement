using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ATSManagement.Models;

namespace ATSManagement.Controllers
{
    public class InspectionInstitutionsController : Controller
    {
        private readonly AtsdbContext _context;

        public InspectionInstitutionsController(AtsdbContext context)
        {
            _context = context;
        }

        // GET: InspectionInstitutions
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblInspectionInstitutions.Include(t => t.Institution).Include(t => t.ReturnedByNavigation).Include(t => t.SubmittedByNavigation);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: InspectionInstitutions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblInspectionInstitutions == null)
            {
                return NotFound();
            }

            var tblInspectionInstitution = await _context.TblInspectionInstitutions
                .Include(t => t.Institution)
                .Include(t => t.ReturnedByNavigation)
                .Include(t => t.SubmittedByNavigation)
                .FirstOrDefaultAsync(m => m.SubMissionId == id);
            if (tblInspectionInstitution == null)
            {
                return NotFound();
            }

            return View(tblInspectionInstitution);
        }

        // GET: InspectionInstitutions/Create
        public IActionResult Create()
        {
            ViewData["InstitutionId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId");
            ViewData["ReturnedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId");
            ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            return View();
        }

        // POST: InspectionInstitutions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubMissionId,RequestStatus,RecomendationDetail,RecomendationFeedBack,RequestedDate,ExpectedResponseDate,InstitutionId,ResponseStatus,ReComendationFile,SubmittedBy,ReturnedBy")] TblInspectionInstitution tblInspectionInstitution)
        {
            if (ModelState.IsValid)
            {
                tblInspectionInstitution.SubMissionId = Guid.NewGuid();
                _context.Add(tblInspectionInstitution);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InstitutionId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblInspectionInstitution.InstitutionId);
            ViewData["ReturnedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblInspectionInstitution.ReturnedBy);
            ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInspectionInstitution.SubmittedBy);
            return View(tblInspectionInstitution);
        }

        // GET: InspectionInstitutions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblInspectionInstitutions == null)
            {
                return NotFound();
            }

            var tblInspectionInstitution = await _context.TblInspectionInstitutions.FindAsync(id);
            if (tblInspectionInstitution == null)
            {
                return NotFound();
            }
            ViewData["InstitutionId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblInspectionInstitution.InstitutionId);
            ViewData["ReturnedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblInspectionInstitution.ReturnedBy);
            ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInspectionInstitution.SubmittedBy);
            return View(tblInspectionInstitution);
        }

        // POST: InspectionInstitutions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SubMissionId,RequestStatus,RecomendationDetail,RecomendationFeedBack,RequestedDate,ExpectedResponseDate,InstitutionId,ResponseStatus,ReComendationFile,SubmittedBy,ReturnedBy")] TblInspectionInstitution tblInspectionInstitution)
        {
            if (id != tblInspectionInstitution.SubMissionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblInspectionInstitution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblInspectionInstitutionExists(tblInspectionInstitution.SubMissionId))
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
            ViewData["InstitutionId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblInspectionInstitution.InstitutionId);
            ViewData["ReturnedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblInspectionInstitution.ReturnedBy);
            ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInspectionInstitution.SubmittedBy);
            return View(tblInspectionInstitution);
        }

        // GET: InspectionInstitutions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblInspectionInstitutions == null)
            {
                return NotFound();
            }

            var tblInspectionInstitution = await _context.TblInspectionInstitutions
                .Include(t => t.Institution)
                .Include(t => t.ReturnedByNavigation)
                .Include(t => t.SubmittedByNavigation)
                .FirstOrDefaultAsync(m => m.SubMissionId == id);
            if (tblInspectionInstitution == null)
            {
                return NotFound();
            }

            return View(tblInspectionInstitution);
        }

        // POST: InspectionInstitutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblInspectionInstitutions == null)
            {
                return Problem("Entity set 'AtsdbContext.TblInspectionInstitutions'  is null.");
            }
            var tblInspectionInstitution = await _context.TblInspectionInstitutions.FindAsync(id);
            if (tblInspectionInstitution != null)
            {
                _context.TblInspectionInstitutions.Remove(tblInspectionInstitution);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblInspectionInstitutionExists(Guid id)
        {
          return (_context.TblInspectionInstitutions?.Any(e => e.SubMissionId == id)).GetValueOrDefault();
        }
    }
}
