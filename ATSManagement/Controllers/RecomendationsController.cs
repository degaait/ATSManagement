using ATSManagement.Models;
using Microsoft.AspNetCore.Mvc;
using ATSManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var atsdbContext = _context.TblRecomendations.Include(t => t.Inist);
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
                Text=t.Name,
                Value=t.InistId.ToString(),
            }).ToList();
            model.Status=_context.TblRecomendationStatuses.Select(t=> new SelectListItem
            {
                Text = t.Status,
                Value = t.RecostatusId.ToString(),
            }).ToList();
            model.CreatedBy = userId;
            model.IsActive = false;
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "Name");
            return View(model);
        }

        // POST: Recomendations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecomendationModel model)
        {
            TblRecomendation tblRecomendation= new TblRecomendation();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (ModelState.IsValid)
            {
                tblRecomendation.RecoId = Guid.NewGuid();
                tblRecomendation.Recomendation=model.Recomendation;
                tblRecomendation.RecostatusId = model.RecostatusID;
                tblRecomendation.CreatinDate = DateTime.Now;
                tblRecomendation.CreatedBy = model.CreatedBy;
                tblRecomendation.EvaluationYear = model.EvaluationYear;
                tblRecomendation.IsActive=model.IsActive;
                _context.TblRecomendations.Add(tblRecomendation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            model.Inistitutions=_context.TblInistitutions.Select(t=> new SelectListItem
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
            model.RecostatusID=model.RecostatusID;
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
            model.CreatedBy = tblRecomendation.CreatedBy;
            model.EvaluationYear = tblRecomendation.EvaluationYear;
            model.Recomendation = tblRecomendation.Recomendation;
            model.CreatinDate= tblRecomendation.CreatinDate;
            model.EvaluationYear=tblRecomendation.EvaluationYear;
            model.ModifyDate = tblRecomendation.ModifyDate;
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
            if (model.RecoId==null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    tblRecomendation.CreatinDate = model.CreatinDate;
                    tblRecomendation.ModifyDate = DateTime.Now;
                    tblRecomendation.Recomendation = model.Recomendation;
                    tblRecomendation.RecostatusId = model.RecostatusID;
                    tblRecomendation.InistId=model.InistId;
                    tblRecomendation.EvaluationYear=model.EvaluationYear;
                    tblRecomendation.IsActive= model.IsActive;
                    _context.Update(model);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
            }
          return View(model);
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
