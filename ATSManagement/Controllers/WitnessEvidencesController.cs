using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ATSManagement.Controllers
{
    public class WitnessEvidencesController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IToastNotification _toastNotification;
        public WitnessEvidencesController(AtsdbContext context, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _context = context;
        }

        // GET: WitnessEvidences
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblWitnessEvidences.Include(t => t.CreatedByNavigation).Include(t => t.Request);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: WitnessEvidences/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblWitnessEvidences == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblWitnessEvidences
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.WitnessId == id);
            if (tblWitnessEvidence == null)
            {
                return NotFound();
            }

            return View(tblWitnessEvidence);
        }

        // GET: WitnessEvidences/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId");
            return View();
        }

        // POST: WitnessEvidences/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WitnessId,WitnessesName,EvidenceFiles,EvidenceVideos,RequestId,CreatedBy,CreatedDate")] TblWitnessEvidence tblWitnessEvidence)
        {
            if (ModelState.IsValid)
            {
                tblWitnessEvidence.WitnessId = Guid.NewGuid();
                _context.Add(tblWitnessEvidence);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblWitnessEvidence.CreatedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblWitnessEvidence.RequestId);
            return View(tblWitnessEvidence);
        }

        // GET: WitnessEvidences/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblWitnessEvidences == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblWitnessEvidences.FindAsync(id);
            if (tblWitnessEvidence == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblWitnessEvidence.CreatedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblWitnessEvidence.RequestId);
            return View(tblWitnessEvidence);
        }

        // POST: WitnessEvidences/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("WitnessId,WitnessesName,EvidenceFiles,EvidenceVideos,RequestId,CreatedBy,CreatedDate")] TblWitnessEvidence tblWitnessEvidence)
        {
            if (id != tblWitnessEvidence.WitnessId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblWitnessEvidence);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblWitnessEvidenceExists(tblWitnessEvidence.WitnessId))
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
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblWitnessEvidence.CreatedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblWitnessEvidence.RequestId);
            return View(tblWitnessEvidence);
        }

        // GET: WitnessEvidences/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblWitnessEvidences == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblWitnessEvidences
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.WitnessId == id);
            if (tblWitnessEvidence == null)
            {
                return NotFound();
            }

            return View(tblWitnessEvidence);
        }

        // POST: WitnessEvidences/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblWitnessEvidences == null)
            {
                return Problem("Entity set 'AtsdbContext.TblWitnessEvidences'  is null.");
            }
            var tblWitnessEvidence = await _context.TblWitnessEvidences.FindAsync(id);
            if (tblWitnessEvidence != null)
            {
                _context.TblWitnessEvidences.Remove(tblWitnessEvidence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblWitnessEvidenceExists(Guid id)
        {
            return (_context.TblWitnessEvidences?.Any(e => e.WitnessId == id)).GetValueOrDefault();
        }
    }
}
