using NToastNotify;
using System.Text.Json;
using ATSManagement.Models;
using ATSManagement.Security;
using ATSManagement.Services;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotyfService _notifyService;
        private LanguageService _localization;
        public AccountController(AtsdbContext context, IHttpContextAccessor contextAccessor, INotyfService notyfService, LanguageService languageService)
        {
            _notifyService = notyfService;
            _context = context;
            _contextAccessor = contextAccessor;
            _localization = languageService;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            LoginModels models = new LoginModels();
            models.Languages = _context.TblLanguages.Select(s => new SelectListItem
            {
                Value = s.LangId.ToString(),
                Text = s.Language.ToString()
            }).ToList();
            return View(models);
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModels collection)
        {
            string culture = null;
            string userInformation = null;
            try
            {

                if (collection.LangId == 1)
                {
                    culture = "en-US";
                }
                else
                {
                    culture = "am";
                }
                Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                   CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                   new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });

                bool isExist = false;
                List<MenuModels> _menus = new List<MenuModels>();
                if (collection.Password != null && collection.UserName != null)
                {
                    string realPas = PawwordEncryption.DecryptPasswordBase64String("MTIzNDU2");
                    string password = PawwordEncryption.EncryptPasswordBase64Strig(collection.Password);
                    TblInternalUser userinfo = (from items in _context.TblInternalUsers where (items.UserName == collection.UserName && items.Password == password) || (items.EmailAddress == collection.UserName && items.Password == password) select items).FirstOrDefault();

                    if (userinfo != null)
                    {
                        LoginModels _loginCredentials = _context.TblInternalUsers.Where(x => x.UserName.Trim().ToLower() == collection.UserName.Trim().ToLower() && x.Password == password || (x.EmailAddress == collection.UserName)).Select(x => new LoginModels
                        {
                            UserName = x.UserName,
                            DepName = x.Dep.DepName,
                            DepNameAmharic = x.Dep.DepNameAmharic,
                            DepId = x.DepId,
                            UserId = x.UserId,
                            UserFullName = x.FirstName + " " + x.MidleName,

                        }).FirstOrDefault();
                        if (userinfo.IsSuperAdmin == true)
                        {
                            if (culture == "am")
                            {
                                _menus = _context.TblSubmenus.Where(x => x.IsActive == true && x.IsDeleted == false).Select(x => new MenuModels
                                {
                                    MainMenuId = x.Menu.MenuId,
                                    MainMenuName = x.Menu.MenuNameAmharic,
                                    SubMenuId = x.Id,
                                    SubMenuName = x.SubmenuAmharic,
                                    ControllerName = x.Controller,
                                    ActionName = x.Action,
                                    DepId = x.RoleId,
                                    DepName = x.Dep.DepName,
                                    DepNameAmharic = x.Dep.DepNameAmharic,
                                    DisplayOrder = x.Menu.DisplayOrder,
                                    Class_SVC = x.Menu.ClassSvg
                                }).OrderBy(p => p.DisplayOrder).ToList();
                            }
                            else
                            {
                                _menus = _context.TblSubmenus.Where(x => x.IsActive == true && x.IsDeleted == false).Select(x => new MenuModels
                                {
                                    MainMenuId = x.Menu.MenuId,
                                    MainMenuName = x.Menu.MenuName,
                                    SubMenuId = x.Id,
                                    SubMenuName = x.Submenu,
                                    ControllerName = x.Controller,
                                    ActionName = x.Action,
                                    DepId = x.RoleId,
                                    DepName = x.Dep.DepName,
                                    DepNameAmharic = x.Dep.DepNameAmharic,
                                    DisplayOrder = x.Menu.DisplayOrder,
                                    Class_SVC = x.Menu.ClassSvg
                                }).OrderBy(p => p.DisplayOrder).ToList();
                            }

                        }
                        else
                        {
                            // Get the login user details and bind it to LoginModels class  

                            if (culture == "am")
                            {
                                _menus = _context.TblSubmenus.Where(x => x.DepId == _loginCredentials.DepId && x.IsActive == true && x.IsDeleted == false).Select(x => new MenuModels
                                {
                                    MainMenuId = x.Menu.MenuId,
                                    MainMenuName = x.Menu.MenuNameAmharic,
                                    SubMenuId = x.Id,
                                    SubMenuName = x.SubmenuAmharic,
                                    ControllerName = x.Controller,
                                    ActionName = x.Action,
                                    DepId = x.RoleId,
                                    DepName = x.Dep.DepName,
                                    DepNameAmharic = x.Dep.DepNameAmharic,
                                    DisplayOrder = x.Menu.DisplayOrder,
                                    Class_SVC = x.Menu.ClassSvg
                                }).OrderBy(p => p.DisplayOrder).ToList();
                            }
                            else
                            {
                                _menus = _context.TblSubmenus.Where(x => x.DepId == _loginCredentials.DepId && x.IsActive == true && x.IsDeleted == false).Select(x => new MenuModels
                                {
                                    MainMenuId = x.Menu.MenuId,
                                    MainMenuName = x.Menu.MenuName,
                                    SubMenuId = x.Id,
                                    SubMenuName = x.Submenu,
                                    ControllerName = x.Controller,
                                    ActionName = x.Action,
                                    DepId = x.RoleId,
                                    DepName = x.Dep.DepName,
                                    DepNameAmharic = x.Dep.DepNameAmharic,
                                    DisplayOrder = x.Menu.DisplayOrder,
                                    Class_SVC = x.Menu.ClassSvg
                                }).OrderBy(p => p.DisplayOrder).ToList();
                            }
                        }
                        //Get the Menu details from entity and bind it in MenuModels list.  
                        //SetAuthCookie(_loginCredentials.UserName, false); // set the formauthentication cookie  
                        string menusString = JsonSerializer.Serialize(_menus);
                        string loginCredentials = JsonSerializer.Serialize(_loginCredentials);
                        _contextAccessor.HttpContext.Session.SetString("MenuMaster", menusString);
                        _contextAccessor.HttpContext.Session.SetString("LoginCredentials", loginCredentials);
                        _contextAccessor.HttpContext.Session.SetString("UserName", _loginCredentials.UserName);
                        _contextAccessor.HttpContext.Session.SetString("UserFullname", _loginCredentials.UserFullName);
                        _contextAccessor.HttpContext.Session.SetString("userId", _loginCredentials.UserId.ToString());
                        if (userinfo.IsSuperAdmin == true)
                        {
                            if (culture == "am")
                            {
                                userInformation = "የሲይቴም አስተዳደር";
                            }
                            else
                            {
                                userInformation = "System Administrator";
                            }
                        }
                        else if (userinfo.IsDeputy == true)
                        {
                            if (culture == "am")
                            {
                                userInformation = "ሚኒስቴር ደረጃ";
                            }
                            else
                            {
                                userInformation = "Deputy";
                            }
                        }
                        else
                        {
                            if (culture == "am")
                            {
                                userInformation = _loginCredentials.DepNameAmharic.ToString();
                            }
                            else
                            {
                                userInformation = _loginCredentials.DepName.ToString();
                            }
                        }
                        _contextAccessor.HttpContext.Session.SetString("DepName", userInformation);
                        return RedirectToAction("Index", "Home");
                        // return Redirect(Request.Headers["Referer"].ToString());
                    }
                    else
                    {
                        _notifyService.Error(_localization.Getkey("user_not_found").Value);
                        collection.Languages = _context.TblLanguages.Select(s => new SelectListItem
                        {
                            Value = s.LangId.ToString(),
                            Text = s.Language
                        }).ToList();
                        return View(collection);
                    }
                }
                else
                {
                    _notifyService.Error(_localization.Getkey("user_not_found").Value);

                    collection.Languages = _context.TblLanguages.Select(s => new SelectListItem
                    {
                        Value = s.LangId.ToString(),
                        Text = s.Language
                    }).ToList();
                    return View(collection);
                }
            }
            catch (Exception ex)
            {

                _notifyService.Error($"Error:{ex.Message} happened"+_localization.Getkey("user_not_found").Value);
                collection.Languages = _context.TblLanguages.Select(s => new SelectListItem
                {
                    Value = s.LangId.ToString(),
                    Text = s.Language
                }).ToList();
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
            var tblUser = await _context.TblInternalUsers.FirstOrDefaultAsync(m => m.UserId == id);
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
            ViewBag.Error = "User doesn't found!. Please try again";

            return View();
        }

        private bool TblUserExists(Guid id)
        {
            return (_context.TblInternalUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
