using Microsoft.AspNetCore.Mvc;
using ATSManagementExternal.Models;
using ATSManagementExternal.IModels;
using Microsoft.EntityFrameworkCore;
using ATSManagementExternal.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagementExternal.Controllers
{
    public class ExternalRequestsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        public ExternalRequestsController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mail)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
        }

        // GET: ExternalRequests
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblExternalRequests.Include(t => t.ExterUser).Include(t => t.Int).Include(s => s.ExternalRequestStatus).Where(x => x.DepId == null);
            return View(await atsdbContext.ToListAsync());
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
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var atsdbContext = _context.TblLegalStudiesDraftings
               .Include(t => t.AssignedByNavigation)
               .Include(t => t.AssignedToNavigation)
               .Include(t => t.Doc)
               .Include(x=>x.QuestType)
               .Include(t => t.Dep)
               .Include(t => t.Inist)
               .Include(t => t.RequestedByNavigation)
               .Include(x => x.ExternalRequestStatus)
               .Include(t => t.Priority).Where(x => x.Dep.DepCode == "LSDC");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> NewLegalRequest()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var atsdbContext = _context.TblLegalStudiesDraftings
               .Include(t => t.AssignedByNavigation)
               .Include(t => t.AssignedToNavigation)
               .Include(t => t.Doc)
               .Include(x=>x.QuestType)
               .Include(t => t.Dep)
               .Include(t => t.Inist)
               .Include(t => t.RequestedByNavigation)
               .Include(x => x.ExternalRequestStatus)
               .Include(t => t.Priority).Where(x => x.Dep.DepCode == "LSDC" && x.ExternalRequestStatus.StatusName== "New");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> CompletedLegalRequest()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var atsdbContext = _context.TblLegalStudiesDraftings
               .Include(t => t.AssignedByNavigation)
               .Include(t => t.AssignedToNavigation)
               .Include(t => t.Doc)
               .Include(s=>s.QuestType)
               .Include(t => t.Dep)
               .Include(t => t.Inist)
               .Include(t => t.RequestedByNavigation)
               .Include(x => x.ExternalRequestStatus)
               .Include(t => t.Priority).Where(x => x.Dep.DepCode == "LSDC" && x.ExternalRequestStatus.StatusName == "Completed");
            return View(await atsdbContext.ToListAsync());
        }
         public async Task<IActionResult> PendingLegalStudies()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var atsdbContext = _context.TblLegalStudiesDraftings
               .Include(t => t.AssignedByNavigation)
               .Include(t => t.AssignedToNavigation)
               .Include(t => t.Doc)
               .Include(s=>s.QuestType)
               .Include(t => t.Dep)
               .Include(t => t.Inist)
               .Include(t => t.RequestedByNavigation)
               .Include(x => x.ExternalRequestStatus)
               .Include(t => t.Priority).Where(x => x.Dep.DepCode == "LSDC" && x.ExternalRequestStatus.StatusName == "In Progress");
            return View(await atsdbContext.ToListAsync());
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
        public async Task<IActionResult> PendingCivilJustice()
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
                                                        .Include(t => t.Priority).Where(x => x.Dep.DepCode == "CVA" && x.ExternalRequestStatus.StatusName == "In Progress");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> CompletedCivilJustice()
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
                                                        .Include(t => t.Priority).Where(x => x.Dep.DepCode == "CVA" && x.ExternalRequestStatus.StatusName == "Completed");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> NewCivilJustice()
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
                                                        .Include(t => t.Priority).Where(x => x.Dep.DepCode == "CVA" && x.ExternalRequestStatus.StatusName == "New");
            return View(await atsdbContext.ToListAsync());
        }
        // GET: ExternalRequests/Details/5
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

        // GET: ExternalRequests/Create
        public IActionResult Create()
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            model.RequestedDate = DateTime.Now;
            model.ExterUserId = userId;
            model.IntId = instName.InistId;
            model.Deparments = _context.TblDepartments.Select(a => new SelectListItem
            {
                Text = a.DepName,
                Value = a.DepId.ToString()

            }).ToList();
            model.LegalStadiesCasetypes = _context.TblLegalDraftingDocTypes.Select(s => new SelectListItem
            {
                Value = s.DocId.ToString(),
                Text = s.DocName.ToString()
            }).ToList();
            model.LegalStadiesQuestiontypes = _context.TblLegalDraftingQuestionTypes.Select(x => new SelectListItem
            {
                Value = x.QuestTypeId.ToString(),
                Text = x.QuestTypeId.ToString()
            }).ToList();
            model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
            {
                Value = x.CaseTypeId.ToString(),
                Text = x.CaseTypeName.ToString()
            }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CivilJusticeExternalRequestModel model)
        {

            try
            {
                var institutionName = (from items in _context.TblInistitutions where items.InistId == model.IntId select items.Name).FirstOrDefault();
                var users = (from user in _context.TblInternalUsers where (user.IsDepartmentHead == true || user.IsDeputy == true) && user.DepId == model.DepId select user.EmailAddress).ToList();
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "New" select items).FirstOrDefault();
                Guid statusiD = (from id in _context.TblExternalRequestStatuses where id.StatusName == "New" select id.ExternalRequestStatusId).FirstOrDefault();
                var decision = _context.TblDecisionStatuses.Where(x => x.StatusName == "Not set").FirstOrDefault();

                TblExternalRequest requests = new TblExternalRequest();
                TblCivilJustice tblCivilJustice = new TblCivilJustice();
                TblLegalStudiesDrafting drafting = new TblLegalStudiesDrafting();
                TblDepartment department = _context.TblDepartments.FindAsync(model.DepId).Result;
                if (department.DepCode == "CVA")
                {
                    tblCivilJustice.DepId = model.DepId;
                    tblCivilJustice.RequestDetail = model.RequestDetail;
                    tblCivilJustice.InistId = model.IntId;
                    tblCivilJustice.RequestedBy = userId;
                    tblCivilJustice.ExternalRequestStatusId = statusiD;
                    tblCivilJustice.DepartmentUpprovalStatus = decision.DesStatusId;
                    tblCivilJustice.TeamUpprovalStatus = decision.DesStatusId;
                    tblCivilJustice.DeputyUprovalStatus = decision.DesStatusId;
                    tblCivilJustice.UserUpprovalStatus = decision.DesStatusId;
                    _context.TblCivilJustices.Add(tblCivilJustice);
                    int saved = await _context.SaveChangesAsync();
                    if (saved > 0)
                    {
                        await SendMail(users, "Request notifications from " + institutionName, "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");

                        return RedirectToAction(nameof(CivilJustice));
                    }
                    else
                    {

                        model.Deparments = _context.TblDepartments.Select(a => new SelectListItem
                        {
                            Text = a.DepName,
                            Value = a.DepId.ToString()

                        }).ToList();
                        return View(model);
                    }
                }
                else if (department.DepCode == "LSDC")
                {
                    drafting.DepId = model.DepId;
                    drafting.CreatedDate = DateTime.Now;
                    drafting.RequestedBy = userId;
                    drafting.RequestDetail = model.RequestDetail;
                    drafting.DepId = model.DepId;
                    drafting.RequestedBy = userId;
                    drafting.InistId = model.IntId;
                    drafting.DepartmentUpprovalStatus = decision.DesStatusId;
                    drafting.TeamUpprovalStatus = decision.DesStatusId;
                    drafting.DeputyUprovalStatus = decision.DesStatusId;
                    drafting.UserUpprovalStatus = decision.DesStatusId;
                    drafting.ExternalRequestStatusId = statusiD;
                    _context.TblLegalStudiesDraftings.Add(drafting);
                    int saved = await _context.SaveChangesAsync();
                    if (saved > 0)
                    {
                        await SendMail(users, "Request notifications from " + institutionName, "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");

                        return RedirectToAction(nameof(LegalStudies));
                    }
                    else
                    {
                        model.Deparments = _context.TblDepartments.Select(a => new SelectListItem
                        {
                            Text = a.DepName,
                            Value = a.DepId.ToString()

                        }).ToList();
                        return View(model);
                    }
                }
                else if (department.DepCode == "FLIM")
                {
                    model.Deparments = _context.TblDepartments.Select(a => new SelectListItem
                    {
                        Text = a.DepName,
                        Value = a.DepId.ToString()

                    }).ToList();
                    return View(model);
                }
                else
                {
                    model.Deparments = _context.TblDepartments.Select(a => new SelectListItem
                    {
                        Text = a.DepName,
                        Value = a.DepId.ToString()

                    }).ToList();
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                model.Deparments = _context.TblDepartments.Select(a => new SelectListItem
                {
                    Text = a.DepName,
                    Value = a.DepId.ToString()

                }).ToList();
                return View(model);
            }

        }

        // GET: ExternalRequests/Edit/5
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

        // POST: ExternalRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: ExternalRequests/Delete/5
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

        // POST: ExternalRequests/Delete/5
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
            MailData data = new MailData(to, subject, body, "degaait@gmail.com");
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }

        public async Task<IActionResult> Replies(Guid? id)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var replays = await _context.TblCivilJusticeRequestReplys.Where(a => a.RequestId == id).ToListAsync();
            RepliesModel model = new RepliesModel
            {
                RequestId = id,
                ReplyDate = DateTime.Now,
                ExternalReplayedBy = userId,
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
                replay.ExternalReplayedBy = model.ExternalReplayedBy;
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


    }
}
