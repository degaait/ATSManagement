using ATSManagementExternal.Models;
using ATSManagementExternal.Security;
using ATSManagementExternal.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ATSManagementExternal.Controllers
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModels collection)
        {
            bool isExist = false;
            List<MenuModels> _menus = new List<MenuModels>();
            if (collection.Password != null && collection.UserName != null)
            {

                string password = PawwordEncryption.EncryptPasswordBase64Strig(collection.Password);
                TblExternalUser userinfo = (from items in _context.TblExternalUsers where (items.UserName == collection.UserName && items.Password == password) || (items.Email == collection.UserName && items.Password == password) select items).FirstOrDefault();

                if (userinfo != null)
                {
                    LoginModels _loginCredentials = new LoginModels();
                    _loginCredentials.UserFullName = userinfo.FirstName + " " + userinfo.MiddleName + " " + userinfo.LastName;
                    _loginCredentials.UserName = userinfo.UserName;
                    _loginCredentials.UserId = userinfo.ExterUserId;


                    // Get the login user details and bind it to LoginModels class  
                    _menus = _context.TblExternalSubmenus.Where(x => x.IsActive == true && x.IsDeleted == false).Select(x => new MenuModels
                    {
                        MainMenuId = x.Menu.MenuId,
                        MainMenuName = x.Menu.MenuNameAmharic,
                        SubMenuId = x.Id,
                        SubMenuName = x.SubmenuAmharic,
                        ControllerName = x.Controller,
                        ActionName = x.Action,
                        DepId = x.DepId,
                        DepName = x.Dep.DepName,
                        DisplayOrder = x.Menu.DisplayOrder,
                        Class_SVC = x.Menu.ClassSvg
                    }).OrderBy(p => p.DisplayOrder).ToList();

                    //Get the Menu details from entity and bind it in MenuModels list.  
                    //SetAuthCookie(_loginCredentials.UserName, false); // set the formauthentication cookie  
                    string menusString = JsonSerializer.Serialize(_menus);
                    string loginCredentials = JsonSerializer.Serialize(_loginCredentials);
                    _contextAccessor.HttpContext.Session.SetString("MenuMaster", menusString);
                    _contextAccessor.HttpContext.Session.SetString("LoginCredentials", loginCredentials);
                    _contextAccessor.HttpContext.Session.SetString("UserName", _loginCredentials.UserName);
                    _contextAccessor.HttpContext.Session.SetString("UserFullname", _loginCredentials.UserFullName);
                    _contextAccessor.HttpContext.Session.SetString("userId", _loginCredentials.UserId.ToString());
                    //_contextAccessor.HttpContext.Session.SetString("DepName", _loginCredentials.DepName.ToString());
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
            return RedirectToAction("Index", "Home");
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
