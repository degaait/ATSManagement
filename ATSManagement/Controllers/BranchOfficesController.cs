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
    public class BranchOfficesController : Controller
    {
        private readonly AtsdbContext _context;

        public BranchOfficesController(AtsdbContext context)
        {
            _context = context;
        }

        // GET: BranchOffices
        public async Task<IActionResult> Index()
        {
            return View(await _context.TblBranchOffices.ToListAsync());
        }

        // GET: BranchOffices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblBranchOffice = await _context.TblBranchOffices
                .FirstOrDefaultAsync(m => m.BranchId == id);
            if (tblBranchOffice == null)
            {
                return NotFound();
            }

            return View(tblBranchOffice);
        }

        // GET: BranchOffices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BranchOffices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BranchId,Name")] TblBranchOffice tblBranchOffice)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblBranchOffice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblBranchOffice);
        }

        // GET: BranchOffices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblBranchOffice = await _context.TblBranchOffices.FindAsync(id);
            if (tblBranchOffice == null)
            {
                return NotFound();
            }
            return View(tblBranchOffice);
        }

        // POST: BranchOffices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BranchId,Name")] TblBranchOffice tblBranchOffice)
        {
            if (id != tblBranchOffice.BranchId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblBranchOffice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblBranchOfficeExists(tblBranchOffice.BranchId))
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
            return View(tblBranchOffice);
        }

        // GET: BranchOffices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tblBranchOffice = await _context.TblBranchOffices
                .FirstOrDefaultAsync(m => m.BranchId == id);
            if (tblBranchOffice == null)
            {
                return NotFound();
            }

            return View(tblBranchOffice);
        }

        // POST: BranchOffices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tblBranchOffice = await _context.TblBranchOffices.FindAsync(id);
            if (tblBranchOffice != null)
            {
                _context.TblBranchOffices.Remove(tblBranchOffice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblBranchOfficeExists(int id)
        {
            return _context.TblBranchOffices.Any(e => e.BranchId == id);
        }
    }
}
