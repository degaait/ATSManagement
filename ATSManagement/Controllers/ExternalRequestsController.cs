using NToastNotify;
using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.Filters;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public ExternalRequestsController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mail, INotyfService notyfService)
        {
            _notifyService = notyfService;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
        }

        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var userinfor = _context.TblInternalUsers.Where(x => x.UserId == userId).FirstOrDefault();
            if (userinfor == null)
            {
                return NotFound();
            }
            if (userinfor.IsDeputy == false && userinfor.IsSuperAdmin != true)
            {
                _notifyService.Information("Only users who have deputy role can access this page");
                return RedirectToAction("Index", "Home", new { type = "0", message = "Only users who have deputy role can access this page" });
            }
            var atsdbContext = _context.TblRequests
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.CaseType)
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
                .Include(t => t.UserUpprovalStatusNavigation).Where(x => x.IsAssignedTodepartment == false || x.IsAssignedTodepartment == null);
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
                .Include(t => t.CaseType)
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
                request.InistId = model.InistId;
                request.CreatedBy = userId;
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
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    FileInfo fileInfo = new FileInfo(model.DocumentFile.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.DocumentFile.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.DocumentFile.CopyTo(stream);
                    }
                    string dbPath = "/admin/Files/" + fileName;
                    request.DocTypeId = model.DocId;
                    documentHistory.Add(new TblDocumentHistory { DocPath = dbPath, Round = model.Round, RequestId = request.RequestId });
                    request.TblDocumentHistories = documentHistory;
                }
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
                int saved = await _context.SaveChangesAsync();
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
        public IActionResult AssignToDepartment(Guid id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            model.RequestedDate = DateTime.Now;
            model.ExterUserId = userId;
            model.InistId = instName.InistId;

            return View(model);
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }

        public CivilJusticeExternalRequestModel getModel()
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
           
            model.RequestedDate = DateTime.Now;
            model.CreatedDate = DateTime.Now;
            model.ExterUserId = userId;
            model.Round = 1;
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
            model.Intitutions = _context.TblInistitutions.Select(s => new SelectListItem
            {
                Value=s.InistId.ToString(),
                Text=s.Name.ToString()
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
            model.TermsAndCondionts = _context.TblTermsAndConditions.Select(s => s.Details).FirstOrDefault();
            List<CompletedRequests> completeds = new List<CompletedRequests>();
            CompletedRequests completedRequests1;
            List<RoundModel> modelr = new List<RoundModel>
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
            return model;
        }
    }
}
