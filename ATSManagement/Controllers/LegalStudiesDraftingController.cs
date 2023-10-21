using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.Controllers
{
    public class LegalStudiesDraftingController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;

        public LegalStudiesDraftingController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mailService)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
        }

        // GET: CivilJustices
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblLegalStudiesDraftings
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.AssignedToNavigation)
                .Include(t => t.Doc)
                .Include(x=>x.QuestType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(x => x.ExternalRequestStatus)
                .Include(x => x.DepartmentUpprovalStatusNavigation)
                .Include(x => x.DeputyUprovalStatusNavigation)
                .Include(y => y.TeamUpprovalStatusNavigation)
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
            ViewData["Activities"] = _context.TblLegalStudiesActivities
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
            var request = _context.TblLegalStudiesDraftings.Where(x => x.RequestId == model.RequestId).FirstOrDefault();
            assignedBody = (from items in _context.TblInternalUsers where items.UserId == request.AssignedBy select items.EmailAddress).ToList();
            TblLegalStudiesActivity activity = new TblLegalStudiesActivity();
            activity.RequestId = model.RequestId;
            activity.AddedDate = DateTime.Now;
            activity.ActivityDetail = model.ActivityDetail;
            activity.CreatedBy = model.CreatedBy;
            _context.TblLegalStudiesActivities.Add(activity);
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                model.ActivityDetail = null;
                SendMail(assignedBody, "Adding activities notifications.", "<h3>Assigned body is adding activities via <strong> Task tacking Dashboard</strong>. Please check on the system and followup!.</h3>");
                ViewData["Activities"] = _context.TblLegalStudiesActivities
                    .Include(x => x.Request)
                    .Include(x => x.CreatedByNavigation)
                    .Where(x => x.RequestId == model.RequestId).ToList();

                return View(model);
            }
            else
            {
                ViewData["Activities"] = _context.TblLegalStudiesActivities
                    .Include(x => x.Request)
                    .Include(x => x.CreatedByNavigation)
                    .Where(x => x.RequestId == model.RequestId).ToList();

                return View(model);
            }

        }

        // GET: CivilJustices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblLegalStudiesDraftings == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblLegalStudiesDraftings
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.AssignedToNavigation)
                .Include(t => t.Doc)
                .Include(x=>x.QuestType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(x => x.ExternalRequestStatus)
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

        // GET: CivilJustices/Create
        public IActionResult Create()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            LegalStudiesDraftingModel model = new LegalStudiesDraftingModel();
            model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
            {
                Value = x.InistId.ToString(),
                Text = x.Name
            }).ToList();
            model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
            {
                Value = x.DepId.ToString(),
                Text = x.DepName
            }).Where(a=>a.Text== "Legal Studies, Drafting & Consolidation").ToList();
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
            model.CreatedBy = userId;

            return View(model);
        }

        // POST: CivilJustices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LegalStudiesDraftingModel model)
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
                draftings.CreatedBy = model.CreatedBy;
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

        // GET: CivilJustices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblLegalStudiesDraftings == null)
            {
                return NotFound();
            }

            var legalDraftig = await _context.TblLegalStudiesDraftings.FindAsync(id);
            if (legalDraftig == null)
            {
                return NotFound();
            }
            LegalStudiesDraftingModel model = new LegalStudiesDraftingModel();
            model.Intitutions = _context.TblInistitutions.Select(x => new SelectListItem
            {
                Value = x.InistId.ToString(),
                Text = x.Name
            }).ToList();
            model.InistId = legalDraftig.InistId;
            model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
            {
                Value = x.DepId.ToString(),
                Text = x.DepName
            }).ToList();
            model.DepId = legalDraftig.DepId;
            model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
            {
                Text = x.PriorityName,
                Value = x.PriorityId.ToString()
            }).ToList();
            model.PriorityId = legalDraftig.PriorityId;
            model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
            {
                Value = x.DocId.ToString(),
                Text = x.DocName
            }).ToList();
            model.DocId = legalDraftig.DocId;
            model.LegalStadiesQuestiontypes = _context.TblLegalDraftingQuestionTypes.Select(x => new SelectListItem
            {
                Value = x.QuestTypeId.ToString(),
                Text = x.QuestTypeName
            }).ToList();
            model.QuestTypeId = legalDraftig.QuestTypeId;
            model.RequestDetail = legalDraftig.RequestDetail;
            model.RequestId = legalDraftig.RequestId;
            model.RequestedDate = legalDraftig.CreatedDate;
            return View(model);


        }

        // POST: CivilJustices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(LegalStudiesDraftingModel model)
        {
            if (model.RequestId == Guid.Empty)
            {
                return NotFound();
            }
            if (!TblCivilJusticeExists(model.RequestId))
            {
                return NotFound();
            }
            try
            {
                TblLegalStudiesDrafting draftings = await _context.TblLegalStudiesDraftings.FindAsync(model.RequestId);
                draftings.RequestDetail = model.RequestDetail;
                draftings.CaseTypeId = model.CaseTypeId;
                draftings.InistId = model.InistId;
                draftings.PriorityId = model.PriorityId;
                draftings.DepId = model.DepId;
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
            catch (Exception)
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

        // GET: CivilJustices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblLegalStudiesDraftings == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblLegalStudiesDraftings
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.AssignedToNavigation)
                .Include(t => t.Doc)
                .Include(x=>x.QuestType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(x => x.ExternalRequestStatus)
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

        // POST: CivilJustices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblLegalStudiesDraftings == null)
            {
                return Problem("Entity set 'AtsdbContext.TblCivilJustices'  is null.");
            }
            var tblCivilJustice = await _context.TblLegalStudiesDraftings.FindAsync(id);
            if (tblCivilJustice != null)
            {
                _context.TblLegalStudiesDraftings.Remove(tblCivilJustice);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblCivilJusticeExists(Guid id)
        {
            return (_context.TblLegalStudiesDraftings?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> AssignToUser(Guid id)
        {
            LegalStudiesDraftingModel model = new LegalStudiesDraftingModel();
            TblLegalStudiesDrafting drafting = await _context.TblLegalStudiesDraftings.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            model.RequestDetail = drafting.RequestDetail;
            model.RequestId = id;
            model.AssignedBy = userId;
            model.AssignedDate = DateTime.Now;
            model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
            {
                Value = x.DocId.ToString(),
                Text = x.DocName
            }).ToList();
            model.DocId = drafting.DocId;
            model.LegalStadiesQuestiontypes = _context.TblLegalDraftingQuestionTypes.Select(x => new SelectListItem
            {
                Value = x.QuestTypeId.ToString(),
                Text = x.QuestTypeName
            }).ToList();
            model.QuestTypeId = drafting.QuestTypeId;
            model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
            {
                Value = x.DepId.ToString(),
                Text = x.DepName

            }).ToList();
            model.DepId = drafting.DepId;
            model.AssignedTos = _context.TblInternalUsers.Where(x=>x.Dep.DepCode== "LSDC").Select(x => new SelectListItem
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
        public async Task<IActionResult> AssignToUser(LegalStudiesDraftingModel model)
        {
            var userEmails = (from user in _context.TblInternalUsers where user.UserId == model.AssignedTo select user.EmailAddress).ToList();

            TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "In Progress" select items).FirstOrDefault();

            if (model.RequestId == null)
            {
                return BadRequest();
            }
            TblLegalStudiesDrafting drafting = await _context.TblLegalStudiesDraftings.FindAsync(model.RequestId);
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
                drafting.DocId=model.DocId;
                drafting.QuestTypeId= model.QuestTypeId;
                drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    await SendMail(userEmails, "Task is assign notification", "<h3>Some tasks are assigned to you via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");

                    return RedirectToAction(nameof(Index));
                }
                else
                {
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
                    model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                    {
                        Value = x.DepId.ToString(),
                        Text = x.DepName

                    }).ToList();
                    model.DepId = drafting.DepId;
                    model.AssignedTos = _context.TblInternalUsers.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
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
                model.AssignedTos = _context.TblInternalUsers.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
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
            var replays = await _context.TblLegalStudiesReplays.Where(a => a.RequestId == id).ToListAsync();
            RepliesModel model = new RepliesModel
            {
                RequestId = id,
                ReplyDate = DateTime.Now,
                InternalReplayedBy = userId,
            };
            ViewData["Replies"] = _context.TblLegalStudiesReplays
                .Include(x => x.ExternalReplayedBy1)
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
                TblLegalStudiesReplay replay = new TblLegalStudiesReplay();
                replay.ReplyDate = DateTime.Now;
                replay.InternalReplayedBy = model.InternalReplayedBy;
                replay.RequestId = model.RequestId;
                replay.ReplayDetail = model.ReplayDetail;
                _context.TblLegalStudiesReplays.Add(replay);
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
            List<TblLegalStudiesDrafting>? atsdbContext = new List<TblLegalStudiesDrafting>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
            if (tblInternalUser.IsDeputy == true || tblInternalUser.IsSuperAdmin == true|| tblInternalUser.IsDepartmentHead == true)
            {
                atsdbContext = _context.TblLegalStudiesDraftings
                                        .Include(t => t.AssignedByNavigation)
                                        .Include(t => t.AssignedToNavigation)
                                        .Include(t => t.Doc)
                                        .Include(x => x.QuestType)
                                        .Include(t => t.Dep)
                                        .Include(t => t.Inist)
                                        .Include(t => t.RequestedByNavigation)
                                        .Include(t => t.CreatedByNavigation)
                                        .Include(x => x.ExternalRequestStatus)
                                        .Include(x => x.DepartmentUpprovalStatusNavigation)
                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                        .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "Completed").ToList();
            }
            else
            {
                atsdbContext = _context.TblLegalStudiesDraftings
                        .Include(t => t.AssignedByNavigation)
                        .Include(t => t.AssignedToNavigation)
                        .Include(t => t.Doc)
                        .Include(x => x.QuestType)
                        .Include(t => t.Dep)
                        .Include(t => t.Inist)
                        .Include(t => t.RequestedByNavigation)
                        .Include(t => t.CreatedByNavigation)
                        .Include(x => x.ExternalRequestStatus)
                        .Include(x => x.DepartmentUpprovalStatusNavigation)
                        .Include(x => x.DeputyUprovalStatusNavigation)
                        .Include(y => y.TeamUpprovalStatusNavigation)
                        .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "Completed"&& x.AssignedTo==userId).ToList();
            }


            return View(atsdbContext);
        }
        public async Task<IActionResult> PendingRequests()
        {

            List<TblLegalStudiesDrafting>? atsdbContext = new List<TblLegalStudiesDrafting>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
            if (tblInternalUser.IsDeputy == true || tblInternalUser.IsSuperAdmin == true || tblInternalUser.IsDepartmentHead == true)
            {
                atsdbContext = _context.TblLegalStudiesDraftings
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.AssignedToNavigation)
                                                        .Include(t => t.Doc)
                                                        .Include(x => x.QuestType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "In Progress").ToList();
            }
            else
            {
                atsdbContext = _context.TblLegalStudiesDraftings
                                                      .Include(t => t.AssignedByNavigation)
                                                      .Include(t => t.AssignedToNavigation)
                                                      .Include(t => t.Doc)
                                                      .Include(x => x.QuestType)
                                                      .Include(t => t.Dep)
                                                      .Include(t => t.Inist)
                                                      .Include(t => t.RequestedByNavigation)
                                                      .Include(t => t.CreatedByNavigation)
                                                      .Include(x => x.ExternalRequestStatus)
                                                      .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                      .Include(x => x.DeputyUprovalStatusNavigation)
                                                      .Include(y => y.TeamUpprovalStatusNavigation)
                                                      .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "In Progress" && x.AssignedTo == userId).ToList();
            }
            return View(atsdbContext);
        }
        public async Task<IActionResult> AssignedRequests()
        {
            List<TblLegalStudiesDrafting>? atsdbContext= new List<TblLegalStudiesDrafting>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);   
                atsdbContext = _context.TblLegalStudiesDraftings
                                                           .Include(t => t.AssignedByNavigation)
                                                           .Include(t => t.AssignedToNavigation)
                                                           .Include(t => t.Doc)
                                                           .Include(x => x.QuestType)
                                                           .Include(t => t.Dep)
                                                           .Include(t => t.Inist)
                                                           .Include(t => t.RequestedByNavigation)
                                                           .Include(t => t.CreatedByNavigation)
                                                           .Include(x => x.ExternalRequestStatus)
                                                           .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                           .Include(x => x.DeputyUprovalStatusNavigation)
                                                           .Include(y => y.TeamUpprovalStatusNavigation)
                                                           .Include(t => t.Priority).Where(a => a.AssignedTo == userId).ToList();

            return View(atsdbContext);

        }

        public async Task<IActionResult> UpploadFinalReport(Guid id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            var detail = _context.TblLegalStudiesDraftings.FindAsync(id).Result;
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
            TblLegalStudiesDrafting civilJustice = await _context.TblLegalStudiesDraftings.FindAsync(model.RequestId);
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
            if (id == null || _context.TblLegalStudiesActivities == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblLegalStudiesActivities
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
            if (_context.TblLegalStudiesActivities == null)
            {
                return Problem("Entity set 'AtsdbContext.TblLegalStudiesActivities'  is null.");
            }
            var tblWitnessEvidence = await _context.TblLegalStudiesActivities.FindAsync(id);
            if (tblWitnessEvidence != null)
            {
                _context.TblLegalStudiesActivities.Remove(tblWitnessEvidence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(AddActivity), new { id = tblWitnessEvidence.RequestId });
        }


        public async Task<IActionResult> UppdateProgressStatus(Guid? id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblLegalStudiesDrafting tblCivilJustice = await _context.TblLegalStudiesDraftings.FindAsync(id);
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
            TblLegalStudiesDrafting tblCivilJustice = await _context.TblLegalStudiesDraftings.FindAsync(model.RequestId);
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
            TblLegalStudiesDrafting tblCivilJustice = await _context.TblLegalStudiesDraftings.FindAsync(id);
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
            TblLegalStudiesDrafting tblCivilJustice = await _context.TblLegalStudiesDraftings.FindAsync(model.RequestId);
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
            var atsdbContext = _context.TblLegalStudiesActivities.Include(t => t.CreatedByNavigation).Include(t => t.Request);
            return View(await atsdbContext.ToListAsync());
        }

        public async Task<IActionResult> DeleteReply(Guid? id)
        {
            if (id == null || _context.TblLegalStudiesReplays == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblLegalStudiesReplays
                .Include(t => t.ExternalReplayedBy1)
                .Include(t=>t.ExternalReplayedByNavigation)
                .Include(t => t.Request)
                .FirstOrDefaultAsync(m => m.ReplyId == id);
            if (tblWitnessEvidence == null)
            {
                return NotFound();
            }

            return View(tblWitnessEvidence);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteReply(Guid id)
        {
            if (_context.TblLegalStudiesReplays == null)
            {
                return Problem("Entity set 'AtsdbContext.TblLegalStudiesReplays'  is null.");
            }
            var tblWitnessEvidence = await _context.TblLegalStudiesReplays.FindAsync(id);
            if (tblWitnessEvidence != null)
            {
                _context.TblLegalStudiesReplays.Remove(tblWitnessEvidence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Replies), new { id = tblWitnessEvidence.RequestId });
        }

    }
}
