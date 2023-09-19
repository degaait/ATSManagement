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
    public class DepartmentsController : Controller
    {
        private readonly AtsdbContext _context;

        public DepartmentsController(AtsdbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
              return _context.TblDepartments != null ? 
                          View(await _context.TblDepartments.ToListAsync()) :
                          Problem("Entity set 'AtsdbContext.TblDepartments'  is null.");
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblDepartments == null)
            {
                return NotFound();
            }

            var tblDepartment = await _context.TblDepartments
                .FirstOrDefaultAsync(m => m.DepId == id);
            if (tblDepartment == null)
            {
                return NotFound();
            }

            return View(tblDepartment);
        }

        // GET: Departments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepId,DepName")] TblDepartment tblDepartment)
        {
            if (ModelState.IsValid)
            {
                tblDepartment.DepId = Guid.NewGuid();
                _context.Add(tblDepartment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblDepartment);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblDepartments == null)
            {
                return NotFound();
            }

            var tblDepartment = await _context.TblDepartments.FindAsync(id);
            if (tblDepartment == null)
            {
                return NotFound();
            }
            return View(tblDepartment);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DepId,DepName")] TblDepartment tblDepartment)
        {
            if (id != tblDepartment.DepId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblDepartment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblDepartmentExists(tblDepartment.DepId))
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
            return View(tblDepartment);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblDepartments == null)
            {
                return NotFound();
            }

            var tblDepartment = await _context.TblDepartments
                .FirstOrDefaultAsync(m => m.DepId == id);
            if (tblDepartment == null)
            {
                return NotFound();
            }

            return View(tblDepartment);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblDepartments == null)
            {
                return Problem("Entity set 'AtsdbContext.TblDepartments'  is null.");
            }
            var tblDepartment = await _context.TblDepartments.FindAsync(id);
            if (tblDepartment != null)
            {
                _context.TblDepartments.Remove(tblDepartment);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblDepartmentExists(Guid id)
        {
          return (_context.TblDepartments?.Any(e => e.DepId == id)).GetValueOrDefault();
        }
    }
}
