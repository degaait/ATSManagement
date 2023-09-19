using ATSManagement.Models;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ATSManagement.Controllers
{
    public class ExternalUsersController : Controller
    {
        private readonly AtsdbContext _context;

        public ExternalUsersController(AtsdbContext context)
        {
            _context = context;
        }

        // GET: ExternalUsers
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblExternalUsers.Include(t => t.Inist);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: ExternalUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblExternalUsers == null)
            {
                return NotFound();
            }

            var tblExternalUser = await _context.TblExternalUsers
                .Include(t => t.Inist)
                .FirstOrDefaultAsync(m => m.ExterUserId == id);
            if (tblExternalUser == null)
            {
                return NotFound();
            }

            return View(tblExternalUser);
        }

        // GET: ExternalUsers/Create
        public IActionResult Create()
        {
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "Name");
            return View();
        }

        // POST: ExternalUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExterUserId,FirstName,MiddleName,LastName,Email,PhoneNumber,UserName,Password,InistId")] TblExternalUser tblExternalUser)
        {
            if (ModelState.IsValid)
            {
                tblExternalUser.ExterUserId = Guid.NewGuid();
                _context.Add(tblExternalUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblExternalUser.InistId);
            return View(tblExternalUser);
        }

        // GET: ExternalUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            ExternalUser externalUser = new ExternalUser();
            if (id == null || _context.TblExternalUsers == null)
            {
                return NotFound();
            }
            var tblExternalUser = await _context.TblExternalUsers.FindAsync(id);
            externalUser.ExterUserId = tblExternalUser.ExterUserId;
            externalUser.UserName = tblExternalUser.UserName;
            externalUser.FirstName = tblExternalUser.FirstName;
            externalUser.MiddleName = tblExternalUser.MiddleName;
            externalUser.LastName = tblExternalUser.LastName;
            externalUser.PhoneNumber = tblExternalUser.PhoneNumber;
            if (tblExternalUser.IsActive == true)
            {
                externalUser.IsActive = true;
            }
            else
            {
                externalUser.IsActive = false;
            }

            if (tblExternalUser == null)
            {
                return NotFound();
            }
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "Name", tblExternalUser.InistId);
            return View(externalUser);
        }

        // POST: ExternalUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ExterUserId,FirstName,MiddleName,LastName,Email,PhoneNumber,UserName,Password,InistId")] TblExternalUser tblExternalUser)
        {
            if (id != tblExternalUser.ExterUserId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblExternalUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblExternalUserExists(tblExternalUser.ExterUserId))
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
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "Name", tblExternalUser.InistId);
            return View(tblExternalUser);
        }

        // GET: ExternalUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblExternalUsers == null)
            {
                return NotFound();
            }

            var tblExternalUser = await _context.TblExternalUsers
                .Include(t => t.Inist)
                .FirstOrDefaultAsync(m => m.ExterUserId == id);
            if (tblExternalUser == null)
            {
                return NotFound();
            }

            return View(tblExternalUser);
        }

        // POST: ExternalUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblExternalUsers == null)
            {
                return Problem("Entity set 'AtsdbContext.TblExternalUsers'  is null.");
            }
            var tblExternalUser = await _context.TblExternalUsers.FindAsync(id);
            if (tblExternalUser != null)
            {
                _context.TblExternalUsers.Remove(tblExternalUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblExternalUserExists(Guid id)
        {
            return (_context.TblExternalUsers?.Any(e => e.ExterUserId == id)).GetValueOrDefault();
        }
    }
}
