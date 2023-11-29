namespace ATSManagement.Controllers
{
    using NToastNotify;
    using ATSManagement.Models;
    using ATSManagement.Filters;
    using ATSManagement.IModels;
    using ATSManagement.ViewModels;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.AspNetCore.StaticFiles;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using AspNetCoreHero.ToastNotification.Abstractions;

    [CheckSessionIsAvailable]
    public class AssignedYearlyPlansController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotyfService _notifyService;
        private readonly IMailService _mail;
        public AssignedYearlyPlansController(AtsdbContext context, IHttpContextAccessor contextAccessor, INotyfService toastNotification, IMailService mail)
        {
            _notifyService = toastNotification;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
        }
        public async Task<IActionResult> Index(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));

            var atsdbContext = _context.TblAssignedYearlyPlans.Include(t => t.AssignedToNavigation).Include(t => t.Plan).Include(p => p.Status).Include(s=>s.AssignedByNavigation).Where(a => a.AssignedTo == userId);
            return View(await atsdbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblAssignedYearlyPlans == null)
            {
                return NotFound();
            }

            var tblAssignedYearlyPlan = await _context.TblAssignedYearlyPlans
                .Include(t => t.AssignedToNavigation)
                .Include(t => t.Plan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblAssignedYearlyPlan == null)
            {
                return NotFound();
            }

            return View(tblAssignedYearlyPlan);
        }
        public IActionResult Create(Guid? id)
        {
            ViewData["AssignedTo"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["PlanId"] = new SelectList(_context.TblInspectionPlans, "InspectionPlanId", "InspectionPlanId");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,AssignedTo,AssignedBy,PlanId,AssignedDate,DueDate,EvaluationCheckLists,EngagementLetter,MeetingLetter,Remark,ProgressStatus")] TblAssignedYearlyPlan tblAssignedYearlyPlan)
        {
            if (ModelState.IsValid)
            {
                tblAssignedYearlyPlan.Id = Guid.NewGuid();
                _context.Add(tblAssignedYearlyPlan);
             int saved=   await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedTo"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblAssignedYearlyPlan.AssignedTo);
            ViewData["PlanId"] = new SelectList(_context.TblInspectionPlans, "InspectionPlanId", "InspectionPlanId", tblAssignedYearlyPlan.PlanId);
            return View(tblAssignedYearlyPlan);
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblAssignedYearlyPlans == null)
            {
                return NotFound();
            }

            var tblAssignedYearlyPlan = await _context.TblAssignedYearlyPlans.FindAsync(id);
            if (tblAssignedYearlyPlan == null)
            {
                return NotFound();
            }
            ViewData["AssignedTo"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblAssignedYearlyPlan.AssignedTo);
            ViewData["PlanId"] = new SelectList(_context.TblInspectionPlans, "InspectionPlanId", "InspectionPlanId", tblAssignedYearlyPlan.PlanId);
            return View(tblAssignedYearlyPlan);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,AssignedTo,AssignedBy,PlanId,AssignedDate,DueDate,EvaluationCheckLists,EngagementLetter,MeetingLetter,Remark,ProgressStatus")] TblAssignedYearlyPlan tblAssignedYearlyPlan)
        {
            if (id != tblAssignedYearlyPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblAssignedYearlyPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblAssignedYearlyPlanExists(tblAssignedYearlyPlan.Id))
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
            ViewData["AssignedTo"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblAssignedYearlyPlan.AssignedTo);
            ViewData["PlanId"] = new SelectList(_context.TblInspectionPlans, "InspectionPlanId", "InspectionPlanId", tblAssignedYearlyPlan.PlanId);
            return View(tblAssignedYearlyPlan);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblAssignedYearlyPlans == null)
            {
                return NotFound();
            }

            var tblAssignedYearlyPlan = await _context.TblAssignedYearlyPlans
                .Include(t => t.AssignedToNavigation)
                .Include(t => t.Plan)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblAssignedYearlyPlan == null)
            {
                return NotFound();
            }

            return View(tblAssignedYearlyPlan);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblAssignedYearlyPlans == null)
            {
                return Problem("Entity set 'AtsdbContext.TblAssignedYearlyPlans'  is null.");
            }
            var tblAssignedYearlyPlan = await _context.TblAssignedYearlyPlans.FindAsync(id);
            if (tblAssignedYearlyPlan != null)
            {
                _context.TblAssignedYearlyPlans.Remove(tblAssignedYearlyPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblAssignedYearlyPlanExists(Guid id)
        {
            return (_context.TblAssignedYearlyPlans?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public IActionResult AddCheckList(Guid? id)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);          
            var yearlyPlan = _context.TblInspectionPlans.Find(assignedTasks.PlanId);
            model.Id = assignedTasks.Id;
            model.AssignedBy= assignedTasks.AssignedBy;
            model.PlanTitle = yearlyPlan.PlanTitle;
            model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCheckList(InspectionAssignModel? model)
        {
            var emails = _context.TblInternalUsers.Where(s => s.UserId == model.AssignedBy).Select(s=>s.EmailAddress).ToList();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
            assignedTasks.EvaluationCheckLists = model.EvaluationCheckLists;
            int added = _context.SaveChanges();
            if (added > 0)
            {
                await SendMail(emails, "Notification from Task Tracking Dashboard", "Expert is added Evaluation criteria for his assigned task. Please review on system.");
                _notifyService.Success("Check lists are successfully added");
                return RedirectToAction("Index");
            }
            else
            {
                _notifyService.Error("Check Lists aren't successfully created. Please try again");
                return View(model);
            }           
        }
        public IActionResult AddEngagementLetter(Guid? id)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            var yearlyPlan = _context.TblInspectionPlans.Find(assignedTasks.PlanId);
            model.Id = assignedTasks.Id;
            model.AssignedBy = assignedTasks.AssignedBy;
            model.PlanTitle = yearlyPlan.PlanTitle;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> AddEngagementLetter(InspectionAssignModel? model)
        {
            try
            {
                var emails = _context.TblInternalUsers.Where(s => s.UserId == model.AssignedBy).Select(s => s.EmailAddress).ToList();

                TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(model.EngagementLetter.FileName);
                string fileName = Guid.NewGuid().ToString() + model.EngagementLetter.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.EngagementLetter.CopyTo(stream);
                }
                string dbPath = "/Files/" + fileName;
                assignedTasks.Remark = model.Remark;
                assignedTasks.EngagementLetter = dbPath;
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    await SendMail(emails, "Notification from Task Tracking Dashboard", "Expert added Engagement letter for his assigned task. Please review on system.");
                    _notifyService.Success("Engagement letter is successfully added.");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notifyService.Error("Engagement letter isn't added successfully. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                _notifyService.Error($"Error:{ex.Message} happened. Engagement letter isn't added successfully. Please try again");
                return View(model);
            }
         
        }
        public IActionResult AddMeetingLetter(Guid? id)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            var yearlyPlan = _context.TblInspectionPlans.Find(assignedTasks.PlanId);
            model.Id = assignedTasks.Id;
            model.PlanTitle = yearlyPlan.PlanTitle;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult AddMeetingLetter(InspectionAssignModel? model)
        {
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //get file extension
            FileInfo fileInfo = new FileInfo(model.MeetingLetter.FileName);
            string fileName = Guid.NewGuid().ToString() + model.MeetingLetter.FileName;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.MeetingLetter.CopyTo(stream);
            }
            string dbPath = "/Files/" + fileName;
            assignedTasks.Remark = model.Remark;
            assignedTasks.MeetingLetter = dbPath;
            int updated = _context.SaveChanges();
            if (updated > 0)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }
        public IActionResult UpdateProgressStatus(Guid? id)
        {
           
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            var yearlyPlan = _context.TblInspectionPlans.Find(assignedTasks.PlanId);
            model.AssignedBy = assignedTasks.AssignedBy;
            model.Id = assignedTasks.Id;
            model.PlanTitle = yearlyPlan.PlanTitle;
            model.Remark = assignedTasks.Remark;
            model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
            model.status = _context.TblStatuses.Where(a => a.Status == "Done" || a.Status == "Pending").Select(p => new SelectListItem
            {
                Value = p.StatusId.ToString(),
                Text = p.Status,
                Selected = p.StatusId == assignedTasks.StatusId ? true : false

            }).ToList();
            ViewBag.StatusId = new SelectList(_context.TblStatuses.Where(a => a.Status == "Done" || a.Status == "Pending").ToList(), "StatusId", "Status", assignedTasks.StatusId);

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task< IActionResult> UpdateProgressStatus(InspectionAssignModel? model)
        {
            try
            {
                var emails = _context.TblInternalUsers.Where(s => s.UserId == model.AssignedBy).Select(s => s.EmailAddress).ToList();

                TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);
                var yearlyPlan = _context.TblInspectionPlans.Find(assignedTasks.PlanId);
                if (model.StatusID == Guid.Parse("47fc4e29-8bff-4a7b-90d8-c58ca5b7c7e8") && assignedTasks.FinalReport == null)
                {
                    model.Id = assignedTasks.Id;
                    model.PlanTitle = yearlyPlan.PlanTitle;
                    model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
                    model.status = _context.TblStatuses.Where(a => a.Status == "Done" || a.Status == "Pending").Select(p => new SelectListItem
                    {
                        Value = p.StatusId.ToString(),
                        Text = p.Status

                    }).ToList();
                    ViewBag.StatusId = new SelectList(_context.TblStatuses.Where(a => a.Status == "Done" || a.Status == "Pending").ToList(), "StatusId", "Status", assignedTasks.StatusId);

                    return View(model);
                }
                assignedTasks.ProgressStatus = model.ProgressStatus;
                yearlyPlan.StatusId = model.StatusID;
                assignedTasks.Remark = model.Remark;
                assignedTasks.StatusId = model.StatusID;
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    await SendMail(emails, "Notification from Task Tracking Dashboard", "Expert uppdated progress status for his assigned task. Please review on system.");
                    _notifyService.Success("Progress is successfully updated.");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notifyService.Error("Progress isn't successfully uppdated. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error:{ex.Message} happened. Progress isn't successfully uppdated. Please try again");
                return View(model);
            }          
        }
        public IActionResult AddFinalReport(Guid? id)
        {
            InspectionAssignModel model = new InspectionAssignModel();
            TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(id);
            var yearlyPlan = _context.TblInspectionPlans.Find(assignedTasks.PlanId);
            model.Id = assignedTasks.Id;
            model.PlanTitle = yearlyPlan.PlanTitle;
            model.Remark = assignedTasks.Remark;
            model.EvaluationCheckLists = assignedTasks.EvaluationCheckLists;
            model.status = _context.TblStatuses.Where(a => a.Status == "Done" || a.Status == "Pending").Select(p => new SelectListItem
            {
                Value = p.StatusId.ToString(),
                Text = p.Status,
                Selected = p.StatusId == assignedTasks.StatusId ? true : false

            }).ToList();
            ViewBag.StatusId = new SelectList(_context.TblStatuses.Where(a => a.Status == "Done" || a.Status == "Pending").ToList(), "StatusId", "Status", assignedTasks.StatusId);

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task< IActionResult> AddFinalReport(InspectionAssignModel? model)
        {
            try
            {
                var emails = _context.TblInternalUsers.Where(s => s.UserId == model.AssignedBy).Select(s => s.EmailAddress).ToList();

                TblAssignedYearlyPlan assignedTasks = _context.TblAssignedYearlyPlans.Find(model.Id);

                if (assignedTasks.EngagementLetter==null||assignedTasks.EvaluationCheckLists==null)
                {
                    _notifyService.Error("Before Upploading final report you should add evaluation criteria and Engagement letter");
                    return View(model);
                }
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(model.FinalReport.FileName);
                string fileName = Guid.NewGuid().ToString() + model.FinalReport.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.FinalReport.CopyTo(stream);
                }
                string dbPath = "/Files/" + fileName;
                assignedTasks.FinalReport = dbPath;
                int updated = _context.SaveChanges();
                if (updated > 0)
                {
                    await SendMail(emails, "Notification from Task Tracking Dashboard", "Expert is added Engagement letter for his assigned task. Please review on system.");

                    _notifyService.Success("Final report is successfully upploaded");
                    return RedirectToAction("Index");
                }
                else
                {
                    _notifyService.Error("Report isn't successfully upploaded");
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                _notifyService.Error($"Error:{ex.Message} happened. Report isn't successfully upploaded");
                return View(model);
            }
        
        }
        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string path)
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
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }


    }
}
