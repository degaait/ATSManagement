using ATSManagement.Models;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ATSManagement.Controllers
{
    public class LegalStudiesDraftingController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public LegalStudiesDraftingController(AtsdbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
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
    }
}
