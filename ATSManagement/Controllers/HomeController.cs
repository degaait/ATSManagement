using System.Diagnostics;
using ATSManagement.Models;
using ATSManagement.IModels;
using Microsoft.AspNetCore.Mvc;

namespace ATSManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly AtsdbContext _context;
        public HomeController(ILogger<HomeController> logger, IMailService mail,AtsdbContext atsdbContext)
        {
            _logger = logger;
            _mail = mail;
            _context = atsdbContext;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.internalUser=_context.TblInternalUsers.ToList().Count;
            ViewBag.ExternalUser=_context.TblExternalUsers.ToList().Count;
            ViewBag.Insititutions=_context.TblInistitutions.ToList().Count;
            ViewBag.Inspects=_context.TblInspectionPlans.ToList().Count;
            List<string> to = new List<string>();
            to.Add("degaait@gmail.com");
            MailData data = new MailData(to, "Test Email", "<h1>H1 tag response</h1>", "degaait@gmail.com");
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}