using ATSManagement.Models;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ATSManagement.Controllers
{
    public class RecomendationsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public RecomendationsController(AtsdbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // GET: Recomendations
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblRecomendations.Include(t => t.Inist).Include(s => s.CreatedByNavigation).Include(s => s.Recostatus);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: Recomendations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblRecomendations == null)
            {
                return NotFound();
            }

            var tblRecomendation = await _context.TblRecomendations
                .Include(t => t.Inist)
                .FirstOrDefaultAsync(m => m.RecoId == id);
            if (tblRecomendation == null)
            {
                return NotFound();
            }

            return View(tblRecomendation);
        }

        // GET: Recomendations/Create
        public IActionResult Create()
        {
            RecomendationModel model = new RecomendationModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.InistId.ToString(),
            }).ToList();
            model.Status = _context.TblRecomendationStatuses.Select(t => new SelectListItem
            {
                Text = t.Status,
                Value = t.RecostatusId.ToString(),
            }).ToList();
            model.CreatedBy = userId;
            model.IsActive = false;
            return View(model);
        }

        // POST: Recomendations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecomendationModel model)
        {

            TblRecomendation tblRecomendation = new TblRecomendation();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (ModelState.IsValid)
            {
                tblRecomendation.RecoId = Guid.NewGuid();
                tblRecomendation.Recomendation = model.Recomendation;
                tblRecomendation.RecostatusId = model.RecostatusID;
                tblRecomendation.CreatinDate = DateTime.Now;
                tblRecomendation.CreatedBy = userId;
                tblRecomendation.EvaluationYear = model.EvaluationYear;
                tblRecomendation.IsActive = model.IsActive;
                tblRecomendation.InistId = model.InistId;

                _context.TblRecomendations.Add(tblRecomendation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.InistId.ToString(),
            }).ToList();
            model.InistId = model.InistId;
            model.Status = _context.TblRecomendationStatuses.Select(t => new SelectListItem
            {
                Text = t.Status,
                Value = t.RecostatusId.ToString(),
            }).ToList();
            model.RecostatusID = model.RecostatusID;
            return View(model);
        }

        // GET: Recomendations/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            RecomendationModel model = new RecomendationModel();
            if (id == null || _context.TblRecomendations == null)
            {
                return NotFound();
            }
            var tblRecomendation = await _context.TblRecomendations.FindAsync(id);
            model.RecoId = tblRecomendation.RecoId;
            model.EvaluationYear = tblRecomendation.EvaluationYear;
            model.Recomendation = tblRecomendation.Recomendation;
            model.CreatedBy = tblRecomendation.CreatedBy;
            model.IsActive = tblRecomendation.IsActive;
            model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.InistId.ToString(),
            }).ToList();
            model.InistId = tblRecomendation.InistId;
            model.Status = _context.TblRecomendationStatuses.Select(t => new SelectListItem
            {
                Text = t.Status,
                Value = t.RecostatusId.ToString(),
            }).ToList();
            model.RecostatusID = tblRecomendation.RecostatusId;
            if (tblRecomendation == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: Recomendations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RecomendationModel model)
        {
            TblRecomendation tblRecomendation = await _context.TblRecomendations.FindAsync(model.RecoId);
            if (model.RecoId == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    tblRecomendation.ModifyDate = DateTime.Now;
                    tblRecomendation.Recomendation = model.Recomendation;
                    tblRecomendation.RecostatusId = model.RecostatusID;
                    tblRecomendation.InistId = model.InistId;
                    tblRecomendation.EvaluationYear = model.EvaluationYear;
                    tblRecomendation.IsActive = model.IsActive;
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
                        {
                            Text = t.Name,
                            Value = t.InistId.ToString(),
                        }).ToList();
                        model.InistId = tblRecomendation.InistId;
                        model.Status = _context.TblRecomendationStatuses.Select(t => new SelectListItem
                        {
                            Text = t.Status,
                            Value = t.RecostatusId.ToString(),
                        }).ToList();
                        model.RecostatusID = tblRecomendation.RecostatusId;
                        return View(model);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblRecomendationExists(model.RecoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.InistId.ToString(),
                }).ToList();
                model.InistId = tblRecomendation.InistId;
                model.Status = _context.TblRecomendationStatuses.Select(t => new SelectListItem
                {
                    Text = t.Status,
                    Value = t.RecostatusId.ToString(),
                }).ToList();
                model.RecostatusID = tblRecomendation.RecostatusId;
                return View(model);
            }
        }

        // GET: Recomendations/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblRecomendations == null)
            {
                return NotFound();
            }

            var tblRecomendation = await _context.TblRecomendations
                .Include(t => t.Inist)
                .FirstOrDefaultAsync(m => m.RecoId == id);
            if (tblRecomendation == null)
            {
                return NotFound();
            }

            return View(tblRecomendation);
        }

        // POST: Recomendations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblRecomendations == null)
            {
                return Problem("Entity set 'AtsdbContext.TblRecomendations'  is null.");
            }
            var tblRecomendation = await _context.TblRecomendations.FindAsync(id);
            if (tblRecomendation != null)
            {
                _context.TblRecomendations.Remove(tblRecomendation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblRecomendationExists(Guid id)
        {
            return (_context.TblRecomendations?.Any(e => e.RecoId == id)).GetValueOrDefault();
        }
    }
}
