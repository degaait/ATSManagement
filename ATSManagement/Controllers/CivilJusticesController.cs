using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http.Features;

namespace ATSManagement.Controllers
{
    public class CivilJusticesController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;

        public CivilJusticesController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mailService)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
        }

        // GET: CivilJustices
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblCivilJustices
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.AssignedToNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(t => t.Priority);
            return View(await atsdbContext.ToListAsync());
        }
       
        public async Task<IActionResult> AddActivity(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            ActivityModel model = new ActivityModel();
            model.RequestId = id;
            model.AddedDate = DateTime.Now;
            model.CreatedBy = userId;
            ViewData["Activities"] = _context.TblCivilJusticeRequestActivities
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
            List<string> assignedBody=new List<string>();
            var request=_context.TblCivilJustices.Where(x=>x.RequestId==model.RequestId).FirstOrDefault();
            assignedBody=(from items in _context.TblInternalUsers where items.UserId==request.AssignedBy select items.EmailAddress).ToList();
            TblCivilJusticeRequestActivity activity = new TblCivilJusticeRequestActivity();
            activity.RequestId = model.RequestId;
            activity.AddedDate = DateTime.Now;
            activity.ActivityDetail= model.ActivityDetail;
            activity.CreatedBy = model.CreatedBy;
            _context.Add(activity);
            int saved=await _context.SaveChangesAsync();
            if (saved>0)
            {
                model.ActivityDetail = null;
                SendMail(assignedBody, "Adding activities notifications.", "<h3>Assigned body is adding activities via <strong> Task tacking Dashboard</strong>. Please check on the system and followup!.</h3>");
                ViewData["Activities"] = _context.TblCivilJusticeRequestActivities
                    .Include(x=>x.Request)
                    .Include(x=>x.CreatedByNavigation)
                    .Where(x => x.RequestId == model.RequestId).ToList();

                return View(model);
            }
            else
            {
                ViewData["Activities"] = _context.TblCivilJusticeRequestActivities
                    .Include(x => x.Request)
                    .Include(x => x.CreatedByNavigation)
                    .Where(x => x.RequestId == model.RequestId).ToList();

                return View(model);
            }

        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblCivilJustices == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblCivilJustices
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.AssignedToNavigation)
                .Include(t => t.CaseType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(t => t.ExternalRequestStatus)
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
                TblCivilJustice tblCivilJustice = new TblCivilJustice();
                Guid statusiD = (from id in _context.TblExternalRequestStatuses where id.StatusName == "New" select id.ExternalRequestStatusId).FirstOrDefault();
                tblCivilJustice.RequestDetail = model.RequestDetail;
                tblCivilJustice.CreatedBy = model.CreatedBy;
                tblCivilJustice.CreatedDate = DateTime.Now;
                tblCivilJustice.CaseTypeId = model.CaseTypeId;
                tblCivilJustice.InistId = model.InistId;
                tblCivilJustice.PriorityId = model.PriorityId;
                tblCivilJustice.DepId = model.DepId;
                tblCivilJustice.IsUpprovedByUser = false;
                tblCivilJustice.IsUprovedByDeputy = false;
                tblCivilJustice.IsUprovedByTeam = false;
                tblCivilJustice.IsUprovedbyDepartment = false;
                tblCivilJustice.ExternalRequestStatusId = statusiD;
                _context.TblCivilJustices.Add(tblCivilJustice);
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

            var tblCivilJustice = await _context.TblCivilJustices.FindAsync(id);
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
                TblCivilJustice tblCivilJustice = await _context.TblCivilJustices.FindAsync(model.RequestId);
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

            var tblCivilJustice = await _context.TblCivilJustices
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.AssignedToNavigation)
                .Include(t => t.CaseType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(t => t.ExternalRequestStatus)
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
            if (_context.TblCivilJustices == null)
            {
                return Problem("Entity set 'AtsdbContext.TblCivilJustices'  is null.");
            }
            var tblCivilJustice = await _context.TblCivilJustices.FindAsync(id);
            if (tblCivilJustice != null)
            {
                _context.TblCivilJustices.Remove(tblCivilJustice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblCivilJusticeExists(Guid id)
        {
            return (_context.TblCivilJustices?.Any(e => e.RequestId == id)).GetValueOrDefault();
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
            var userEmails = (from user in _context.TblInternalUsers where user.UserId == model.AssignedTo select user.EmailAddress).ToList();
            TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "In Progress" select items).FirstOrDefault();
            if (model.RequestId == null)
            {
                return NotFound();
            }
            TblCivilJustice drafting = await _context.TblCivilJustices.FindAsync(model.RequestId);
            if (drafting == null)
            {
                return NotFound();
            }
            try
            {
                drafting.DueDate = model.DueDate;
                drafting.AssignedDate = model.AssignedDate;
                drafting.PriorityId = model.PriorityId;
                drafting.AssignedTo = model.AssignedTo;
                drafting.AssignedBy = model.AssignedBy;
                drafting.AssingmentRemark = model.AssingmentRemark;
                drafting.CreatedBy = model.CreatedBy;
                drafting.CaseTypeId = model.CaseTypeId;
                drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    await SendMail(userEmails, "Task is assign notification", "<h3>Some tasks are assigned to you via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
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
            var replays = await _context.TblCivilJusticeRequestReplys.Where(a => a.RequestId == id).ToListAsync();
            RepliesModel model = new RepliesModel
            {
                RequestId = id,
                ReplyDate = DateTime.Now,
                InternalReplayedBy = userId,
            };
            ViewData["Replies"] = _context.TblCivilJusticeRequestReplys
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
                TblCivilJusticeRequestReply replay = new TblCivilJusticeRequestReply();
                replay.ReplyDate = DateTime.Now;
                replay.InternalReplayedBy = model.InternalReplayedBy;
                replay.RequestId = model.RequestId;
                replay.ReplayDetail = model.ReplayDetail;
                _context.TblCivilJusticeRequestReplys.Add(replay);
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
            MailData data = new MailData(to, subject, body, "degaait@gmail.com");
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }

        public async Task<IActionResult> CompletedRequests()
        {
            var atsdbContext = _context.TblCivilJustices
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.AssignedToNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(t => t.Priority).Where(x=>x.ExternalRequestStatus.StatusName== "Completed");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> PendingRequests()
        {
            var atsdbContext = _context.TblCivilJustices
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.AssignedToNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(t => t.Priority).Where(x=>x.ExternalRequestStatus.StatusName== "In Progress");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> AssignedRequests()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));

            var atsdbContext = _context.TblCivilJustices
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.AssignedToNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(t => t.Priority).Where(a => a.AssignedTo == userId);
            return View(await atsdbContext.ToListAsync());
        }

    }
}
