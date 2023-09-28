using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ATSManagement.Controllers
{
    public class ExternalRequestsController : Controller
    {
        private readonly AtsdbContext _context;

        public ExternalRequestsController(AtsdbContext context)
        {
            _context = context;
        }

        // GET: ExternalRequests
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblExternalRequests.Include(t => t.ExterUser).Include(t => t.Int);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: ExternalRequests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblExternalRequests == null)
            {
                return NotFound();
            }

            var tblExternalRequest = await _context.TblExternalRequests
                .Include(t => t.ExterUser)
                .Include(t => t.Int)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblExternalRequest == null)
            {
                return NotFound();
            }

            return View(tblExternalRequest);
        }

        // GET: ExternalRequests/Create
        public IActionResult Create()
        {
            ViewData["ExterUserId"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId");
            ViewData["IntId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId");
            return View();
        }

        // POST: ExternalRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,RequestDetail,RequestedDate,IntId,ExterUserId")] TblExternalRequest tblExternalRequest)
        {
            if (ModelState.IsValid)
            {
                tblExternalRequest.RequestId = Guid.NewGuid();
                _context.Add(tblExternalRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExterUserId"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblExternalRequest.ExterUserId);
            ViewData["IntId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblExternalRequest.IntId);
            return View(tblExternalRequest);
        }

        // GET: ExternalRequests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblExternalRequests == null)
            {
                return NotFound();
            }

            var tblExternalRequest = await _context.TblExternalRequests.FindAsync(id);
            if (tblExternalRequest == null)
            {
                return NotFound();
            }
            ViewData["ExterUserId"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblExternalRequest.ExterUserId);
            ViewData["IntId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblExternalRequest.IntId);
            return View(tblExternalRequest);
        }

        // POST: ExternalRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RequestId,RequestDetail,RequestedDate,IntId,ExterUserId")] TblExternalRequest tblExternalRequest)
        {
            if (id != tblExternalRequest.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblExternalRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblExternalRequestExists(tblExternalRequest.RequestId))
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
            ViewData["ExterUserId"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblExternalRequest.ExterUserId);
            ViewData["IntId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblExternalRequest.IntId);
            return View(tblExternalRequest);
        }

        // GET: ExternalRequests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblExternalRequests == null)
            {
                return NotFound();
            }

            var tblExternalRequest = await _context.TblExternalRequests
                .Include(t => t.ExterUser)
                .Include(t => t.Int)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblExternalRequest == null)
            {
                return NotFound();
            }

            return View(tblExternalRequest);
        }

        // POST: ExternalRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblExternalRequests == null)
            {
                return Problem("Entity set 'AtsdbContext.TblExternalRequests'  is null.");
            }
            var tblExternalRequest = await _context.TblExternalRequests.FindAsync(id);
            if (tblExternalRequest != null)
            {
                _context.TblExternalRequests.Remove(tblExternalRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblExternalRequestExists(Guid id)
        {
            return (_context.TblExternalRequests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
    }
}
