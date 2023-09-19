using Microsoft.AspNetCore.Mvc;

namespace ATSManagement.Controllers
{
    public class HelpController : Controller
    {
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
