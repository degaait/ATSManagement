using ATSManagement.Models;
using ATSManagement.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class NotificationsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public NotificationsController(IHttpContextAccessor httpContextAccessor, AtsdbContext atsdbContext)
        {
            _context = atsdbContext;
            _contextAccessor = httpContextAccessor;
        }
        public async Task<ActionResult> Index()
        {
            Guid userd = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var notifications = (from items in _context.TblNotifications where items.UserId == userd && (items.IsChecked == false || items.IsChecked == null) select items).ToList();
            return View(notifications);
        }

        // GET: NotificationsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult MarkAsRead(int id)
        {
            TblNotification tblNotification = _context.TblNotifications.Find(id);
            if (tblNotification==null)
            {
                return NotFound();
            }
            tblNotification.IsChecked=true;
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public ActionResult MarkAsReadLayout(int id)
        {
            TblNotification tblNotification = _context.TblNotifications.Find(id);
            if (tblNotification == null)
            {
                return NotFound();
            }
            tblNotification.IsChecked = true;
            _context.SaveChanges();
            return RedirectToAction("Index","Home");
        }
        // GET: NotificationsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NotificationsController/Create
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

        // GET: NotificationsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: NotificationsController/Edit/5
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

        // GET: NotificationsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: NotificationsController/Delete/5
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
