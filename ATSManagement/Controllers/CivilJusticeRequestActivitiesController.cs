using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ATSManagement.Controllers
{
    public class CivilJusticeRequestActivitiesController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IToastNotification _toastNotification;
        public CivilJusticeRequestActivitiesController(AtsdbContext context, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _context = context;
        }

        // GET: CivilJusticeRequestActivities
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblCivilJusticeRequestActivities.Include(t => t.CreatedByNavigation).Include(t => t.Request);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: CivilJusticeRequestActivities/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblCivilJusticeRequestActivities == null)
            {
                return NotFound();
            }

            var tblCivilJusticeRequestActivity = await _context.TblCivilJusticeRequestActivities
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (tblCivilJusticeRequestActivity == null)
            {
                return NotFound();
            }

            return View(tblCivilJusticeRequestActivity);
        }

        // GET: CivilJusticeRequestActivities/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId");
            return View();
        }

        // POST: CivilJusticeRequestActivities/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActivityId,ActivityDetail,AddedDate,RequestId,CreatedBy")] TblCivilJusticeRequestActivity tblCivilJusticeRequestActivity)
        {
            if (ModelState.IsValid)
            {
                tblCivilJusticeRequestActivity.ActivityId = Guid.NewGuid();
                _context.Add(tblCivilJusticeRequestActivity);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJusticeRequestActivity.CreatedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblCivilJusticeRequestActivity.RequestId);
            return View(tblCivilJusticeRequestActivity);
        }

        // GET: CivilJusticeRequestActivities/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblCivilJusticeRequestActivities == null)
            {
                return NotFound();
            }

            var tblCivilJusticeRequestActivity = await _context.TblCivilJusticeRequestActivities.FindAsync(id);
            if (tblCivilJusticeRequestActivity == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJusticeRequestActivity.CreatedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblCivilJusticeRequestActivity.RequestId);
            return View(tblCivilJusticeRequestActivity);
        }

        // POST: CivilJusticeRequestActivities/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ActivityId,ActivityDetail,AddedDate,RequestId,CreatedBy")] TblCivilJusticeRequestActivity tblCivilJusticeRequestActivity)
        {
            if (id != tblCivilJusticeRequestActivity.ActivityId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCivilJusticeRequestActivity);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblCivilJusticeRequestActivityExists(tblCivilJusticeRequestActivity.ActivityId))
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
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJusticeRequestActivity.CreatedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblCivilJusticeRequestActivity.RequestId);
            return View(tblCivilJusticeRequestActivity);
        }

        // GET: CivilJusticeRequestActivities/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblCivilJusticeRequestActivities == null)
            {
                return NotFound();
            }

            var tblCivilJusticeRequestActivity = await _context.TblCivilJusticeRequestActivities
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (tblCivilJusticeRequestActivity == null)
            {
                return NotFound();
            }

            return View(tblCivilJusticeRequestActivity);
        }

        // POST: CivilJusticeRequestActivities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblCivilJusticeRequestActivities == null)
            {
                return Problem("Entity set 'AtsdbContext.TblCivilJusticeRequestActivities'  is null.");
            }
            var tblCivilJusticeRequestActivity = await _context.TblCivilJusticeRequestActivities.FindAsync(id);
            if (tblCivilJusticeRequestActivity != null)
            {
                _context.TblCivilJusticeRequestActivities.Remove(tblCivilJusticeRequestActivity);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblCivilJusticeRequestActivityExists(Guid id)
        {
            return (_context.TblCivilJusticeRequestActivities?.Any(e => e.ActivityId == id)).GetValueOrDefault();
        }
    }
}
