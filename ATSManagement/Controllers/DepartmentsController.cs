using NToastNotify;
using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class DepartmentsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotyfService _notifyService;
        public DepartmentsController(AtsdbContext context, INotyfService notyfService, IHttpContextAccessor contextAccessor)
        {
            _notifyService = notyfService;
            _context = context;
            _contextAccessor = contextAccessor;
        }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepId,DepName,DepNameAmharic")] TblDepartment tblDepartment)
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DepId,DepName,DepNameAmharic")] TblDepartment tblDepartment)
        {
            
            if (id != tblDepartment.DepId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var deps = _context.TblDepartments.Find(tblDepartment.DepId);
                    deps.DepName = tblDepartment.DepName;
                    deps.DepNameAmharic = tblDepartment.DepNameAmharic;
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Department Successfully updated");
                    }
                    else
                    {
                        _notifyService.Error("Not successfully updated. Please try latter");
                        return View(tblDepartment);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblDepartmentExists(tblDepartment.DepId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _notifyService.Error("Not successfully updated. Please try latter");
                        return View(tblDepartment);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notifyService.Error("Invalid Model. Please try again latter");
                return View(tblDepartment);
            }
            
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
            try
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

               int deleted= await _context.SaveChangesAsync();
                if (deleted>0)
                {
                    _notifyService.Success("Successfully Removed");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notifyService.Error("Not successfull. Please try again");
                    return RedirectToAction(nameof(Delete), new { id =id});
                }
               
            }
            catch (Exception ex) 
            {
                _notifyService.Error(ex.Message+" happened. Not successfull. Please try again");
                return RedirectToAction(nameof(Delete), new { id = id });
               
            }

          
        }
        private bool TblDepartmentExists(Guid id)
        {
            return (_context.TblDepartments?.Any(e => e.DepId == id)).GetValueOrDefault();
        }
    }
}
