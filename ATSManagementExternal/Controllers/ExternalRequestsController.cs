using System.IO;
using NToastNotify;
using Microsoft.AspNetCore.Mvc;
using ATSManagementExternal.Models;
using ATSManagementExternal.IModels;
using Microsoft.EntityFrameworkCore;
using ATSManagementExternal.Filters;
using ATSManagementExternal.ViewModels;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagementExternal.Controllers
{
    [CheckSessionIsAvailable]
    public class ExternalRequestsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        public ExternalRequestsController(AtsdbContext context, INotyfService notyfService, IHttpContextAccessor contextAccessor, IMailService mail)
        {
           
            _notifyService = notyfService;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
        }

        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblRequests
                                                     .Include(t => t.AssignedByNavigation)
                                                     .Include(t => t.CaseType)
                                                     .Include(t => t.Inist)
                                                     .Include(t => t.RequestedByNavigation)
                                                     .Include(t => t.CreatedByNavigation)
                                                     .Include(x => x.ExternalRequestStatus)
                                                     .Include(x => x.ServiceType)
                                                     .Include(t => t.Priority).Where(x => x.InistId == instName.InistId);

            return View(await atsdbContext.OrderByDescending(x => x.CreatedDate).ToListAsync());
        }
        public async Task<IActionResult> AssignedRequests()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblExternalRequests.Include(t => t.ExterUser).Include(t => t.Int).Include(s => s.ExternalRequestStatus).Include(x => x.DepId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> InpectionRequests()
        {

            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblExternalRequests.Include(t => t.ExterUser).Include(t => t.Int).Include(s => s.ExternalRequestStatus).Where(x => x.DepId == null);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> LegalStudies()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser user = _context.TblExternalUsers.Find(userId);
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var atsdbContext = _context.TblRequests
               .Include(t => t.AssignedByNavigation)
               .Include(t => t.DocType)
               .Include(x => x.QuestType)
               .Include(t => t.Inist)
               .Include(t => t.RequestedByNavigation)
               .Include(x => x.ExternalRequestStatus)
                .Include(x => x.DepartmentUpprovalStatusNavigation)
                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                        .Include(y => y.TeamUpprovalStatusNavigation)
               .Include(t => t.Priority).Where(x=> x.InistId == user.InistId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> NewLegalRequest()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser user = _context.TblExternalUsers.Find(userId);
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var atsdbContext = _context.TblRequests
                                   .Include(t => t.AssignedByNavigation)
                                   .Include(t => t.DocType)
                                   .Include(t => t.QuestType)
                                   .Include(t => t.Inist)
                                   .Include(t => t.RequestedByNavigation)
                                   .Include(x => x.ExternalRequestStatus)
                                   .Include(x => x.DepartmentUpprovalStatusNavigation)
                                   .Include(x => x.DeputyUprovalStatusNavigation)
                                   .Include(y => y.TeamUpprovalStatusNavigation)
                                   .Include(t => t.Priority)
                                   .Where(x => x.ExternalRequestStatus.StatusName == "New" && x.InistId == user.InistId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> CompletedLegalRequest()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser user = _context.TblExternalUsers.Find(userId);
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var atsdbContext = _context.TblRequests
               .Include(t => t.AssignedByNavigation)
               .Include(t => t.DocType)
               .Include(s => s.QuestType)
               .Include(t => t.Inist)
               .Include(t => t.RequestedByNavigation)
               .Include(x => x.ExternalRequestStatus)
                .Include(x => x.DepartmentUpprovalStatusNavigation)
                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                        .Include(y => y.TeamUpprovalStatusNavigation)
               .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "Completed");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> PendingLegalStudies()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser user = _context.TblExternalUsers.Find(userId);
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var atsdbContext = _context.TblRequests
               .Include(t => t.AssignedByNavigation)
               .Include(t => t.DocType)
               .Include(s => s.QuestType)
               .Include(t => t.Inist)
               .Include(t => t.RequestedByNavigation)
               .Include(x => x.ExternalRequestStatus)
                .Include(x => x.DepartmentUpprovalStatusNavigation)
                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                        .Include(y => y.TeamUpprovalStatusNavigation)
               .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "In Progress" && x.InistId == user.InistId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> CivilJustice()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser user = _context.TblExternalUsers.Find(userId);
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                         .Include(x => x.DepartmentUpprovalStatusNavigation)
                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.InistId == user.InistId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> PendingCivilJustice()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser user = _context.TblExternalUsers.Find(userId);
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                         .Include(x => x.DepartmentUpprovalStatusNavigation)
                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "In Progress" && x.InistId == user.InistId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> CompletedCivilJustice()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser user = _context.TblExternalUsers.Find(userId);
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                         .Include(x => x.DepartmentUpprovalStatusNavigation)
                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "Completed" && x.InistId == user.InistId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> NewCivilJustice()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser user = _context.TblExternalUsers.Find(userId);
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                         .Include(x => x.DepartmentUpprovalStatusNavigation)
                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "New" && x.InistId == user.InistId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblExternalRequests == null)
            {
                return NotFound();
            }
            var tblExternalRequest = await _context.TblExternalRequests
                .Include(t => t.ExterUser)
                .Include(t => t.Int)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblExternalRequest == null)
            {
                return NotFound();
            }
            return View(tblExternalRequest);
        }
        public IActionResult Create()
        {
            
            return View(getModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CivilJusticeExternalRequestModel model)
        {
            try
            {
                TblRequest request = new TblRequest();
                List<TblDocumentHistory> documentHistory = new List<TblDocumentHistory>();
                List<TblRequestPriorityQuestionsRelation> relations = new List<TblRequestPriorityQuestionsRelation>();
                var institutionName = (from items in _context.TblInistitutions where items.InistId == model.IntId select items.Name).FirstOrDefault();
                var users = (from user in _context.TblInternalUsers where (user.IsDepartmentHead == true || user.IsDeputy == true) select user.EmailAddress).ToList();
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "New" select items).FirstOrDefault();
                Guid statusiD = (from id in _context.TblExternalRequestStatuses where id.StatusName == "New" select id.ExternalRequestStatusId).FirstOrDefault();
                var decision = _context.TblDecisionStatuses.Where(x => x.StatusName == "Not set").FirstOrDefault();
                var requestType = _context.TblRequestTypes.Where(s => s.TypeId == model.TypeId).FirstOrDefault();

                if (model.ServiceTypeID == null)
                {
                    _notifyService.Error("Please Select Service type");
                    return View(getModel());
                }
                if (model.ServiceTypeID == Guid.Parse("F935DCD1-2651-4C64-947C-0A877F652FB5") ||
                   model.ServiceTypeID == Guid.Parse("ACB92222-4872-4A9D-8EDB-8BCD5317129A") ||
                   model.ServiceTypeID == Guid.Parse("6E00B0EA-7B4D-40C4-8289-A566FE16E88E") ||
                   model.ServiceTypeID == Guid.Parse("92FC1FC5-95D2-4250-98FB-0BA862F6DB02") ||
                   model.ServiceTypeID == Guid.Parse("C792938C-7952-488C-A23E-1A27F8B2B8E6"))
                {
                    if (model.DocumentFile.FileName == null)
                    {
                        _notifyService.Error("Please add Document file and try again");
                        return View(getModel());
                    }
                    if (model.DocId == null)
                    {
                        _notifyService.Error("Please select Document type");
                        return View(getModel());
                    }
                }

                request.RequestDetail = model.RequestDetail;
                request.InistId = model.IntId;
                request.RequestedBy = userId;
                request.CreatedDate = DateTime.Now;
                request.ExternalRequestStatusId = statusiD;
                request.DepartmentUpprovalStatus = decision.DesStatusId;
                request.TeamUpprovalStatus = decision.DesStatusId;
                request.DeputyUprovalStatus = decision.DesStatusId;
                request.UserUpprovalStatus = decision.DesStatusId;
                request.ServiceTypeId = model.ServiceTypeID;
                request.FullName = model.FullName;
                request.PhoneNumber = model.PhoneNumber;
                request.EmailAddress = model.EmailAddress;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "admin/Files");
                if (model.DocumentFile != null)
                {
                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    //get file extension
                    FileInfo fileInfo = new FileInfo(model.DocumentFile.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.DocumentFile.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.DocumentFile.CopyTo(stream);
                    }
                    string dbPath = "/admin/Files/" + fileName;
                    request.DocTypeId = model.DocId;
                    request.DocTypeId = model.DocId;
                    documentHistory.Add(new TblDocumentHistory { DocPath = dbPath, Round = model.Round, RequestId = request.RequestId });
                    request.TblDocumentHistories = documentHistory;
                }
                if (model.PrioritiesQues != null)
                {
                    foreach (var item in model.PrioritiesQues)
                    {
                        if (item.IsSelected == true)
                        {
                            relations.Add(new TblRequestPriorityQuestionsRelation { RequestId = request.RequestId, PriorityQueId = item.PriorityQueId });
                            request.PriorityId = _context.TblPriorities.Where(x => x.PriorityId == Guid.Parse("12fba758-fa2a-406a-ae64-0a561d0f5e73")).Select(x => x.PriorityId).FirstOrDefault();
                        }
                        else
                        {
                            request.PriorityId = Guid.Parse("79fa9e18-c973-40d4-b77d-1d5d31ded31f");
                        }
                    }
                    request.TblRequestPriorityQuestionsRelations = relations;
                }
                _context.TblRequests.Add(request);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Your request is submitted Successfully. Responsive body is notified by email");
                    await SendMail(users, "Request notifications from " + institutionName, "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notifyService.Error("Your request isn't successfully submitted!. Please try again");
                    return View(getModel());
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message + " happened. Please try again");
                return View(getModel());
            }
        }
        public async Task<IActionResult> Appointments()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblAppointments.Include(t => t.Inist).Include(t => t.RequestedByNavigation).Where(x => x.InistId == instName.InistId && x.RequestedBy == userId);
            return View(await atsdbContext.OrderByDescending(X => X.CreatedDate).ToListAsync());
        }
        public async Task<IActionResult> AddAppointments()
        {
            AppointmentModel model = AppointmentModel();
            return View(model);
        }
        private AppointmentModel AppointmentModel()
        {
            AppointmentModel model = new AppointmentModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            model.ExterUserId = userId;
            model.IntId = instName.InistId;
            model.AppointmentDate = DateTime.Now;
            return model;
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Addppointments(AppointmentModel model)
        {
            TblAppointment appointment = new TblAppointment();
            appointment.CreatedDate = DateTime.Now;
            appointment.AppointmentDetail = model.AppointmentDetail;
            appointment.InistId = model.IntId;
            appointment.RequestedBy = model.ExterUserId;
            _context.TblAppointments.Add(appointment);
            int saved = _context.SaveChanges();
            if (saved > 0)
            {
                _notifyService.Success("Your Appointment is submitted successfully!");
                return View(AppointmentModel());
            }
            else
            {
                _notifyService.Error("Your request isn't submitted successfully!. Please try again.");
                return View(model);
            }
        }
        public async Task<IActionResult> EditAppointments(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            TblAppointment appointment = await _context.TblAppointments.FindAsync(id);
            AppointmentModel model = new AppointmentModel();
            model.AppointmentDetail = appointment.AppointmentDetail;
            model.AppointmentDate = appointment.AppointmentDate;
            model.ExterUserId = appointment.RequestedBy;
            model.IntId = appointment.InistId;
            model.AppointmentId = id;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> EditAppointments(AppointmentModel appointmentModel)
        {
            if (appointmentModel.AppointmentId == null)
            {
                return NotFound();
            }
            TblAppointment appointment = await _context.TblAppointments.FindAsync(appointmentModel.AppointmentId);
            AppointmentModel model = new AppointmentModel();
            appointment.AppointmentDetail = appointmentModel.AppointmentDetail;
            appointment.AppointmentDate = appointmentModel.AppointmentDate;
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Your Appointment is uppdated successfully!");
                return RedirectToAction(nameof(Appointments));
            }
            else
            {
                _notifyService.Error("Your request isn't submitted successfully!. Please try again.");
                return View(model);
            }
        }
        public CivilJusticeExternalRequestModel getModel()
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            model.RequestedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
            model.ExterUserId = userId;
            model.Round = 1;
            model.IntId = instName.InistId;
            model.Deparments = _context.TblDepartments.Select(a => new SelectListItem
            {
                Text = a.DepName,
                Value = a.DepId.ToString()
            }).ToList();
            model.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
            {
                Value = s.ServiceTypeId.ToString(),
                Text = s.ServiceTypeName
            }).ToList();
            model.RequestTypes = _context.TblRequestTypes.Select(r => new SelectListItem
            {
                Text = r.TypeName,
                Value = r.TypeId.ToString()
            }).ToList();
            model.LegalStadiesCasetypes = _context.TblLegalDraftingDocTypes.Select(s => new SelectListItem
            {
                Value = s.DocId.ToString(),
                Text = s.DocName.ToString()
            }).ToList();
            model.LegalStadiesQuestiontypes = _context.TblLegalDraftingQuestionTypes.Select(x => new SelectListItem
            {
                Value = x.QuestTypeId.ToString(),
                Text = x.QuestTypeName.ToString()
            }).ToList();
            model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
            {
                Value = x.CaseTypeId.ToString(),
                Text = x.CaseTypeName.ToString()
            }).ToList();
            model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
            {
                Value = x.PriorityId.ToString(),
                Text = x.PriorityName
            }).ToList();
            model.PrioritiesQues = _context.TblPriorityQuestions.Select(x => new CheckBoxItem
            {
                PriorityQueId = x.PriorityQueId,
                Title = x.QuestionName,
                IsSelected = false
            }).ToList();
            model.TermsAndCondionts= _context.TblTermsAndConditions.Select(s => s.Details).FirstOrDefault();
            return model;
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            if (id == null || _context.TblExternalRequests == null)
            {
                return NotFound();
            }

            var tblExternalRequest = await _context.TblExternalRequests.FindAsync(id);
            if (tblExternalRequest == null)
            {
                return NotFound();
            }
            model.ExterUserId = tblExternalRequest.ExterUserId;
            model.IntId = tblExternalRequest.IntId;
            model.RequestedDate = tblExternalRequest.RequestedDate;
            model.RequestId = tblExternalRequest.RequestId;
            model.RequestDetail = tblExternalRequest.RequestDetail;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CivilJusticeExternalRequestModel model)
        {
            try
            {
                TblExternalRequest tblExternalRequest = await _context.TblExternalRequests.FindAsync(model.RequestId);
                if (tblExternalRequest == null)
                {
                    return NotFound();
                }
                tblExternalRequest.RequestDetail = model.RequestDetail;

                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblExternalRequestExists(model.RequestId))
                {
                    return NotFound();
                }
                else
                {
                    return View(model);
                }
            }
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblExternalRequests == null)
            {
                return NotFound();
            }

            var tblExternalRequest = await _context.TblExternalRequests
                .Include(t => t.ExterUser)
                .Include(t => t.Int)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblExternalRequest == null)
            {
                return NotFound();
            }
            return View(tblExternalRequest);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblExternalRequests == null)
            {
                return Problem("Entity set 'AtsdbContext.TblExternalRequests'  is null.");
            }
            var tblExternalRequest = await _context.TblExternalRequests.FindAsync(id);
            if (tblExternalRequest != null)
            {
                _context.TblExternalRequests.Remove(tblExternalRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblExternalRequestExists(Guid id)
        {
            return (_context.TblExternalRequests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
        public async Task<IActionResult> Replies(Guid? id)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var replays = await _context.TblReplays.Where(a => a.RequestId == id&&a.IsSent==true).ToListAsync();
            RepliesModel model = new RepliesModel
            {
                RequestId = id,
                ReplyDate = DateTime.Now,
                ExternalReplayedBy = userId,
            };
            ViewData["Replies"] = _context.TblReplays
                .Include(x => x.InternalReplayedByNavigation)
                .Include(x => x.ExternalReplayedByNavigation)
                .Include(x => x.Request)
                .Where(y => y.RequestId == id && y.IsSent == true).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Replies(RepliesModel model)
        {
            try
            {
                TblReplay replay = new();
                replay.ReplyDate = DateTime.Now;
                replay.ExternalReplayedBy = model.ExternalReplayedBy;
                replay.RequestId = model.RequestId;
                replay.ReplayDetail = model.ReplayDetail;
                replay.IsSent = true;
                _context.TblReplays.Add(replay);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply submitted");
                    return RedirectToAction("Replies", new { id = model.RequestId });
                }
                else
                {
                    _notifyService.Error("Reply isn't subbmitted. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error("Reply isn't subbmitted because of " + ex.Message + ". Please try again");
                return View(model);
            }
        }
        public async Task<IActionResult> LegalReplies(Guid? id)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var replays = await _context.TblLegalStudiesReplays.Where(a => a.RequestId == id).ToListAsync();
            RepliesModel model = new RepliesModel
            {
                RequestId = id,
                ReplyDate = DateTime.Now,
                ExternalReplayedBy = userId,
            };
            ViewData["Replies"] = _context.TblReplays
                .Include(x => x.ExternalReplayedByNavigation)
                .Include(x => x.InternalReplayedBy)
                .Include(x => x.Request)
                .Where(_context => _context.RequestId == id).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> LegalReplies(RepliesModel model)
        {
            try
            {
                TblReplay replay = new TblReplay();
                replay.ReplyDate = DateTime.Now;
                replay.ExternalReplayedBy = model.ExternalReplayedBy;
                replay.RequestId = model.RequestId;
                replay.ReplayDetail = model.ReplayDetail;
                _context.TblReplays.Add(replay);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    return RedirectToAction("Replies", new { id = model.RequestId });
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                return View(model);
            }
        }
        public IActionResult CreateExternalRequest()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser user = _context.TblExternalUsers.Find(userId);
            LegalStudiesDraftingModel model = new LegalStudiesDraftingModel();
            model.Intitutions = _context.TblInistitutions.Where(x => x.InistId == user.InistId).Select(x => new SelectListItem
            {
                Value = x.InistId.ToString(),
                Text = x.Name
            }).ToList();
            model.Deparments = _context.TblDepartments.Where(x => x.DepCode == "LSDC").Select(x => new SelectListItem
            {
                Value = x.DepId.ToString(),
                Text = x.DepName
            }).Where(a => a.Text == "Legal Studies, Drafting & Consolidation").ToList();
            model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
            {
                Text = x.PriorityName,
                Value = x.PriorityId.ToString()
            }).ToList();
            model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
            {
                Value = x.DocId.ToString(),
                Text = x.DocName
            }).ToList();
            model.LegalStadiesQuestiontypes = _context.TblLegalDraftingQuestionTypes.Select(x => new SelectListItem
            {
                Value = x.QuestTypeId.ToString(),
                Text = x.QuestTypeName
            }).ToList();
            model.CreatedDate = DateTime.Now;
            model.ExterUserId = userId;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult CreateExternalRequest(LegalStudiesDraftingModel model)
        {
            try
            {
                TblLegalStudiesDrafting draftings = new TblLegalStudiesDrafting();
                Guid statusiD = (from id in _context.TblExternalRequestStatuses where id.StatusName == "New" select id.ExternalRequestStatusId).FirstOrDefault();
                var decision = _context.TblDecisionStatuses.Where(x => x.StatusName == "Not set").FirstOrDefault();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(model.DocumentFile.FileName);
                string fileName = Guid.NewGuid().ToString() + model.DocumentFile.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.DocumentFile.CopyTo(stream);
                }
                string dbPath = "/Files/" + fileName;
                draftings.RequestDetail = model.RequestDetail;
                draftings.RequestedBy = model.ExterUserId;
                draftings.CreatedDate = DateTime.Now;
                draftings.QuestTypeId = model.QuestTypeId;
                draftings.DocId = model.DocId;
                draftings.InistId = model.InistId;
                draftings.PriorityId = model.PriorityId;
                draftings.DepartmentUpprovalStatus = decision.DesStatusId;
                draftings.TeamUpprovalStatus = decision.DesStatusId;
                draftings.DeputyUprovalStatus = decision.DesStatusId;
                draftings.UserUpprovalStatus = decision.DesStatusId;
                draftings.DepId = model.DepId;
                draftings.DocumentFile = dbPath;
                draftings.ExternalRequestStatusId = statusiD;
                _context.TblLegalStudiesDraftings.Add(draftings);
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
                    {
                        Value = x.InistId.ToString(),
                        Text = x.Name
                    }).ToList();
                    model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                    {
                        Value = x.DepId.ToString(),
                        Text = x.DepName
                    }).ToList();
                    model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                    {
                        Text = x.PriorityName,
                        Value = x.PriorityId.ToString()
                    }).ToList();
                    model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
                    {
                        Value = x.DocId.ToString(),
                        Text = x.DocName
                    }).ToList();
                    model.LegalStadiesQuestiontypes = _context.TblLegalDraftingQuestionTypes.Select(x => new SelectListItem
                    {
                        Value = x.QuestTypeId.ToString(),
                        Text = x.QuestTypeName
                    }).ToList();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
                {
                    Value = x.InistId.ToString(),
                    Text = x.Name
                }).ToList();
                model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                {
                    Value = x.DepId.ToString(),
                    Text = x.DepName
                }).ToList();
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Text = x.PriorityName,
                    Value = x.PriorityId.ToString()
                }).ToList();
                model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
                {
                    Value = x.DocId.ToString(),
                    Text = x.DocName
                }).ToList();
                model.LegalStadiesQuestiontypes = _context.TblLegalDraftingQuestionTypes.Select(x => new SelectListItem
                {
                    Value = x.QuestTypeId.ToString(),
                    Text = x.QuestTypeName
                }).ToList();
                return View(model);
            }
        }
        public async Task<IActionResult> AppointmentDetail(Guid? id)
        {
            if (id == null || _context.TblAppointments == null)
            {
                return NotFound();
            }
            var tblAppointment = await _context.TblAppointments
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (tblAppointment == null)
            {
                return NotFound();
            }

            return View(tblAppointment);
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
        public async Task<IActionResult> AddHistory(Guid? id)
        {
            DocumentHistoryModel model = await historyModel(id);

            return View(model);
        }
        private async Task<DocumentHistoryModel> historyModel(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblDocumentHistory tblDocument =  _context.TblDocumentHistories.Include(x=>x.Request).Where(x => x.RequestId == id).OrderBy(x=>x.Round).Last();
            DocumentHistoryModel model = new DocumentHistoryModel();
            ViewData["histories"] = _context.TblDocumentHistories.Where(x => x.RequestId == id).ToList();
            model.RequestId = id;            
            model.ExternalRepliedBy = userId;
            if (tblDocument.Round==null)
            {
                model.Round = 1;
            }
            else
            {
                model.Round =Convert.ToInt32(tblDocument.Round) + 1;
            }            
            return model;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddHistory(DocumentHistoryModel? model)
        {
            List<string> emails=new List<string>();


            var users = (from user in _context.TblRequestAssignees
                         join assignees in _context.TblInternalUsers on user.UserId equals assignees.UserId
                          where (assignees.IsDepartmentHead == true || assignees.IsDeputy == true )&&user.RequestId==model.RequestId select assignees.EmailAddress).ToList();
            if (users.Count!=0)
            {
                emails = users;
            }
            else
            {
                emails= (from  assignees in _context.TblInternalUsers 
                         where assignees.IsDepartmentHead == true || assignees.IsDeputy == true
                         select assignees.EmailAddress).ToList();
            }
            var institutionName = (from items in _context.TblRequests where items.RequestId == model.RequestId select items.Inist.Name).FirstOrDefault();

            TblDocumentHistory history = new TblDocumentHistory();
            history.ExternalRepliedBy = model.ExternalRepliedBy;
            history.Round = model.Round;
            history.Description = model.Description;
            history.FileDescription= model.FileDescription;
            history.RequestId = model.RequestId;
            history.CreatedDate=DateTime.Now;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (model.DocPath != null)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);               
                FileInfo fileInfo = new FileInfo(model.DocPath.FileName);
                string fileName = Guid.NewGuid().ToString() + model.DocPath.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.DocPath.CopyTo(stream);
                }
                string dbPath = "/Files/" + fileName;
                history.DocPath = dbPath;
            }

            _context.TblDocumentHistories.Add(history);
            int saved = await _context.SaveChangesAsync();
            if (saved>0)
            {
                await SendMail(users, "Request notifications from " + institutionName, "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");
                _notifyService.Success("Document added successfully!");
                return RedirectToAction(nameof(AddHistory));
            }
            else
            {
                _notifyService.Error("Document is not successfully added. Please try again");
                return View(model);
            }           
        }
        public async Task<IActionResult> ContinuationRequests()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblRequests
                                                     .Include(t => t.AssignedByNavigation)
                                                     .Include(t => t.CaseType)
                                                     .Include(t => t.Inist)
                                                     .Include(t => t.RequestedByNavigation)
                                                     .Include(t => t.CreatedByNavigation)
                                                     .Include(x => x.ExternalRequestStatus)
                                                     .Include(x => x.ServiceType)
                                                     .Include(t => t.Priority).Where(x => x.InistId == instName.InistId);
            return View(await atsdbContext.OrderByDescending(x => x.CreatedDate).ToListAsync());
        }
    }
}
