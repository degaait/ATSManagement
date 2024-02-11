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

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class RequestsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        public RequestsController(AtsdbContext context, INotyfService notyfService, IMailService mailService, IHttpContextAccessor httpContextAccessor)
        {
            _mail = mailService;
            _notifyService = notyfService;
            _contextAccessor = httpContextAccessor;
            _context = context;
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
                .Include(t => t.UserUpprovalStatusNavigation).Where(x => x.IsAssignedTodepartment == false || x.IsAssignedTodepartment == null);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> AssignToDepartment(Guid? id)

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
            TblTopStatus topStatus = _context.TblTopStatuses.Where(s => s.StatusName == "In Progress").FirstOrDefault();
            List<TblRequestDepartmentRelation> departmentRelations;
            List<String> depHeadEmail = new List<string>();
            request.ServiceTypeId = requestModel.ServiceTypeID;
            request.IsAssignedTodepartment = true;
            request.DeputyRemark = requestModel.DeputyRemark;
            request.TopStatusId = topStatus.TopStatusId;
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
        public RequestModel getModels(Guid? id)
        {
            var reques = _context.TblRequests.Find(id);
            RequestModel requestModel = new RequestModel();
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
            requestModel.Intitutions = _context.TblInistitutions.Where(s => s.InistId == reques.InistId).Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.InistId.ToString()
            }).ToList();
            requestModel.InistId = reques.InistId;
            requestModel.RequestedUsers = _context.TblExternalUsers.Where(x => x.ExterUserId == reques.RequestedBy).Select(x => new SelectListItem
            {
                Text = x.FirstName + " " + x.MiddleName,
                Value = x.ExterUserId.ToString()
            }).ToList();
            requestModel.RequestedBy = reques.RequestedBy;
            return requestModel;
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
            ViewData["AssignedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["CaseTypeId"] = new SelectList(_context.TblCivilJusticeCaseTypes, "CaseTypeId", "CaseTypeId");
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepId");
            ViewData["DepartmentUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId");
            ViewData["DeputyUprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId");
            ViewData["DocTypeId"] = new SelectList(_context.TblLegalDraftingDocTypes, "DocId", "DocId");
            ViewData["ExternalRequestStatusId"] = new SelectList(_context.TblExternalRequestStatuses, "ExternalRequestStatusId", "ExternalRequestStatusId");
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId");
            ViewData["PriorityId"] = new SelectList(_context.TblPriorities, "PriorityId", "PriorityId");
            ViewData["QuestTypeId"] = new SelectList(_context.TblLegalDraftingQuestionTypes, "QuestTypeId", "QuestTypeId");
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId");
            ViewData["ServiceTypeId"] = new SelectList(_context.TblServiceTypes, "ServiceTypeId", "ServiceTypeId");
            ViewData["TeamUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId");
            ViewData["UserUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,RequestDetail,InistId,RequestedBy,CreatedDate,CreatedBy,DepId,CaseTypeId,AssignedBy,AssignedDate,DueDate,AssingmentRemark,AssignedTo,ExternalRequestStatusId,TopStatus,PriorityId,UserUpprovalStatus,TeamUpprovalStatus,DeputyUprovalStatus,DepartmentUpprovalStatus,DocTypeId,QuestTypeId,FinalReport,DocumentFile,ServiceTypeId,RequestRound")] TblRequest tblRequest)
        {
            if (ModelState.IsValid)
            {
                tblRequest.RequestId = Guid.NewGuid();
                _context.Add(tblRequest);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AssignedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRequest.AssignedBy);
            ViewData["CaseTypeId"] = new SelectList(_context.TblCivilJusticeCaseTypes, "CaseTypeId", "CaseTypeId", tblRequest.CaseTypeId);
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRequest.CreatedBy);
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepId");
            ViewData["DepartmentUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.DepartmentUpprovalStatus);
            ViewData["DeputyUprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.DeputyUprovalStatus);
            ViewData["DocTypeId"] = new SelectList(_context.TblLegalDraftingDocTypes, "DocId", "DocId", tblRequest.DocTypeId);
            ViewData["ExternalRequestStatusId"] = new SelectList(_context.TblExternalRequestStatuses, "ExternalRequestStatusId", "ExternalRequestStatusId", tblRequest.ExternalRequestStatusId);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblRequest.InistId);
            ViewData["PriorityId"] = new SelectList(_context.TblPriorities, "PriorityId", "PriorityId", tblRequest.PriorityId);
            ViewData["QuestTypeId"] = new SelectList(_context.TblLegalDraftingQuestionTypes, "QuestTypeId", "QuestTypeId", tblRequest.QuestTypeId);
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblRequest.RequestedBy);
            ViewData["ServiceTypeId"] = new SelectList(_context.TblServiceTypes, "ServiceTypeId", "ServiceTypeId", tblRequest.ServiceTypeId);
            ViewData["TeamUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.TeamUpprovalStatus);
            ViewData["UserUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.UserUpprovalStatus);
            return View(tblRequest);
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblRequests == null)
            {
                return NotFound();
            }

            var tblRequest = await _context.TblRequests.FindAsync(id);
            if (tblRequest == null)
            {
                return NotFound();
            }
            ViewData["AssignedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRequest.AssignedBy);
            ViewData["CaseTypeId"] = new SelectList(_context.TblCivilJusticeCaseTypes, "CaseTypeId", "CaseTypeId", tblRequest.CaseTypeId);
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRequest.CreatedBy);
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepId", tblRequest);
            ViewData["DepartmentUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.DepartmentUpprovalStatus);
            ViewData["DeputyUprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.DeputyUprovalStatus);
            ViewData["DocTypeId"] = new SelectList(_context.TblLegalDraftingDocTypes, "DocId", "DocId", tblRequest.DocTypeId);
            ViewData["ExternalRequestStatusId"] = new SelectList(_context.TblExternalRequestStatuses, "ExternalRequestStatusId", "ExternalRequestStatusId", tblRequest.ExternalRequestStatusId);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblRequest.InistId);
            ViewData["PriorityId"] = new SelectList(_context.TblPriorities, "PriorityId", "PriorityId", tblRequest.PriorityId);
            ViewData["QuestTypeId"] = new SelectList(_context.TblLegalDraftingQuestionTypes, "QuestTypeId", "QuestTypeId", tblRequest.QuestTypeId);
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblRequest.RequestedBy);
            ViewData["ServiceTypeId"] = new SelectList(_context.TblServiceTypes, "ServiceTypeId", "ServiceTypeId", tblRequest.ServiceTypeId);
            ViewData["TeamUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.TeamUpprovalStatus);
            ViewData["UserUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.UserUpprovalStatus);
            return View(tblRequest);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RequestId,RequestDetail,InistId,RequestedBy,CreatedDate,CreatedBy,DepId,CaseTypeId,AssignedBy,AssignedDate,DueDate,AssingmentRemark,AssignedTo,ExternalRequestStatusId,TopStatus,PriorityId,UserUpprovalStatus,TeamUpprovalStatus,DeputyUprovalStatus,DepartmentUpprovalStatus,DocTypeId,QuestTypeId,FinalReport,DocumentFile,ServiceTypeId,RequestRound")] TblRequest tblRequest)
        {
            if (id != tblRequest.RequestId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblRequest);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblRequestExists(tblRequest.RequestId))
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
            ViewData["AssignedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRequest.AssignedBy);
            ViewData["CaseTypeId"] = new SelectList(_context.TblCivilJusticeCaseTypes, "CaseTypeId", "CaseTypeId", tblRequest.CaseTypeId);
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRequest.CreatedBy);

            ViewData["DepartmentUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.DepartmentUpprovalStatus);
            ViewData["DeputyUprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.DeputyUprovalStatus);
            ViewData["DocTypeId"] = new SelectList(_context.TblLegalDraftingDocTypes, "DocId", "DocId", tblRequest.DocTypeId);
            ViewData["ExternalRequestStatusId"] = new SelectList(_context.TblExternalRequestStatuses, "ExternalRequestStatusId", "ExternalRequestStatusId", tblRequest.ExternalRequestStatusId);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblRequest.InistId);
            ViewData["PriorityId"] = new SelectList(_context.TblPriorities, "PriorityId", "PriorityId", tblRequest.PriorityId);
            ViewData["QuestTypeId"] = new SelectList(_context.TblLegalDraftingQuestionTypes, "QuestTypeId", "QuestTypeId", tblRequest.QuestTypeId);
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblRequest.RequestedBy);
            ViewData["ServiceTypeId"] = new SelectList(_context.TblServiceTypes, "ServiceTypeId", "ServiceTypeId", tblRequest.ServiceTypeId);
            ViewData["TeamUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.TeamUpprovalStatus);
            ViewData["UserUpprovalStatus"] = new SelectList(_context.TblDecisionStatuses, "DesStatusId", "DesStatusId", tblRequest.UserUpprovalStatus);
            return View(tblRequest);
        }
        public async Task<IActionResult> Delete(Guid? id)
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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblRequests == null)
            {
                return Problem("Entity set 'AtsdbContext.TblRequests'  is null.");
            }
            var tblRequest = await _context.TblRequests.FindAsync(id);
            if (tblRequest != null)
            {
                _context.TblRequests.Remove(tblRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblRequestExists(Guid id)
        {
            return (_context.TblRequests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
        public async Task<IActionResult> HighPriorityRequsts()
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
                .Include(t => t.UserUpprovalStatusNavigation).Where(x => x.PriorityId == Guid.Parse("12fba758-fa2a-406a-ae64-0a561d0f5e73"));
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> DownloadEvidenceFile(string path)
        {
            string filename = path.Substring(7);
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "admin\\", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }

       
    }
}
