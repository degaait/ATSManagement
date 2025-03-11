using System.Data;
using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.IModels;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
        private readonly INotificationService _notificationService;
        public RequestsController(AtsdbContext context, INotyfService notyfService, IMailService mailService, IHttpContextAccessor httpContextAccessor, INotificationService notificationService)
        {
            _mail = mailService;
            _notifyService = notyfService;
            _contextAccessor = httpContextAccessor;
            _context = context;
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
            if (userinfor.IsDeputy == false && userinfor.IsSuperAdmin != true)
            {
                _notifyService.Information("Only users who have deputy role can access this page");
                return RedirectToAction("Index", "Home", new { type = "0", message = "Only users who have deputy role can access this page" });
            }
            //var atsdbContext = _context.TblRequests
            //    .Include(t => t.AssignedByNavigation)
            //    .Include(t => t.CreatedByNavigation)
            //    .Include(t => t.DepartmentUpprovalStatusNavigation)
            //    .Include(t => t.DeputyUprovalStatusNavigation)
            //    .Include(t => t.DocType)
            //    .Include(t => t.ExternalRequestStatus)
            //    .Include(t => t.Inist)
            //    .Include(t => t.Priority)
            //    .Include(t => t.QuestType)
            //    .Include(t => t.RequestedByNavigation)
            //    .Include(t => t.ServiceType)
            //    .Include(t => t.TeamUpprovalStatusNavigation)
            //    .Include(t => t.UserUpprovalStatusNavigation).Where(x => x.IsAssignedTodepartment == false || x.IsAssignedTodepartment == null).OrderByDescending(s => s.OrderId);
            //return View(await atsdbContext.ToListAsync());

            var result = _context.NewRequestViewModels.FromSqlRaw($"EXEC GetNewRequests")
             .ToList(); //Materialize the result
            return View(result);



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
            List<Guid> cretatedTos = new List<Guid>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblRequest request = await _context.TblRequests.FindAsync(requestModel.RequestId);
            TblTopStatus topStatus = _context.TblTopStatuses.Where(s => s.StatusName == "In Progress").FirstOrDefault();
            List<TblRequestDepartmentRelation> departmentRelations;
            List<String> depHeadEmail = new List<string>();
            List<Guid> departmentIds = new List<Guid>();
            request.ServiceTypeId = requestModel.ServiceTypeID;
            request.DeputyRemark = requestModel.DeputyRemark;
            request.TopStatusId = topStatus.TopStatusId;
            if (requestModel.DepId.Length > 0)
            {
                request.IsAssignedTodepartment = true;
                departmentRelations = new List<TblRequestDepartmentRelation>();
                foreach (var item in requestModel.DepId)
                {
                    departmentRelations.Add(new TblRequestDepartmentRelation { DepId = item, RequestId = requestModel.RequestId, IsAssingedToUser = false, TeamId = null });
                }
                request.TblRequestDepartmentRelations = departmentRelations;
            }
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {

                foreach (var item in requestModel.DepId)
                {
                    var users = _context.TblInternalUsers.Where(x => x.DepId == item&&x.IsDepartmentHead==true||x.IsDeputy==true).Select(s => s.UserId).ToList();
                    var DepartmentHeade = _context.TblInternalUsers.Where(x => x.DepId == item && x.IsDepartmentHead == true || x.IsDeputy == true).Select(s => s.EmailAddress).ToList();
                    depHeadEmail.AddRange(DepartmentHeade);
                    departmentIds.AddRange(users);
                }
                _notifyService.Success("Request is assigned to Department");
                await SendMail(depHeadEmail, "Request Assignment notifications from Deputy director", "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");
                _notificationService.saveNotification(userId, departmentIds, "Request Assignment notifications from Deputy director");
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
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            var reques = _context.TblRequests.Find(id);
            RequestModel requestModel = new RequestModel();
            requestModel.RequestId = id;
            requestModel.DeputyRemark = null;
            requestModel.CreatedDate = reques.CreatedDate;
            requestModel.RequestDetail = reques.RequestDetail;
            requestModel.ServiceTypeID = reques.ServiceTypeId;
            if (cultur == "am")
            {
                requestModel.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                {
                    Text = x.DepNameAmharic,
                    Value = x.DepId.ToString(),

                }).ToList();
                requestModel.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
                {
                    Text = s.ServiceTypeNameAmharic,
                    Value = s.ServiceTypeId.ToString()
                }).ToList();

            }
            else
            {
                requestModel.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                {
                    Text = x.DepName,
                    Value = x.DepId.ToString()
                }).ToList();
                requestModel.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
                {
                    Text = s.ServiceTypeName,
                    Value = s.ServiceTypeId.ToString()
                }).ToList();

            }
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
