using ATSManagement.Models;
using ATSManagement.Security;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ATSManagement.Controllers
{
    public class InternalUsersController : Controller
    {
        private readonly AtsdbContext _context;

        public InternalUsersController(AtsdbContext context)
        {
            _context = context;
        }

        // GET: InternalUsers
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblInternalUsers.Include(t => t.Dep);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: InternalUsers/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblInternalUsers == null)
            {
                return NotFound();
            }

            var tblInternalUser = await _context.TblInternalUsers
                .Include(t => t.Dep)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (tblInternalUser == null)
            {
                return NotFound();
            }

            return View(tblInternalUser);
        }

        // GET: InternalUsers/Create
        public IActionResult Create()
        {
            UserModel user = new UserModel();
            user.IsActive = true;
            user.IsSuperAdmin = false;

            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName");
            return View(user);
        }

        // POST: InternalUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel userModel)
        {
            TblInternalUser tblInternalUser = new TblInternalUser();
            if (ModelState.IsValid)
            {
                tblInternalUser.UserId = Guid.NewGuid();
                tblInternalUser.IsSuperAdmin = userModel.IsSuperAdmin;
                if (userModel.specialRoles.ToString() == "IsDeputy")
                {
                    tblInternalUser.IsDeputy = true;
                }
                else
                {
                    tblInternalUser.IsDeputy = false;
                }
                if (userModel.specialRoles.ToString() == "IsDepartmentHead")
                {
                    tblInternalUser.IsDepartmentHead = true;
                }
                else
                {
                    tblInternalUser.IsDepartmentHead = false;
                }
                if (userModel.specialRoles.ToString() == "IsTeamLeader")
                {
                    tblInternalUser.IsTeamLeader = true;
                }
                else
                {
                    tblInternalUser.IsTeamLeader = false;
                }
                tblInternalUser.FirstName = userModel.FirstName;
                tblInternalUser.LastName = userModel.LastName;
                tblInternalUser.IsActive = userModel.IsActive;
                tblInternalUser.MidleName = userModel.MiddleName;
                tblInternalUser.PhoneNumber = userModel.PhoneNumber;
                tblInternalUser.UserName = userModel.UserName;
                tblInternalUser.Password = PawwordEncryption.EncryptPasswordBase64Strig(userModel.Password);
                tblInternalUser.EmailAddress = userModel.EmailAddress;
                tblInternalUser.DepId = userModel.DepId;

                _context.Add(tblInternalUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", userModel.DepId);
            return View(userModel);
        }

        // GET: InternalUsers/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            UserModel userModel = new UserModel();

            if (id == null || _context.TblInternalUsers == null)
            {
                return NotFound();
            }

            var tblInternalUser = await _context.TblInternalUsers.FindAsync(id);
            userModel.PhoneNumber = tblInternalUser.PhoneNumber;
            userModel.UserName = tblInternalUser.UserName;
            userModel.LastName = tblInternalUser.LastName;
            userModel.MiddleName = tblInternalUser.MidleName;
            userModel.EmailAddress = tblInternalUser.EmailAddress;
            userModel.FirstName = tblInternalUser.FirstName;

            if (tblInternalUser.IsSuperAdmin == true)
            {
                userModel.IsSuperAdmin = true;
            }
            else
            {
                userModel.IsSuperAdmin = false;
            }
            if (tblInternalUser.IsActive == true)
            {
                userModel.IsActive = true;
            }
            else
            {
                userModel.IsActive = false;
            }
            userModel.UserId = tblInternalUser.UserId;
            if (tblInternalUser.IsDeputy == true)
            {
                ViewBag.IsDeputy = true;
            }
            else
            {
                ViewBag.IsDeputy = false;
            }
            if (tblInternalUser.IsDepartmentHead == true)
            {
                ViewBag.IsDepartmentHead = true;
            }
            else
            {
            }
            if (tblInternalUser.IsTeamLeader == true)
            {
                ViewBag.IsTeamLeader = true;
            }
            else
            {
                ViewBag.IsTeamLeader = false;
            }
            if (tblInternalUser == null)
            {
                return NotFound();
            }
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", tblInternalUser.DepId);
            return View(userModel);
        }

        // POST: InternalUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserModel userModel)
        {
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userModel.UserId);
            if (tblInternalUser == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    tblInternalUser.DepId = userModel.DepId;
                    tblInternalUser.FirstName = userModel.FirstName;
                    tblInternalUser.LastName = userModel.LastName;
                    tblInternalUser.MidleName = userModel.MiddleName;
                    tblInternalUser.PhoneNumber = userModel.PhoneNumber;
                    tblInternalUser.UserName = userModel.UserName;
                    tblInternalUser.EmailAddress = userModel.EmailAddress;
                    tblInternalUser.IsActive = userModel.IsActive;
                    tblInternalUser.IsSuperAdmin = userModel.IsSuperAdmin;
                    if (userModel.specialRoles.ToString() == "IsDeputy")
                    {
                        tblInternalUser.IsDeputy = true;
                    }
                    else
                    {
                        tblInternalUser.IsDeputy = false;
                    }
                    if (userModel.specialRoles.ToString() == "IsDepartmentHead")
                    {
                        tblInternalUser.IsDepartmentHead = true;
                    }
                    else
                    {
                        tblInternalUser.IsDepartmentHead = false;
                    }
                    if (userModel.specialRoles.ToString() == "IsTeamLeader")
                    {
                        tblInternalUser.IsTeamLeader = true;
                    }
                    else
                    {
                        tblInternalUser.IsTeamLeader = false;
                    }
                    _context.Update(tblInternalUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    ViewBag.error = ex.Message;
                    if (!TblInternalUserExists(tblInternalUser.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        return View(userModel);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", userModel.DepId);
            return View(userModel);
        }

        // GET: InternalUsers/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblInternalUsers == null)
            {
                return NotFound();
            }

            var tblInternalUser = await _context.TblInternalUsers
                .Include(t => t.Dep)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (tblInternalUser == null)
            {
                return NotFound();
            }

            return View(tblInternalUser);
        }

        // POST: InternalUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblInternalUsers == null)
            {
                return Problem("Entity set 'AtsdbContext.TblInternalUsers'  is null.");
            }
            var tblInternalUser = await _context.TblInternalUsers.FindAsync(id);
            if (tblInternalUser != null)
            {
                _context.TblInternalUsers.Remove(tblInternalUser);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblInternalUserExists(Guid id)
        {
            return (_context.TblInternalUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
