using ATSManagement.IModels;
using ATSManagement.Models;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using NToastNotify;

namespace ATSManagement.Controllers
{
    public class CivilJusticesController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly IToastNotification _toastNotification;
        public CivilJusticesController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mailService, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
        }
        // GET: CivilJustices
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.Dep.DepCode == "CVA");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> AddActivity(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            ActivityModel model = new ActivityModel();
            model.RequestId = id;
            model.AddedDate = DateTime.Now;
            model.CreatedBy = userId;
            ViewData["Activities"] = _context.TblActivities
                 .Include(x => x.Request)
                 .Include(x => x.CreatedByNavigation)
                 .Where(x => x.RequestId == model.RequestId).ToList();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddActivity(ActivityModel model)
        {
            List<string> assignedBody = new List<string>();
            var request = _context.TblRequests.Where(x => x.RequestId == model.RequestId).FirstOrDefault();
            assignedBody = (from items in _context.TblInternalUsers where items.UserId == request.AssignedBy select items.EmailAddress).ToList();
            TblActivity activity = new TblActivity();
            activity.RequestId = model.RequestId;
            activity.AddedDate = DateTime.Now;
            activity.ActivityDetail = model.ActivityDetail;
            activity.CreatedBy = model.CreatedBy;
            _context.TblActivities.Add(activity);
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                model.ActivityDetail = null;
                SendMail(assignedBody, "Adding activities notifications.", "<h3>Assigned body is adding activities via <strong> Task tacking Dashboard</strong>. Please check on the system and followup!.</h3>");
                ViewData["Activities"] = _context.TblActivities
                    .Include(x => x.Request)
                    .Include(x => x.CreatedByNavigation)
                    .Where(x => x.RequestId == model.RequestId).ToList();

                return View(model);
            }
            else
            {
                ViewData["Activities"] = _context.TblActivities
                    .Include(x => x.Request)
                    .Include(x => x.CreatedByNavigation)
                    .Where(x => x.RequestId == model.RequestId).ToList();

                return View(model);
            }

        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblActivities == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblRequests
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.CaseType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(t => t.ExternalRequestStatus)
                .Include(x => x.DepartmentUpprovalStatusNavigation)
                .Include(x => x.DeputyUprovalStatusNavigation)
                .Include(y => y.TeamUpprovalStatusNavigation)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblCivilJustice == null)
            {
                return NotFound();
            }

            return View(tblCivilJustice);
        }
        public IActionResult Create()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
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
            model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
            {
                Value = x.CaseTypeId.ToString(),
                Text = x.CaseTypeName
            }).ToList();
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = userId;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CivilJusticeExternalRequestModel model)
        {
            try
            {
                TblRequest tblCivilJustice = new TblRequest();
                var decision = _context.TblDecisionStatuses.Where(x => x.StatusName == "Not set").FirstOrDefault();
                Guid statusiD = (from id in _context.TblExternalRequestStatuses where id.StatusName == "New" select id.ExternalRequestStatusId).FirstOrDefault();
                tblCivilJustice.RequestDetail = model.RequestDetail;
                tblCivilJustice.CreatedBy = model.CreatedBy;
                tblCivilJustice.CreatedDate = DateTime.Now;
                tblCivilJustice.CaseTypeId = model.CaseTypeId;
                tblCivilJustice.InistId = model.InistId;
                tblCivilJustice.PriorityId = model.PriorityId;
                tblCivilJustice.DepId = model.DepId;
                tblCivilJustice.DepartmentUpprovalStatus = decision.DesStatusId;
                tblCivilJustice.TeamUpprovalStatus = decision.DesStatusId;
                tblCivilJustice.DeputyUprovalStatus = decision.DesStatusId;
                tblCivilJustice.UserUpprovalStatus = decision.DesStatusId;
                tblCivilJustice.ExternalRequestStatusId = statusiD;
                _context.TblRequests.Add(tblCivilJustice);
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
                    model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
                    {
                        Value = x.CaseTypeId.ToString(),
                        Text = x.CaseTypeName
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
                model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
                {
                    Value = x.CaseTypeId.ToString(),
                    Text = x.CaseTypeName
                }).ToList();
                return View(model);
            }

        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            if (id == null || _context.TblCivilJustices == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblRequests.FindAsync(id);
            if (tblCivilJustice == null)
            {
                return NotFound();
            }
            model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
            {
                Value = x.InistId.ToString(),
                Text = x.Name
            }).ToList();
            model.InistId = tblCivilJustice.InistId;
            model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
            {
                Value = x.DepId.ToString(),
                Text = x.DepName
            }).ToList();
            model.DepId = tblCivilJustice.DepId;
            model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
            {
                Text = x.PriorityName,
                Value = x.PriorityId.ToString()
            }).ToList();
            model.PriorityId = tblCivilJustice.PriorityId;
            model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
            {
                Value = x.CaseTypeId.ToString(),
                Text = x.CaseTypeName
            }).ToList();
            model.CaseTypeId = tblCivilJustice.CaseTypeId;
            model.RequestDetail = tblCivilJustice.RequestDetail;
            model.RequestId = tblCivilJustice.RequestId;
            model.CreatedDate = tblCivilJustice.CreatedDate;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CivilJusticeExternalRequestModel model)
        {
            try
            {
                if (model.RequestId == Guid.Empty)
                {
                    return NotFound();
                }
                if (!TblCivilJusticeExists(model.RequestId))
                {
                    return NotFound();
                }
                TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(model.RequestId);
                tblCivilJustice.RequestDetail = model.RequestDetail;
                tblCivilJustice.PriorityId = model.PriorityId;
                tblCivilJustice.DepId = model.DepId;
                tblCivilJustice.CaseTypeId = model.CaseTypeId;
                tblCivilJustice.InistId = model.InistId;
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    return RedirectToAction("Index");

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
                    model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
                    {
                        Value = x.CaseTypeId.ToString(),
                        Text = x.CaseTypeName
                    }).ToList();
                    return View(model);

                }
            }
            catch (Exception EX)
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
                model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
                {
                    Value = x.CaseTypeId.ToString(),
                    Text = x.CaseTypeName
                }).ToList();
                return View(model);
            }

        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblCivilJustices == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblRequests
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.CaseType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(t => t.ExternalRequestStatus)
                .Include(x => x.DepartmentUpprovalStatusNavigation)
                .Include(x => x.DeputyUprovalStatusNavigation)
                .Include(y => y.TeamUpprovalStatusNavigation)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblCivilJustice == null)
            {
                return NotFound();
            }

            return View(tblCivilJustice);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblRequests == null)
            {
                return Problem("Entity set 'AtsdbContext.TblCivilJustices'  is null.");
            }
            var tblCivilJustice = await _context.TblRequests.FindAsync(id);
            if (tblCivilJustice != null)
            {
                _context.TblRequests.Remove(tblCivilJustice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblCivilJusticeExists(Guid id)
        {
            return (_context.TblRequests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> AssignToUser(Guid id)
        {

            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblCivilJustice drafting = await _context.TblCivilJustices.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            model.RequestDetail = drafting.RequestDetail;
            model.RequestId = id;
            model.AssignedBy = userId;
            model.AssignedDate = DateTime.Now;
            model.CreatedBy = drafting.CreatedBy;
            model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
            {
                Text = x.CaseTypeName,
                Value = x.CaseTypeId.ToString()

            }).ToList();
            model.CaseTypeId = drafting.CaseTypeId;
            model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
            {
                Value = x.DepId.ToString(),
                Text = x.DepName

            }).ToList();
            model.DepId = drafting.DepId;
            model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(),
                Text = x.FirstName.ToString() + " " + x.MidleName

            }).ToList();
            model.DueDate = DateTime.Now.AddDays(10);
            model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
            {
                Value = x.PriorityId.ToString(),
                Text = x.PriorityName.ToString()

            }).ToList();
            model.PriorityId = drafting.PriorityId;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToUser(CivilJusticeExternalRequestModel model)
        {
            List<string>? emails = new List<string>();
            List<TblRequestAssignee> assignees;
            foreach (var item in model.AssignedTo)
            {
                var userEmails = (from user in _context.TblInternalUsers where user.UserId == item select user.EmailAddress).FirstOrDefault();
                emails.Add(userEmails);
            }
            TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "In Progress" select items).FirstOrDefault();
            if (model.RequestId == null)
            {
                return NotFound();
            }
            TblRequest drafting = await _context.TblRequests.FindAsync(model.RequestId);
            if (drafting == null)
            {
                return NotFound();
            }
            try
            {
                drafting.DueDate = model.DueDate;
                drafting.AssignedDate = model.AssignedDate;
                drafting.PriorityId = model.PriorityId;
                drafting.AssignedBy = model.AssignedBy;
                drafting.AssingmentRemark = model.AssingmentRemark;
                drafting.CreatedBy = model.CreatedBy;
                drafting.CaseTypeId = model.CaseTypeId;
                drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                if (model.AssignedTo.Length > 0)
                {
                    assignees = new List<TblRequestAssignee>();
                    foreach (var item in model.AssignedTo)
                    {
                        assignees.Add(new TblRequestAssignee { UserId = item, RequestId = model.RequestId });
                    }
                    drafting.TblRequestAssignees = assignees;
                }
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    await SendMail(emails, "Task is assign notification", "<h3>Some tasks are assigned to you via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
                    {
                        Text = x.CaseTypeName,
                        Value = x.CaseTypeId.ToString()

                    }).ToList();
                    model.CaseTypeId = drafting.CaseTypeId;
                    model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                    {
                        Value = x.DepId.ToString(),
                        Text = x.DepName

                    }).ToList();
                    model.DepId = drafting.DepId;
                    model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                    {
                        Value = x.UserId.ToString(),
                        Text = x.FirstName.ToString() + " " + x.MidleName

                    }).ToList();
                    model.DueDate = DateTime.Now.AddDays(10);
                    model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                    {
                        Value = x.PriorityId.ToString(),
                        Text = x.PriorityName.ToString()

                    }).ToList();
                    model.PriorityId = drafting.PriorityId;
                    return View(model);

                }

            }
            catch (Exception)
            {
                model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
                {
                    Text = x.CaseTypeName,
                    Value = x.CaseTypeId.ToString()

                }).ToList();
                model.CaseTypeId = drafting.CaseTypeId;
                model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                {
                    Value = x.DepId.ToString(),
                    Text = x.DepName

                }).ToList();
                model.DepId = drafting.DepId;
                model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                {
                    Value = x.UserId.ToString(),
                    Text = x.FirstName.ToString() + " " + x.MidleName

                }).ToList();
                model.DueDate = DateTime.Now.AddDays(10);
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityName.ToString()

                }).ToList();
                model.PriorityId = drafting.PriorityId;
                return View(model);
            }
        }
        public async Task<IActionResult> Replies(Guid? id)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var replays = await _context.TblReplays.Where(a => a.RequestId == id).ToListAsync();
            RepliesModel model = new RepliesModel
            {
                RequestId = id,
                ReplyDate = DateTime.Now,
                InternalReplayedBy = userId,
            };
            ViewData["Replies"] = _context.TblReplays
                .Include(x => x.InternalReplayedByNavigation)
                .Include(x => x.ExternalReplayedByNavigation)
                .Include(x => x.Request)
                .Where(_context => _context.RequestId == id).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Replies(RepliesModel model)
        {
            try
            {
                TblReplay replay = new TblReplay();
                replay.ReplyDate = DateTime.Now;
                replay.InternalReplayedBy = model.InternalReplayedBy;
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
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
        public async Task<IActionResult> CompletedRequests()
        {
            var atsdbContext = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                          .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "Completed");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> PendingRequests()
        {
            var atsdbContext = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                          .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "In Progress");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> AssignedRequests()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));

            var atsdbContext = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                         .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(a => a.AssignedTo == userId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> UpploadFinalReport(Guid id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            var detail = _context.TblCivilJustices.FindAsync(id).Result;
            model.RequestId = id;
            model.CreatedDate = DateTime.UtcNow;
            model.RequestDetail = detail.RequestDetail;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> UpploadFinalReport(CivilJusticeExternalRequestModel model)
        {
            TblCivilJustice civilJustice = await _context.TblCivilJustices.FindAsync(model.RequestId);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //get file extension
            FileInfo fileInfo = new FileInfo(model.finalReport.FileName);
            string fileName = Guid.NewGuid().ToString() + model.finalReport.FileName;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.finalReport.CopyTo(stream);
            }
            string dbPath = "/Files/" + fileName;
            civilJustice.FinalReport = dbPath;
            int updated = _context.SaveChanges();
            if (updated > 0)
            {
                return RedirectToAction(nameof(AssignedRequests));
            }
            else
            {
                return View(model);
            }
        }
        public async Task<IActionResult> AddAdjornyDates(Guid? id)
        {
            ViewData["Adjornies"] = _context.TblAdjornments.ToList();
            AjornyDateModel model = new AjornyDateModel();
            model.RequestId = id;
            model.CreatedDate = DateTime.UtcNow;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddAdjornyDates(AjornyDateModel ajornyDateModel)
        {
            TblAdjornment tblAdjornment = new TblAdjornment();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            tblAdjornment.AdjorneyDate = ajornyDateModel.AdjorneyDate;
            tblAdjornment.RequestId = ajornyDateModel.RequestId;
            tblAdjornment.CreatedDate = DateTime.UtcNow;
            tblAdjornment.WhatIsDone = ajornyDateModel.WhatIsDone;
            tblAdjornment.CreatedBy = userId;
            _context.TblAdjornments.Add(tblAdjornment);
            int saved = _context.SaveChanges();
            ViewData["Adjornies"] = _context.TblAdjornments.ToList();
            if (saved > 0)
            {
                return View(ajornyDateModel);
            }
            return View(ajornyDateModel);
        }
        public async Task<IActionResult> AddEvidencesAndWitnesses(Guid? id)
        {
            WitnessEvidenceModel model = new WitnessEvidenceModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            model.RequestId = id;
            model.CreatedDate = DateTime.UtcNow;
            model.CreatedBy = userId;
            ViewData["evidences"] = _context.TblWitnessEvidences.Include(x => x.Request).ToList();
            if (id == null)
            {
                return NotFound();
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddEvidencesAndWitnesses(WitnessEvidenceModel evidenceModel)
        {
            TblWitnessEvidence evidence = new TblWitnessEvidence();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            string dbPath = null, dbVideoPath = null;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //get file extension
            if (evidenceModel.EvidenceFiles != null)
            {
                string file = Guid.NewGuid().ToString() + evidenceModel.EvidenceFiles.FileName;
                string fileWithPath = Path.Combine(path, file);
                using (var stream = new FileStream(fileWithPath, FileMode.Create))
                {
                    evidenceModel.EvidenceFiles.CopyTo(stream);
                }
                dbPath = "/Files/" + file;
            }

            if (evidenceModel.EvidenceVideos != null)
            {
                string videos = Guid.NewGuid().ToString() + evidenceModel.EvidenceVideos.FileName;
                string videoWithPath = Path.Combine(path, videos);
                using (var stream = new FileStream(videoWithPath, FileMode.Create))
                {
                    evidenceModel.EvidenceVideos.CopyTo(stream);
                }
                dbVideoPath = "/Files/" + videos;
            }
            evidence.RequestId = evidenceModel.RequestId;
            evidence.CreatedDate = DateTime.UtcNow;
            evidence.CreatedBy = userId;
            evidence.WitnessesName = evidenceModel.WitnessesName;
            evidence.EvidenceVideos = dbVideoPath;
            evidence.EvidenceFiles = dbPath;
            _context.TblWitnessEvidences.Add(evidence);
            int saved = _context.SaveChanges();
            ViewData["evidences"] = _context.TblWitnessEvidences.Include(x => x.Request).ToList();
            if (saved > 0)
            {
                return View(evidenceModel);
            }
            else
            {
                return View(evidenceModel);
            }
        }
        [HttpGet]
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
        public async Task<IActionResult> DeleteActivity(Guid? id)
        {
            if (id == null || _context.TblActivities == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblActivities
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.ActivityId == id);
            if (tblWitnessEvidence == null)
            {
                return NotFound();
            }

            return View(tblWitnessEvidence);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            if (_context.TblActivities == null)
            {
                return Problem("Entity set 'AtsdbContext.TblActivities'  is null.");
            }
            var tblWitnessEvidence = await _context.TblActivities.FindAsync(id);
            if (tblWitnessEvidence != null)
            {
                _context.TblActivities.Remove(tblWitnessEvidence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AddActivity), new { id = tblWitnessEvidence.RequestId });
        }
        public async Task<IActionResult> DeleteWitness(Guid? id)
        {
            if (id == null || _context.TblWitnessEvidences == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblWitnessEvidences
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.WitnessId == id);
            if (tblWitnessEvidence == null)
            {
                return NotFound();
            }

            return View(tblWitnessEvidence);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteWitness(Guid id)
        {
            if (_context.TblWitnessEvidences == null)
            {
                return Problem("Entity set 'AtsdbContext.TblWitnessEvidences'  is null.");
            }
            var tblWitnessEvidence = await _context.TblWitnessEvidences.FindAsync(id);
            if (tblWitnessEvidence != null)
            {
                _context.TblWitnessEvidences.Remove(tblWitnessEvidence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AddEvidencesAndWitnesses), new { id = tblWitnessEvidence.RequestId });
        }
        public async Task<IActionResult> DeleteAdjorny(Guid? id)
        {
            if (id == null || _context.TblAdjornments == null)
            {
                return NotFound();
            }

            var tblAdjornment = await _context.TblAdjornments
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.AdjoryId == id);
            if (tblAdjornment == null)
            {
                return NotFound();
            }

            return View(tblAdjornment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAdjorny(Guid id)
        {
            if (_context.TblAdjornments == null)
            {
                return Problem("Entity set 'AtsdbContext.TblAdjornments'  is null.");
            }
            var tblAdjornment = await _context.TblAdjornments.FindAsync(id);
            if (tblAdjornment != null)
            {
                _context.TblAdjornments.Remove(tblAdjornment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AddAdjornyDates), new { id = tblAdjornment.RequestId });
        }

        public async Task<IActionResult> UppdateProgressStatus(Guid? id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(id);
            model.RequestDetail = tblCivilJustice.RequestDetail;
            model.RequestId = tblCivilJustice.RequestId;
            model.ExternalStatus = _context.TblExternalRequestStatuses.Where(x => x.StatusName == "Completed").Select(x => new SelectListItem
            {
                Text = x.StatusName,
                Value = x.ExternalRequestStatusId.ToString()
            }).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UppdateProgressStatus(CivilJusticeExternalRequestModel model)
        {
            TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(model.RequestId);
            TblDecisionStatus status = _context.TblDecisionStatuses.Where(x => x.StatusName == "Waiting for Upproval").FirstOrDefault();

            tblCivilJustice.ExternalRequestStatusId = model.ExternalRequestStatusID;
            tblCivilJustice.TeamUpprovalStatus = status.DesStatusId;
            tblCivilJustice.DepartmentUpprovalStatus = status.DesStatusId;
            tblCivilJustice.DeputyUprovalStatus = status.DesStatusId;
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                return RedirectToAction(nameof(AssignedRequests));
            }
            return View(model);
        }

        public async Task<IActionResult> UppdateDesicionStatus(Guid? id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(id);
            model.RequestDetail = tblCivilJustice.RequestDetail;
            model.RequestId = tblCivilJustice.RequestId;
            model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
            {
                Text = x.StatusName,
                Value = x.DesStatusId.ToString()
            }).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UppdateDesicionStatus(CivilJusticeExternalRequestModel model)
        {
            TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(model.RequestId);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser user = await _context.TblInternalUsers.FindAsync(userId);
            if (user.IsTeamLeader == true)
            {
                tblCivilJustice.TeamUpprovalStatus = model.DesStatusId;
            }
            else if (user.IsDepartmentHead == true)
            {
                tblCivilJustice.DepartmentUpprovalStatus = model.DesStatusId;
            }
            else if (user.IsDeputy == true)
            {
                tblCivilJustice.DeputyUprovalStatus = model.DesStatusId;
            }
            else
            {
                model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.DesStatusId.ToString()
                }).ToList();
                return View(model);
            }
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                return RedirectToAction(nameof(CompletedRequests));
            }
            else
            {
                model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.DesStatusId.ToString()
                }).ToList();
                return View(model);
            }
        }

        public async Task<IActionResult> RequestActivities(Guid? id)
        {
            var atsdbContext = _context.TblActivities.Include(t => t.CreatedByNavigation).Include(t => t.Request);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> RequestAdjornies(Guid? id)
        {
            var atsdbContext = _context.TblAdjornments.Include(t => t.CreatedByNavigation).Include(t => t.Request);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> WitnessesandEvidences(Guid? id)
        {
            var atsdbContext = _context.TblWitnessEvidences.Include(t => t.CreatedByNavigation).Include(t => t.Request);
            return View(await atsdbContext.ToListAsync());
        }
    }
}
