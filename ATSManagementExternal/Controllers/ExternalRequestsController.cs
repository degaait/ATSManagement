using System.Data;
using MailKit.Search;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ATSManagementExternal.Models;
using ATSManagementExternal.Filters;
using ATSManagementExternal.IModels;
using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
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
        private readonly INotificationService _notificationService;
        public ExternalRequestsController(AtsdbContext context, INotyfService notyfService, IHttpContextAccessor contextAccessor, IMailService mail, INotificationService notificationService)
        {

            _notifyService = notyfService;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
            _notificationService = notificationService;
        }
        public async Task<IActionResult> Index()
        {
            string errorMessage = _contextAccessor.HttpContext.Session.GetString("errorMessage");
            string successMessage=_contextAccessor.HttpContext.Session.GetString("successMessage");
            if (successMessage!=null)
            {
                _notifyService.Success("Request successfully sent");
                _contextAccessor.HttpContext.Session.Remove("successMessage");
            }
            if (errorMessage!=null)
            {
                _notifyService.Error("Request isn't successfully sent. Please try again");
                _contextAccessor.HttpContext.Session.Remove("errorMessage");
            }
           
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var InistId = new SqlParameter("@InistId", SqlDbType.UniqueIdentifier)
            {
                Value = instName.InistId
            };
            var status = new SqlParameter("@StatusName", SqlDbType.NVarChar) { 
            Value= "Completed"
            } ;
           var result = _context.RequestViewModels.FromSqlRaw($"EXEC GetExternalRequests @InistId,@StatusName", InistId, status)
            .ToList(); //Materialize the result



            return View(result);
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
               .Include(t => t.Priority).Where(x => x.InistId == user.InistId);
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
            return View(await atsdbContext.OrderByDescending(s => s.OrderId).ToListAsync());
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
            if (id == null || _context.TblRequests == null)
            {
                return NotFound();
            }

            var tblRequest = await _context.TblRequests
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.DepartmentUpprovalStatusNavigation)
                .Include(t => t.DeputyUprovalStatusNavigation)
                .Include(t => t.DocType)
                .Include(t => t.ExternalRequestStatus)
                .Include(t => t.Inist)
                .Include(t => t.Priority)
                .Include(t => t.QuestType)
                .Include(t => t.RequestedByNavigation)
                .Include(t => t.ServiceType)
                .Include(t => t.TeamUpprovalStatusNavigation)
                .Include(t => t.UserUpprovalStatusNavigation)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblRequest == null)
            {
                return NotFound();
            }

            return View(tblRequest);
        }
        public IActionResult Create()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser? instName = _context.TblExternalUsers.FindAsync(userId).Result;
            if (instName.AcceptedTerms == null || instName.AcceptedTerms == false)
            {
                ViewBag.accepted = "false";
            }
            else
            {
                ViewBag.accepted = "true";
            }
            return View(getModel());
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Create(CivilJusticeExternalRequestModel model)
        {
            try
            {
                
                string errorMessage = null;
                string successMessage = null;
                List<string>? users = new List<string>();
                string country = null;
                var deputy = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.IsDeputy == true).Select(S => S.UserId).ToList();
                if (model.CountryId == null)
                {
                    country = "Ethiopia";
                }
                else
                {
                    country = _context.TblCountries.Find(model.CountryId).CountryName;
                }
                TblRequest request = new TblRequest();
                List<TblDocumentHistory> documentHistory = new List<TblDocumentHistory>();
                List<TblRequestPriorityQuestionsRelation> relations = new List<TblRequestPriorityQuestionsRelation>();
                TblTopStatus topStatus = _context.TblTopStatuses.Where(s => s.StatusName == "New").FirstOrDefault();
                var institutionName = (from items in _context.TblInistitutions where items.InistId == model.IntId select items.Name).FirstOrDefault();
           
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                TblExternalUser externalUser = _context.TblExternalUsers.Find(userId);
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



                    users = (from user in _context.TblInternalUsers
                             join deps in _context.TblDepartments on user.DepId equals deps.DepId
                             where (user.IsDepartmentHead == true && deps.DepCode == "LSDC") || user.IsDeputy == true
                             select user.EmailAddress).ToList();
                    if (model.MultipleFiles == null)
                    {
                        _notifyService.Error("Please add Document file and try again");
                        return View(getModel());
                    }
                    if (model.DocId == null)
                    {
                        _notifyService.Error("Please select Document type");
                        return View(getModel());
                    }
                    request.DocTypeId = model.DocId;
                }
                else
                {
                    users = (from user in _context.TblInternalUsers
                             join deps in _context.TblDepartments on user.DepId equals deps.DepId
                             where (user.IsDepartmentHead == true && deps.DepCode== "CVA") || user.IsDeputy == true select user.EmailAddress).ToList();
                }
                request.RequestDetail = model.RequestDetail;
                request.InistId = model.IntId;
                request.RequestedBy = externalUser.ExterUserId;
                request.CreatedDate = DateTime.Now;
                if (model.MoneyAmount != null)
                {
                    request.MoneyAmount = int.Parse(model.MoneyAmount);
                    request.MoneyCurrency = model.CurrencyId;
                }
                request.TopStatusId = topStatus.TopStatusId;
                request.ExternalRequestStatusId = statusiD;
                request.DepartmentUpprovalStatus = decision.DesStatusId;
                request.TeamUpprovalStatus = decision.DesStatusId;
                request.DeputyUprovalStatus = decision.DesStatusId;
                request.UserUpprovalStatus = decision.DesStatusId;
                request.ServiceTypeId = model.ServiceTypeID;
                request.FullName = model.FullName;
                request.PhoneNumber = model.PhoneNumber;
                request.EmailAddress = model.EmailAddress;
                request.ContractNeServiceType = model.ContractNegotiationId;
                request.ListigationStatus = model.LitigationStatusID;
                request.ListigationResult = model.LitigationResultID;
                request.LegalAdviceResult = model.LegalAdviceResultID;
                request.LegalAdviceStatus = model.LegalAdviceStatusID;
                request.ContractNeResult = model.ContractNegotiationResultId;
                request.ContractNeStatus = model.ContractNegotiationStatusId;
                request.InternationalCaseResult = model.InternationalCaseResultID;
                request.InternationalCaseStatus = model.InternationalCaseStatusID;
                request.AdrResult = model.AdrResultID;
                request.AdrStatus = model.AdrStatusId;
                request.OtherDocumentType = model.OtherDocumentType;
                request.OtherServiceType = model.OtherServiceType;
                request.Specialization = model.SpecializationId;
                request.Adrtype = model.ADRTypeId;
                request.ActingAs = model.ActingAsId;
                request.Bench = model.Bench;
                request.Respondent = model.Respondent;
                request.Claimant = model.Claimant;
                request.Country = country;
                request.DateOfAdjournment = model.DateOfAdjournment;
                request.CaseType = model.CaseTypeID;
                request.Defendent = model.Defendent;
                request.Plaintiful = model.Plaintiful;
                request.DateofJudgement = model.DateofJudgement;
                request.Jursidiction = model.JuristrictionId;
                request.LitigationType = model.LitigationtypeId;
                request.CourtCenter = model.CourtCenter;
                request.SecretaryFullName= externalUser.FirstName+" "+ externalUser.LastName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "admin/Files");
                foreach (var item in model.MultipleFiles)
                {
                    if (item.Length > 0)
                    {
                        if (item.FileName != null)
                        {
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            FileInfo fileInfo = new FileInfo(item.FileName);
                            string ExactFileName = item.FileName;
                            string fileName = Guid.NewGuid().ToString()+ item.FileName;
                            string fileNameWithPath = Path.Combine(path, fileName);
                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            string dbPath = "/admin/Files/" + fileName;
                            request.DocTypeId = model.DocId;
                            documentHistory.Add(new TblDocumentHistory { DocPath = dbPath, Round = model.Round, RequestId = request.RequestId, ExactFileName = ExactFileName,FileDescription= ExactFileName });

                        }
                    }
                }
                request.TblDocumentHistories = documentHistory;
                var selected = model.PrioritiesQues.Where(s => s.IsSelected == true).ToList();
                if (selected.Count != 0)
                {
                    request.PriorityId = _context.TblPriorities.Where(x => x.PriorityId == Guid.Parse("12fba758-fa2a-406a-ae64-0a561d0f5e73")).Select(x => x.PriorityId).FirstOrDefault();
                    foreach (var item in selected)
                    {
                        if (item.IsSelected == true)
                        {
                            relations.Add(new TblRequestPriorityQuestionsRelation { RequestId = request.RequestId, PriorityQueId = item.PriorityQueId });
                        }
                    }
                    request.TblRequestPriorityQuestionsRelations = relations;
                }
                else
                {
                    request.PriorityId = Guid.Parse("79fa9e18-c973-40d4-b77d-1d5d31ded31f");
                }
                _context.TblRequests.Add(request);
                if (externalUser.AcceptedTerms == false || externalUser.AcceptedTerms == null)
                {
                    externalUser.AcceptedTerms = true;
                }
                else
                {
                    externalUser.AcceptedTerms = true;
                }
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                    var existingRequ = _context.TblRounds.Where(s => s.RequestIid == request.RequestId).FirstOrDefault();
                    TblRound roundS = new TblRound();
                    roundS.RequestIid = request.RequestId;
                    if (existingRequ != null)
                    {
                        roundS.RoundNumber = existingRequ.RoundNumber + 1;
                    }
                    roundS.RoundNumber = 1;
                    _context.TblRounds.Add(roundS);
                    _context.SaveChanges();
                    successMessage = "Your request is submitted Successfully. Responsive body is notified by email";
                    _contextAccessor.HttpContext.Session.SetString("successMessage", successMessage);
                    await SendMail(users, "Request notifications from " + institutionName, "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");
                    _notificationService.saveNotification(userId, deputy, "New service request is added from " + institutionName);
                    return RedirectToAction(nameof(Index), "ExternalRequests");
                }
                else
                {
                    errorMessage = "Your request isn't successfully submitted!. Please try again";
                    _contextAccessor.HttpContext.Session.SetString("errorMessage", errorMessage);
                    return RedirectToAction(nameof(Index), "ExternalRequests");

                }
            }
            catch (Exception ex)
            {
                string errorMessage;
                errorMessage = ex.Message+" happened. Please try again.";
                _contextAccessor.HttpContext.Session.SetString("errorMessage", errorMessage);
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
            model.AppointmentDate = DateTime.Now.AddDays(2);
            model.AppointmentDetail = String.Empty;
            return model;
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAppointments(AppointmentModel model)
        {
            TblAppointment appointment = new TblAppointment();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (model.AppointmentDate < DateTime.Now)
            {
                _notifyService.Error("Invalid Appointment date");
                return View(model);
            }

            var depHeadId = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.IsDepartmentHead == true && s.Dep.DepCode == "CVA").Select(S => S.UserId).ToList();

            var institution = _context.TblInistitutions.Where(s => s.InistId == model.IntId).FirstOrDefault();
            var users = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "CVA" && s.IsDepartmentHead == true).Select(s => s.EmailAddress).ToList();
            appointment.CreatedDate = DateTime.Now;
            appointment.AppointmentDetail = model.AppointmentDetail;
            appointment.InistId = model.IntId;
            appointment.AppointmentDate = model.AppointmentDate;
            appointment.RequestedBy = model.ExterUserId;
            _context.TblAppointments.Add(appointment);
            int saved = _context.SaveChanges();
            if (saved > 0)
            {
                if (users != null)
                {
                    await SendMail(users, "Appointment request alert", "<h3>Appointment request is sent from " + institution.Name + ". Please check detail on Task tracking Management Dashboard</h3>");
                    _notificationService.saveNotification(userId, depHeadId, "External appointment request added from " + institution.Name);
                }
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
            List<RoundModel> modelr;
            List<Juristriction> juris;
            List<Specializations> specializations;
            List<CurrencyModel> currencyModels;
            List<CaseType> caseTypes;
            List<ADRType> aDRTypes;
            List<ActingAs> actingAs;

            List<Litigationtype> litigationtypes;
            List<StatusReason> statusReasons;
            List<Result> results;
            List<ServiceType> serviceTypes;
            List<LegalAdviceResult> legalAdvices;
            List<ContractNegotiation> contractNegotiations;

            List<AdrStatus> adrStatuses;
            List<AdrResult> adrResults;
            List<ContractNegotiationStatus> contractNegotiationStatus;
            List<ContractNegotiationResult> contractNegotiationResults;
            List<LitigationStatus> litigationStatus;
            List<LitigationResult> litigationResults;
            List<InternationalCaseStatus> internationalCaseStatuses;
            List<InternationalCaseResult> internationalCaseResults;
            List<LegalAdviceStatus> legalAdviceStatuses;
            List<LegalAdviceResult> legalAdviceResults;


            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();

            if (cultur == "am")
            {
                modelr = new List<RoundModel>
            {
                new RoundModel
                {
                    RoundTypeId = 1,
                    Name = "አዲስ"
                },
                new RoundModel
                {
                    Name = "የቀጠለ",
                    RoundTypeId = 2,
                }
            };
                currencyModels = new List<CurrencyModel>()
            {
                new CurrencyModel
                {
                    CurrencyId="USD",
                    CurrencyName="የአሜሪካን ዶላር"
                },
                new CurrencyModel
                {
                    CurrencyId="ETB",
                    CurrencyName="ብር"
                },
                new CurrencyModel
                {
                    CurrencyId="Euro",
                    CurrencyName="ዩሮ"
                },
                new CurrencyModel
                {
                    CurrencyId="Pound",
                    CurrencyName="የእግሊዝ ፓውንድ"
                },
                new CurrencyModel
                {
                    CurrencyId="Other",
                    CurrencyName="ሌላ..."
                },
            };
                contractNegotiations = new List<ContractNegotiation>
                {
                    new ContractNegotiation
                    {
                    ContractNegotiationId="Negotiation",
                    ContractNegotiationName="ድርድር"
                    },
                     new ContractNegotiation
                    {
                    ContractNegotiationId="Review",
                    ContractNegotiationName="ውል ምርመራ"
                    },
                      new ContractNegotiation
                    {
                    ContractNegotiationId="Draft Preparation",
                    ContractNegotiationName="ረቂቅ ውል ማዘጋጀት"
                    },
                       new ContractNegotiation
                    {
                    ContractNegotiationId="Other",
                    ContractNegotiationName="ሌላ..."
                    },
                };

                contractNegotiationStatus = new List<ContractNegotiationStatus>
                {
                    new ContractNegotiationStatus
                    {
                        ContractNegotiationStatusId="Pending",
                        ContractNegotiationStatusName="በምርመራ ላይ"
                    },
                     new ContractNegotiationStatus
                    {
                        ContractNegotiationStatusId="Reviewed",
                        ContractNegotiationStatusName="ተመርምሯል"
                    },
                      new ContractNegotiationStatus
                    {
                        ContractNegotiationStatusId="Drafted",
                        ContractNegotiationStatusName="ተረቋል"
                    },

                };
                contractNegotiationResults = new List<ContractNegotiationResult>
                {
                    new ContractNegotiationResult
                    {
                        ContractNegotiationResultId="Negotiation Conducted",
                        ContractNegotiationResultName="ድርድር ተከናውኗል"
                    },
                    new ContractNegotiationResult
                    {
                        ContractNegotiationResultId="Review result/draft delivered",
                        ContractNegotiationResultName="የምርመራ ውጤት/የተዘጋጀ ረቂቅ ተልኳል"
                    },
                    new ContractNegotiationResult
                    {
                         ContractNegotiationResultId="Other",
                        ContractNegotiationResultName="ሌላ..."
                    }
                };

                adrStatuses = new List<AdrStatus>
                {
                    new AdrStatus
                    {
                        AdrStatusId="Adjourned",
                        AdrStatusName= "በቀጠሮ ላይ"
                    },
                    new AdrStatus
                    {
                        AdrStatusId="Settled",
                        AdrStatusName="በስምምነት ተቋጭቷል"
                    },
                     new AdrStatus
                    {
                        AdrStatusId="Award rendered",
                        AdrStatusName="የግልግል ዳኝነት ውሳኔ ተሰጥቷል"
                    },
                       new AdrStatus
                    {
                        AdrStatusId="Decided",
                        AdrStatusName="ውሳኔ ያገኘ"
                    }
                };
                litigationStatus = new List<LitigationStatus>
                {
                    new LitigationStatus
                    {
                        LitigationStatusID="Adjourned",
                        LitigationStatusName="በቀጠሮ ላይ"
                    },
                    new LitigationStatus
                    {
                        LitigationStatusID="Temporarily closed",
                        LitigationStatusName="ለጊዜው የተቋረጠ"
                    },
                    new LitigationStatus
                    {
                        LitigationStatusID="Decided",
                        LitigationStatusName="ተወስኗል"
                    },
                    new LitigationStatus
                    {
                        LitigationStatusID="Executed",
                        LitigationStatusName="የተፈጸመ"
                    }
                };
                litigationResults = new List<LitigationResult>
                {
                    new LitigationResult
                    {
                        LitigationResultID="File Closed",
                        LitigationResultName="መዝገቡ ተዘግቷል"
                    },
                     new LitigationResult
                    {
                        LitigationResultID="Other",
                        LitigationResultName="ሌላ..."
                    },
                };
                legalAdviceResults = new List<LegalAdviceResult>
                {
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Orally Delivered",
                        LegalAdviceResultName="በቃል የተሰጠ ምክር"
                    },
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Delivered in writing",
                        LegalAdviceResultName="በፅሁፍ የተሰጠ ምክር"
                    },
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Other",
                        LegalAdviceResultName="ሌላ..."
                    }
                };
                legalAdviceStatuses = new List<LegalAdviceStatus>
                {
                    new LegalAdviceStatus
                    {
                        LegalAdviceStatusID="Pending",
                        LegalAdviceStatusName="በሂደት ላይ",
                    },
                    new LegalAdviceStatus
                    {
                        LegalAdviceStatusID="Finalized",
                        LegalAdviceStatusName="የተጠናቀቀ",
                    }
                };
                internationalCaseResults = new List<InternationalCaseResult>
                {
                    new InternationalCaseResult
                    {
                        InternationalCaseResultID="Settlement Delivered ",
                        InternationalCaseResultName="ስምምነት ላይ ተደርሷል"
                    },
                    new InternationalCaseResult
                    {
                        InternationalCaseResultID="Award released ",
                        InternationalCaseResultName="የግልግል ውሳኔ ይፋ ተደርጓል"
                    },
                    new InternationalCaseResult
                    {
                        InternationalCaseResultID="Decision given ",
                        InternationalCaseResultName="ውሳኔ ተሰጥቷል"
                    },
                    new InternationalCaseResult
                    {
                        InternationalCaseResultID="Others",
                        InternationalCaseResultName="ሌላ..."
                    },
                };
                internationalCaseStatuses = new List<InternationalCaseStatus>
                {
                    new InternationalCaseStatus
                    {
                        InternationalCaseStatusID="Adjourned",
                        InternationalCaseStatusName="ቀጠሮ ላይ"
                    },
                      new InternationalCaseStatus
                    {
                        InternationalCaseStatusID="Settled",
                        InternationalCaseStatusName="በስምምነት ተጠናቋል"
                    },
                        new InternationalCaseStatus
                    {
                        InternationalCaseStatusID="Award rendered ",
                        InternationalCaseStatusName="ፍርድ ተሰጥቷል"
                    },
                          new InternationalCaseStatus
                    {
                        InternationalCaseStatusID="Decided",
                        InternationalCaseStatusName="ተወስኗል"
                    },
                            new InternationalCaseStatus
                    {
                        InternationalCaseStatusID="Withdrawn",
                        InternationalCaseStatusName="ቀሪ ተደርጓል"
                    },
                };
                adrResults = new List<AdrResult>
                {
                    new AdrResult
                    {
                        AdrResultID="Settlement delivered",
                        AdrResultName="ስምምነት ደርሷል"
                    },
                      new AdrResult
                    {
                        AdrResultID="Award released",
                        AdrResultName="የግልግል ዳኝነት ውሳኔ ይፋ ሆኗል"
                    },
                        new AdrResult
                    {
                        AdrResultID="Decision given",
                        AdrResultName="ውሳኔ ወጪ ተደርጓል"
                    },  new AdrResult
                    {
                        AdrResultID="Other",
                        AdrResultName="ሌላ..."
                    }
                };


                juris = new List<Juristriction>
                {
                    new Juristriction
                    {
                        JuristrictionId="First instance",
                        JuristrictionName="የመጀመሪያ ደረጃ ፍ/ቤት",
                    },
                     new Juristriction
                    {
                        JuristrictionId="High Court",
                        JuristrictionName="ከፍተኛ ደረጃ ፍ/ቤት",
                    },
                      new Juristriction
                    {
                        JuristrictionId="Supreme",
                        JuristrictionName="ጠቅላይ ፍ/ቤት",
                    },
                       new Juristriction
                    {
                        JuristrictionId="Cassation",
                        JuristrictionName="ሰበር ሰሚ ችሎት",
                    },
                        new Juristriction
                    {
                        JuristrictionId="Administrative",
                        JuristrictionName="አስተዳደራዊ ፍ/ቤት",
                    }
                };
                specializations = new List<Specializations>
                {
                   new Specializations
                   {
                       SpecializationId="Commericial",
                       SpecializationName="ንግድ ነክ አለመግባባት"
                   },
                   new Specializations
                   {
                       SpecializationId="Investiment",
                       SpecializationName="የኢንቨስትመንት አለመግባባት"
                   },
                   new Specializations
                   {
                       SpecializationId="Other",
                       SpecializationName="ሌላ..."
                   },
                };
                caseTypes = new List<CaseType>
                {
                    new CaseType
                    {
                        CaseTypeID="Negotiation",
                        CaseTypeName="ድርድር"
                    },
                      new CaseType
                    {
                        CaseTypeID="Meditation",
                        CaseTypeName="ሽምግልና"
                    },
                        new CaseType
                    {
                        CaseTypeID="Conciliation",
                        CaseTypeName="ዕርቅ"
                    },
                          new CaseType
                    {
                        CaseTypeID="Arbitration",
                        CaseTypeName="የግልግል ዳኝነት"
                    },
                            new CaseType
                    {
                        CaseTypeID="Suit",
                        CaseTypeName="ክስ"
                    },
                };
                aDRTypes = new List<ADRType>
                {
                    new ADRType
                    {
                        ADRTypeId="Mediation",
                        ADRTypeName="ሽምግልና"
                    },
                     new ADRType
                    {
                        ADRTypeId="Conciliation",
                        ADRTypeName="እርቅ"
                    },
                      new ADRType
                    {
                        ADRTypeId="Arbitration",
                        ADRTypeName="የግልግል ዳኝነት"
                    },
                       new ADRType
                    {
                        ADRTypeId="Other",
                        ADRTypeName="ሌላ..."
                    },
                };
                actingAs = new List<ActingAs>
                {
                    new ActingAs
                    {
                        ActingAsId="Mediator",
                        ActingAsName="አሸማጋይ"
                    },
                     new ActingAs
                    {
                        ActingAsId="Conciliator",
                        ActingAsName="አስታራቂ"
                    },
                      new ActingAs
                    {
                        ActingAsId="Arbitrator",
                        ActingAsName="የግልግል ዳኛ"
                    },
                       new ActingAs
                    {
                        ActingAsId="Decision Maker",
                        ActingAsName="ውሳኔ ሰጭ"
                    },
                        new ActingAs
                    {
                        ActingAsId="Representation",
                        ActingAsName="ውክልና"
                    }
                };
                litigationtypes = new List<Litigationtype>
                {
                    new Litigationtype
                    {
                        LitigationtypeId="Suit",
                        LitigationtypeName="የቀጥታ ክስ"
                    },
                    new Litigationtype
                    {
                        LitigationtypeId="Appeal",
                        LitigationtypeName="ይግባኝ"
                    },
                    new Litigationtype
                    {
                        LitigationtypeId="Execution",
                        LitigationtypeName="የፍርድ አፈፃፀም"
                    },
                };
                statusReasons = new List<StatusReason>
                {
                    new StatusReason
                    {
                        StatusReasonId="Adjourned",
                        StatusReasonName="ችሎቱ ሌላ ጊዜ ተቀጠረ(መቅጠር)"
                    },
                    new StatusReason
                    {
                        StatusReasonId="Temporarily closed",
                        StatusReasonName="ለጊዜው ማቋረጥ"
                    },
                    new StatusReason
                    {
                        StatusReasonId="Decided",
                        StatusReasonName="ዉሳኔ ያገኘ"
                    },
                    new StatusReason
                    {
                        StatusReasonId="Executed",
                        StatusReasonName="የተፈፀመ(ውሳኔ)"
                    },
                };
                results = new List<Result>
                {
                    new Result
                    {
                        ResultId="File closed",
                        ResultName="መዝገቡ ተዘግቷል"
                    }, new Result
                    {
                        ResultId="Other",
                        ResultName="ሌላ..."
                    }
                };
                serviceTypes = new List<ServiceType>
                {
                    new ServiceType
                    {
                        ServiceTypeId="Negotiation",
                        ServiceTypeName="ድርድር"
                    },
                     new ServiceType
                    {
                        ServiceTypeId="Review",
                        ServiceTypeName="ውል መመርመር"
                    },
                      new ServiceType
                    {
                        ServiceTypeId="Draft Preparation",
                        ServiceTypeName="ረቂቅ ውል ማዘጋጀት"
                    },
                       new ServiceType
                    {
                        ServiceTypeId="Other",
                        ServiceTypeName="ሌላ..."
                    },
                };
                legalAdvices = new List<LegalAdviceResult>
                {
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Orally Delivered",
                        LegalAdviceResultName="በቃል ቀርቧል"
                    },
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Delivered in writing",
                        LegalAdviceResultName="በጽሑፍ ቀርቧል"
                    },
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Other",
                        LegalAdviceResultName="ሌላ... "
                    },
                };
                var servs = _context.TblServiceTypes.OrderByDescending(s => s.ServiceOrderOrder).ToList();
                model.ServiceTypes = servs.OrderBy(s => s.ServiceOrderOrder).Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeNameAmharic
                }).ToList();
                model.RequestTypes = _context.TblRequestTypes.Select(r => new SelectListItem
                {
                    Text = r.TypeNameAmharic,
                    Value = r.TypeId.ToString()
                }).ToList();
                model.LegalStadiesCasetypes = _context.TblLegalDraftingDocTypes.OrderBy(s => s.DocmentOrder).Select(s => new SelectListItem
                {
                    Value = s.DocId.ToString(),
                    Text = s.DocNameAmharic.ToString()
                }).ToList();
                model.PrioritiesQues = _context.TblPriorityQuestions.Select(x => new CheckBoxItem
                {
                    PriorityQueId = x.PriorityQueId,
                    Title = x.QuestionsNameAmharic,
                    IsSelected = false
                }).ToList();

                model.Jursidictions = juris.Select(s => new SelectListItem
                {
                    Text = s.JuristrictionName,
                    Value = s.JuristrictionId
                }).ToList();
                model.Specializations = specializations.Select(s => new SelectListItem
                {
                    Text = s.SpecializationName,
                    Value = s.SpecializationId
                }).ToList();
                model.CaseType = caseTypes.Select(s => new SelectListItem
                {
                    Text = s.CaseTypeName,
                    Value = s.CaseTypeID
                }).ToList();
                model.Adrtypes = aDRTypes.Select(s => new SelectListItem
                {
                    Value = s.ADRTypeId,
                    Text = s.ADRTypeName
                }).ToList();
                model.ActingAs = actingAs.Select(s => new SelectListItem
                {
                    Value = s.ActingAsId,
                    Text = s.ActingAsName
                }).ToList();
                model.LitigationTypes = litigationtypes.Select(s => new SelectListItem
                {
                    Value = s.LitigationtypeId,
                    Text = s.LitigationtypeName
                }).ToList();
                model.Reasults = results.Select(s => new SelectListItem
                {
                    Value = s.ResultId,
                    Text = s.ResultName
                }).ToList();
                model.ContractNegotiations = contractNegotiations.Select(s => new SelectListItem
                {
                    Value = s.ContractNegotiationId,
                    Text = s.ContractNegotiationName
                }).ToList();

                model.AdrStatus = adrStatuses.Select(s => new SelectListItem
                {
                    Text = s.AdrStatusName,
                    Value = s.AdrStatusId
                }).ToList();
                model.AdrResults = adrResults.Select(s => new SelectListItem
                {
                    Value = s.AdrResultID,
                    Text = s.AdrResultName
                }).ToList();

                model.LitigationResults = litigationResults.Select(s => new SelectListItem
                {
                    Text = s.LitigationResultName,
                    Value = s.LitigationResultID
                }).ToList();
                model.LitigationStatus = litigationStatus.Select(s => new SelectListItem
                {
                    Text = s.LitigationStatusName,
                    Value = s.LitigationStatusID
                }).ToList();

                model.InternationCaseResult = internationalCaseResults.Select(s => new SelectListItem
                {
                    Text = s.InternationalCaseResultName,
                    Value = s.InternationalCaseResultID
                }).ToList();
                model.InternationCaseStatus = internationalCaseStatuses.Select(s => new SelectListItem
                {
                    Value = s.InternationalCaseStatusID,
                    Text = s.InternationalCaseStatusName
                }).ToList();

                model.ContractNegotiationResult = contractNegotiationResults.Select(s => new SelectListItem
                {
                    Value = s.ContractNegotiationResultId,
                    Text = s.ContractNegotiationResultName
                }).ToList();
                model.ContractNegotiationsStatus = contractNegotiationStatus.Select(s => new SelectListItem
                {
                    Text = s.ContractNegotiationStatusName,
                    Value = s.ContractNegotiationStatusId
                }).ToList();

                model.LegalAdviceResults = legalAdviceResults.Select(s => new SelectListItem
                {
                    Text = s.LegalAdviceResultName,
                    Value = s.LegalAdviceResultID
                }).ToList();
                model.LegalAdviceStatus = legalAdviceStatuses.Select(s => new SelectListItem
                {
                    Text = s.LegalAdviceStatusName,
                    Value = s.LegalAdviceStatusID
                }).ToList();
            }
            else
            {
                currencyModels = new List<CurrencyModel>()
            {
                new CurrencyModel
                {
                    CurrencyId="USD",
                    CurrencyName="USD"
                },
                new CurrencyModel
                {
                    CurrencyId="ETB",
                    CurrencyName="ETB"
                },
                new CurrencyModel
                {
                    CurrencyName="Euro",
                    CurrencyId="Euro"
                },
                new CurrencyModel
                {
                    CurrencyId="Pound",
                    CurrencyName="Pound"
                },
                new CurrencyModel
                {
                    CurrencyId="Other",
                    CurrencyName="Other"
                },
            };

                modelr = new List<RoundModel>
            {
                new RoundModel
                {
                    RoundTypeId = 1,
                    Name = "New"
                },
                new RoundModel
                {
                    Name = "Continuation",
                    RoundTypeId = 2,
                }
            };
                contractNegotiations = new List<ContractNegotiation>
                {
                    new ContractNegotiation
                    {
                    ContractNegotiationId="Negotiation",
                    ContractNegotiationName="Negotiation"
                    },
                     new ContractNegotiation
                    {
                    ContractNegotiationId="Review",
                    ContractNegotiationName="Review"
                    },
                      new ContractNegotiation
                    {
                    ContractNegotiationId="Draft Preparation",
                    ContractNegotiationName="Draft Preparation"
                    },
                       new ContractNegotiation
                    {
                    ContractNegotiationId="Other",
                    ContractNegotiationName="Other"
                    },
                };
                contractNegotiationStatus = new List<ContractNegotiationStatus>
                {
                    new ContractNegotiationStatus
                    {
                        ContractNegotiationStatusId="Pending",
                        ContractNegotiationStatusName="Pending"
                    },
                     new ContractNegotiationStatus
                    {
                        ContractNegotiationStatusId="Reviewed",
                        ContractNegotiationStatusName="Reviewed"
                    },
                      new ContractNegotiationStatus
                    {
                        ContractNegotiationStatusId="Drafted",
                        ContractNegotiationStatusName="Drafted"
                    },

                };

                contractNegotiationResults = new List<ContractNegotiationResult>
                {
                    new ContractNegotiationResult
                    {
                        ContractNegotiationResultId="Negotiation Conducted",
                        ContractNegotiationResultName="Negotiation Conducted"
                    },
                    new ContractNegotiationResult
                    {
                        ContractNegotiationResultId="Review result/draft delivered",
                        ContractNegotiationResultName="Review result/draft delivered"
                    },
                    new ContractNegotiationResult
                    {
                         ContractNegotiationResultId="Other",
                        ContractNegotiationResultName="Other"
                    }
                };

                adrStatuses = new List<AdrStatus>
                {
                    new AdrStatus
                    {
                        AdrStatusId="Adjourned",
                        AdrStatusName= "Adjourned"
                    },
                    new AdrStatus
                    {
                        AdrStatusId="Settled",
                        AdrStatusName="Settled"
                    },
                     new AdrStatus
                    {
                        AdrStatusId="Award rendered",
                        AdrStatusName="Award rendered"
                    },
                       new AdrStatus
                    {
                        AdrStatusId="Decided",
                        AdrStatusName="Decided"
                    }
                };
                litigationStatus = new List<LitigationStatus>
                {
                    new LitigationStatus
                    {
                        LitigationStatusID="Adjourned",
                        LitigationStatusName="Adjourned"
                    },
                    new LitigationStatus
                    {
                        LitigationStatusID="Temporarily closed",
                        LitigationStatusName="Temporarily closed"
                    },
                    new LitigationStatus
                    {
                        LitigationStatusID="Decided",
                        LitigationStatusName="Decided"
                    },
                    new LitigationStatus
                    {
                        LitigationStatusID="Executed",
                        LitigationStatusName="Executed"
                    }
                };
                litigationResults = new List<LitigationResult>
                {
                    new LitigationResult
                    {
                        LitigationResultID="File Closed",
                        LitigationResultName="File Closed"
                    },
                     new LitigationResult
                    {
                        LitigationResultID="Other",
                        LitigationResultName="Other"
                    },
                };
                legalAdviceResults = new List<LegalAdviceResult>
                {
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Orally Delivered",
                        LegalAdviceResultName="Orally Delivered"
                    },
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Delivered in writing",
                        LegalAdviceResultName="Delivered in writing"
                    },
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Other",
                        LegalAdviceResultName="Other"
                    }
                };
                legalAdviceStatuses = new List<LegalAdviceStatus>
                {
                    new LegalAdviceStatus
                    {
                        LegalAdviceStatusID="Pending",
                        LegalAdviceStatusName="Pending",
                    },
                    new LegalAdviceStatus
                    {
                        LegalAdviceStatusID="Finalized",
                        LegalAdviceStatusName="Finalized",
                    }
                };
                internationalCaseResults = new List<InternationalCaseResult>
                {
                    new InternationalCaseResult
                    {
                        InternationalCaseResultID="Settlement Delivered",
                        InternationalCaseResultName="Settlement Delivered"
                    },
                    new InternationalCaseResult
                    {
                        InternationalCaseResultID="Award released",
                        InternationalCaseResultName="Award released"
                    },
                    new InternationalCaseResult
                    {
                        InternationalCaseResultID="Decision given",
                        InternationalCaseResultName="Decision given"
                    },
                    new InternationalCaseResult
                    {
                        InternationalCaseResultID="Others",
                        InternationalCaseResultName="Other"
                    },
                };
                internationalCaseStatuses = new List<InternationalCaseStatus>
                {
                    new InternationalCaseStatus
                    {
                        InternationalCaseStatusID="Adjourned",
                        InternationalCaseStatusName="Adjourned"
                    },
                      new InternationalCaseStatus
                    {
                        InternationalCaseStatusID="Settled",
                        InternationalCaseStatusName="Settled"
                    },
                        new InternationalCaseStatus
                    {
                        InternationalCaseStatusID="Award rendered",
                        InternationalCaseStatusName="Award rendered"
                    },
                          new InternationalCaseStatus
                    {
                        InternationalCaseStatusID="Decided",
                        InternationalCaseStatusName="Decided"
                    },
                            new InternationalCaseStatus
                    {
                        InternationalCaseStatusID="Withdrawn",
                        InternationalCaseStatusName="Withdrawn"
                    },
                };
                adrResults = new List<AdrResult>
                {
                    new AdrResult
                    {
                        AdrResultID="Settlement delivered",
                        AdrResultName="Settlement delivered"
                    },
                      new AdrResult
                    {
                        AdrResultID="Award released",
                        AdrResultName="Award released"
                    },
                        new AdrResult
                    {
                        AdrResultID="Decision given",
                        AdrResultName="Decision given"
                    },  new AdrResult
                    {
                        AdrResultID="Other",
                        AdrResultName="Other"
                    }
                };




                juris = new List<Juristriction>
                {
                    new Juristriction
                    {
                        JuristrictionId="First instance",
                        JuristrictionName="First instance",
                    },
                     new Juristriction
                    {
                        JuristrictionId="High Court",
                        JuristrictionName="High Court",
                    },
                      new Juristriction
                    {
                        JuristrictionId="Supreme",
                        JuristrictionName="Supreme",
                    },
                       new Juristriction
                    {
                        JuristrictionId="Cassation",
                        JuristrictionName="Cassation",
                    },
                        new Juristriction
                    {
                        JuristrictionId="Administrative",
                        JuristrictionName="Administrative",
                    }
                };
                specializations = new List<Specializations>
                {
                   new Specializations
                   {
                       SpecializationId="Commericial",
                       SpecializationName="Commericial"
                   },
                   new Specializations
                   {
                       SpecializationId="Investiment",
                       SpecializationName="Investiment"
                   },
                   new Specializations
                   {
                       SpecializationId="Other",
                       SpecializationName="Other"
                   },
                };
                caseTypes = new List<CaseType>
                {
                    new CaseType
                    {
                        CaseTypeID="Negotiation",
                        CaseTypeName="Negotiation"
                    },
                      new CaseType
                    {
                        CaseTypeID="Meditation",
                        CaseTypeName="Meditation"
                    },
                        new CaseType
                    {
                        CaseTypeID="Conciliation",
                        CaseTypeName="Conciliation"
                    },
                          new CaseType
                    {
                        CaseTypeID="Arbitration",
                        CaseTypeName="Arbitration"
                    },
                            new CaseType
                    {
                        CaseTypeID="Suit",
                        CaseTypeName="Suit"
                    },
                };
                aDRTypes = new List<ADRType>
                {
                    new ADRType
                    {
                        ADRTypeId="Mediation",
                        ADRTypeName="Mediation"
                    },
                     new ADRType
                    {
                        ADRTypeId="Conciliation",
                        ADRTypeName="Conciliation"
                    },
                      new ADRType
                    {
                        ADRTypeId="Arbitration",
                        ADRTypeName="Arbitration"
                    },
                       new ADRType
                    {
                        ADRTypeId="Others",
                        ADRTypeName="Others"
                    },
                };
                actingAs = new List<ActingAs>
                {
                    new ActingAs
                    {
                        ActingAsId="Mediator",
                        ActingAsName="Mediator"
                    },
                     new ActingAs
                    {
                        ActingAsId="Conciliator",
                        ActingAsName="Conciliator"
                    },
                      new ActingAs
                    {
                        ActingAsId="Arbitrator",
                        ActingAsName="Arbitrator"
                    },
                       new ActingAs
                    {
                        ActingAsId="Decision Maker",
                        ActingAsName="Decision Maker"
                    },
                        new ActingAs
                    {
                        ActingAsId="Representation",
                        ActingAsName="Representation"
                    }
                };
                adrStatuses = new List<AdrStatus>
                {
                    new AdrStatus
                    {
                        AdrStatusId="Adjourned",
                        AdrStatusName="Adjourned"
                    },
                    new AdrStatus
                    {
                        AdrStatusId="Settled",
                        AdrStatusName="Settled"
                    },
                      new AdrStatus
                    {
                        AdrStatusId="Award rendered",
                        AdrStatusName="Award rendered"
                    },
                        new AdrStatus
                    {
                        AdrStatusId="Award rendered",
                        AdrStatusName="Award rendered"
                    },

                      new AdrStatus
                      {
                          AdrStatusId="Decided",
                          AdrStatusName="የተወሰነ"
                      }
                };
                litigationtypes = new List<Litigationtype>
                {
                    new Litigationtype
                    {
                        LitigationtypeId="Suit",
                        LitigationtypeName="Suit"
                    },
                    new Litigationtype
                    {
                        LitigationtypeId="Appeal",
                        LitigationtypeName="Appeal"
                    },
                    new Litigationtype
                    {
                        LitigationtypeId="Execution",
                        LitigationtypeName="Execution"
                    },
                };
                statusReasons = new List<StatusReason>
                {
                    new StatusReason
                    {
                        StatusReasonId="Adjourned",
                        StatusReasonName="Adjourned"
                    },
                    new StatusReason
                    {
                        StatusReasonId="Temporarily closed",
                        StatusReasonName="Temporarily closed"
                    },
                    new StatusReason
                    {
                        StatusReasonId="Decided",
                        StatusReasonName="Decided"
                    },
                    new StatusReason
                    {
                        StatusReasonId="Executed",
                        StatusReasonName="Executed"
                    },
                };
                results = new List<Result>
                {
                    new Result
                    {
                        ResultId="File closed",
                        ResultName="File closed"
                    }, new Result
                    {
                        ResultId="Other",
                        ResultName="Other"
                    }
                };
                serviceTypes = new List<ServiceType>
                {
                    new ServiceType
                    {
                        ServiceTypeId="Negotiation",
                        ServiceTypeName="Negotiation"
                    },
                     new ServiceType
                    {
                        ServiceTypeId="Review",
                        ServiceTypeName="Review"
                    },
                      new ServiceType
                    {
                        ServiceTypeId="Draft Preparation",
                        ServiceTypeName="Draft Preparation"
                    },
                       new ServiceType
                    {
                        ServiceTypeId="Other ",
                        ServiceTypeName="Other "
                    },
                };
                legalAdvices = new List<LegalAdviceResult>
                {
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Orally Delivered ",
                        LegalAdviceResultName="Orally Delivered "
                    },
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Delivered in writing ",
                        LegalAdviceResultName="Delivered in writing "
                    },
                    new LegalAdviceResult
                    {
                        LegalAdviceResultID="Other ",
                        LegalAdviceResultName="Other "
                    },
                };
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
                model.PrioritiesQues = _context.TblPriorityQuestions.Select(x => new CheckBoxItem
                {
                    PriorityQueId = x.PriorityQueId,
                    Title = x.QuestionName,
                    IsSelected = false
                }).ToList();

                model.Jursidictions = juris.Select(s => new SelectListItem
                {
                    Text = s.JuristrictionName,
                    Value = s.JuristrictionId
                }).ToList();
                model.Specializations = specializations.Select(s => new SelectListItem
                {
                    Text = s.SpecializationName,
                    Value = s.SpecializationId
                }).ToList();
                model.CaseType = caseTypes.Select(s => new SelectListItem
                {
                    Text = s.CaseTypeName,
                    Value = s.CaseTypeID
                }).ToList();
                model.Adrtypes = aDRTypes.Select(s => new SelectListItem
                {
                    Value = s.ADRTypeName,
                    Text = s.ADRTypeId
                }).ToList();
                model.ActingAs = actingAs.Select(s => new SelectListItem
                {
                    Value = s.ActingAsId,
                    Text = s.ActingAsName
                }).ToList();
                model.LitigationTypes = litigationtypes.Select(s => new SelectListItem
                {
                    Value = s.LitigationtypeId,
                    Text = s.LitigationtypeName
                }).ToList();
                model.Reasults = results.Select(s => new SelectListItem
                {
                    Value = s.ResultId,
                    Text = s.ResultName
                }).ToList();
                model.ContractNegotiations = contractNegotiations.Select(s => new SelectListItem
                {
                    Value = s.ContractNegotiationId,
                    Text = s.ContractNegotiationName
                }).ToList();

                model.AdrStatus = adrStatuses.Select(s => new SelectListItem
                {
                    Text = s.AdrStatusName,
                    Value = s.AdrStatusId
                }).ToList();
                model.AdrResults = adrResults.Select(s => new SelectListItem
                {
                    Value = s.AdrResultID,
                    Text = s.AdrResultName
                }).ToList();

                model.LitigationResults = litigationResults.Select(s => new SelectListItem
                {
                    Text = s.LitigationResultName,
                    Value = s.LitigationResultID
                }).ToList();
                model.LitigationStatus = litigationStatus.Select(s => new SelectListItem
                {
                    Text = s.LitigationStatusName,
                    Value = s.LitigationStatusID
                }).ToList();

                model.InternationCaseResult = internationalCaseResults.Select(s => new SelectListItem
                {
                    Text = s.InternationalCaseResultName,
                    Value = s.InternationalCaseResultID
                }).ToList();
                model.InternationCaseStatus = internationalCaseStatuses.Select(s => new SelectListItem
                {
                    Value = s.InternationalCaseStatusID,
                    Text = s.InternationalCaseStatusName
                }).ToList();

                model.ContractNegotiationResult = contractNegotiationResults.Select(s => new SelectListItem
                {
                    Value = s.ContractNegotiationResultId,
                    Text = s.ContractNegotiationResultName
                }).ToList();
                model.ContractNegotiationsStatus = contractNegotiationStatus.Select(s => new SelectListItem
                {
                    Text = s.ContractNegotiationStatusName,
                    Value = s.ContractNegotiationStatusId
                }).ToList();

                model.LegalAdviceResults = legalAdviceResults.Select(s => new SelectListItem
                {
                    Text = s.LegalAdviceResultName,
                    Value = s.LegalAdviceResultID
                }).ToList();
                model.LegalAdviceStatus = legalAdviceStatuses.Select(s => new SelectListItem
                {
                    Text = s.LegalAdviceStatusName,
                    Value = s.LegalAdviceStatusID
                }).ToList();
            }

            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            model.RequestedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
            model.ExterUserId = userId;
            model.Round = 1;
            model.IntId = instName.InistId;
            model.TermsAndCondionts = _context.TblTermsAndConditions.Select(s => s.Details).FirstOrDefault();
            List<CompletedRequests> completeds = new List<CompletedRequests>();
            CompletedRequests completedRequests1;

            var completedRequests = _context.TblRequests.Where(x => x.IsArchived == true).ToList();
            foreach (var item in completedRequests)
            {
                completedRequests1 = new CompletedRequests();
                completedRequests1.RequestDetail = item.RequestDetail;
                completedRequests1.CompleteRequestID = item.RequestId;
                completeds.Add(completedRequests1);
            }
            model.RoundTypes = modelr.Select(s => new SelectListItem
            {
                Text = s.Name.ToString(),
                Value = s.RoundTypeId.ToString()
            }).ToList();
            model.CompletedRequests = completeds.Select(s => new SelectListItem
            {
                Text = s.RequestDetail.ToString(),
                Value = s.CompleteRequestID.ToString()
            }).ToList();
            model.Currency = currencyModels.Select(d => new SelectListItem
            {
                Text = d.CurrencyName.ToString(),
                Value = d.CurrencyId.ToString()
            }).ToList();
            model.Countries = _context.TblCountries.Select(s => new SelectListItem
            {
                Text = s.CountryName,
                Value = s.CountryId.ToString()
            }).ToList();
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
            TblDocumentHistory tblDocument = _context.TblDocumentHistories.Include(x => x.Request).Where(x => x.RequestId == id).OrderBy(x => x.Round).Last();
            DocumentHistoryModel model = new DocumentHistoryModel();
            ViewData["histories"] = _context.TblDocumentHistories.Where(x => x.RequestId == id).ToList();
            model.RequestId = id;
            model.ExternalRepliedBy = userId;
            if (tblDocument.Round == null)
            {
                model.Round = 1;
            }
            else
            {
                model.Round = Convert.ToInt32(tblDocument.Round) + 1;
            }
            return model;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddHistory(DocumentHistoryModel? model)
        {
            List<string> emails = new List<string>();


            var users = (from user in _context.TblRequestAssignees
                         join assignees in _context.TblInternalUsers on user.UserId equals assignees.UserId
                         where (assignees.IsDepartmentHead == true || assignees.IsDeputy == true) && user.RequestId == model.RequestId
                         select assignees.EmailAddress).ToList();
            if (users.Count != 0)
            {
                emails = users;
            }
            else
            {
                emails = (from assignees in _context.TblInternalUsers
                          where assignees.IsDepartmentHead == true || assignees.IsDeputy == true
                          select assignees.EmailAddress).ToList();
            }
            var institutionName = (from items in _context.TblRequests where items.RequestId == model.RequestId select items.Inist.Name).FirstOrDefault();

            TblDocumentHistory history = new TblDocumentHistory();
            history.ExternalRepliedBy = model.ExternalRepliedBy;
            history.Round = model.Round;
            history.Description = model.Description;
            history.FileDescription = model.FileDescription;
            history.RequestId = model.RequestId;
            history.CreatedDate = DateTime.Now;
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
            if (saved > 0)
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
        public async Task<IActionResult> SentBackRequests()
        {
            TblTopStatus tblTopStatus = _context.TblTopStatuses.Where(x => x.StatusName == "Completed").FirstOrDefault();
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            TblRequest tblRequest;
            var moreDeps = _context.TblRequestDepartmentRelations.Select(a => a.RequestId).ToList();
            foreach (var item in moreDeps)
            {
                tblRequest = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(s => s.TopStatus)
                                                        .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.TopStatusId == tblTopStatus.TopStatusId && x.RequestId == item && x.InistId == instName.InistId&&x.IsSenttoInst==true).FirstOrDefault();
                if (tblRequest != null)
                {
                    atsdbContext.Add(tblRequest);
                }
            }
            return View(atsdbContext);
        }
        public async Task<IActionResult> Replies(Guid? id)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var replays = await _context.TblReplays.Where(a => a.RequestId == id && a.IsSent == true).ToListAsync();
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
                replay.IsExternal = true;
                replay.IsInternal = false;
                replay.IsSent = true;
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
                    replay.Attachment = dbPath;
                }
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
        public async Task<IActionResult> EditReplies(Guid? ReplyId)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var reply = _context.TblReplays.Find(ReplyId);
            RepliesModel model = new RepliesModel
            {
                ReplyId = ReplyId,
                RequestId = reply.RequestId,
                ReplayDetail = reply.ReplayDetail,
                ReplyDate = DateTime.Now,
                ExternalReplayedBy = userId,
            };
            ViewData["Replies"] = _context.TblReplays
                .Include(x => x.InternalReplayedByNavigation)
                .Include(x => x.ExternalReplayedByNavigation)
                .Include(x => x.Request)
                .Where(y => y.RequestId == reply.RequestId && y.IsSent == true).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> EditReplies(RepliesModel model)
        {
            try
            {
                TblReplay replay = _context.TblReplays.Find(model.ReplyId);
                replay.ReplyDate = DateTime.Now;
                replay.ReplayDetail = model.ReplayDetail;
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
                    replay.Attachment = dbPath;
                }
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply updated");
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
        public JsonResult GetDoctypes(Guid? ServiceTypeID)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            List<TblLegalDraftingDocType> subcategoryModels;
            List<DocTypes> docList = new List<DocTypes>();
            DocTypes type;
            Guid doctype = Guid.Parse("73e82d1b-3c6d-4a20-8f99-e12e27218cd0");
            if (ServiceTypeID == Guid.Parse("f935dcd1-2651-4c64-947c-0a877f652fb5"))
            {
                if (cultur == "am")
                {
                    subcategoryModels = (from items in _context.TblLegalDraftingDocTypes where items.DocId == doctype select items).OrderBy(s => s.DocmentOrder).ToList();
                    foreach (var item in subcategoryModels)
                    {
                        type = new DocTypes();
                        type.DocId = item.DocId;
                        type.DocName = item.DocNameAmharic;
                        type.Order = item.DocmentOrder;
                        docList.Add(type);
                    }
                }
                else
                {
                    subcategoryModels = (from items in _context.TblLegalDraftingDocTypes where items.DocId == doctype select items).OrderBy(s => s.DocmentOrder).ToList();
                    foreach (var item in subcategoryModels)
                    {
                        type = new DocTypes();
                        type.DocId = item.DocId;
                        type.DocName = item.DocName;
                        type.Order = item.DocmentOrder;
                        docList.Add(type);
                    }
                }

                return Json(new SelectList(docList, "DocId", "DocName"));
            }
            else
            {
                if (cultur == "am")
                {
                    subcategoryModels = (from items in _context.TblLegalDraftingDocTypes select items).OrderBy(s => s.DocmentOrder).ToList();
                    foreach (var item in subcategoryModels)
                    {
                        type = new DocTypes();
                        type.DocId = item.DocId;
                        type.DocName = item.DocNameAmharic;
                        type.Order = item.DocmentOrder;
                        docList.Add(type);
                    }
                }
                else
                {
                    subcategoryModels = (from items in _context.TblLegalDraftingDocTypes select items).OrderBy(s=>s.DocmentOrder).ToList();
                    foreach (var item in subcategoryModels)
                    {
                        type = new DocTypes();
                        type.DocId = item.DocId;
                        type.Order = item.DocmentOrder;
                        type.DocName = item.DocName;
                        docList.Add(type);
                    }
                }
                return Json(new SelectList(docList, "DocId", "DocName"));
            }

        }
        public IActionResult PDFViewerNewTab(string? filePath)
        {
            return File(System.IO.File.ReadAllBytes(filePath), "application/pdf");
        }
        public IActionResult priveweier(string filePath)
        {
            using (FileStream fileStream = System.IO.File.Create(filePath))
            {
                fileStream.Flush();
            }
            return View();

        }

        public IActionResult Redirection()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblExternalUser? instName = _context.TblExternalUsers.FindAsync(userId).Result;
            if (instName.AcceptedTerms == null || instName.AcceptedTerms == false)
            {
                ViewBag.accepted = "false";
            }
            else
            {
                ViewBag.accepted = "true";
            }
            return View(getModel());
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public JsonResult Redirection(CivilJusticeExternalRequestModel model)
        {
            try
            {
                string errorMessage = null;
                string successMessage = null;
               
                string country = null;
                var deputy = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.IsDeputy == true).Select(S => S.UserId).ToList();
                if (model.CountryId == null)
                {
                    country = "Ethiopia";
                }
                else
                {
                    country = _context.TblCountries.Find(model.CountryId).CountryName;
                }
                TblRequest request = new TblRequest();
                List<TblDocumentHistory> documentHistory = new List<TblDocumentHistory>();
                List<TblRequestPriorityQuestionsRelation> relations = new List<TblRequestPriorityQuestionsRelation>();
                TblTopStatus topStatus = _context.TblTopStatuses.Where(s => s.StatusName == "New").FirstOrDefault();
                var institutionName = (from items in _context.TblInistitutions where items.InistId == model.IntId select items.Name).FirstOrDefault();
                var users = (from user in _context.TblInternalUsers where (user.IsDepartmentHead == true || user.IsDeputy == true) select user.EmailAddress).ToList();
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                TblExternalUser externalUser = _context.TblExternalUsers.Find(userId);
                TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "New" select items).FirstOrDefault();
                Guid statusiD = (from id in _context.TblExternalRequestStatuses where id.StatusName == "New" select id.ExternalRequestStatusId).FirstOrDefault();
                var decision = _context.TblDecisionStatuses.Where(x => x.StatusName == "Not set").FirstOrDefault();
                var requestType = _context.TblRequestTypes.Where(s => s.TypeId == model.TypeId).FirstOrDefault();

                if (model.ServiceTypeID == null)
                {
                    errorMessage = "Please Select Service type";
                    return Json(errorMessage);
                }
                if (model.ServiceTypeID == Guid.Parse("F935DCD1-2651-4C64-947C-0A877F652FB5") ||
                   model.ServiceTypeID == Guid.Parse("ACB92222-4872-4A9D-8EDB-8BCD5317129A") ||
                   model.ServiceTypeID == Guid.Parse("6E00B0EA-7B4D-40C4-8289-A566FE16E88E") ||
                   model.ServiceTypeID == Guid.Parse("92FC1FC5-95D2-4250-98FB-0BA862F6DB02") ||
                   model.ServiceTypeID == Guid.Parse("C792938C-7952-488C-A23E-1A27F8B2B8E6"))
                {
                    if (model.MultipleFiles == null)
                    {
                        errorMessage = "Please add Document file and try again";
                        _notifyService.Error("Please add Document file and try again");
                        return Json(errorMessage);
                    }
                    if (model.DocId == null)
                    {
                        _notifyService.Error("Please select Document type");
                        return Json(getModel());
                    }
                    request.DocTypeId = model.DocId;
                }

                request.RequestDetail = model.RequestDetail;
                request.InistId = model.IntId;
                request.RequestedBy = userId;
                request.CreatedDate = DateTime.Now;
                if (model.MoneyAmount != null)
                {
                    request.MoneyAmount = int.Parse(model.MoneyAmount);
                    request.MoneyCurrency = model.CurrencyId;
                }
                request.TopStatusId = topStatus.TopStatusId;
                request.ExternalRequestStatusId = statusiD;
                request.DepartmentUpprovalStatus = decision.DesStatusId;
                request.TeamUpprovalStatus = decision.DesStatusId;
                request.DeputyUprovalStatus = decision.DesStatusId;
                request.UserUpprovalStatus = decision.DesStatusId;
                request.ServiceTypeId = model.ServiceTypeID;
                request.FullName = model.FullName;
                request.PhoneNumber = model.PhoneNumber;
                request.EmailAddress = model.EmailAddress;
                request.ContractNeServiceType = model.ContractNegotiationId;
                request.ListigationStatus = model.LitigationStatusID;
                request.ListigationResult = model.LitigationResultID;
                request.LegalAdviceResult = model.LegalAdviceResultID;
                request.LegalAdviceStatus = model.LegalAdviceStatusID;
                request.ContractNeResult = model.ContractNegotiationResultId;
                request.ContractNeStatus = model.ContractNegotiationStatusId;
                request.InternationalCaseResult = model.InternationalCaseResultID;
                request.InternationalCaseStatus = model.InternationalCaseStatusID;
                request.AdrResult = model.AdrResultID;
                request.AdrStatus = model.AdrStatusId;
                request.OtherDocumentType = model.OtherDocumentType;
                request.OtherServiceType = model.OtherServiceType;
                request.Specialization = model.SpecializationId;
                request.Adrtype = model.ADRTypeId;
                request.ActingAs = model.ActingAsId;
                request.Bench = model.Bench;
                request.Respondent = model.Respondent;
                request.Claimant = model.Claimant;

                request.Country = country;
                request.DateOfAdjournment = model.DateOfAdjournment;
                request.CaseType = model.CaseTypeID;
                request.Defendent = model.Defendent;
                request.Plaintiful = model.Plaintiful;
                request.DateofJudgement = model.DateofJudgement;
                request.Jursidiction = model.JuristrictionId;
                request.LitigationType = model.LitigationtypeId;
                request.CourtCenter = model.CourtCenter;

                string path = Path.Combine(Directory.GetCurrentDirectory(), "admin/Files");

                foreach (var item in model.MultipleFiles)
                {
                    if (item.Length > 0)
                    {
                        if (item.FileName != null)
                        {
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            FileInfo fileInfo = new FileInfo(item.FileName);
                            string ExactFileName = item.FileName;
                            string fileName = Guid.NewGuid().ToString() + item.FileName;
                            string fileNameWithPath = Path.Combine(path, fileName);
                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                item.CopyTo(stream);
                            }
                            string dbPath = "/admin/Files/" + fileName;
                            request.DocTypeId = model.DocId;
                            documentHistory.Add(new TblDocumentHistory { DocPath = dbPath, Round = model.Round, RequestId = request.RequestId, ExactFileName = ExactFileName });

                        }
                    }

                }
                request.TblDocumentHistories = documentHistory;
                var selected = model.PrioritiesQues.Where(s => s.IsSelected == true).ToList();
                if (selected.Count != 0)
                {
                    request.PriorityId = _context.TblPriorities.Where(x => x.PriorityId == Guid.Parse("12fba758-fa2a-406a-ae64-0a561d0f5e73")).Select(x => x.PriorityId).FirstOrDefault();
                    foreach (var item in selected)
                    {
                        if (item.IsSelected == true)
                        {
                            relations.Add(new TblRequestPriorityQuestionsRelation { RequestId = request.RequestId, PriorityQueId = item.PriorityQueId });
                        }
                    }
                    request.TblRequestPriorityQuestionsRelations = relations;
                }
                else
                {
                    request.PriorityId = Guid.Parse("79fa9e18-c973-40d4-b77d-1d5d31ded31f");
                }
                _context.TblRequests.Add(request);
                if (externalUser.AcceptedTerms == false || externalUser.AcceptedTerms == null)
                {
                    externalUser.AcceptedTerms = true;
                }
                else
                {
                    externalUser.AcceptedTerms = true;
                }
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                    var existingRequ = _context.TblRounds.Where(s => s.RequestIid == request.RequestId).FirstOrDefault();
                    TblRound roundS = new TblRound();
                    roundS.RequestIid = request.RequestId;
                    if (existingRequ != null)
                    {
                        roundS.RoundNumber = existingRequ.RoundNumber + 1;
                    }
                    roundS.RoundNumber = 1;
                    _context.TblRounds.Add(roundS);
                    _context.SaveChanges();
                    successMessage = "Your request is submitted Successfully. Responsive body is notified by email";
                    _contextAccessor.HttpContext.Session.SetString("successMessage", successMessage);
                    _notifyService.Success("Your request is submitted Successfully. Responsive body is notified by email");
                      SendMail(users, "Request notifications from " + institutionName, "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");
                    _notificationService.saveNotification(userId, deputy, "New service request is added from " + institutionName);
                    return Json(successMessage);
                }
                else
                {
                    errorMessage = "Your request isn't successfully submitted!. Please try again";
                    _contextAccessor.HttpContext.Session.SetString("errorMessage", errorMessage);
                    _notifyService.Error("Your request isn't successfully submitted!. Please try again");
                    return Json(errorMessage);
                }
            }
            catch (Exception ex)
            {
                string message= ex.Message+" happened. Please try again";
                _notifyService.Error(ex.Message + " happened. Please try again");
                return Json(message);
            }
        }

        public async Task<IActionResult> AppointmentChats(Guid? id)
        {
            AppointmentChatModel model = new AppointmentChatModel();
            var appointment = _context.TblAppointments.Find(id);
            model.AppointmentId = id;
            model.RequestedBy = appointment.RequestedBy;
            ViewBag.Chats = _context.TblAppointmentChats.Include(s => s.User).Include(s => s.ExterUser).Where(s => s.AppointmentId == id).ToList();
            ViewBag.id = id;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AppointmentChats(AppointmentChatModel chatModel)
        {
            try
            {
                var DepheadUser = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "CVA").FirstOrDefault();
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                TblAppointmentChat chat = new TblAppointmentChat();
                chat.AppointmentId = chatModel.AppointmentId;
                chat.ChatContent = chatModel.ChatContent;
                chat.Datetime = DateTime.Now;
                chat.UserId = DepheadUser.UserId;
                chat.ExterUserId = userId;
                chat.IsInternal = false;
                chat.IsEnternal = true;
                _context.TblAppointmentChats.Add(chat);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Sent");
                    return RedirectToAction(nameof(AppointmentChats), new { id = chatModel.AppointmentId });
                }
                else
                {
                    _notifyService.Error("Not sent. Please try again");
                    return View(chatModel);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message + " happened. Please try again");
                return View(chatModel);
            }
        }

        public async Task<IActionResult> AddAditionalDocs(Guid RequestID)
        {
            AddionalDocument addional= new AddionalDocument();
            addional.RequestID = RequestID;
            addional.tblDocuments=_context.TblDocumentHistories.Include(s=>s.Request).Where(s=>s.RequestId== RequestID).ToList();
            return View(addional);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAditionalDocs(AddionalDocument document)
        {
            try
            {
                TblDocumentHistory history = new TblDocumentHistory();
                history.FileTitle = document.Title;
                history.RequestId = document.RequestID;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "admin/Files");

               
                        if (document.formFile.FileName != null)
                        {
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            FileInfo fileInfo = new FileInfo(document.formFile.FileName);
                            string ExactFileName = document.formFile.FileName;
                            string fileName = Guid.NewGuid().ToString()+ document.formFile.FileName;
                            string fileNameWithPath = Path.Combine(path, fileName);
                            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                            {
                                document.formFile.CopyTo(stream);
                            }
                            string dbPath = "/admin/Files/" + fileName;
                    history.DocPath = dbPath;
                    history.ExactFileName = document.formFile.FileName;
                        }
                        _context.TblDocumentHistories.Add(history);
                int saved= await _context.SaveChangesAsync();
                if (saved>0)
                {
                    _notifyService.Success("Successfully Added");
                    return RedirectToAction(nameof(AddAditionalDocs), new { RequestID = document.RequestID });

                }
                else
                {
                    _notifyService.Error("Not added. Please try again");
                    return RedirectToAction(nameof(AddAditionalDocs), new { RequestID = document.RequestID });

                }
            }
            catch (Exception ex)
            {

                _notifyService.Error(ex.Message+" happened. Please try again");
                return RedirectToAction(nameof(AddAditionalDocs), new { RequestID = document.RequestID });

            }

        }
        public async Task<IActionResult> DeleteDocument(Guid RequestID,Guid HistoryID)
        {
            try
            {
                var docs = _context.TblDocumentHistories.Find(HistoryID);
                _context.TblDocumentHistories.Remove(docs);
                int deleted= await _context.SaveChangesAsync();
                if (deleted>0)
                {
                    _notifyService.Success("Successfully deleted");
                    return RedirectToAction(nameof(AddAditionalDocs), new { RequestID = RequestID });
                    
                }
                else
                {
                    _notifyService.Error("Not successfull. Please try again");
                    return RedirectToAction(nameof(AddAditionalDocs), new { RequestID = RequestID });
                }
            }
            catch (Exception ex)
            {

                _notifyService.Error(ex.Message+" happened. Please try again latter");
                return RedirectToAction(nameof(AddAditionalDocs), new { RequestID = RequestID });
            }
        }

        public async Task<IActionResult> EditRequestDetail(Guid? RequestID)
        {
            var request=_context.TblRequests.Find(RequestID);
            RequestModel requestModel= new RequestModel();
            requestModel.RequestId= RequestID;
            requestModel.RequestDetail = request.RequestDetail;
            return View(requestModel);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRequestDetail(RequestModel model)
        {
            try
            {
                var request = _context.TblRequests.Find(model.RequestId);
                request.RequestDetail = model.RequestDetail;
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    _notifyService.Success("Request detail successfully updated");
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    _notifyService.Error("Detail isn't successfully updated. Please try again latter");
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                _notifyService.Error(ex.Message+" happened. Detail isn't successfully updated. Please try again latter");
                return View(model);
            }
           
        }


    }
}
