using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace ATSManagement.Controllers
{

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
