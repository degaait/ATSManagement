namespace ATSManagement.Controllers
{
    using NToastNotify;
    using ATSManagement.Models;
    using ATSManagement.Filters;
    using ATSManagement.Security;
    using ATSManagement.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using AspNetCoreHero.ToastNotification.Abstractions;

    [CheckSessionIsAvailable]
    public class InternalUsersController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        public InternalUsersController(AtsdbContext context, INotyfService notyfService)
        {
            _notifyService = notyfService;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblInternalUsers.Include(t => t.Dep).Include(s=>s.Team);
            return View(await atsdbContext.ToListAsync());
        }     
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
        public IActionResult Create()
        {
            UserModel user = new UserModel();
            user.Departments = _context.TblDepartments.Select(s => new SelectListItem
            {
                Value = s.DepId.ToString(),
                Text=s.DepName
            }).ToList();
            user.IsActive = true;
            user.IsSuperAdmin = false;
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName");
            return View(user);
        }        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserModel userModel)
        {
            TblInternalUser tblInternalUser = new TblInternalUser();
            try
            {
                if (userModel.IsSuperAdmin== false || userModel.specialRoles.ToString() != "IsDeputy")
                {
                    if (userModel.DepId==null)
                    {
                        _notifyService.Information("Department is required. Please Select and try again.");
                        userModel.Departments = _context.TblDepartments.Select(s => new SelectListItem
                        {
                            Text = s.DepName,
                            Value = s.DepId.ToString(),
                        }).ToList();
                        return View(userModel);
                    }
                }
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
                    if (userModel.specialRoles.ToString()== "DefaultUser")
                    {
                        tblInternalUser.IsDefaultUser = true;
                    }
                    else
                    {
                        tblInternalUser.IsDefaultUser = false;
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
                    if (userModel.TeamID.ToString() != "00000000-0000-0000-0000-000000000000"|| userModel.TeamID.ToString()!=null)
                    {
                        tblInternalUser.TeamId = userModel.TeamID;
                    }
                    _context.Add(tblInternalUser);
                    int saved = await _context.SaveChangesAsync();
                   
                    if (saved > 0)
                    {
                        TblTeam tblTeam = _context.TblTeams.Find(userModel.TeamID);
                        tblTeam.TeamLeaderId = tblInternalUser.UserId;
                        await _context.SaveChangesAsync();
                        _notifyService.Success("User created successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("User isn't created succefully. Please try again");
                        userModel.Departments = _context.TblDepartments.Select(s => new SelectListItem
                        {
                            Text = s.DepName,
                            Value = s.DepId.ToString(),
                        }).ToList();
                        return View(userModel);
                    }
                }
                else
                {
                    _notifyService.Error("User isn't created succefully. Please fill all neccessary fields and try again");
                    userModel.Departments = _context.TblDepartments.Select(s => new SelectListItem
                    {
                        Text = s.DepName,
                        Value = s.DepId.ToString(),
                    }).ToList();
                    return View(userModel);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error:{ex.Message} happened. Please try again");
                userModel.Departments = _context.TblDepartments.Select(s => new SelectListItem
                {
                    Text = s.DepName,
                    Value = s.DepId.ToString(),
                }).ToList();
                return View(userModel);
            }              
        }
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
            if (tblInternalUser.IsDefaultUser==true)
            {
                ViewBag.IsDefaultUser = true;
            }
            else
            {
                ViewBag.IsDefaultUser=false;
            }
            if (tblInternalUser == null)
            {
                return NotFound();
            }
            if (tblInternalUser.DepId!=null)
            {
                if (tblInternalUser.TeamId!=null)
                {
                    ViewBag.Teams = new SelectList(_context.TblTeams.Where(s => s.DepId == tblInternalUser.DepId), "TeamId", "TeamName", tblInternalUser.TeamId);
                }
                else
                {
                    SelectList sle = new SelectList(new List<SelectListItem>
                                    {
                                        new SelectListItem { Selected = true, Text = "--Select--", Value = "00000000-0000-0000-0000-000000000000"}
                                    }, "Value", "Text", 1);
                    ViewBag.Teams = sle;
                }
               
            }
            else
            {
                SelectList sle= new SelectList(new List<SelectListItem>
                                    {
                                        new SelectListItem { Selected = true, Text = "--Select--", Value = "00000000-0000-0000-0000-000000000000"}
                                    }, "Value", "Text", 1);
                ViewBag.Teams = sle;
            }            
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", tblInternalUser.DepId);
            return View(userModel);
        }
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
                    if (userModel.IsSuperAdmin == false || userModel.specialRoles.ToString() != "IsDeputy")
                    {
                        if (userModel.DepId == null)
                        {
                            _notifyService.Information("Department is required. Please Select and try again.");
                            userModel.Departments = _context.TblDepartments.Select(s => new SelectListItem
                            {
                                Text = s.DepName,
                                Value = s.DepId.ToString(),
                            }).ToList();
                            return View(userModel);
                        }
                    }
                    tblInternalUser.DepId = userModel.DepId;
                    tblInternalUser.FirstName = userModel.FirstName;
                    tblInternalUser.LastName = userModel.LastName;
                    tblInternalUser.MidleName = userModel.MiddleName;
                    tblInternalUser.PhoneNumber = userModel.PhoneNumber;
                    tblInternalUser.UserName = userModel.UserName;
                    tblInternalUser.EmailAddress = userModel.EmailAddress;
                    tblInternalUser.IsActive = userModel.IsActive;
                    tblInternalUser.IsSuperAdmin = userModel.IsSuperAdmin;
                    if (userModel.TeamID.ToString() != "00000000-0000-0000-0000-000000000000")
                    {
                        tblInternalUser.TeamId = userModel.TeamID;
                    }
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
                    int updated=  await _context.SaveChangesAsync();
                    if (updated>0)
                    {
                        if (userModel.specialRoles.ToString() == "IsDepartmentHead"||userModel.TeamID!=Guid.Empty)
                        {
                            TblTeam tblTeam = _context.TblTeams.Find(userModel.TeamID);
                            tblTeam.TeamLeaderId = tblInternalUser.UserId;
                            await _context.SaveChangesAsync();
                        }                           
                        _notifyService.Success("User susccessfully updated");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("User isn't successfully updated. Please try again");
                        ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", tblInternalUser.DepId);
                        if (userModel.DepId != null)
                        {
                            if (userModel.TeamID != null)
                            {
                                ViewBag.Teams = new SelectList(_context.TblTeams.Where(s => s.DepId == tblInternalUser.DepId), "TeamId", "TeamName", userModel.TeamID);
                            }
                            else
                            {
                                SelectList sle = new SelectList(new List<SelectListItem>
                                    {
                                        new SelectListItem { Selected = true, Text = "--Select--", Value = "00000000-0000-0000-0000-000000000000"}
                                    }, "Value", "Text", 1);
                                
                                ViewBag.Teams = sle;
                            }
                        }
                        else
                        {
                            SelectList sle = new SelectList(new List<SelectListItem>{new SelectListItem { Selected = true, Text = "--Select--", Value = "00000000-0000-0000-0000-000000000000"}}, "Value", "Text", 1);
                            ViewBag.Teams = sle;
                        }
                        ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", userModel.DepId);
                        return View(userModel);
                    }
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
                        _notifyService.Error($"Error: {ex.Message}. Please try again");
                       
                    }
                    ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", userModel.DepId);
                    return View(userModel);
                }
            }
            else
            {
                _notifyService.Information("Uppdate isn't successfull. Please fill all neccessary field and try again.");
                ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", userModel.DepId);
                return View(userModel);
            }
          
        }
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
        public JsonResult GetTeams(Guid? DepId)
        {
            List<TblTeam> subcategoryModels = new List<TblTeam>();
            subcategoryModels = (from items in _context.TblTeams where items.DepId == DepId select items).ToList();
            return Json(new SelectList(subcategoryModels, "TeamId", "TeamName"));
        }
    }
}
