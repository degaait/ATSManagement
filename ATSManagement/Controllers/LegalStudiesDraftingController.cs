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
    public class LegalStudiesDraftingController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;

        public LegalStudiesDraftingController(AtsdbContext context,
            IHttpContextAccessor contextAccessor, IMailService mailService,
             INotyfService notyfService)
        {
            _notifyService = notyfService;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
        }

        // GET: CivilJustices
        public async Task<IActionResult> Index()
        {
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest tblRequest;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var userinfor = _context.TblInternalUsers.Include(c => c.Dep).Where(x => x.UserId == userId).FirstOrDefault();
            if (userinfor.IsDeputy == true || (userinfor.Dep.DepCode == "LSDC" && userinfor.IsDepartmentHead == true) || userinfor.IsSuperAdmin == true)
            {
                var moreDeps = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "LSDC").Select(a => a.RequestId).ToList();
                foreach (var item in moreDeps)
                {
                    tblRequest = _context.TblRequests
                                                                          .Include(t => t.AssignedByNavigation)
                                                                          .Include(t => t.CaseType)
                                                                          .Include(t => t.Inist)
                                                                          .Include(t => t.RequestedByNavigation)
                                                                          .Include(t => t.CreatedByNavigation)
                                                                          .Include(x => x.ExternalRequestStatus)
                                                                          .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                                          .Include(x => x.DeputyUprovalStatusNavigation)
                                                                          .Include(y => y.TeamUpprovalStatusNavigation)
                                                                          .Include(t => t.Priority).Where(x => x.RequestId == item).FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }
                }
                return View(atsdbContext);
            }

            else
            {
                return RedirectToAction(nameof(AssignedRequests));
            }

        }
        public async Task<IActionResult> TeamRequests()
        {
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest tblRequest;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var userinfor = _context.TblInternalUsers.Include(c => c.Dep).Where(x => x.UserId == userId).FirstOrDefault();
            if (userinfor.IsDeputy == true || (userinfor.Dep.DepCode == "LSDC" && userinfor.IsDepartmentHead == true) || userinfor.IsSuperAdmin == true)
            {
                var moreDeps = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "LSDC" && x.TeamId == userinfor.TeamId).Select(a => a.RequestId).ToList();
                foreach (var item in moreDeps)
                {
                    tblRequest = _context.TblRequests
                                                                          .Include(t => t.AssignedByNavigation)
                                                                          .Include(t => t.CaseType)
                                                                          .Include(t => t.Inist)
                                                                          .Include(t => t.RequestedByNavigation)
                                                                          .Include(t => t.CreatedByNavigation)
                                                                          .Include(x => x.ExternalRequestStatus)
                                                                          .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                                          .Include(x => x.DeputyUprovalStatusNavigation)
                                                                          .Include(y => y.TeamUpprovalStatusNavigation)
                                                                          .Include(t => t.Priority).Where(x => x.RequestId == item).FirstOrDefault();
                    atsdbContext.Add(tblRequest);
                }
                return View(atsdbContext);
            }

            else
            {
                return RedirectToAction(nameof(AssignedRequests));
            }

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
                await SendMail(assignedBody, "Adding activities notifications.", "<h3>Assigned body is adding activities via <strong> Task tacking Dashboard</strong>. Please check on the system and followup!.</h3>");
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
                .Include(t => t.DocType)
                .Include(x => x.QuestType)
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
            model.CreatedBy = userId;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(LegalStudiesDraftingModel model)
        {
            try
            {
                TblRequest draftings = new TblRequest();
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
                draftings.DocTypeId = model.DocId;
                draftings.InistId = model.InistId;
                draftings.PriorityId = model.PriorityId;
                draftings.DepartmentUpprovalStatus = decision.DesStatusId;
                draftings.TeamUpprovalStatus = decision.DesStatusId;
                draftings.DeputyUprovalStatus = decision.DesStatusId;
                draftings.UserUpprovalStatus = decision.DesStatusId;
                draftings.DocumentFile = dbPath;
                draftings.ExternalRequestStatusId = statusiD;
                _context.TblRequests.Add(draftings);
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {

                    return View(getModel());
                }
            }
            catch (Exception ex)
            {

                return View(getModel());
            }
        }
        public LegalStudiesDraftingModel getModel()
        {
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
            return model;
        }
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
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblRequests == null)
            {
                return NotFound();
            }

            var tblCivilJustice = await _context.TblRequests
                .Include(t => t.AssignedByNavigation)
                .Include(t => t.DocType)
                .Include(x => x.QuestType)
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
            LegalStudiesDraftingModel model = new LegalStudiesDraftingModel();
            TblRequest drafting = await _context.TblRequests.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (drafting.RequestDetail != null)
            {
                model.RequestDetail = drafting.RequestDetail;
            }
            model.RequestId = id;
            model.AssignedBy = userId;
            model.AssignedDate = DateTime.Now;
            model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
            {
                Value = x.DocId.ToString(),
                Text = x.DocName
            }).ToList();
            model.DocId = drafting.DocTypeId;
            model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
            {
                Value = x.ServiceTypeId.ToString(),
                Text = x.ServiceTypeName
            }).ToList();
            model.ServiceTypeID = drafting.ServiceTypeId;
            model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
            {
                Text = s.AssigneeType.ToString(),
                Value = s.AssigneeTypeId.ToString(),
            }).ToList();
            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
            {
                Text = s.TeamName,
                Value = s.TeamId.ToString(),
            }).ToList();
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
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToUser(LegalStudiesDraftingModel model)
        {
            List<string>? emails = new List<string>();
            List<TblRequestAssignee> assignees;

            if (model.RequestId == null)
            {
                return NotFound();
            }
            TblRequest drafting = await _context.TblRequests.FindAsync(model.RequestId);
            TblRequestDepartmentRelation relation = await _context.TblRequestDepartmentRelations.Where(s => s.RequestId == model.RequestId).FirstOrDefaultAsync();
            if (drafting == null)
            {
                return NotFound();
            }
            try
            {
                if (model.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847"))
                {
                    if (model.TeamId == null)
                    {
                        _notifyService.Error("Since assignment type is team , You should select one the team");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
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
                    TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "Assigned to team" select items).FirstOrDefault();
                    var TeamEmail = _context.TblInternalUsers.Where(x => x.TeamId == model.TeamId && x.IsTeamLeader == true).Select(s => s.EmailAddress).ToList();
                    relation.AssigneeTypeId = model.AssigneeTypeId;
                    relation.TeamId = model.TeamId;
                    relation.IsAssingedToUser = false;
                    drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Request is successfully assigned to team");
                        await SendMail(TeamEmail, "Task is assign notification", "<h3>Some tasks are assigned to your team via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Assignment isn't successfull. Please try again");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
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
                else
                {
                    TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "Assigned to user" select items).FirstOrDefault();
                    foreach (var item in model.AssignedTo)
                    {
                        var userEmails = (from user in _context.TblInternalUsers where user.UserId == item select user.EmailAddress).FirstOrDefault();
                        emails.Add(userEmails);
                    }
                    if (model.AssignedTo.Length == 0)
                    {
                        _notifyService.Error("Since assignment type is Expert , You should select one the experts");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
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
                    relation.IsAssingedToUser = true;
                    drafting.DueDate = model.DueDate;
                    drafting.AssignedDate = model.AssignedDate;
                    drafting.PriorityId = model.PriorityId;
                    drafting.AssignedBy = model.AssignedBy;
                    drafting.AssingmentRemark = model.AssingmentRemark;
                    drafting.CreatedBy = model.CreatedBy;
                    drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                    if (model.AssignedTo.Length > 0)
                    {
                        assignees = new List<TblRequestAssignee>();
                        foreach (var item in model.AssignedTo)
                        {
                            var ifExists = _context.TblRequestAssignees.Where(x => x.RequestId == model.RequestId && x.UserId == item).FirstOrDefault();
                            if (ifExists == null)
                            {
                                assignees.Add(new TblRequestAssignee { UserId = item, RequestId = model.RequestId });
                            }
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

                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
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
            }
            catch (Exception)
            {
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                model.ServiceTypeID = drafting.ServiceTypeId;
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeType.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                {
                    Text = s.TeamName,
                    Value = s.TeamId.ToString(),
                }).ToList();
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
        public async Task<IActionResult> Archive(Guid? id)
        {
            ArchiveModel model = new ArchiveModel();
            var request = _context.TblRequests.FindAsync(id).Result;
            model.RequestId = id;
            model.RequestDetail = request.RequestDetail;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Archive(ArchiveModel? model)
        {
            TblRequest? tblRequest = await _context.TblRequests.FindAsync(model.RequestId);
            tblRequest.IsArchived = true;
            int updated = await _context.SaveChangesAsync();
            if (updated > 0)
            {
                _notifyService.Success("Request is successfully Archived");
                return RedirectToAction(nameof(CompletedRequests));
            }
            else
            {
                _notifyService.Error("Request isn't successfully archived. Please try again");
                return View(model);
            }
        }
        public async Task<IActionResult> Replies(Guid? id)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var dep = _context.TblInternalUsers.Find(userId);
            if (dep.IsDepartmentHead==true)
            {
                ViewBag.value = "flex";

            }
            else
            {
                ViewBag.value = "none";
            }
            var replays = await _context.TblReplays.Where(a => a.RequestId == id).ToListAsync();
            RepliesModel replies = new RepliesModel();
            replies.RequestId = id;
            replies.ReplyDate = DateTime.UtcNow;
            replies.InternalReplayedBy = userId;
            replies.IsSent = false;
            ViewData["Replies"] = _context.TblReplays
                .Include(x => x.InternalReplayedByNavigation)
                .Include(x => x.ExternalReplayedByNavigation)
                .Include(x => x.Request)
                .Where(_context => _context.RequestId == id).ToList();
            return View(replies);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Replies(RepliesModel model)
        {
            try
            {
                Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var dep = _context.TblInternalUsers.Find(userId);
                if (dep.IsDepartmentHead == true)
                {
                    ViewBag.value = "flex";

                }
                else
                {
                    ViewBag.value = "none";
                }
                TblReplay replay = new TblReplay();
                replay.ReplyDate = DateTime.Now;
                replay.InternalReplayedBy = model.InternalReplayedBy;
                replay.RequestId = model.RequestId;
                replay.ReplayDetail = model.ReplayDetail;
                replay.IsExternal = false;
                replay.IsInternal = true;
                if (model.IsSent == true)
                {
                    replay.IsSent = true;
                }
                else
                {
                    replay.IsSent = false;
                }
                string? dbPath = null;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (model.Attachement != null)
                {
                    //get file extension
                    FileInfo fileInfo = new FileInfo(model.Attachement.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.Attachement.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.Attachement.CopyTo(stream);
                    }
                    dbPath = "/Files/" + fileName;
                    replay.Attachment = dbPath;
                }
                    _context.TblReplays.Add(replay);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply successfully added.");
                    return RedirectToAction("Replies", new { id = model.RequestId });
                }
                else
                {

                    _notifyService.Error("Reply isn't added. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again.");
                return View(model);
            }
        }
        public async Task<IActionResult> EditReplies(Guid? ReplyId)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var dep = _context.TblInternalUsers.Find(userId);
            if (dep.IsDepartmentHead == true)
            {
                ViewBag.value = "flex";

            }
            else
            {
                ViewBag.value = "none";
            }
            var reply = _context.TblReplays.Where(s => s.ReplyId == ReplyId).FirstOrDefault();
            RepliesModel repliesModel = new RepliesModel();
            repliesModel.ReplyId = reply.ReplyId;
            repliesModel.RequestId = reply.RequestId;
            if (reply.IsSent == null || reply.IsSent == false)
            {
                repliesModel.IsSent = false;
            }
            else
            {
                repliesModel.IsSent = true;
            }
            repliesModel.ReplayDetail = reply.ReplayDetail;
            return View(repliesModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> EditReplies(RepliesModel model)
        {
            try
            {
                Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var dep = _context.TblInternalUsers.Find(userId);
                if (dep.IsDepartmentHead == true)
                {
                    ViewBag.value = "flex";

                }
                else
                {
                    ViewBag.value = "none";
                }
                TblReplay replay = _context.TblReplays.Find(model.ReplyId);
                replay.ReplayDetail = model.ReplayDetail;
                if (model.IsSent == true)
                {
                    replay.IsSent = true;
                }
                else
                {
                    replay.IsSent = false;
                }
                string? dbPath = null;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (model.Attachement != null)
                {
                    //get file extension
                    FileInfo fileInfo = new FileInfo(model.Attachement.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.Attachement.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.Attachement.CopyTo(stream);
                    }
                    dbPath = "/Files/" + fileName;
                    replay.Attachment = dbPath;
                }
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply successfully added.");
                    return RedirectToAction("Replies", new { id = model.RequestId });
                }
                else
                {
                    _notifyService.Error("Reply isn't added. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again.");
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
            TblTopStatus tblTopStatus = _context.TblTopStatuses.Where(x => x.StatusName == "Completed").FirstOrDefault();

            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest tblRequest;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
            var moreDeps = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "LSDC").Select(a => a.RequestId).ToList();
            if (tblInternalUser.IsDeputy == true || tblInternalUser.IsSuperAdmin == true || tblInternalUser.IsDepartmentHead == true)
            {
                foreach (var item in moreDeps)
                {

                    tblRequest = _context.TblRequests
                                                                       .Include(t => t.AssignedByNavigation)
                                                                       .Include(t => t.CaseType)
                                                                       .Include(t => t.Inist)
                                                                       .Include(t => t.RequestedByNavigation)
                                                                       .Include(t => t.CreatedByNavigation)
                                                                       .Include(x => x.ExternalRequestStatus)
                                                                       .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                                       .Include(x => x.DeputyUprovalStatusNavigation)
                                                                       .Include(y => y.TeamUpprovalStatusNavigation)
                                                                       .Include(t => t.Priority).Where(x => x.RequestId == item && x.TopStatusId == tblTopStatus.TopStatusId).FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }

                }
            }
            else
            {
                foreach (var item in moreDeps)
                {
                    tblRequest = _context.TblRequests
                                                                       .Include(t => t.AssignedByNavigation)
                                                                       .Include(t => t.CaseType)
                                                                       .Include(t => t.Inist)
                                                                       .Include(t => t.RequestedByNavigation)
                                                                       .Include(t => t.CreatedByNavigation)
                                                                       .Include(x => x.ExternalRequestStatus)
                                                                       .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                                       .Include(x => x.DeputyUprovalStatusNavigation)
                                                                       .Include(y => y.TeamUpprovalStatusNavigation)
                                                                       .Include(t => t.Priority).Where(x => x.RequestId == item && x.TopStatusId == tblTopStatus.TopStatusId && x.AssignedTo == userId).FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }


                }
            }
            return View(atsdbContext);
        }
        public async Task<IActionResult> PendingRequests()
        {
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest tblRequest;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
            var moreDeps = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "LSDC").Select(a => a.RequestId).ToList();

            if (tblInternalUser.IsDeputy == true || tblInternalUser.IsSuperAdmin == true || tblInternalUser.IsDepartmentHead == true)
            {

                foreach (var item in moreDeps)
                {

                    tblRequest = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.DocType)
                                                        .Include(x => x.QuestType)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.RequestId == item && x.ExternalRequestStatus.StatusName != "New" &&x.ExternalRequestStatus.StatusName!="Completed").FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }
                }
            }
            else
            {
                foreach (var item in moreDeps)
                {
                    tblRequest = _context.TblRequests
                                                      .Include(t => t.AssignedByNavigation)
                                                      .Include(t => t.DocType)
                                                      .Include(x => x.QuestType)
                                                      .Include(t => t.Inist)
                                                      .Include(t => t.RequestedByNavigation)
                                                      .Include(t => t.CreatedByNavigation)
                                                      .Include(x => x.ExternalRequestStatus)
                                                      .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                      .Include(x => x.DeputyUprovalStatusNavigation)
                                                      .Include(y => y.TeamUpprovalStatusNavigation)
                                                      .Include(t => t.Priority).Where(x => x.RequestId == item && x.ExternalRequestStatus.StatusName == "In Progress" && x.AssignedTo == userId).FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }
                }
            }
            return View(atsdbContext);
        }
        public async Task<IActionResult> AssignedRequests()
        {
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest Request;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
            var assignedReq = _context.TblRequestAssignees.Where(x => x.UserId == userId).ToList();

            foreach (var item in assignedReq)
            {
                Request = new TblRequest();
                Request = _context.TblRequests
                                     .Include(t => t.AssignedByNavigation)
                                     .Include(t => t.CaseType)
                                     .Include(t => t.Inist)
                                     .Include(t => t.RequestedByNavigation)
                                     .Include(t => t.CreatedByNavigation)
                                     .Include(x => x.ExternalRequestStatus)
                                     .Include(x => x.DepartmentUpprovalStatusNavigation)
                                     .Include(x => x.DeputyUprovalStatusNavigation)
                                     .Include(y => y.TeamUpprovalStatusNavigation)
                                     .Include(t => t.Priority).Where(a => a.RequestId == item.RequestId).FirstOrDefault();
                atsdbContext.Add(Request);

            }

            return View(atsdbContext);
        }
        public async Task<IActionResult> UpploadFinalReport(Guid id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            var detail = _context.TblRequests.FindAsync(id).Result;
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
            TblRequest civilJustice = await _context.TblRequests.FindAsync(model.RequestId);
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
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "admin\\", filename);
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
        public async Task<IActionResult> UppdateProgressStatus(Guid? id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(id);
            model.RequestDetail = tblCivilJustice.RequestDetail;
            model.RequestId = tblCivilJustice.RequestId;
            model.ExternalStatus = _context.TblExternalRequestStatuses.Where(x => x.StatusName == "Completed" || x.StatusName == "Report Preparation" || x.StatusName == "Data Collection").Select(x => new SelectListItem
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
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser user = await _context.TblInternalUsers.FindAsync(userId);
            model.RequestDetail = tblCivilJustice.RequestDetail;
            model.RequestId = tblCivilJustice.RequestId;
            model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
            {
                Text = x.StatusName,
                Value = x.DesStatusId.ToString()
            }).ToList();
           if (user.IsDepartmentHead == true)
            {
                ViewBag.visible = true;
            }
            else
            {
                ViewBag.visible = false;
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UppdateDesicionStatus(CivilJusticeExternalRequestModel model)
        {
            TblRequest tblCivilJustice = await _context.TblRequests.FindAsync(model.RequestId);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser user = await _context.TblInternalUsers.FindAsync(userId);
            TblTopStatus topStatus = _context.TblTopStatuses.Where(s => s.StatusName == "Completed").FirstOrDefault();
            if (user.IsTeamLeader == true)
            {
                tblCivilJustice.TeamUpprovalStatus = model.DesStatusId;
            }
            else if (user.IsDepartmentHead == true)
            {

                tblCivilJustice.DepartmentUpprovalStatus = model.DesStatusId;
                if (model.IsDeputyApprovalNeeded == true)
                {
                    tblCivilJustice.TopStatusId = topStatus.TopStatusId;
                    tblCivilJustice.DeputyUprovalStatus = model.DesStatusId;
                }
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
                _notifyService.Success("Upproval status changed successfully!");
                return RedirectToAction(nameof(CompletedRequests));
            }
            else
            {
                if (user.IsDepartmentHead == true)
                {
                    ViewBag.visible = true;
                }
                else
                {
                    ViewBag.visible = false;
                }
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
        public async Task<IActionResult> DeleteReply(Guid? id)
        {
            if (id == null || _context.TblReplays == null)
            {
                return NotFound();
            }

            var tblWitnessEvidence = await _context.TblReplays
                .Include(t => t.ExternalReplayedByNavigation)
                .Include(t => t.InternalReplayedByNavigation)
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
            if (_context.TblReplays == null)
            {
                return Problem("Entity set 'AtsdbContext.TblLegalStudiesReplays'  is null.");
            }
            var tblWitnessEvidence = await _context.TblReplays.FindAsync(id);
            if (tblWitnessEvidence != null)
            {
                _context.TblReplays.Remove(tblWitnessEvidence);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Replies), new { id = tblWitnessEvidence.RequestId });
        }
        public async Task<IActionResult> AddHistory(Guid? id)
        {
            DocumentHistoryModel model = await historyModel(id);
            return View(model);
        }
        private async Task<DocumentHistoryModel> historyModel(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblDocumentHistory tblDocument = _context.TblDocumentHistories.Where(x => x.RequestId == id).OrderBy(x => x.Round).Last();
            DocumentHistoryModel model = new DocumentHistoryModel();
            ViewData["histories"] = _context.TblDocumentHistories.Include(x => x.Request).Where(x => x.RequestId == id).ToList();
            model.RequestId = id;
            model.InternalReplyId = userId;
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
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));

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
            history.InternalReplyId = userId;
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
        public async Task<IActionResult> ArchivedRequests()
        {
            ArchiveFilterModel model = new ArchiveFilterModel();
            model.ServiceType = _context.TblServiceTypes.Where(x => x.DepId == Guid.Parse("159f57e9-bc26-4b6e-859e-c577ce8a86a8")).Select(x => new SelectListItem
            {
                Value = x.ServiceTypeId.ToString(),
                Text = x.ServiceTypeName
            }).ToList();
            model.Institution = _context.TblInistitutions.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.InistId.ToString()

            }).ToList();
            model.DocumentType = _context.TblLegalDraftingDocTypes.Select(d => new SelectListItem
            {
                Value = d.DocId.ToString(),
                Text = d.DocName
            }).ToList();


            //IQueryable<TblRequest>? atsdbContext = _context.TblRequests
            //   .Include(t => t.AssignedByNavigation)
            //   .Include(t => t.DocType)
            //   .Include(x => x.ServiceType)
            //   .Include(t => t.Dep)
            //   .Include(t => t.Inist)
            //   .Include(t => t.RequestedByNavigation)
            //   .Include(x => x.ExternalRequestStatus)
            //   .Include(x => x.DepartmentUpprovalStatusNavigation)
            //   .Include(x => x.DeputyUprovalStatusNavigation)
            //   .Include(y => y.TeamUpprovalStatusNavigation)
            //   .Include(t => t.Priority).Where(x => x.Dep.DepCode == "LSDC"&&x.IsArchived==true);
            return View(model);
        }

        public async Task<IActionResult> AssignFromTeam(Guid id)
        {

            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblRequest drafting = await _context.TblRequests.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            model.RequestDetail = drafting.RequestDetail;
            model.RequestId = id;
            model.AssignedBy = userId;
            model.AssignedDate = DateTime.Now;
            model.CreatedBy = drafting.CreatedBy;
            model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
            {
                Value = s.ServiceTypeId.ToString(),
                Text = s.ServiceTypeName
            }).ToList();
            model.ServiceTypeId = drafting.ServiceTypeId;
            model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
            {
                Value = x.DocId.ToString(),
                Text = x.DocName
            }).ToList();
            model.DocId = drafting.DocTypeId;
            model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
            {
                Text = s.AssigneeType.ToString(),
                Value = s.AssigneeTypeId.ToString(),
            }).ToList();
            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
            {
                Text = s.TeamName,
                Value = s.TeamId.ToString(),
            }).ToList();
            
            model.AssignedTos = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "LSDC").Select(x => new SelectListItem
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
        public async Task<IActionResult> AssignFromTeam(CivilJusticeExternalRequestModel model)
        {
            List<string>? emails = new List<string>();
            List<TblRequestAssignee> assignees;
            foreach (var item in model.AssignedTo)
            {
                var userEmails = (from user in _context.TblInternalUsers where user.UserId == item select user.EmailAddress).FirstOrDefault();
                emails.Add(userEmails);
            }
            TblRequestDepartmentRelation departmentRelation = await _context.TblRequestDepartmentRelations.Where(s => s.RequestId == model.RequestId).FirstOrDefaultAsync();
            TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "Assigned to user" select items).FirstOrDefault();
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
                departmentRelation.IsAssingedToUser = true;
                drafting.DueDate = model.DueDate;
                drafting.AssignedDate = model.AssignedDate;
                drafting.PriorityId = model.PriorityId;
                drafting.AssignedBy = model.AssignedBy;
                drafting.AssingmentRemark = model.AssingmentRemark;
                drafting.CreatedBy = model.CreatedBy;
                drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                if (model.AssignedTo.Length > 0)
                {
                    assignees = new List<TblRequestAssignee>();
                    foreach (var item in model.AssignedTo)
                    {
                        var ifExists = _context.TblRequestAssignees.Where(x => x.RequestId == model.RequestId && x.UserId == item).FirstOrDefault();
                        if (ifExists == null)
                        {
                            assignees.Add(new TblRequestAssignee { UserId = item, RequestId = model.RequestId });
                        }
                    }
                    drafting.TblRequestAssignees = assignees;
                }
                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    _notifyService.Success("Task successfully assigned.");
                    await SendMail(emails, "Task is assign notification", "<h3>Some tasks are assigned to you via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                    return RedirectToAction(nameof(TeamRequests));
                }
                else
                {
                    _notifyService.Error("Task isn't successfully assigned. Please try again");
                    model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                    {
                        Value = s.ServiceTypeId.ToString(),
                        Text = s.ServiceTypeName
                    }).ToList();
                    model.ServiceTypeId = drafting.ServiceTypeId;
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Text = s.AssigneeType.ToString(),
                        Value = s.AssigneeTypeId.ToString(),
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                    {
                        Text = s.TeamName,
                        Value = s.TeamId.ToString(),
                    }).ToList();
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
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                model.ServiceTypeId = drafting.ServiceTypeId;
                model.Deparments = _context.TblDepartments.Select(x => new SelectListItem
                {
                    Value = x.DepId.ToString(),
                    Text = x.DepName

                }).ToList();
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
        public async Task<IActionResult> SendToInstitution(Guid? RequestId)
        {
            SendModel model = new SendModel();
            model.RequestId = RequestId;
            return View(model);

        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendToInstitution(SendModel sendModel)
        {
            try
            {
                string dbPath = null, FinallySentReport = null;
                TblRequest request = _context.TblRequests.Where(s => s.RequestId == sendModel.RequestId).FirstOrDefault(); ;
                request.SendingRemark = sendModel.SendingRemark;
                request.IsSenttoInst = true;
                request.SentDate = DateTime.Now;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                if (sendModel.ApprovalLetter != null)
                {
                    string file = Guid.NewGuid().ToString() + sendModel.ApprovalLetter.FileName;
                    string fileWithPath = Path.Combine(path, file);
                    using (var stream = new FileStream(fileWithPath, FileMode.Create))
                    {
                        sendModel.ApprovalLetter.CopyTo(stream);
                    }
                    dbPath = "/Files/" + file;
                    request.LetterofUpproval = dbPath;
                }
                if (sendModel.FinalReport != null)
                {
                    string file = Guid.NewGuid().ToString() + sendModel.FinalReport.FileName;
                    string fileWithPath = Path.Combine(path, file);
                    using (var stream = new FileStream(fileWithPath, FileMode.Create))
                    {
                        sendModel.FinalReport.CopyTo(stream);
                    }
                    FinallySentReport = "/Files/" + file;
                    request.SentReport = FinallySentReport;
                }
                int sent = await _context.SaveChangesAsync();
                if (sent > 0)
                {
                    _notifyService.Success("Final report is sent successfully!");
                    return RedirectToAction(nameof(SentBackRequests));

                }
                else
                {
                    _notifyService.Error("Final report isn't sent successfully. Please try again ");
                    return View(sendModel);
                }


            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again");
                return View(sendModel);
            }

        }
        public async Task<IActionResult> SentBackRequests()
        {
            TblTopStatus tblTopStatus = _context.TblTopStatuses.Where(x => x.StatusName == "Completed").FirstOrDefault();
            List<TblRequest>? atsdbContext = new List<TblRequest>();
            TblRequest tblRequest;
            var moreDeps = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "LSDC").Select(a => a.RequestId).ToList();
            foreach (var item in moreDeps)
            {
                tblRequest = _context.TblRequests
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(x => x.DepartmentUpprovalStatusNavigation)
                                                        .Include(x => x.DeputyUprovalStatusNavigation)
                                                        .Include(y => y.TeamUpprovalStatusNavigation)
                                                        .Include(t => t.Priority).Where(x => x.TopStatusId == tblTopStatus.TopStatusId && x.IsSenttoInst == true && x.RequestId == item).FirstOrDefault();
                if (tblRequest != null)
                {
                    atsdbContext.Add(tblRequest);
                }
            }
            return View(atsdbContext);
        }
    }
}
