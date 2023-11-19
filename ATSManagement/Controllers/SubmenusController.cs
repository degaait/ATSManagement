using NToastNotify;
using ATSManagement.Models;
using ATSManagement.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class SubmenusController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IToastNotification _toastNotification;
        public SubmenusController(AtsdbContext context, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _context = context;
        }

        // GET: Submenus
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblSubmenus.Include(t => t.Dep).Include(t => t.Menu).Include(t => t.Role);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: Submenus/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblSubmenus == null)
            {
                return NotFound();
            }

            var tblSubmenu = await _context.TblSubmenus
                .Include(t => t.Dep)
                .Include(t => t.Menu)
                .Include(t => t.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblSubmenu == null)
            {
                return NotFound();
            }

            return View(tblSubmenu);
        }

        // GET: Submenus/Create
        public IActionResult Create()
        {
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName");
            ViewData["MenuId"] = new SelectList(_context.TblMainMenus, "MenuId", "MenuName");
            ViewData["RoleId"] = new SelectList(_context.TblRoles, "RoleId", "RoleId");
            return View();
        }

        // POST: Submenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Submenu,Controller,Action,RoleId,MenuId,IsActive,IsDeleted,DepId")] TblSubmenu tblSubmenu)
        {
            if (ModelState.IsValid)
            {
                tblSubmenu.Id = Guid.NewGuid();
                _context.Add(tblSubmenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", tblSubmenu.DepId);
            ViewData["MenuId"] = new SelectList(_context.TblMainMenus, "MenuId", "MenuName", tblSubmenu.MenuId);
            ViewData["RoleId"] = new SelectList(_context.TblRoles, "RoleId", "RoleId", tblSubmenu.RoleId);
            return View(tblSubmenu);
        }

        // GET: Submenus/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblSubmenus == null)
            {
                return NotFound();
            }

            var tblSubmenu = await _context.TblSubmenus.FindAsync(id);
            if (tblSubmenu == null)
            {
                return NotFound();
            }
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", tblSubmenu.DepId);
            ViewData["MenuId"] = new SelectList(_context.TblMainMenus, "MenuId", "MenuName", tblSubmenu.MenuId);
            ViewData["RoleId"] = new SelectList(_context.TblRoles, "RoleId", "RoleId", tblSubmenu.RoleId);
            return View(tblSubmenu);
        }

        // POST: Submenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Submenu,Controller,Action,RoleId,MenuId,IsActive,IsDeleted,DepId")] TblSubmenu tblSubmenu)
        {
            if (id != tblSubmenu.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblSubmenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblSubmenuExists(tblSubmenu.Id))
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
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", tblSubmenu.DepId);
            ViewData["MenuId"] = new SelectList(_context.TblMainMenus, "MenuId", "MenuName", tblSubmenu.MenuId);
            ViewData["RoleId"] = new SelectList(_context.TblRoles, "RoleId", "RoleId", tblSubmenu.RoleId);
            return View(tblSubmenu);
        }

        // GET: Submenus/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblSubmenus == null)
            {
                return NotFound();
            }

            var tblSubmenu = await _context.TblSubmenus
                .Include(t => t.Dep)
                .Include(t => t.Menu)
                .Include(t => t.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblSubmenu == null)
            {
                return NotFound();
            }

            return View(tblSubmenu);
        }

        // POST: Submenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblSubmenus == null)
            {
                return Problem("Entity set 'AtsdbContext.TblSubmenus'  is null.");
            }
            var tblSubmenu = await _context.TblSubmenus.FindAsync(id);
            if (tblSubmenu != null)
            {
                _context.TblSubmenus.Remove(tblSubmenu);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblSubmenuExists(Guid id)
        {
            return (_context.TblSubmenus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
