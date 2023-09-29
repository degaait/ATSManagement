using ATSManagement.IModels;
using ATSManagement.Models;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ATSManagement.Controllers
{
    public class ExternalRequestsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        public ExternalRequestsController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mail)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
        }

        // GET: ExternalRequests
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblExternalRequests.Include(t => t.ExterUser).Include(t => t.Int).Include(s => s.ExternalRequestStatus);
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
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            model.RequestedDate = DateTime.Now;
            model.ExterUserId = userId;
            model.IntId = instName.InistId;

            return View(model);
        }

        // POST: ExternalRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CivilJusticeExternalRequestModel model)
        {

            try
            {
                TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "New" select items).FirstOrDefault();
                TblExternalRequest requests = new TblExternalRequest();
                requests.RequestDetail = model.RequestDetail;
                requests.ExterUserId = model.ExterUserId;
                requests.IntId = model.IntId;
                requests.RequestedDate = DateTime.Now;
                requests.ExternalRequestStatusId = status.ExternalRequestStatusId;
                _context.TblExternalRequests.Add(requests);

                int saved = await _context.SaveChangesAsync();
                if (saved > 1)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                return View(model);
            }

        }

        // GET: ExternalRequests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            if (id == null || _context.TblExternalRequests == null)
            {
                return NotFound();
            }

            var tblExternalRequest = await _context.TblExternalRequests.FindAsync(id);
            if (tblExternalRequest == null)
            {
                return NotFound();
            }
            model.ExterUserId = tblExternalRequest.ExterUserId;
            model.IntId = tblExternalRequest.IntId;
            model.RequestedDate = tblExternalRequest.RequestedDate;
            model.RequestId = tblExternalRequest.RequestId;
            model.RequestDetail = tblExternalRequest.RequestDetail;


            return View(model);
        }

        // POST: ExternalRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CivilJusticeExternalRequestModel model)
        {
            try
            {
                TblExternalRequest tblExternalRequest = await _context.TblExternalRequests.FindAsync(model.RequestId);
                if (tblExternalRequest == null)
                {
                    return NotFound();
                }
                tblExternalRequest.RequestDetail = model.RequestDetail;

                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblExternalRequestExists(model.RequestId))
                {
                    return NotFound();
                }
                else
                {
                    return View(model);
                }
            }
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
        public IActionResult AssignToDepartment(Guid id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            model.RequestedDate = DateTime.Now;
            model.ExterUserId = userId;
            model.IntId = instName.InistId;

            return View(model);
        }

    }
}
