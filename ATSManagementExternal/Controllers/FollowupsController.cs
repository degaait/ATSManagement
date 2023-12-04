using Microsoft.AspNetCore.Mvc;
using ATSManagementExternal.Models;
using ATSManagementExternal.IModels;
using Microsoft.EntityFrameworkCore;
using ATSManagementExternal.Filters;
using ATSManagementExternal.ViewModels;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagementExternal.Controllers
{
    [CheckSessionIsAvailable]
    public class FollowupsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;

        public FollowupsController(AtsdbContext context, INotyfService notyfService, IHttpContextAccessor contextAccessor, IMailService mailService)
        {
            _context = context;
            _notifyService = notyfService;
            _contextAccessor = contextAccessor;
            _mail = mailService;
        }

        public async Task<IActionResult> Index(Guid? RequestId)
        {
            ViewBag.id = RequestId;
            var atsdbContext = _context.TblFollowups.Include(t => t.ExternalUser).Include(t => t.Inist).Include(t => t.Request).Include(t => t.User).Where(s => s.RequestId == RequestId).OrderByDescending(s => s.FollowUpId);
            return View(await atsdbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblFollowups == null)
            {
                return NotFound();
            }

            var tblFollowup = await _context.TblFollowups
                .Include(t => t.ExternalUser)
                .Include(t => t.Inist)
                .Include(t => t.Request)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.FollowUpId == id);
            if (tblFollowup == null)
            {
                return NotFound();
            }

            return View(tblFollowup);
        }

        public IActionResult Create(Guid? RequestId)
        {
            if (RequestId == null)
            {
                return NotFound();
            }
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var request = _context.TblRequests.FindAsync(RequestId);
            FollowUpModel model = new FollowUpModel();
            ViewBag.id = RequestId;
            model.RequestId = RequestId;
            model.ExternalUserId = userId;
            model.InistId = request.Result.InistId;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FollowUpModel model)
        {
            try
            {
                ViewBag.id = model.RequestId;
                var emails = (from users in _context.TblInternalUsers where users.Dep.DepCode == "CVA" && (users.IsDeputy == true || users.IsDepartmentHead == true) select users.EmailAddress).ToList();
                TblFollowup followup = new TblFollowup();
                followup.RequestId = model.RequestId;
                followup.Message = model.Message;
                followup.ExternalUserId = model.ExternalUserId;
                followup.CreatedDate = DateTime.UtcNow;
                followup.InistId=model.InistId;
                followup.FromExternal = true;
                followup.FromInternal = false;
                _context.Add(followup);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    await SendMail(emails, "Follow up message submitted", "Follow up message from Task Tracking Dashboard is submitted. Please review it");
                    _notifyService.Success("Follow up message successfully submitted");
                    return RedirectToAction("Index", new { RequestId = model.RequestId });
                }
                else
                {
                    _notifyService.Error("Message doesn't submitted");
                    return View(model);
                }

            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error:{ex.Message} happened. Please try again");
                return View(model);
            }
        }

        public async Task<IActionResult> Edit(int? id)
        {
            FollowUpModel model = new FollowUpModel();

            if (id == null || _context.TblFollowups == null)
            {
                return NotFound();
            }

            var tblFollowup = await _context.TblFollowups.FindAsync(id);
            ViewBag.id = tblFollowup.RequestId;
            if (tblFollowup == null)
            {
                return NotFound();
            }
            model.FollowUpId = id;
            model.Message = tblFollowup.Message;
            model.UserId = tblFollowup.UserId;

            return View(tblFollowup);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FollowUpModel model)
        {
            if (model.FollowUpId == null)
            {
                return NotFound();
            }
            var tblFollowup = await _context.TblFollowups.FindAsync(model.FollowUpId);
            ViewBag.id = tblFollowup.RequestId;
            if (tblFollowup == null)
            {
                return NotFound();

            }
            try
            {

                tblFollowup.Message = model.Message;
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    _notifyService.Success("Follow up message is successfully updated");
                    return RedirectToAction("Index", new { RequestId = tblFollowup.RequestId });
                }
                else
                {
                    _notifyService.Error("Followup message isn't updated successfully. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again");
                return View(model);
            }
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblFollowups == null)
            {
                return NotFound();
            }

            var tblFollowup = await _context.TblFollowups
                .Include(t => t.ExternalUser)
                .Include(t => t.Inist)
                .Include(t => t.Request)
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.FollowUpId == id);
            if (tblFollowup == null)
            {
                return NotFound();
            }

            return View(tblFollowup);
        }

        // POST: Followups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblFollowups == null)
            {
                return Problem("Entity set 'AtsdbContext.TblFollowups'  is null.");
            }
            var tblFollowup = await _context.TblFollowups.FindAsync(id);
            if (tblFollowup != null)
            {
                _context.TblFollowups.Remove(tblFollowup);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblFollowupExists(int id)
        {
            return (_context.TblFollowups?.Any(e => e.FollowUpId == id)).GetValueOrDefault();
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
    }
}
