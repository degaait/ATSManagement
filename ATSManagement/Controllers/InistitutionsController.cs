using NToastNotify;
using ATSManagement.Models;
using ATSManagement.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class InistitutionsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        public InistitutionsController(AtsdbContext context, INotyfService toastNotification)
        {
            _notifyService = toastNotification;
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InistId,Name,Description,NameAmharic")] TblInistitution tblInistitution)
        {
            var isexist=_context.TblInistitutions.Where(s=>s.Name==tblInistitution.Name).FirstOrDefault();
            if (isexist != null)
            {
                _notifyService.Warning(tblInistitution.Name + " already exists. Please try again");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    tblInistitution.InistId = Guid.NewGuid();
                    _context.Add(tblInistitution);
                    int saved = await _context.SaveChangesAsync();
                    if (saved > 0)
                    {
                        _notifyService.Success("Successfully Added");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Not added. Please try again");
                        return View(tblInistitution);
                    }
                }
                else
                {
                    _notifyService.Error("Please try again latter");
                    return View(tblInistitution);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message+". Not added. Please try again");
                return View(tblInistitution);
            }
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
        public async Task<IActionResult> Edit(Guid id, [Bind("InistId,Name,Description,NameAmharic")] TblInistitution tblInistitution)
        {
            if (id != tblInistitution.InistId)
            {
                return NotFound();
            }
            var isexist = _context.TblInistitutions.Where(s => s.Name == tblInistitution.Name).FirstOrDefault();
            if (isexist != null)
            {
                _notifyService.Warning(tblInistitution.Name + " already exists. Please try again");
                return View(tblInistitution);
            }
                try
                {
                    _context.Update(tblInistitution);
                   int updated= await _context.SaveChangesAsync();
                if (updated>0)
                {
                    _notifyService.Success("Successfully updated");
                    return RedirectToAction("Index");

                }
                else
                {
                    _notifyService.Error("Not updated successfully. Please try again");
                    return View(tblInistitution);
                }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TblInistitutionExists(tblInistitution.InistId))
                    {
                    _notifyService.Error(ex.Message+". Not updated successfully. Please try again");
                    return View(tblInistitution);
                }
                    else
                    {
                    _notifyService.Error(ex.Message + ". Not updated successfully. Please try again");
                    return View(tblInistitution);
                }
                }
            
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

           int deleted= await _context.SaveChangesAsync();
            if (deleted > 0)
            {
                _notifyService.Success("Successfully Deleted.");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notifyService.Error("Not successfull. Please try again");
                return RedirectToAction(nameof(Index));
            }
           
        }

        private bool TblInistitutionExists(Guid id)
        {
            return (_context.TblInistitutions?.Any(e => e.InistId == id)).GetValueOrDefault();
        }
    }
}
