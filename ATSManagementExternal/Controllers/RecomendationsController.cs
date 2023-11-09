using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ATSManagementExternal.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagementExternal.Controllers
{
    public class RecomendationsController : Controller
    {
        private readonly AtsdbContext _context;

        public RecomendationsController(AtsdbContext context)
        {
            _context = context;
        }

        // GET: Recomendations
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblRecomendations.Include(t => t.CreatedByNavigation).Include(t => t.Inist).Include(t => t.Recostatus);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: Recomendations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblRecomendations == null)
            {
                return NotFound();
            }

            var tblRecomendation = await _context.TblRecomendations
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Inist)
                .Include(t => t.Recostatus)
                .FirstOrDefaultAsync(m => m.RecoId == id);
            if (tblRecomendation == null)
            {
                return NotFound();
            }

            return View(tblRecomendation);
        }

        // GET: Recomendations/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId");
            ViewData["RecostatusId"] = new SelectList(_context.TblRecomendationStatuses, "RecostatusId", "RecostatusId");
            return View();
        }

        // POST: Recomendations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecoId,Recomendation,InistId,RecostatusId,EvaluationYear,CreatedBy,CreatinDate,ModifyDate,IsActive")] TblRecomendation tblRecomendation)
        {
            if (ModelState.IsValid)
            {
                tblRecomendation.RecoId = Guid.NewGuid();
                _context.Add(tblRecomendation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRecomendation.CreatedBy);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblRecomendation.InistId);
            ViewData["RecostatusId"] = new SelectList(_context.TblRecomendationStatuses, "RecostatusId", "RecostatusId", tblRecomendation.RecostatusId);
            return View(tblRecomendation);
        }

        // GET: Recomendations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblRecomendations == null)
            {
                return NotFound();
            }

            var tblRecomendation = await _context.TblRecomendations.FindAsync(id);
            if (tblRecomendation == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRecomendation.CreatedBy);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblRecomendation.InistId);
            ViewData["RecostatusId"] = new SelectList(_context.TblRecomendationStatuses, "RecostatusId", "RecostatusId", tblRecomendation.RecostatusId);
            return View(tblRecomendation);
        }

        // POST: Recomendations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RecoId,Recomendation,InistId,RecostatusId,EvaluationYear,CreatedBy,CreatinDate,ModifyDate,IsActive")] TblRecomendation tblRecomendation)
        {
            if (id != tblRecomendation.RecoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblRecomendation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblRecomendationExists(tblRecomendation.RecoId))
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
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRecomendation.CreatedBy);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblRecomendation.InistId);
            ViewData["RecostatusId"] = new SelectList(_context.TblRecomendationStatuses, "RecostatusId", "RecostatusId", tblRecomendation.RecostatusId);
            return View(tblRecomendation);
        }

        // GET: Recomendations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblRecomendations == null)
            {
                return NotFound();
            }

            var tblRecomendation = await _context.TblRecomendations
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Inist)
                .Include(t => t.Recostatus)
                .FirstOrDefaultAsync(m => m.RecoId == id);
            if (tblRecomendation == null)
            {
                return NotFound();
            }

            return View(tblRecomendation);
        }

        // POST: Recomendations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblRecomendations == null)
            {
                return Problem("Entity set 'AtsdbContext.TblRecomendations'  is null.");
            }
            var tblRecomendation = await _context.TblRecomendations.FindAsync(id);
            if (tblRecomendation != null)
            {
                _context.TblRecomendations.Remove(tblRecomendation);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblRecomendationExists(Guid id)
        {
          return (_context.TblRecomendations?.Any(e => e.RecoId == id)).GetValueOrDefault();
        }
    }
}
