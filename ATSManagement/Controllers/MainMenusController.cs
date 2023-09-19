using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ATSManagement.Models;

namespace ATSManagement.Controllers
{
    public class MainMenusController : Controller
    {
        private readonly AtsdbContext _context;

        public MainMenusController(AtsdbContext context)
        {
            _context = context;
        }

        // GET: MainMenus
        public async Task<IActionResult> Index()
        {
              return _context.TblMainMenus != null ? 
                          View(await _context.TblMainMenus.ToListAsync()) :
                          Problem("Entity set 'AtsdbContext.TblMainMenus'  is null.");
        }

        // GET: MainMenus/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblMainMenus == null)
            {
                return NotFound();
            }

            var tblMainMenu = await _context.TblMainMenus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (tblMainMenu == null)
            {
                return NotFound();
            }

            return View(tblMainMenu);
        }

        // GET: MainMenus/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MainMenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MenuId,MenuName,MenuDescription")] TblMainMenu tblMainMenu)
        {
            if (ModelState.IsValid)
            {
                tblMainMenu.MenuId = Guid.NewGuid();
                _context.Add(tblMainMenu);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblMainMenu);
        }

        // GET: MainMenus/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblMainMenus == null)
            {
                return NotFound();
            }

            var tblMainMenu = await _context.TblMainMenus.FindAsync(id);
            if (tblMainMenu == null)
            {
                return NotFound();
            }
            return View(tblMainMenu);
        }

        // POST: MainMenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MenuId,MenuName,MenuDescription")] TblMainMenu tblMainMenu)
        {
            if (id != tblMainMenu.MenuId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblMainMenu);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblMainMenuExists(tblMainMenu.MenuId))
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
            return View(tblMainMenu);
        }

        // GET: MainMenus/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblMainMenus == null)
            {
                return NotFound();
            }

            var tblMainMenu = await _context.TblMainMenus
                .FirstOrDefaultAsync(m => m.MenuId == id);
            if (tblMainMenu == null)
            {
                return NotFound();
            }

            return View(tblMainMenu);
        }

        // POST: MainMenus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblMainMenus == null)
            {
                return Problem("Entity set 'AtsdbContext.TblMainMenus'  is null.");
            }
            var tblMainMenu = await _context.TblMainMenus.FindAsync(id);
            if (tblMainMenu != null)
            {
                _context.TblMainMenus.Remove(tblMainMenu);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblMainMenuExists(Guid id)
        {
          return (_context.TblMainMenus?.Any(e => e.MenuId == id)).GetValueOrDefault();
        }
    }
}
