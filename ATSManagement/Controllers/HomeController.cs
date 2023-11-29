using NToastNotify;
using System.Diagnostics;
using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        public HomeController(ILogger<HomeController> logger, IMailService mail, AtsdbContext atsdbContext, INotyfService notyfService)
        {
            _notifyService = notyfService;
            _logger = logger;
            _mail = mail;
            _context = atsdbContext;
        }
        public async Task<IActionResult> Index(String? type,string? message)
        {
            if (type=="0")
            {
                _notifyService.Error(message);
            }
            ViewBag.internalUser = _context.TblInternalUsers.ToList().Count;
            ViewBag.ExternalUser = _context.TblExternalUsers.ToList().Count;
            ViewBag.Insititutions = _context.TblInistitutions.ToList().Count;
            ViewBag.Inspects = _context.TblInspectionPlans.ToList().Count;
            ViewBag.NewRequests = _context.TblRequests.Where(x => x.IsAssignedTodepartment ==null ||x.IsAssignedTodepartment==false).ToList().Count;
            ViewBag.HighPriorityRequests = _context.TblRequests.Where(x => x.PriorityId == Guid.Parse("12fba758-fa2a-406a-ae64-0a561d0f5e73")).ToList().Count;
            ViewBag.RequestsFromeCJAD = _context.TblRequestDepartmentRelations.Include(x=>x.Dep).Where(x => x.Dep.DepCode == "CVA").ToList().Count;
            ViewBag.RequestFromLegalStudies = _context.TblRequestDepartmentRelations.Include(x => x.Dep).Where(x => x.Dep.DepCode == "LSDC").ToList().Count;
            // await SendMail();
            return View();
        }

        private async Task SendMail()
        {
            List<string> to = new List<string>();
            to.Add("degaait@gmail.com");
            MailData data = new MailData(to, "Test Email", "<h1>H1 tag response</h1>", "degaait@gmail.com");
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
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