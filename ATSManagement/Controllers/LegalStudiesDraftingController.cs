using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                .Include(t => t.CaseType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(x => x.ExternalRequestStatus)
                .Include(t => t.Priority);
            return View(await atsdbContext.ToListAsync());
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
                .Include(t => t.CaseType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(x => x.ExternalRequestStatus)
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

                draftings.RequestDetail = model.RequestDetail;
                draftings.CreatedBy = model.CreatedBy;
                draftings.CreatedDate = DateTime.Now;
                draftings.CaseTypeId = model.CaseTypeId;
                draftings.InistId = model.InistId;
                draftings.PriorityId = model.PriorityId;
                draftings.DepId = model.DepId;
                draftings.IsUprovedbyDepartment = false;
                draftings.IsUpprovedByUser = false;
                draftings.IsUprovedByDeputy = false;
                draftings.IsUprovedByTeam = false;
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
            model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
            {
                Value = x.CaseTypeId.ToString(),
                Text = x.CaseTypeName
            }).ToList();
            model.CaseTypeId = legalDraftig.CaseTypeId;
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
                    model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
                    {
                        Value = x.CaseTypeId.ToString(),
                        Text = x.CaseTypeName
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
                model.CaseTypes = _context.TblCivilJusticeCaseTypes.Select(x => new SelectListItem
                {
                    Value = x.CaseTypeId.ToString(),
                    Text = x.CaseTypeName
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
                .Include(t => t.CaseType)
                .Include(t => t.Dep)
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .Include(x => x.ExternalRequestStatus)
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
            model.RequestDetail = drafting.RequestDetail;
            model.RequestId = id;
            model.AssignedDate = DateTime.Now;
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
        public async Task<IActionResult> AssignToUser(LegalStudiesDraftingModel model)
        {
            var userEmails = (from user in _context.TblInternalUsers where user.UserId == model.AssignedTo select user.EmailAddress).ToList();

            TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "In Progress" select items).FirstOrDefault();

            if (model.RequestId != null)
            {
                return NotFound();
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
        private async Task SendMail(List<string> to, string subject, string body)
        {
            MailData data = new MailData(to, subject, body, "degaait@gmail.com");
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }

        public async Task<IActionResult> CompletedRequests()
        {
            var atsdbContext = _context.TblLegalStudiesDraftings
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.AssignedToNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "Completed");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> PendingRequests()
        {
            var atsdbContext = _context.TblLegalStudiesDraftings
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.AssignedToNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(t => t.Priority).Where(x => x.ExternalRequestStatus.StatusName == "In Progress");
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> AssignedRequests()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));

            var atsdbContext = _context.TblLegalStudiesDraftings
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
