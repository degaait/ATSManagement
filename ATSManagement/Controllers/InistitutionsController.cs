using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ATSManagement.Controllers
{
    public class InistitutionsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IToastNotification _toastNotification;
        public InistitutionsController(AtsdbContext context, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _context = context;
        }

        // GET: Inistitutions
        public async Task<IActionResult> Index()
        {
            return _context.TblInistitutions != null ?
                        View(await _context.TblInistitutions.ToListAsync()) :
                        Problem("Entity set 'AtsdbContext.TblInistitutions'  is null.");
        }

        // GET: Inistitutions/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblInistitutions == null)
            {
                return NotFound();
            }

            var tblInistitution = await _context.TblInistitutions
                .FirstOrDefaultAsync(m => m.InistId == id);
            if (tblInistitution == null)
            {
                return NotFound();
            }

            return View(tblInistitution);
        }

        // GET: Inistitutions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Inistitutions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InistId,Name,Description")] TblInistitution tblInistitution)
        {
            if (ModelState.IsValid)
            {
                tblInistitution.InistId = Guid.NewGuid();
                _context.Add(tblInistitution);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblInistitution);
        }

        // GET: Inistitutions/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblInistitutions == null)
            {
                return NotFound();
            }

            var tblInistitution = await _context.TblInistitutions.FindAsync(id);
            if (tblInistitution == null)
            {
                return NotFound();
            }
            return View(tblInistitution);
        }

        // POST: Inistitutions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("InistId,Name,Description")] TblInistitution tblInistitution)
        {
            if (id != tblInistitution.InistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblInistitution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblInistitutionExists(tblInistitution.InistId))
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
            return View(tblInistitution);
        }

        // GET: Inistitutions/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblInistitutions == null)
            {
                return NotFound();
            }

            var tblInistitution = await _context.TblInistitutions
                .FirstOrDefaultAsync(m => m.InistId == id);
            if (tblInistitution == null)
            {
                return NotFound();
            }

            return View(tblInistitution);
        }

        // POST: Inistitutions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblInistitutions == null)
            {
                return Problem("Entity set 'AtsdbContext.TblInistitutions'  is null.");
            }
            var tblInistitution = await _context.TblInistitutions.FindAsync(id);
            if (tblInistitution != null)
            {
                _context.TblInistitutions.Remove(tblInistitution);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblInistitutionExists(Guid id)
        {
            return (_context.TblInistitutions?.Any(e => e.InistId == id)).GetValueOrDefault();
        }
    }
}
