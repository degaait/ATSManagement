using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ATSManagement.Controllers
{
    public class AdjornmentsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IToastNotification _toastNotification;
        public AdjornmentsController(AtsdbContext context, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _context = context;
        }

        // GET: Adjornments
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblAdjornments.Include(t => t.CreatedByNavigation).Include(t => t.Request);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: Adjornments/Details/5
        public async Task<IActionResult> Details(Guid? id)
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

        // GET: Adjornments/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId");
            return View();
        }

        // POST: Adjornments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdjoryId,RequestId,AdjorneyDate,WhatIsDone,CreatedBy,CreatedDate")] TblAdjornment tblAdjornment)
        {
            if (ModelState.IsValid)
            {
                tblAdjornment.AdjoryId = Guid.NewGuid();
                _context.Add(tblAdjornment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblAdjornment.CreatedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblAdjornment.RequestId);
            return View(tblAdjornment);
        }

        // GET: Adjornments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblAdjornments == null)
            {
                return NotFound();
            }

            var tblAdjornment = await _context.TblAdjornments.FindAsync(id);
            if (tblAdjornment == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblAdjornment.CreatedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblAdjornment.RequestId);
            return View(tblAdjornment);
        }

        // POST: Adjornments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AdjoryId,RequestId,AdjorneyDate,WhatIsDone,CreatedBy,CreatedDate")] TblAdjornment tblAdjornment)
        {
            if (id != tblAdjornment.AdjoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblAdjornment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblAdjornmentExists(tblAdjornment.AdjoryId))
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
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblAdjornment.CreatedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblAdjornment.RequestId);
            return View(tblAdjornment);
        }

        // GET: Adjornments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
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

        // POST: Adjornments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
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
            return RedirectToAction(nameof(Index));
        }

        private bool TblAdjornmentExists(Guid id)
        {
            return (_context.TblAdjornments?.Any(e => e.AdjoryId == id)).GetValueOrDefault();
        }
    }
}
