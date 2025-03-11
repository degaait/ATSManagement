using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.IModels;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class ExternalRequestsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        private readonly INotificationService _notificationService;
        public ExternalRequestsController(AtsdbContext context, IHttpContextAccessor contextAccessor,
            IMailService mail, INotyfService notyfService, INotificationService notificationService)
        {
            _notifyService = notyfService;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
            _notificationService = notificationService;
        }

        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var userinfor = _context.TblInternalUsers.Where(x => x.UserId == userId).FirstOrDefault();
            if (userinfor == null)
            {
                return NotFound();
            }
            string fullname= userinfor.FirstName + " " + userinfor.MidleName + " " + userinfor.LastName;
            if (userinfor.IsSecretary == false)
            {
                _notifyService.Information("Only users who have Secretary role can access this page");
                return RedirectToAction("Index", "Home", new { type = "0", message = "Only users who have Secretary role can access this page" });

            }  
            var atsdbContext = _context.TblRequests
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.DepartmentUpprovalStatusNavigation)
                .Include(t => t.DeputyUprovalStatusNavigation)
                .Include(t => t.DocType)
                .Include(t => t.ExternalRequestStatus)
                .Include(t => t.Inist)
                .Include(t => t.Priority)
                .Include(t => t.QuestType)
                .Include(t => t.ServiceType)
                .Include(t => t.TeamUpprovalStatusNavigation)
                .Include(t => t.UserUpprovalStatusNavigation).Where(x=>x.SecretaryFullName==fullname);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> AssignedRequests()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var atsdbContext = _context.TblExternalRequests
                .Include(t => t.ExterUser)
                .Include(t => t.Int)
                .Include(s => s.ExternalRequestStatus).Where(x => x.DepId == null);
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
        public async Task<IActionResult> DetailsNewRequest(Guid? id)
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
        public async Task<IActionResult> DetailsPendingRequest(Guid? id)
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
        public async Task<IActionResult> DetailsCompleted(Guid? id)
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
            return View(getModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CivilJusticeExternalRequestModel model)
        {
            try
            {
                var ExternalUser = (from item in _context.TblInistitutions
                                     join ext in _context.TblExternalUsers on item.InistId equals ext.InistId
                                     select ext.ExterUserId).FirstOrDefault();
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                List<Guid> depsID = new List<Guid>();
                List<TblRequestDepartmentRelation> departmentRelations;
                List<string> depHeadEmail = new List<string>();
                List<Guid> departmentIds = new List<Guid>();
                var userDetail = _context.TblInternalUsers.Find(userId);
                if (userDetail.IsSecretary==true)
                {
                    if (userDetail.IsLegalStudySecretary==true)
                    {
                        depsID = _context.TblDepartments.Where(s => s.DepCode == "LSDC").Select(s => s.DepId).ToList();
                    }
                    else if (userDetail.IsCivilJusticeSecretay==true)
                    {
                        depsID = _context.TblDepartments.Where(s => s.DepCode == "CVA").Select(s => s.DepId).ToList();
                    }
                   
                }
                
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
                var institutionName = (from items in _context.TblInistitutions where items.InistId == model.InistId select items.Name).FirstOrDefault();
                if (depsID!=null)
                {
                    foreach (var item in depsID)
                    {
                        var DepartmentHeade = _context.TblInternalUsers.Where(x => x.Dep.DepId == item && x.IsDepartmentHead == true).Select(s => s.EmailAddress).ToList();
                        depHeadEmail.AddRange(DepartmentHeade);
                    }
                }
                else
                {
                    var DepartmentHeade = _context.TblInternalUsers.Where(x => x.IsDeputy == true).Select(s => s.EmailAddress).ToList();
                    depHeadEmail.AddRange(DepartmentHeade);
                }
                              
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
                request.RequestDetail = model.RequestDetail;
                request.InistId = model.InistId;               
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
                request.SecretaryFullName = userDetail.FirstName + " " + userDetail.MidleName + " " + userDetail.LastName;
                request.CourtCenter = model.CourtCenter;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
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
                    if (depsID.Count > 0)
                    {
                        request.IsAssignedTodepartment = true;
                        departmentRelations = new List<TblRequestDepartmentRelation>();
                        foreach (var item in depsID)
                        {
                            departmentRelations.Add(new TblRequestDepartmentRelation { DepId = item, RequestId = request.RequestId, IsAssingedToUser = false, TeamId = null });
                        }
                        request.TblRequestDepartmentRelations = departmentRelations;
                    }
                else
                {
                    request.IsAssignedTodepartment = false;                
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
                    _notifyService.Success("Your request is submitted Successfully. Responsive body is notified by email");
                    await SendMail(depHeadEmail, "Request notifications from " + institutionName, "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");
                    _notificationService.saveNotification(userId, deputy, "New service request is added from " + institutionName);
                    return RedirectToAction(nameof(ExternalRequestsController.Index), "ExternalRequests");
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
        public async Task<IActionResult> CivilJustice()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblCivilJustices
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.AssignedToNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(t => t.Priority).Where(x => x.Dep.DepCode == "CVA");
            return View(await atsdbContext.ToListAsync());
        }     
        public async Task<IActionResult> LegalStudies()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var atsdbContext = _context.TblLegalStudiesDraftings
               .Include(t => t.AssignedByNavigation)
               .Include(t => t.AssignedToNavigation)
               .Include(t => t.Doc)
               .Include(x => x.QuestType)
               .Include(t => t.Dep)
               .Include(t => t.Inist)
               .Include(t => t.RequestedByNavigation)
               .Include(x => x.ExternalRequestStatus)
               .Include(t => t.Priority).Where(x => x.Dep.DepCode == "LSDC");
            return View(await atsdbContext.ToListAsync());
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
            model.InistId = tblExternalRequest.IntId;
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
                var ExternalUser = (from item in _context.TblInistitutions
                                    join ext in _context.TblExternalUsers on item.InistId equals ext.InistId
                                    select ext.ExterUserId).FirstOrDefault();
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                List<Guid> depsID = new List<Guid>();

                var userDetail = _context.TblInternalUsers.Find(userId);
                if (userDetail.IsSecretary == true)
                {
                    if (userDetail.IsLegalStudySecretary == true)
                    {
                        depsID = _context.TblDepartments.Where(s => s.DepCode == "LSDC").Select(s => s.DepId).ToList();
                    }
                    else if (userDetail.IsCivilJusticeSecretay == true)
                    {
                        depsID = _context.TblDepartments.Where(s => s.DepCode == "CVA").Select(s => s.DepId).ToList();
                    }
                }
                List<TblRequestDepartmentRelation> departmentRelations;
                List<string> depHeadEmail = new List<string>();
                List<Guid> departmentIds = new List<Guid>();
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
                var institutionName = (from items in _context.TblInistitutions where items.InistId == model.InistId select items.Name).FirstOrDefault();

                foreach (var item in depsID)
                {
                    var users = _context.TblInternalUsers.Where(x => x.DepId == item).Select(s => s.UserId).ToList();
                    var DepartmentHeade = _context.TblInternalUsers.Where(x => x.Dep.DepId == item).Select(s => s.EmailAddress).ToList();
                    depHeadEmail.AddRange(DepartmentHeade);
                    departmentIds.AddRange(users);
                }
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
                request.RequestDetail = model.RequestDetail;
                request.InistId = model.InistId;

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
                request.SecretaryFullName = userDetail.FirstName + " " + userDetail.MidleName + " " + userDetail.LastName;
                request.CourtCenter = model.CourtCenter;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
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
                //if (userDetail.!=true)
                //{
                if (depsID.Count > 0)
                {
                    request.IsAssignedTodepartment = true;
                    departmentRelations = new List<TblRequestDepartmentRelation>();
                    foreach (var item in depsID)
                    {
                        departmentRelations.Add(new TblRequestDepartmentRelation { DepId = item, RequestId = request.RequestId, IsAssingedToUser = false, TeamId = null });
                    }
                    request.TblRequestDepartmentRelations = departmentRelations;
                }
                //}              
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
                    _notifyService.Success("Your request is submitted Successfully. Responsive body is notified by email");
                    await SendMail(depHeadEmail, "Request notifications from " + institutionName, "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");
                    _notificationService.saveNotification(userId, deputy, "New service request is added from " + institutionName);
                    return RedirectToAction(nameof(ExternalRequestsController.Index), "ExternalRequests");
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
        public IActionResult AssignToDepartment(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            return View(getModels(id));
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToDepartment(RequestModel requestModel)
        {
            TblRequest request = await _context.TblRequests.FindAsync(requestModel.RequestId);
            List<TblRequestDepartmentRelation> departmentRelations;
            List<String> depHeadEmail = new List<string>();
            request.ServiceTypeId = requestModel.ServiceTypeID;
            request.IsAssignedTodepartment = true;
            if (requestModel.DepId.Length > 0)
            {
                departmentRelations = new List<TblRequestDepartmentRelation>();
                foreach (var item in requestModel.DepId)
                {
                    departmentRelations.Add(new TblRequestDepartmentRelation { DepId = item, RequestId = requestModel.RequestId });
                }
                request.TblRequestDepartmentRelations = departmentRelations;
            }
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                foreach (var item in requestModel.DepId)
                {
                    var DepartmentHeade = _context.TblInternalUsers.Where(x => x.Dep.DepId == item).Select(x => x.EmailAddress).ToList();
                    depHeadEmail.AddRange(DepartmentHeade);
                }
                _notifyService.Success("Request is assigned to Department");
                await SendMail(depHeadEmail, "Request Assignment notifications from Deputy director", "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");

                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notifyService.Error("Request isn't assigned. Please try again");
                return View(getModels(requestModel.RequestId));
            }
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            if (to!=null)
            {
                var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
                MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
                bool sentResult = await _mail.SendAsync(data, new CancellationToken());
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

                model.ServiceTypes = _context.TblServiceTypes.OrderBy(s => s.ServiceOrderOrder).Select(s => new SelectListItem
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

            model.RequestedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
            model.ExterUserId = userId;
            model.Round = 1;
            if (cultur == "am")
            {
                model.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                {
                    Value = s.InistId.ToString(),
                    Text = s.NameAmharic.ToString()
                }).ToList();
            }
            else
            {
                model.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                    {
                        Value = s.InistId.ToString(),
                        Text = s.Name.ToString()
                    }).ToList();
            }
        
            model.Deparments = _context.TblDepartments.Select(s => new SelectListItem
            {
                Value = s.DepId.ToString(),
                Text = s.DepName.ToString()
            }).ToList();
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
        public RequestModel getModels(Guid? id)
        {
            var reques = _context.TblRequests.Find(id);
            RequestModel requestModel = new RequestModel();
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();

            requestModel.RequestId = id;
            requestModel.CreatedDate = reques.CreatedDate;
            requestModel.Deparments = _context.TblDepartments.Select(x => new SelectListItem
            {
                Text = x.DepName,
                Value = x.DepId.ToString()
            }).ToList();
            requestModel.RequestDetail = reques.RequestDetail;
            requestModel.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
            {
                Text = s.ServiceTypeName,
                Value = s.ServiceTypeId.ToString()
            }).ToList();
            requestModel.ServiceTypeID = reques.ServiceTypeId;
            if (cultur=="am")
            {
                requestModel.Intitutions = _context.TblInistitutions.Where(s => s.InistId == reques.InistId).Select(x => new SelectListItem
                {
                    Text = x.NameAmharic,
                    Value = x.InistId.ToString()
                }).ToList();
            }
            else
            {
                requestModel.Intitutions = _context.TblInistitutions.Where(s => s.InistId == reques.InistId).Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.InistId.ToString()
                }).ToList();
            }
           
            requestModel.InistId = reques.InistId;
            requestModel.RequestedUsers = _context.TblExternalUsers.Where(x => x.ExterUserId == reques.RequestedBy).Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.MiddleName,
                Value = x.ExterUserId.ToString()
            }).ToList();
            requestModel.RequestedBy = reques.RequestedBy;
            return requestModel;
        }
        private async Task<DocumentHistoryModel> historyModel(Guid? id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblDocumentHistory tblDocument = _context.TblDocumentHistories.Include(x => x.Request).Where(x => x.RequestId == id).OrderBy(x => x.Round).Last();
            DocumentHistoryModel model = new DocumentHistoryModel();
            ViewData["histories"] = _context.TblDocumentHistories.Where(x => x.RequestId == id).ToList();
            model.RequestId = id;
            model.ExternalRepliedBy = userId;
            if (cultur == "am")
            {
                model.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                {
                    Value = s.InistId.ToString(),
                    Text = s.NameAmharic.ToString()
                }).ToList();
            }
            else
            {
                model.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                {
                    Value = s.InistId.ToString(),
                    Text = s.Name.ToString()
                }).ToList();
            }
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
        public async Task<IActionResult> AddAditionalDocs(Guid? RequestID)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            AddionalDocument addional = new AddionalDocument();
            addional.RequestID = RequestID;
            if (cultur == "am")
            {
                addional.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                {
                    Value = s.InistId.ToString(),
                    Text = s.NameAmharic.ToString()
                }).ToList();
            }
            else
            {
                addional.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                {
                    Value = s.InistId.ToString(),
                    Text = s.Name.ToString()
                }).ToList();
            }
            addional.tblDocuments = _context.TblDocumentHistories.Include(s => s.Request).Where(s => s.RequestId == RequestID).ToList();
            return View(addional);
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddAditionalDocs(AddionalDocument? document)
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
                    string fileName = Guid.NewGuid().ToString() + document.formFile.FileName;
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
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
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

                _notifyService.Error(ex.Message + " happened. Please try again");
                return RedirectToAction(nameof(AddAditionalDocs), new { RequestID = document.RequestID });

            }
        }
        public async Task<IActionResult> DeleteDocument(Guid RequestID, Guid HistoryID)
        {
            try
            {
                var docs = _context.TblDocumentHistories.Find(HistoryID);
                _context.TblDocumentHistories.Remove(docs);
                int deleted = await _context.SaveChangesAsync();
                if (deleted > 0)
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

                _notifyService.Error(ex.Message + " happened. Please try again latter");
                return RedirectToAction(nameof(AddAditionalDocs), new { RequestID = RequestID });
            }
        }
        public async Task<IActionResult> UpdateInstitute(Guid? RequestID)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            AddionalDocument addional = new AddionalDocument();
            TblRequest request = _context.TblRequests.Find(RequestID);
            addional.RequestID = RequestID;
            addional.InistId = request.InistId;
            if (cultur == "am")
            {
                addional.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                {
                    Value = s.InistId.ToString(),
                    Text = s.NameAmharic.ToString()
                }).ToList();
            }
            else
            {
                addional.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                {
                    Value = s.InistId.ToString(),
                    Text = s.Name.ToString()
                }).ToList();
            }
            addional.tblDocuments = _context.TblDocumentHistories.Include(s => s.Request).Where(s => s.RequestId == RequestID).ToList();
            return View(addional);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateInstitute(AddionalDocument? document)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            try
            {
                TblRequest request = _context.TblRequests.Find(document.RequestID);               
                request.InistId = document.InistId;
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Successfully Added");
                    return RedirectToAction(nameof(Index));

                }
                else
                {
                    _notifyService.Error("Not added. Please try again");
                    if (cultur == "am")
                    {
                        document.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                        {
                            Value = s.InistId.ToString(),
                            Text = s.NameAmharic.ToString()
                        }).ToList();
                    }
                    else
                    {
                        document.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                        {
                            Value = s.InistId.ToString(),
                            Text = s.Name.ToString()
                        }).ToList();
                    }
                    document.tblDocuments = _context.TblDocumentHistories.Include(s => s.Request).Where(s => s.RequestId == document.RequestID).ToList();

                    return RedirectToAction(nameof(UpdateInstitute), new { RequestID = document.RequestID });

                }
            }
            catch (Exception ex)
            {

                _notifyService.Error(ex.Message + " happened. Please try again");
                if (cultur == "am")
                {
                    document.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                    {
                        Value = s.InistId.ToString(),
                        Text = s.NameAmharic.ToString()
                    }).ToList();
                }
                else
                {
                    document.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
                    {
                        Value = s.InistId.ToString(),
                        Text = s.Name.ToString()
                    }).ToList();
                }
                document.tblDocuments = _context.TblDocumentHistories.Include(s => s.Request).Where(s => s.RequestId == document.RequestID).ToList();

                return RedirectToAction(nameof(UpdateInstitute), new { RequestID = document.RequestID });

            }
        }

    }
}
