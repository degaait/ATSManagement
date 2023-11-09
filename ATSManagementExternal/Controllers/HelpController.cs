using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace ATSManagementExternal.Controllers
{
    public class HelpController : Controller
    {
        // GET: HelpController
        public ActionResult Index()
        {
            return View();
        }

        // GET: HelpController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HelpController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HelpController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HelpController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HelpController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: HelpController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HelpController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
