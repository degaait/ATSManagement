using AspNetCoreHero.ToastNotification.Abstractions;
using ATSManagementExternal.IModels;
using ATSManagementExternal.Models;
using ATSManagementExternal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ATSManagementExternal.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        public HomeController(ILogger<HomeController> logger, INotyfService notyfService, IMailService mailService, AtsdbContext atsdbContext, IHttpContextAccessor httpContext)
        {
            _notifyService = notyfService;
            _mail = mailService;
            _context = atsdbContext;
            _logger = logger;
            _contextAccessor = httpContext;
        }

        public async Task<IActionResult> Index()
        {
            String culture;
            if (_contextAccessor.HttpContext.Session.GetString("culture") != null)
            {
                culture = _contextAccessor.HttpContext.Session.GetString("culture");
                _contextAccessor.HttpContext.Session.SetString("culture", culture);
            }
            else
            {
                culture = "am";
                _contextAccessor.HttpContext.Session.SetString("culture", "am");
            }

            Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName,
                 CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                 new CookieOptions() { Expires = DateTimeOffset.UtcNow.AddYears(1) });
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ContactModel model)
        {
            try
            {
                ContactModel coModel = new ContactModel();
                TblContactInformation tblContact = new TblContactInformation();
                var userEmails = _context.TblInternalUsers.Where(x => x.IsDeputy == true || x.IsDepartmentHead == true).Select(s => s.EmailAddress).ToList();
                tblContact.ContactPhoneNumber = model.ContactPhoneNumber;
                tblContact.FullName = model.FullName;
                tblContact.ContactEmail = model.ContactEmail;
                tblContact.ContactDetaiMessage = model.ContactDetaiMessage;
                tblContact.ContactCountry = model.ContactCountry;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "admin/Files");
                if (model.formFile != null)
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    FileInfo fileInfo = new FileInfo(model.formFile.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.formFile.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.formFile.CopyTo(stream);
                    }
                    string dbPath = "/admin/Files/" + fileName;
                    tblContact.FileUplaod = dbPath;
                }
                _context.TblContactInformations.Add(tblContact);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Your Message is successfully submitted.");
                    await SendMail(userEmails, "Contact message notification from Task Tracking Dashboard", "<h3>Please review the detail on Task Tracking Dashboard and reply accordingly.<h3>");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notifyService.Error("Your message isn't submitted successfully. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error("Your message isn't submitted successfully because of " + ex.Message + ". Please try again");
                return View(model);
            }
        }
        public IActionResult IndexLocalIzation(String? Culture)
        {
            _contextAccessor.HttpContext.Session.SetString("culture", Culture);
            return RedirectToAction("Index");
        }

        public IActionResult AboutUs()
        {
            return View();
        }
        public IActionResult ContactUs()
        {
            return View();
        }
        public IActionResult Help()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactUs(ContactModel model)
        {
            try
            {
                ContactModel coModel = new ContactModel();
                TblContactInformation tblContact = new TblContactInformation();
                var userEmails = _context.TblInternalUsers.Where(x => x.IsDeputy == true || x.IsDepartmentHead == true).Select(s => s.EmailAddress).ToList();
                tblContact.ContactPhoneNumber = model.ContactPhoneNumber;
                tblContact.ContactEmail = model.ContactEmail;
                tblContact.ContactDetaiMessage = model.ContactDetaiMessage;
                tblContact.ContactCountry = model.ContactCountry;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "admin/Files");
                if (model.formFile != null)
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    FileInfo fileInfo = new FileInfo(model.formFile.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.formFile.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.formFile.CopyTo(stream);
                    }
                    string dbPath = "/admin/Files/" + fileName;
                    tblContact.FileUplaod = dbPath;
                }
                _context.TblContactInformations.Add(tblContact);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Your Message is successfully submitted.");
                    await SendMail(userEmails, "Contact message notification from Task Tracking Dashboard", "<h3>Please review the detail on Task Tracking Dashboard and reply accordingly.<h3>");
                    return RedirectToAction(nameof(ContactUs));
                }
                else
                {
                    _notifyService.Error("Your message isn't submitted successfully. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error("Your message isn't submitted successfully because of " + ex.Message + ". Please try again");
                return View(model);
            }

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }

    }
}