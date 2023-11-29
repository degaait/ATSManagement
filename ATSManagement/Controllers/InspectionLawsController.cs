using System;
using System.Linq;
using ATSManagement.Models;
using ATSManagement.IModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    public class InspectionLawsController : Controller
    {
        private readonly AtsdbContext _context;

        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        private readonly IHttpContextAccessor _contextAccessor;

        public InspectionLawsController(AtsdbContext context, IHttpContextAccessor contextAccessor, INotyfService notyfService, IMailService mailService, ILogger<HomeController> logger)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
            _logger = logger;
            _notifyService = notyfService;
        }

        // GET: InspectionLaws
        public async Task<IActionResult> Index()
        {
              return _context.TblInspectionLaws != null ? 
                          View(await _context.TblInspectionLaws.ToListAsync()) :
                          Problem("Entity set 'AtsdbContext.TblInspectionLaws'  is null.");
        }

        // GET: InspectionLaws/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblInspectionLaws == null)
            {
                return NotFound();
            }

            var tblInspectionLaw = await _context.TblInspectionLaws
                .FirstOrDefaultAsync(m => m.LawId == id);
            if (tblInspectionLaw == null)
            {
                return NotFound();
            }

            return View(tblInspectionLaw);
        }

        // GET: InspectionLaws/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LawId,LawDescription,ReferenceArticle")] TblInspectionLaw tblInspectionLaw)
        {
            if (ModelState.IsValid)
            {
                tblInspectionLaw.LawId = Guid.NewGuid();
                _context.Add(tblInspectionLaw);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblInspectionLaw);
        }

        // GET: InspectionLaws/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblInspectionLaws == null)
            {
                return NotFound();
            }

            var tblInspectionLaw = await _context.TblInspectionLaws.FindAsync(id);
            if (tblInspectionLaw == null)
            {
                return NotFound();
            }
            return View(tblInspectionLaw);
        }

        // POST: InspectionLaws/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("LawId,LawDescription,ReferenceArticle")] TblInspectionLaw tblInspectionLaw)
        {
            if (id != tblInspectionLaw.LawId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblInspectionLaw);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblInspectionLawExists(tblInspectionLaw.LawId))
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
            return View(tblInspectionLaw);
        }

        // GET: InspectionLaws/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblInspectionLaws == null)
            {
                return NotFound();
            }

            var tblInspectionLaw = await _context.TblInspectionLaws
                .FirstOrDefaultAsync(m => m.LawId == id);
            if (tblInspectionLaw == null)
            {
                return NotFound();
            }

            return View(tblInspectionLaw);
        }

        // POST: InspectionLaws/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblInspectionLaws == null)
            {
                return Problem("Entity set 'AtsdbContext.TblInspectionLaws'  is null.");
            }
            var tblInspectionLaw = await _context.TblInspectionLaws.FindAsync(id);
            if (tblInspectionLaw != null)
            {
                _context.TblInspectionLaws.Remove(tblInspectionLaw);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblInspectionLawExists(Guid id)
        {
          return (_context.TblInspectionLaws?.Any(e => e.LawId == id)).GetValueOrDefault();
        }
    }
}
