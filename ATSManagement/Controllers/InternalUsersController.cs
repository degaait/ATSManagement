using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.Security;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
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
            var atsdbContext = _context.TblInternalUsers.Include(t => t.Dep).Include(s => s.Team).Include(s=>s.Branch);
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
            List<SecretaryTypes> secretaryTypes = new List<SecretaryTypes>();
           
            secretaryTypes.Add(new SecretaryTypes
            {
                SecID = "Deputy Secretary",
                SecName = "Deputy Secretay"
            });
            secretaryTypes.Add(new SecretaryTypes
            {
                SecID = "Legal studies Secretary",
                SecName = "Legal studies Secretary"
            });
            secretaryTypes.Add(new SecretaryTypes
            {
                SecName = "Civil Justice Secretary",
                SecID = "Civil Justice Secretary"
            });
            user.SecretaryTypes = secretaryTypes.Select(s => new SelectListItem
            {
                Value = s.SecID,
                Text = s.SecName
            }).ToList();
          
            user.Departments = _context.TblDepartments.Select(s => new SelectListItem
            {
                Value = s.DepId.ToString(),
                Text = s.DepName
            }).ToList();
            user.Branches = _context.TblBranchOffices.Select(s => new SelectListItem
            {
                Value = s.BranchId.ToString(),
                Text = s.Name
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
            List<SecretaryTypes> secretaryTypes = new List<SecretaryTypes>();

            secretaryTypes.Add(new SecretaryTypes
            {
                SecID = "Deputy Secretary",
                SecName = "Deputy Secretay"
            });
            secretaryTypes.Add(new SecretaryTypes
            {
                SecID = "Legal studies Secretary",
                SecName = "Legal studies Secretary"
            });
            secretaryTypes.Add(new SecretaryTypes
            {
                SecName = "Civil Justice Secretary",
                SecID = "Civil Justice Secretary"
            });        
            try
            {
                if (userModel.specialRoles.ToString() != "IsDeputy")
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
                if (ModelState.IsValid)
                {
                    tblInternalUser.UserId = Guid.NewGuid();
                    tblInternalUser.IsSuperAdmin = userModel.IsSuperAdmin;
                    if (userModel.IsSuperAdmin == true)
                    {
                        tblInternalUser.IsDeputy = false;
                        tblInternalUser.IsDefaultUser = false;
                        tblInternalUser.IsDepartmentHead = false;
                        tblInternalUser.IsSecretary = false;
                        tblInternalUser.IsTeamLeader = false;
                        tblInternalUser.IsBranchOfficeUser = false;
                        tblInternalUser.IsInternalRequestUser = false;
                        tblInternalUser.DepId = null;
                        tblInternalUser.BranchId = null;
                    }
                    else if (userModel.specialRoles.ToString() == "IsDeputy")
                    {
                        tblInternalUser.IsDeputy = true;
                        tblInternalUser.BranchId = null;
                    }
                    else
                    {

                        tblInternalUser.IsSuperAdmin = false;
                        tblInternalUser.IsDeputy = false;
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

                            if (userModel.TeamID.ToString() != "00000000-0000-0000-0000-000000000000" || userModel.TeamID.ToString() != null)
                            {
                                tblInternalUser.TeamId = userModel.TeamID;
                                tblInternalUser.IsTeamLeader = true;
                            }
                        }
                        else
                        {
                            tblInternalUser.IsTeamLeader = false;
                        }
                        if (userModel.specialRoles.ToString() == "DefaultUser")
                        {
                            tblInternalUser.IsDefaultUser = true;
                        }
                        else
                        {
                            tblInternalUser.IsDefaultUser = false;
                        }
                        if (userModel.specialRoles.ToString() == "IsSecretary")
                        {
                            tblInternalUser.IsSecretary = true;
                            if (userModel.SecID== "Deputy Secretary")
                            {
                                tblInternalUser.IsDeputySecretary = true;
                                tblInternalUser.IsCivilJusticeSecretay = false;
                                tblInternalUser.IsLegalStudySecretary = false;
                            }
                            else if (userModel.SecID == "Legal studies Secretary")
                            {
                                tblInternalUser.IsLegalStudySecretary = true;
                                tblInternalUser.IsDeputySecretary = false;
                                tblInternalUser.IsCivilJusticeSecretay = false;
                            }
                           else if (userModel.SecID == "Civil Justice Secretary")
                            {
                                tblInternalUser.IsCivilJusticeSecretay = true;
                                tblInternalUser.IsDeputySecretary = false;
                                tblInternalUser.IsLegalStudySecretary = false;
                            }
                            else
                            {
                                tblInternalUser.IsDeputySecretary = false;
                                tblInternalUser.IsCivilJusticeSecretay = false;
                                tblInternalUser.IsLegalStudySecretary = false;
                            }
                        }
                        else
                        {
                            tblInternalUser.IsSecretary = false;
                            tblInternalUser.IsDeputySecretary = false;
                            tblInternalUser.IsCivilJusticeSecretay = false;
                            tblInternalUser.IsLegalStudySecretary = false;
                        }
                        if (userModel.specialRoles.ToString() == "IsBranchOfficeUser")
                        {
                            tblInternalUser.BranchId = userModel.BranchId;
                            tblInternalUser.IsBranchOfficeUser = true;
                        }
                        else
                        {
                            tblInternalUser.BranchId = null;
                            tblInternalUser.IsBranchOfficeUser = false;
                        }
                        if (userModel.specialRoles.ToString() == "IsInternalRequestUser")
                        {
                            tblInternalUser.IsInternalRequestUser = true;
                        }
                        else
                        {
                            tblInternalUser.IsInternalRequestUser = false;
                        }
                        tblInternalUser.DepId = userModel.DepId;
                    }
                    tblInternalUser.FirstName = userModel.FirstName;
                    tblInternalUser.LastName = userModel.LastName;
                    tblInternalUser.IsActive = userModel.IsActive;
                    tblInternalUser.MidleName = userModel.MiddleName;
                    tblInternalUser.PhoneNumber = userModel.PhoneNumber;
                    tblInternalUser.UserName = userModel.UserName;
                    tblInternalUser.Password = PawwordEncryption.EncryptPasswordBase64Strig(userModel.Password);
                    tblInternalUser.EmailAddress = userModel.EmailAddress;
                    _context.Add(tblInternalUser);
                    int saved = await _context.SaveChangesAsync();
                    if (saved > 0)
                    {
                        if (userModel.specialRoles.ToString() != "IsDepartmentHead" && userModel.specialRoles.ToString() != "IsDeputy")
                        {
                            if (userModel.TeamID != null)
                            {
                                TblTeam tblTeam = _context.TblTeams.Find(userModel.TeamID);
                                tblTeam.TeamLeaderId = tblInternalUser.UserId;
                                await _context.SaveChangesAsync();
                            }
                        }
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
                        userModel.Branches = _context.TblBranchOffices.Select(s => new SelectListItem
                        {
                            Value = s.BranchId.ToString(),
                            Text = s.Name
                        }).ToList();
                        userModel.SecretaryTypes = secretaryTypes.Select(s => new SelectListItem
                        {
                            Value = s.SecID,
                            Text = s.SecName
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
                    userModel.Branches = _context.TblBranchOffices.Select(s => new SelectListItem
                    {
                        Value = s.BranchId.ToString(),
                        Text = s.Name
                    }).ToList();
                    userModel.SecretaryTypes = secretaryTypes.Select(s => new SelectListItem
                    {
                        Value = s.SecID,
                        Text = s.SecName
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
                userModel.Branches = _context.TblBranchOffices.Select(s => new SelectListItem
                {
                    Value = s.BranchId.ToString(),
                    Text = s.Name
                }).ToList();
                userModel.SecretaryTypes = secretaryTypes.Select(s => new SelectListItem
                {
                    Value = s.SecID,
                    Text = s.SecName
                }).ToList();
                return View(userModel);
            }
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            UserModel userModel = new UserModel();
            List<SecretaryTypes> secretaryTypes = new List<SecretaryTypes>();

            secretaryTypes.Add(new SecretaryTypes
            {
                SecID = "Deputy Secretary",
                SecName = "Deputy Secretay"
            });
            secretaryTypes.Add(new SecretaryTypes
            {
                SecID = "Legal studies Secretary",
                SecName = "Legal studies Secretary"
            });
            secretaryTypes.Add(new SecretaryTypes
            {
                SecName = "Civil Justice Secretary",
                SecID = "Civil Justice Secretary"
            });
          
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
            userModel.BranchId = tblInternalUser.BranchId;
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
            if (tblInternalUser.IsDefaultUser == true)
            {
                ViewBag.IsDefaultUser = true;
            }
            else
            {
                ViewBag.IsDefaultUser = false;
            }

            if (tblInternalUser.IsSecretary == true)
            {
                ViewBag.IsSecretary = true;
                if (tblInternalUser.IsDeputySecretary==true)
                {
                    userModel.SecID = "Deputy Secretary";
                }
                if (tblInternalUser.IsLegalStudySecretary==true)
                {
                    userModel.SecID = "Legal studies Secretary";
                }
                if (tblInternalUser.IsCivilJusticeSecretay==true)
                {
                    userModel.SecID = "Civil Justice Secretary";
                }

            }
            else
            {
                ViewBag.IsSecretary = false;
            }
            if (tblInternalUser.IsBranchOfficeUser == true)
            {
                ViewBag.IsBranchOfficeUser = true;
            }
            else
            {
                ViewBag.IsBranchOfficeUser = false;
            }
            if (tblInternalUser.IsInternalRequestUser == true)
            {
                ViewBag.IsInternalRequestUser = true;
            }
            else
            {
                ViewBag.IsInternalRequestUser = false;
            }
            userModel.SecretaryTypes = secretaryTypes.Select(s => new SelectListItem
            {
                Value = s.SecID,
                Text = s.SecName
            }).ToList();
            if (tblInternalUser == null)
            {
                return NotFound();
            }
            if (tblInternalUser.DepId != null)
            {
                if (tblInternalUser.TeamId != null)
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
                SelectList sle = new SelectList(new List<SelectListItem>
                                    {
                                        new SelectListItem { Selected = true, Text = "--Select--", Value = "00000000-0000-0000-0000-000000000000"}
                                    }, "Value", "Text", 1);
                ViewBag.Teams = sle;
            }
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", tblInternalUser.DepId);
            userModel.Branches = _context.TblBranchOffices.Select(s => new SelectListItem
            {
                Value = s.BranchId.ToString(),
                Text = s.Name
            }).ToList(); return View(userModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserModel userModel)
        {
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userModel.UserId);
            List<SecretaryTypes> secretaryTypes = new List<SecretaryTypes>();

            secretaryTypes.Add(new SecretaryTypes
            {
                SecID = "Deputy Secretary",
                SecName = "Deputy Secretay"
            });
            secretaryTypes.Add(new SecretaryTypes
            {
                SecID = "Legal studies Secretary",
                SecName = "Legal studies Secretary"
            });
            secretaryTypes.Add(new SecretaryTypes
            {
                SecName = "Civil Justice Secretary",
                SecID = "Civil Justice Secretary"
            });
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
                        if (userModel.TeamID.ToString() != "00000000-0000-0000-0000-000000000000" || userModel.TeamID.ToString() != null)
                        {
                            tblInternalUser.TeamId = userModel.TeamID;
                        }
                        tblInternalUser.IsTeamLeader = true;
                    }
                    else
                    {
                        tblInternalUser.IsTeamLeader = false;
                    }
                    if (userModel.specialRoles.ToString() == "DefaultUser")
                    {
                        tblInternalUser.IsDefaultUser = true;
                    }
                    else
                    {
                        tblInternalUser.IsDefaultUser = false;
                    }
                    if (userModel.specialRoles.ToString() == "IsSecretary")
                    {
                        tblInternalUser.IsSecretary = true;
                        if (userModel.SecID == "Deputy Secretary")
                        {
                            tblInternalUser.IsDeputySecretary = true;
                            tblInternalUser.IsCivilJusticeSecretay = false;
                            tblInternalUser.IsLegalStudySecretary = false;
                        }
                        else if (userModel.SecID == "Legal studies Secretary")
                        {
                            tblInternalUser.IsLegalStudySecretary = true;
                            tblInternalUser.IsDeputySecretary = false;
                            tblInternalUser.IsCivilJusticeSecretay = false;
                        }
                        else if (userModel.SecID == "Civil Justice Secretary")
                        {
                            tblInternalUser.IsCivilJusticeSecretay = true;
                            tblInternalUser.IsDeputySecretary = false;
                            tblInternalUser.IsLegalStudySecretary = false;
                        }
                        else
                        {
                            tblInternalUser.IsDeputySecretary = false;
                            tblInternalUser.IsCivilJusticeSecretay = false;
                            tblInternalUser.IsLegalStudySecretary = false;
                        }
                    }
                    else
                    {
                        tblInternalUser.IsSecretary = false;
                        tblInternalUser.IsDeputySecretary = false;
                        tblInternalUser.IsCivilJusticeSecretay = false;
                        tblInternalUser.IsLegalStudySecretary = false;
                    }
                    if (userModel.specialRoles.ToString() == "IsBranchOfficeUser")
                    {
                        tblInternalUser.IsBranchOfficeUser = true;
                        tblInternalUser.BranchId = userModel.BranchId;
                    }
                    else
                    {
                        tblInternalUser.IsBranchOfficeUser = false;
                        tblInternalUser.BranchId = null;
                    }
                    if (userModel.specialRoles.ToString() == "IsInternalRequestUser")
                    {
                        tblInternalUser.IsInternalRequestUser = true;
                    }
                    else
                    {
                        tblInternalUser.IsInternalRequestUser = false;
                    }
                    tblInternalUser.DepId = userModel.DepId;

                    _context.Update(tblInternalUser);
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        if (userModel.specialRoles.ToString() == "IsTeamLeader" && userModel.TeamID != Guid.Empty)
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
                        ViewData["BranchId"] = new SelectList(_context.TblBranchOffices, "BranchID", "Name", tblInternalUser.BranchId);
                        userModel.SecretaryTypes = secretaryTypes.Select(s => new SelectListItem
                        {
                            Value = s.SecID,
                            Text = s.SecName
                        }).ToList();
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
                            SelectList sle = new SelectList(new List<SelectListItem> { new SelectListItem { Selected = true, Text = "--Select--", Value = "00000000-0000-0000-0000-000000000000" } }, "Value", "Text", 1);
                            ViewBag.Teams = sle;
                        }
                        ViewData["BranchId"] = new SelectList(_context.TblBranchOffices, "BranchID", "Name", tblInternalUser.BranchId);
                        userModel.Branches = _context.TblBranchOffices.Select(s => new SelectListItem
                        {
                            Value = s.BranchId.ToString(),
                            Text = s.Name
                        }).ToList();
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
                    ViewBag.BranchId = new SelectList(_context.TblBranchOffices, "BranchID", "Name", tblInternalUser.BranchId);
                    userModel.Branches = _context.TblBranchOffices.Select(s => new SelectListItem
                    {
                        Value = s.BranchId.ToString(),
                        Text = s.Name
                    }).ToList();
                    userModel.SecretaryTypes = secretaryTypes.Select(s => new SelectListItem
                    {
                        Value = s.SecID,
                        Text = s.SecName
                    }).ToList();
                    ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", userModel.DepId);
                    return View(userModel);
                }
            }
            else
            {
                _notifyService.Information("Uppdate isn't successfull. Please fill all neccessary field and try again.");
                ViewBag.BranchId = new SelectList(_context.TblBranchOffices, "BranchID", "Name", tblInternalUser.BranchId);
                userModel.Branches = _context.TblBranchOffices.Select(s => new SelectListItem
                {
                    Value = s.BranchId.ToString(),
                    Text = s.Name
                }).ToList();
                userModel.SecretaryTypes = secretaryTypes.Select(s => new SelectListItem
                {
                    Value = s.SecID,
                    Text = s.SecName
                }).ToList();
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

        public IActionResult CreateAdminUser()
        {
            UserModel user = new UserModel();

            user.IsActive = true;
            user.IsSuperAdmin = true;
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName");
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAdminUser(UserModel userModel)
        {
            TblInternalUser tblInternalUser = new TblInternalUser();
            try
            {
                if (ModelState.IsValid)
                {
                    tblInternalUser.UserId = Guid.NewGuid();
                    tblInternalUser.IsSuperAdmin = true;
                    tblInternalUser.IsDeputy = false;
                    tblInternalUser.IsDepartmentHead = false;
                    tblInternalUser.IsTeamLeader = false;
                    tblInternalUser.IsDefaultUser = false;
                    tblInternalUser.FirstName = userModel.FirstName;
                    tblInternalUser.LastName = userModel.LastName;
                    tblInternalUser.IsActive = userModel.IsActive;
                    tblInternalUser.MidleName = userModel.MiddleName;
                    tblInternalUser.PhoneNumber = userModel.PhoneNumber;
                    tblInternalUser.UserName = userModel.UserName;
                    tblInternalUser.Password = PawwordEncryption.EncryptPasswordBase64Strig(userModel.Password);
                    tblInternalUser.EmailAddress = userModel.EmailAddress;

                    _context.TblInternalUsers.Add(tblInternalUser);
                    int saved = await _context.SaveChangesAsync();

                    if (saved > 0)
                    {
                        _notifyService.Success("User created successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("User isn't created succefully. Please try again");
                        return View(userModel);
                    }
                }
                else
                {
                    _notifyService.Error("User isn't created succefully. Please fill all neccessary fields and try again");
                    return View(userModel);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error:{ex.Message} happened. Please try again");
                return View(userModel);
            }
        }
        public async Task<IActionResult> AdminUsers()
        {
            var atsdbContext = _context.TblInternalUsers.Where(s => s.IsSuperAdmin == true);
            return View(await atsdbContext.ToListAsync());
        }

        public async Task<IActionResult> EditAdminUser(Guid? id)
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
            userModel.UserId = tblInternalUser.UserId;
            userModel.IsSuperAdmin = tblInternalUser.IsSuperAdmin;
            userModel.IsActive = tblInternalUser.IsActive;
            if (tblInternalUser == null)
            {
                return NotFound();
            }
            return View(userModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAdminUser(UserModel userModel)
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
                    _context.Update(tblInternalUser);
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("User susccessfully updated");
                        return RedirectToAction(nameof(AdminUsers));
                    }
                    else
                    {
                        _notifyService.Error("User isn't successfully updated. Please try again");
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
                    return View(userModel);
                }
            }
            else
            {
                _notifyService.Information("Uppdate isn't successfull. Please fill all neccessary field and try again.");
                return View(userModel);
            }

        }

        public async Task<IActionResult> ResetPassword(Guid? id)
        {
            LoginModels userModel = new LoginModels();
            var users = _context.TblInternalUsers.Where(s => s.UserId == id).FirstOrDefault();
            if (users == null)
            {
                return RedirectToAction(nameof(DataNotFound));
            }
            else
            {
                userModel.UserId = users.UserId;
                userModel.UserName = users.UserName;
                return View(userModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(LoginModels userModel)
        {
            var users = _context.TblInternalUsers.Where(s => s.UserId == userModel.UserId).FirstOrDefault();
            if (users == null)
            {
                return RedirectToAction(nameof(DataNotFound));
            }
            else
            {
                userModel.UserId = users.UserId;
                userModel.UserName = users.UserName;
                users.Password = PawwordEncryption.EncryptPasswordBase64Strig(userModel.NewPassword);
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    _notifyService.Success("Password reseted successfully!.");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notifyService.Error("Password reset isn't successful. Please try again");
                    return View(userModel);
                }
            }
        }
        public async Task<ActionResult> DataNotFound()
        {
            return View();
        }
    }
}
