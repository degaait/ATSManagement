using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class SubmenusController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotyfService _notifyService;
        public SubmenusController(AtsdbContext context, IHttpContextAccessor httpContext, INotyfService notyfService)
        {
            _context = context;
            _contextAccessor = httpContext;
            _notifyService = notyfService;
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
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            SubmenuModel model = new SubmenuModel();
            if (cultur == "am")
            {
                model.Departments = _context.TblDepartments.Select(s => new SelectListItem
                {
                    Text = s.DepNameAmharic,
                    Value = s.DepId.ToString()

                }).ToList();
                model.MainMenus = _context.TblMainMenus.Select(s => new SelectListItem
                {
                    Value = s.MenuId.ToString(),
                    Text = s.MenuNameAmharic
                }).ToList();
            }
            else
            {
                model.Departments = _context.TblDepartments.Select(s => new SelectListItem
                {
                    Text = s.DepName,
                    Value = s.DepId.ToString()
                }).ToList();
                model.MainMenus = _context.TblMainMenus.Select(s => new SelectListItem
                {
                    Value = s.MenuId.ToString(),
                    Text = s.MenuName
                }).ToList();
            }

            return View(model);
        }

        // POST: Submenus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubmenuModel model)
        {
            try
            {
                TblSubmenu tblSubmenu = new TblSubmenu();
                tblSubmenu.Submenu = model.SubMenuName;
                tblSubmenu.MenuId = model.MainMenuId;
                tblSubmenu.Controller = model.ControllerName;
                tblSubmenu.DepId = model.DepId;
                tblSubmenu.SubmenuAmharic = model.SubmenuAmharic;
                tblSubmenu.Action = model.ActionName;
                tblSubmenu.ForSuperAdmin = model.forSuperAdmin;
                tblSubmenu.ForDefaulUser = model.forDefaulUser;
                tblSubmenu.ForTeamLeader = model.forTeamLeader;
                tblSubmenu.ForDeputy = model.forDeputy;
                tblSubmenu.ForDepHead = model.forDepHead;
                tblSubmenu.ForSecretary=model.forSecretary;
                tblSubmenu.ForBranchOfficer = model.forBranchOfficer;
                tblSubmenu.ForInternalUser = model.forInternalUser;
                tblSubmenu.IsActive=true;
                _context.TblSubmenus.Add(tblSubmenu);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Data successfully added");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notifyService.Error("Operation isn't successfully added. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message + " happened. Please try again");
                return View(model);
            }

        }

        // GET: Submenus/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            SubmenuModel model = new SubmenuModel();
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();

            if (id == null || _context.TblSubmenus == null)
            {
                return NotFound();
            }

            var tblSubmenu = await _context.TblSubmenus.FindAsync(id);
            if (tblSubmenu == null)
            {
                return NotFound();
            }
            model.SubMenuName = tblSubmenu.Submenu;
            model.MainMenuId = tblSubmenu.MenuId;
            model.ControllerName = tblSubmenu.Controller;
            model.DepId = tblSubmenu.DepId;
            model.SubmenuAmharic = tblSubmenu.SubmenuAmharic;
            model.ActionName = tblSubmenu.Action;
            model.forSuperAdmin = tblSubmenu.ForSuperAdmin ?? false;
            model.forDefaulUser = tblSubmenu.ForDefaulUser ?? false;
            model.forTeamLeader = tblSubmenu.ForTeamLeader ?? false;
            model.forDeputy = tblSubmenu.ForDeputy ?? false;
            model.forDepHead = tblSubmenu.ForDepHead ?? false;
            model.forBranchOfficer=tblSubmenu.ForBranchOfficer ?? false;
            model.forSecretary=tblSubmenu.ForSecretary ?? false;
            model.forInternalUser = tblSubmenu.ForInternalUser ?? false;
            model.DepId = tblSubmenu.DepId;
            model.Id = tblSubmenu.Id;
            if (cultur == "am")
            {
                model.Departments = _context.TblDepartments.Select(s => new SelectListItem
                {
                    Text = s.DepNameAmharic,
                    Value = s.DepId.ToString()

                }).ToList();
                model.MainMenus = _context.TblMainMenus.Select(s => new SelectListItem
                {
                    Value = s.MenuId.ToString(),
                    Text = s.MenuNameAmharic
                }).ToList();
            }
            else
            {
                model.Departments = _context.TblDepartments.Select(s => new SelectListItem
                {
                    Text = s.DepName,
                    Value = s.DepId.ToString()
                }).ToList();
                model.MainMenus = _context.TblMainMenus.Select(s => new SelectListItem
                {
                    Value = s.MenuId.ToString(),
                    Text = s.MenuName
                }).ToList();
            }

            return View(model);
        }

        // POST: Submenus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubmenuModel model)
        {
            try
            {
                TblSubmenu tblSubmenu = _context.TblSubmenus.Find(model.Id);
                var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();

                if (tblSubmenu == null)
                {
                    return NotFound();
                }
                tblSubmenu.Submenu = model.SubMenuName;
                tblSubmenu.MenuId = model.MainMenuId;
                tblSubmenu.Controller = model.ControllerName;
                tblSubmenu.DepId = model.DepId;
                tblSubmenu.SubmenuAmharic = model.SubmenuAmharic;
                tblSubmenu.Action = model.ActionName;
                tblSubmenu.ForSuperAdmin = model.forSuperAdmin;
                tblSubmenu.ForDefaulUser = model.forDefaulUser;
                tblSubmenu.ForTeamLeader = model.forTeamLeader;
                tblSubmenu.ForDeputy = model.forDeputy;
                tblSubmenu.ForDepHead = model.forDepHead;
                tblSubmenu.ForSecretary = model.forSecretary;
                tblSubmenu.ForBranchOfficer = model.forBranchOfficer;
                tblSubmenu.ForInternalUser = model.forInternalUser;
                tblSubmenu.IsActive = true;
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    _notifyService.Success("Data uppdated successfully");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notifyService.Error("Data isn't uppdated successfully. Please try again");
                    if (cultur == "am")
                    {
                        model.Departments = _context.TblDepartments.Select(s => new SelectListItem
                        {
                            Text = s.DepNameAmharic,
                            Value = s.DepId.ToString()

                        }).ToList();
                        model.MainMenus = _context.TblMainMenus.Select(s => new SelectListItem
                        {
                            Value = s.MenuId.ToString(),
                            Text = s.MenuNameAmharic
                        }).ToList();
                    }
                    else
                    {
                        model.Departments = _context.TblDepartments.Select(s => new SelectListItem
                        {
                            Text = s.DepName,
                            Value = s.DepId.ToString()
                        }).ToList();
                        model.MainMenus = _context.TblMainMenus.Select(s => new SelectListItem
                        {
                            Value = s.MenuId.ToString(),
                            Text = s.MenuName
                        }).ToList();
                    }
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
                _notifyService.Error(ex.Message + " happened. Data isn't uppdated successfully. Please try again");
                if (cultur == "am")
                {
                    model.Departments = _context.TblDepartments.Select(s => new SelectListItem
                    {
                        Text = s.DepNameAmharic,
                        Value = s.DepId.ToString()

                    }).ToList();
                    model.MainMenus = _context.TblMainMenus.Select(s => new SelectListItem
                    {
                        Value = s.MenuId.ToString(),
                        Text = s.MenuNameAmharic
                    }).ToList();
                }
                else
                {
                    model.Departments = _context.TblDepartments.Select(s => new SelectListItem
                    {
                        Text = s.DepName,
                        Value = s.DepId.ToString()
                    }).ToList();
                    model.MainMenus = _context.TblMainMenus.Select(s => new SelectListItem
                    {
                        Value = s.MenuId.ToString(),
                        Text = s.MenuName
                    }).ToList();
                }
                return View(model);
            }
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

            int deleted = await _context.SaveChangesAsync();
            if (deleted > 0)
            {
                _notifyService.Error("Operation isn't successfull. Please try again");
                var tblSubmenus = await _context.TblSubmenus
               .Include(t => t.Dep)
               .Include(t => t.Menu)
               .Include(t => t.Role)
               .FirstOrDefaultAsync(m => m.Id == id);
                return View(tblSubmenus);
            }
            else
            {
                _notifyService.Success("Operation is successfull");
                return RedirectToAction(nameof(Index));
            }

        }

        private bool TblSubmenuExists(Guid id)
        {
            return (_context.TblSubmenus?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
