using NToastNotify;
using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.Filters;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class ExternalRequestsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly IToastNotification _toastNotification;
        public ExternalRequestsController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mail, IToastNotification toastNotification)
        {
            _toastNotification = toastNotification;
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
        }

        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblExternalRequests.Include(t => t.ExterUser).Include(t => t.Int).Include(s => s.ExternalRequestStatus).Where(x => x.DepId == null);
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
            model.ServiceTypes = _context.TblServiceTypes.Select(s => new SelectListItem
            {
                Value = s.ServiceTypeId.ToString(),
                Text = s.ServiceTypeName
            }).ToList();
            model.LegalStadiesCasetypes = _context.TblLegalDraftingDocTypes.Select(s => new SelectListItem
            {
                Value = s.DocId.ToString(),
                Text = s.DocName
            }).ToList();
            model.LegalStadiesQuestiontypes = _context.TblLegalDraftingQuestionTypes.Select(s => new SelectListItem
            {
                Value = s.QuestTypeId.ToString(),
                Text = s.QuestTypeName
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
                var institutionName = (from items in _context.TblInistitutions where items.InistId == model.InistId select items.Name).FirstOrDefault();
                var users = (from user in _context.TblInternalUsers where (user.IsDepartmentHead == true || user.IsDeputy == true) && user.DepId == model.DepId select user.EmailAddress).ToList();
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "New" select items).FirstOrDefault();
                Guid statusiD = (from id in _context.TblExternalRequestStatuses where id.StatusName == "New" select id.ExternalRequestStatusId).FirstOrDefault();
                var decision = _context.TblDecisionStatuses.Where(x => x.StatusName == "Not set").FirstOrDefault();

                TblExternalRequest requests = new TblExternalRequest();
                TblRequest tblCivilJustice = new TblRequest();
                TblLegalStudiesDrafting drafting = new TblLegalStudiesDrafting();
                TblDepartment department = _context.TblDepartments.FindAsync(model.DepId).Result;
                if (department.DepCode == "CVA")
                {
                    tblCivilJustice.RequestDetail = model.RequestDetail;
                    tblCivilJustice.InistId = model.InistId;
                    tblCivilJustice.CreatedBy = userId;
                    tblCivilJustice.ServiceTypeId = model.ServiceTypeID;
                    tblCivilJustice.ExternalRequestStatusId = statusiD;
                    tblCivilJustice.CreatedDate = DateTime.Now;
                    tblCivilJustice.DepartmentUpprovalStatus = decision.DesStatusId;
                    tblCivilJustice.TeamUpprovalStatus = decision.DesStatusId;
                    tblCivilJustice.DeputyUprovalStatus = decision.DesStatusId;
                    tblCivilJustice.UserUpprovalStatus = decision.DesStatusId;
                    tblCivilJustice.PriorityId = model.PriorityId;
                    _context.TblRequests.Add(tblCivilJustice);
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
                    drafting.RequestDetail = model.RequestDetail;
                    drafting.DepId = model.DepId;
                    drafting.CreatedBy = userId;
                    drafting.DepartmentUpprovalStatus = decision.DesStatusId;
                    drafting.TeamUpprovalStatus = decision.DesStatusId;
                    drafting.DeputyUprovalStatus = decision.DesStatusId;
                    drafting.UserUpprovalStatus = decision.DesStatusId;
                    drafting.ExternalRequestStatusId = statusiD;
                    drafting.QuestTypeId = model.QuestTypeId;
                    drafting.DocId = model.DocId;
                    drafting.PriorityId = model.PriorityId;
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
    }
}
