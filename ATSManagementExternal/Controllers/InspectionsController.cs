﻿using AspNetCoreHero.ToastNotification.Abstractions;
using ATSManagementExternal.Filters;
using ATSManagementExternal.IModels;
using ATSManagementExternal.Models;
using ATSManagementExternal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;

namespace ATSManagementExternal.Controllers
{
    [CheckSessionIsAvailable]
    public class InspectionsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        public InspectionsController(AtsdbContext context, IHttpContextAccessor httpContext, INotyfService notyfService, IMailService mailService)
        {
            _contextAccessor = httpContext;
            _notifyService = notyfService;
            _context = context;
            _mail = mailService;
        }
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblExternalUsers.Where(s => s.ExterUserId == userId).FirstOrDefault();

            var atsdbContext = _context.TblSentInspections
                  .Include(s => s.SpecificPlan)
                  .Include(t => t.SentByNavigation)
                  .Include(T => T.RepliedByNavigation)
                  .Include(s => s.Inst).Where(s => s.InstId == users.InistId && s.SpecificPlan.Pro.ProgressOrder >= 14);

            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblInspectionPlans == null)
            {
                return NotFound();
            }

            var tblInspectionPlan = await _context.TblInspectionPlans
                .Include(t => t.AssigneeType)
                .Include(t => t.Pro)
                .Include(t => t.Team)
                .Include(t => t.User)
                .Include(t => t.Year)
                .FirstOrDefaultAsync(m => m.InspectionPlanId == id);
            if (tblInspectionPlan == null)
            {
                return NotFound();
            }

            return View(tblInspectionPlan);
        }
        public IActionResult Create()
        {
            ViewData["AssigneeTypeId"] = new SelectList(_context.TblAssignementTypes, "AssigneeTypeId", "AssigneeTypeId");
            ViewData["ProId"] = new SelectList(_context.TblInspectionStatuses, "ProId", "ProId");
            ViewData["TeamId"] = new SelectList(_context.TblTeams, "TeamId", "TeamId");
            ViewData["UserId"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "YearId");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InspectionPlanId,PlanTitle,PlanDescription,CreationDate,ModificationDate,UserId,YearId,ProId,AssigningRemark,TeamId,AssigneeTypeId,IsAssignedToUser,IsUprovedByDeputy,IsUpprovedbyTeam,IsUpprovedbyDepartment,FinalReport,FinalStatus,SendingRemark,ReturningRemark,IsSenttoInst,IsReturned,SentReport,IsAssignedTeam,ReturnDate,SentDate")] TblInspectionPlan tblInspectionPlan)
        {
            if (ModelState.IsValid)
            {
                tblInspectionPlan.InspectionPlanId = Guid.NewGuid();
                _context.Add(tblInspectionPlan);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssigneeTypeId"] = new SelectList(_context.TblAssignementTypes, "AssigneeTypeId", "AssigneeTypeId", tblInspectionPlan.AssigneeTypeId);
            ViewData["ProId"] = new SelectList(_context.TblInspectionStatuses, "ProId", "ProId", tblInspectionPlan.ProId);
            ViewData["TeamId"] = new SelectList(_context.TblTeams, "TeamId", "TeamId", tblInspectionPlan.TeamId);
            ViewData["UserId"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInspectionPlan.UserId);
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "YearId", tblInspectionPlan.YearId);
            return View(tblInspectionPlan);
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblInspectionPlans == null)
            {
                return NotFound();
            }

            var tblInspectionPlan = await _context.TblInspectionPlans.FindAsync(id);
            if (tblInspectionPlan == null)
            {
                return NotFound();
            }
            ViewData["AssigneeTypeId"] = new SelectList(_context.TblAssignementTypes, "AssigneeTypeId", "AssigneeTypeId", tblInspectionPlan.AssigneeTypeId);
            ViewData["ProId"] = new SelectList(_context.TblInspectionStatuses, "ProId", "ProId", tblInspectionPlan.ProId);
            ViewData["TeamId"] = new SelectList(_context.TblTeams, "TeamId", "TeamId", tblInspectionPlan.TeamId);
            ViewData["UserId"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInspectionPlan.UserId);
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "YearId", tblInspectionPlan.YearId);
            return View(tblInspectionPlan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("InspectionPlanId,PlanTitle,PlanDescription,CreationDate,ModificationDate,UserId,YearId,ProId,AssigningRemark,TeamId,AssigneeTypeId,IsAssignedToUser,IsUprovedByDeputy,IsUpprovedbyTeam,IsUpprovedbyDepartment,FinalReport,FinalStatus,SendingRemark,ReturningRemark,IsSenttoInst,IsReturned,SentReport,IsAssignedTeam,ReturnDate,SentDate")] TblInspectionPlan tblInspectionPlan)
        {
            if (id != tblInspectionPlan.InspectionPlanId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblInspectionPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblInspectionPlanExists(tblInspectionPlan.InspectionPlanId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssigneeTypeId"] = new SelectList(_context.TblAssignementTypes, "AssigneeTypeId", "AssigneeTypeId", tblInspectionPlan.AssigneeTypeId);
            ViewData["ProId"] = new SelectList(_context.TblInspectionStatuses, "ProId", "ProId", tblInspectionPlan.ProId);
            ViewData["TeamId"] = new SelectList(_context.TblTeams, "TeamId", "TeamId", tblInspectionPlan.TeamId);
            ViewData["UserId"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInspectionPlan.UserId);
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "YearId", tblInspectionPlan.YearId);
            return View(tblInspectionPlan);
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblInspectionPlans == null)
            {
                return NotFound();
            }

            var tblInspectionPlan = await _context.TblInspectionPlans
                .Include(t => t.AssigneeType)
                .Include(t => t.Pro)
                .Include(t => t.Team)
                .Include(t => t.User)
                .Include(t => t.Year)
                .FirstOrDefaultAsync(m => m.InspectionPlanId == id);
            if (tblInspectionPlan == null)
            {
                return NotFound();
            }

            return View(tblInspectionPlan);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblInspectionPlans == null)
            {
                return Problem("Entity set 'AtsdbContext.TblInspectionPlans'  is null.");
            }
            var tblInspectionPlan = await _context.TblInspectionPlans.FindAsync(id);
            if (tblInspectionPlan != null)
            {
                _context.TblInspectionPlans.Remove(tblInspectionPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblInspectionPlanExists(Guid id)
        {
            return (_context.TblInspectionPlans?.Any(e => e.InspectionPlanId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> DownloadEvidenceFile(string path)
        {
            string filename = path.Substring(7);
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "Files\\", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }
        public async Task<IActionResult> Reply(int RecId)
        {
            ViewData["Replies"] = _context.TblInspectionReplyes.Include(s => s.Rec).Include(s => s.InternalUserNavigation).Include(s => s.ExternalUserNavigation).Where(s => s.RecId == RecId).OrderByDescending(s => s.ReplyId).ToList();
            ReplyModel model = new ReplyModel();
            model.RecId = RecId;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reply(ReplyModel replyModel)
        {
            ReplyModel model = new ReplyModel();
            model.RecId = replyModel.RecId;
            TblInspectionReplye replys = new TblInspectionReplye();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var deputyEmail = _context.TblInternalUsers.Where(s => s.IsDeputy == true && (s.IsDepartmentHead == true || s.Dep.DepCode == "")).Select(s => s.EmailAddress).ToList();
            replys.RecoDetail = replyModel.ResponseDetail;
            replys.DateCreated = DateTime.Now;
            replys.IsInternal = false;
            replys.IsExternal = true;
            replys.RecId = replyModel.RecId;
            replys.ExternalUser = userId;
            string? dbPath = null;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "admin/Files");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (model.Attachement != null)
            {
                FileInfo fileInfo = new FileInfo(model.Attachement.FileName);
                string fileName = Guid.NewGuid().ToString() + model.Attachement.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.Attachement.CopyTo(stream);
                }
                dbPath = "/admin/Files/" + fileName;
                replys.Attachement = dbPath;
            }
            _context.TblInspectionReplyes.Add(replys);
            int sent = _context.SaveChanges();
            if (sent > 0)
            {
                ViewData["Replies"] = _context.TblInspectionReplyes.Include(s => s.Rec).Include(s => s.InternalUserNavigation).Include(s => s.ExternalUserNavigation).Where(s => s.RecId == replyModel.RecId).OrderByDescending(s => s.ReplyId).ToList();
                await SendMail(deputyEmail, "", "");
                _notifyService.Success("Recomendation response is successfully replied");
                return View(model);
            }
            else
            {
                ViewData["Replies"] = _context.TblInspectionReplyes.Include(s => s.Rec).Include(s => s.InternalUserNavigation).Include(s => s.ExternalUserNavigation).Where(s => s.RecId == replyModel.RecId).OrderByDescending(s => s.ReplyId).ToList();
                _notifyService.Error("Recomendation response isn't succeesfully replied. Please try again");
                return View(replyModel);
            }

        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }

        public async Task<IActionResult> EditReply(int ReplyId)
        {
            var reply = _context.TblInspectionReplyes.Find(ReplyId);
            ReplyModel replyModel = new ReplyModel();
            replyModel.ReplyId = ReplyId;
            replyModel.RecId = reply.RecId;
            replyModel.ResponseDetail = reply.RecoDetail;

            return View(replyModel);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditReply(ReplyModel model)
        {
            try
            {
                var reply = _context.TblInspectionReplyes.Find(model.ReplyId);
                string? dbPath = null;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "admin/Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (model.Attachement != null)
                {
                    FileInfo fileInfo = new FileInfo(model.Attachement.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.Attachement.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.Attachement.CopyTo(stream);
                    }
                    dbPath = "/admin/Files/" + fileName;
                    reply.Attachement = dbPath;
                }
                reply.RecoDetail = model.ResponseDetail;
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    _notifyService.Success("Reply is uppdated successfully");
                    return RedirectToAction(nameof(Reply), new { RecId = model.RecId });
                }
                _notifyService.Error("Reply ism't updated successfully. Please try again");
                return View(model);
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error:{ex.Message} happened. Reply isn't updated successfully. Please try again");
                return View(model);
            }

        }

    }
}
