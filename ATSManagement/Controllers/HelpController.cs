using NToastNotify;
using ATSManagement.Filters;
using Microsoft.AspNetCore.Mvc;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class HelpController : Controller
    {
        private readonly IToastNotification _toastNotification;

        public HelpController(IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
        }

        public IActionResult Documentation()
        {

            return View();
        }
        public IActionResult FAQ()
        {
            return View();
        }
        public IActionResult UserManual()
        {
            return View();
        }
    }
}
