using NToastNotify;
using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.Security;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class ExternalUsersController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IToastNotification _toastNotification;
        public ExternalUsersController(AtsdbContext context, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
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
            ExternalUser model = new ExternalUser();
            model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.InistId.ToString(),
            }).ToList();
            return View(model);
        }

        // POST: ExternalUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExternalUser model)
        {
            try
            {
                TblExternalUser tblExternalUser = new TblExternalUser();
                tblExternalUser.InistId = model.InistId;
                tblExternalUser.FirstName = model.FirstName;
                tblExternalUser.LastName = model.LastName;
                tblExternalUser.MiddleName = model.MiddleName;
                tblExternalUser.UserName = model.UserName;
                tblExternalUser.Email = model.EmailAddress;
                tblExternalUser.PhoneNumber = model.PhoneNumber;
                tblExternalUser.IsActive = model.IsActive;
                tblExternalUser.Password = PawwordEncryption.EncryptPasswordBase64Strig(model.Password);
                _context.Add(tblExternalUser);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.InistId.ToString(),
                    }).ToList();
                    return View(model);
                }
            }
            catch (Exception)
            {
                model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.InistId.ToString(),
                }).ToList();
                return View(model);
            }
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
            externalUser.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.InistId.ToString(),
            }).ToList();
            externalUser.InistId = tblExternalUser.InistId;
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
            return View(externalUser);
        }

        // POST: ExternalUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ExternalUser model)
        {
            if (!TblExternalUserExists(model.ExterUserId))
            {
                return NotFound();
            }
            try
            {
                TblExternalUser tblExternalUser = await _context.TblExternalUsers.FindAsync(model.ExterUserId);
                tblExternalUser.Email = model.EmailAddress;
                tblExternalUser.FirstName = model.FirstName;
                tblExternalUser.LastName = model.LastName;
                tblExternalUser.MiddleName = model.MiddleName;
                tblExternalUser.PhoneNumber = model.PhoneNumber;
                tblExternalUser.UserName = model.UserName;
                tblExternalUser.IsActive = model.IsActive;
                tblExternalUser.InistId = model.InistId;
                tblExternalUser.Password = PawwordEncryption.EncryptPasswordBase64Strig(model.Password);
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
                    {
                        Text = t.Name,
                        Value = t.InistId.ToString(),
                    }).ToList();
                    return View(model);


                }
            }
            catch (Exception)
            {

                model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.InistId.ToString(),
                }).ToList();
                return View(model);
            }

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
