using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ATSManagement.Controllers
{
    public class LegalStudiesDraftingController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public LegalStudiesDraftingController(AtsdbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // GET: CivilJustices
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblCivilJustices.Include(t => t.AssignedByNavigation).Include(t => t.AssignedToNavigation).Include(t => t.CaseType).Include(t => t.Dep).Include(t => t.Inist).Include(t => t.RequestedByNavigation);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: CivilJustices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblCivilJustices == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblCivilJustices
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.AssignedToNavigation)
                .Include(t => t.CaseType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblCivilJustice == null)
            {
                return NotFound();
            }

            return View(tblCivilJustice);
        }

        // GET: CivilJustices/Create
        public IActionResult Create()
        {
            ViewData["AssignedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["AssignedTo"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["CaseTypeId"] = new SelectList(_context.TblCivilJusticeCaseTypes, "CaseTypeId", "CaseTypeId");
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepId");
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId");
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId");
            return View();
        }

        // POST: CivilJustices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,RequestDetail,InistId,RequestedBy,CreatedDate,CreatedBy,DepId,CaseTypeId,AssignedBy,AssignedDate,DueDate,AssingmentRemark,AssignedTo,ProgressStatus,IsUpprovedByUser,IsUprovedByTeam,IsUprovedByDeputy,TopStatus")] TblCivilJustice tblCivilJustice)
        {
            if (ModelState.IsValid)
            {
                tblCivilJustice.RequestId = Guid.NewGuid();
                _context.Add(tblCivilJustice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJustice.AssignedBy);
            ViewData["AssignedTo"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJustice.AssignedTo);
            ViewData["CaseTypeId"] = new SelectList(_context.TblCivilJusticeCaseTypes, "CaseTypeId", "CaseTypeId", tblCivilJustice.CaseTypeId);
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepId", tblCivilJustice.DepId);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblCivilJustice.InistId);
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblCivilJustice.RequestedBy);
            return View(tblCivilJustice);
        }

        // GET: CivilJustices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblCivilJustices == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblCivilJustices.FindAsync(id);
            if (tblCivilJustice == null)
            {
                return NotFound();
            }
            ViewData["AssignedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJustice.AssignedBy);
            ViewData["AssignedTo"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJustice.AssignedTo);
            ViewData["CaseTypeId"] = new SelectList(_context.TblCivilJusticeCaseTypes, "CaseTypeId", "CaseTypeId", tblCivilJustice.CaseTypeId);
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepId", tblCivilJustice.DepId);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblCivilJustice.InistId);
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblCivilJustice.RequestedBy);
            return View(tblCivilJustice);
        }

        // POST: CivilJustices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RequestId,RequestDetail,InistId,RequestedBy,CreatedDate,CreatedBy,DepId,CaseTypeId,AssignedBy,AssignedDate,DueDate,AssingmentRemark,AssignedTo,ProgressStatus,IsUpprovedByUser,IsUprovedByTeam,IsUprovedByDeputy,TopStatus")] TblCivilJustice tblCivilJustice)
        {
            if (id != tblCivilJustice.RequestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCivilJustice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblCivilJusticeExists(tblCivilJustice.RequestId))
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
            ViewData["AssignedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJustice.AssignedBy);
            ViewData["AssignedTo"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblCivilJustice.AssignedTo);
            ViewData["CaseTypeId"] = new SelectList(_context.TblCivilJusticeCaseTypes, "CaseTypeId", "CaseTypeId", tblCivilJustice.CaseTypeId);
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepId", tblCivilJustice.DepId);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblCivilJustice.InistId);
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblCivilJustice.RequestedBy);
            return View(tblCivilJustice);
        }

        // GET: CivilJustices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblCivilJustices == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblCivilJustices
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.AssignedToNavigation)
                .Include(t => t.CaseType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblCivilJustice == null)
            {
                return NotFound();
            }

            return View(tblCivilJustice);
        }

        // POST: CivilJustices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblCivilJustices == null)
            {
                return Problem("Entity set 'AtsdbContext.TblCivilJustices'  is null.");
            }
            var tblCivilJustice = await _context.TblCivilJustices.FindAsync(id);
            if (tblCivilJustice != null)
            {
                _context.TblCivilJustices.Remove(tblCivilJustice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblCivilJusticeExists(Guid id)
        {
            return (_context.TblCivilJustices?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
    }
}
