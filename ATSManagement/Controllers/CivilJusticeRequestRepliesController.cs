using NToastNotify;
using ATSManagement.Models;
using ATSManagement.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class CivilJusticeRequestRepliesController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IToastNotification _toastNotification;
        public CivilJusticeRequestRepliesController(AtsdbContext context, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _context = context;
        }

        // GET: CivilJusticeRequestReplies
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblCivilJusticeRequestReplys.Include(t => t.ExternalReplayedByNavigation).Include(t => t.InternalReplayedByNavigation).Include(t => t.Request);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: CivilJusticeRequestReplies/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblCivilJusticeRequestReplys == null)
            {
                return NotFound();
            }

            var tblCivilJusticeRequestReply = await _context.TblCivilJusticeRequestReplys
                .Include(t => t.ExternalReplayedByNavigation)
                .Include(t => t.InternalReplayedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.ReplyId == id);
            if (tblCivilJusticeRequestReply == null)
            {
                return NotFound();
            }

            return View(tblCivilJusticeRequestReply);
        }

        // GET: CivilJusticeRequestReplies/Create
        public IActionResult Create()
        {
            ViewData["ExternalReplayedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId");
            ViewData["InternalReplayedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId");
            return View();
        }

        // POST: CivilJusticeRequestReplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReplyId,ReplayDetail,RequestId,InternalReplayedBy,ReplyDate,ExternalReplayedBy")] TblCivilJusticeRequestReply tblCivilJusticeRequestReply)
        {
            if (ModelState.IsValid)
            {
                tblCivilJusticeRequestReply.ReplyId = Guid.NewGuid();
                _context.Add(tblCivilJusticeRequestReply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExternalReplayedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblCivilJusticeRequestReply.ExternalReplayedBy);
            ViewData["InternalReplayedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJusticeRequestReply.InternalReplayedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblCivilJusticeRequestReply.RequestId);
            return View(tblCivilJusticeRequestReply);
        }

        // GET: CivilJusticeRequestReplies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblCivilJusticeRequestReplys == null)
            {
                return NotFound();
            }

            var tblCivilJusticeRequestReply = await _context.TblCivilJusticeRequestReplys.FindAsync(id);
            if (tblCivilJusticeRequestReply == null)
            {
                return NotFound();
            }
            ViewData["ExternalReplayedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblCivilJusticeRequestReply.ExternalReplayedBy);
            ViewData["InternalReplayedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJusticeRequestReply.InternalReplayedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblCivilJusticeRequestReply.RequestId);
            return View(tblCivilJusticeRequestReply);
        }

        // POST: CivilJusticeRequestReplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ReplyId,ReplayDetail,RequestId,InternalReplayedBy,ReplyDate,ExternalReplayedBy")] TblCivilJusticeRequestReply tblCivilJusticeRequestReply)
        {
            if (id != tblCivilJusticeRequestReply.ReplyId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCivilJusticeRequestReply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblCivilJusticeRequestReplyExists(tblCivilJusticeRequestReply.ReplyId))
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
            ViewData["ExternalReplayedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblCivilJusticeRequestReply.ExternalReplayedBy);
            ViewData["InternalReplayedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJusticeRequestReply.InternalReplayedBy);
            ViewData["RequestId"] = new SelectList(_context.TblCivilJustices, "RequestId", "RequestId", tblCivilJusticeRequestReply.RequestId);
            return View(tblCivilJusticeRequestReply);
        }

        // GET: CivilJusticeRequestReplies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblCivilJusticeRequestReplys == null)
            {
                return NotFound();
            }

            var tblCivilJusticeRequestReply = await _context.TblCivilJusticeRequestReplys
                .Include(t => t.ExternalReplayedByNavigation)
                .Include(t => t.InternalReplayedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.ReplyId == id);
            if (tblCivilJusticeRequestReply == null)
            {
                return NotFound();
            }

            return View(tblCivilJusticeRequestReply);
        }

        // POST: CivilJusticeRequestReplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblCivilJusticeRequestReplys == null)
            {
                return Problem("Entity set 'AtsdbContext.TblCivilJusticeRequestReplys'  is null.");
            }
            var tblCivilJusticeRequestReply = await _context.TblCivilJusticeRequestReplys.FindAsync(id);
            if (tblCivilJusticeRequestReply != null)
            {
                _context.TblCivilJusticeRequestReplys.Remove(tblCivilJusticeRequestReply);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblCivilJusticeRequestReplyExists(Guid id)
        {
            return (_context.TblCivilJusticeRequestReplys?.Any(e => e.ReplyId == id)).GetValueOrDefault();
        }
    }
}
