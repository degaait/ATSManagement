using ATSManagement.Models;
using ATSManagement.Security;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ATSManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public AccountController(AtsdbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModels collection)
        {
            bool isExist = false;
            if (collection.Password != null && collection.UserName != null)
            {
                string realPas = PawwordEncryption.DecryptPasswordBase64String("MTIzNDU2");
                string password = PawwordEncryption.EncryptPasswordBase64Strig(collection.Password);
                isExist = _context.TblInternalUsers.Where(a => a.UserName == collection.UserName && a.Password == password).Any();
                if (isExist)
                {
                    LoginModels _loginCredentials = _context.TblInternalUsers.Where(x => x.UserName.Trim().ToLower() == collection.UserName.Trim().ToLower() && x.Password == password).Select(x => new LoginModels
                    {
                        UserName = x.UserName,
                        DepName = x.Dep.DepName,
                        DepId = x.DepId,
                        UserId = x.UserId,
                        UserFullName = x.FirstName + " " + x.MidleName + " " + x.LastName,

                    }).FirstOrDefault();  // Get the login user details and bind it to LoginModels class  
                    List<MenuModels> _menus = _context.TblSubmenus.Where(x => x.DepId == _loginCredentials.DepId && x.IsActive == true && x.IsDeleted == false).Select(x => new MenuModels
                    {
                        MainMenuId = x.Menu.MenuId,
                        MainMenuName = x.Menu.MenuName,
                        SubMenuId = x.Id,
                        SubMenuName = x.Submenu,
                        ControllerName = x.Controller,
                        ActionName = x.Action,
                        DepId = x.RoleId,
                        DepName = x.Dep.DepName
                    }).ToList();
                    //Get the Menu details from entity and bind it in MenuModels list.  
                    //SetAuthCookie(_loginCredentials.UserName, false); // set the formauthentication cookie  
                    string menusString = JsonSerializer.Serialize(_menus);
                    string loginCredentials = JsonSerializer.Serialize(_loginCredentials);
                    _contextAccessor.HttpContext.Session.SetString("MenuMaster", menusString);
                    _contextAccessor.HttpContext.Session.SetString("LoginCredentials", loginCredentials);
                    _contextAccessor.HttpContext.Session.SetString("UserName", _loginCredentials.UserName);
                    _contextAccessor.HttpContext.Session.SetString("UserFullname", _loginCredentials.UserFullName);
                    _contextAccessor.HttpContext.Session.SetString("userId", _loginCredentials.UserId.ToString());
                    _contextAccessor.HttpContext.Session.SetString("DepName", _loginCredentials.DepName.ToString());
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(collection);
                }

            }
            else
            {
                return View(collection);
            }
        }
        public IActionResult Logout()
        {
            _contextAccessor.HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        public IActionResult FeedBack()
        {
            return View();
        }
        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult AccountStatus()
        {
            return View();
        }
        public async Task<IActionResult> ChangePassword()
        {
            LoginModels loginModels = new LoginModels();
            Guid id = new Guid(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (id == null || _context.TblInternalUsers == null)
            {
                return NotFound();
            }
            var tblUser = await _context.TblInternalUsers
                                        .Include(t => t.Dep)
                                        .Include(t => t.TblInspectionPlans)
                                        .FirstOrDefaultAsync(m => m.UserId == id);
            if (tblUser == null)
            {
                return NotFound();
            }
            loginModels.UserId = id;
            loginModels.UserName = tblUser.UserName;
            loginModels.Password = tblUser.Password;

            return View(loginModels);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult ChangePassword(LoginModels loginModels)
        {
            if (TblUserExists(loginModels.UserId))
            {
                TblInternalUser tblUser = _context.TblInternalUsers.Find(loginModels.UserId);
                tblUser.Password = PawwordEncryption.EncryptPasswordBase64Strig(loginModels.NewPassword);
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                    ViewBag.Ok = "Password Successfully changed.";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.Error = "Operation isn't successfully changed. Please try again";
                    return View(loginModels);
                }
            }
            return View();
        }

        private bool TblUserExists(Guid id)
        {
            return (_context.TblInternalUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
