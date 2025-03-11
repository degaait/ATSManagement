using NToastNotify;
using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.Filters;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class RecomendationsController : Controller
    {
        private readonly AtsdbContext _context;

        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        private readonly IHttpContextAccessor _contextAccessor;
        public RecomendationsController(AtsdbContext context, IHttpContextAccessor contextAccessor, INotyfService notyfService,IMailService mailService,ILogger<HomeController> logger)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
            _logger = logger;
            _notifyService= notyfService;
        }

        // GET: Recomendations
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblRecomendations.Include(t => t.Inist).Include(s => s.CreatedByNavigation).Include(s => s.Recostatus).Include(s=>s.Year);
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
            model.Years = _context.TblYears.Select(s => new SelectListItem
            {
                Value = s.YearId.ToString(),
                Text = s.Year
            }).ToList();
            model.IsActive = false;
            model.Laws = _context.TblInspectionLaws.Select(s => new SelectListItem
            {
                Text = s.LawDescription,
                Value = s.LawId.ToString(),
            }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecomendationModel model)
        {

            TblRecomendation tblRecomendation = new TblRecomendation();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (ModelState.IsValid)
            {
                tblRecomendation.RecoId = Guid.NewGuid();
                tblRecomendation.RecomendationTitle = model.Title;
                tblRecomendation.Recomendation = model.Recomendation;
                tblRecomendation.RecostatusId = model.RecostatusID;
                tblRecomendation.CreatinDate = DateTime.Now;
                tblRecomendation.CreatedBy = userId;
                tblRecomendation.IsActive = true;
                tblRecomendation.InistId = model.InistId;
                tblRecomendation.LawId = model.LawId;
                tblRecomendation.ReportPath = null;                            
               _context.TblRecomendations.Add(tblRecomendation);
               int save= await _context.SaveChangesAsync();
                if (save>0)
                {
                    _notifyService.Success("Recomendation successfully Created");                   
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notifyService.Error("Recomendation isn't successfully created.");
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
                    model.Laws = _context.TblInspectionLaws.Select(s => new SelectListItem
                    {
                        Text = s.LawDescription,
                        Value = s.LawId.ToString(),
                    }).ToList();
                    model.RecostatusID = model.RecostatusID;
                    return View(model);
                }
               
            }
            else
            {
                _notifyService.Error("Recomendation isn't successfully Created. Please try again by filling all neccessary fields");
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
                model.Laws = _context.TblInspectionLaws.Select(s => new SelectListItem
                {
                    Text = s.LawDescription,
                    Value = s.LawId.ToString(),
                }).ToList();
                model.RecostatusID = model.RecostatusID;
                return View(model);
            }
          
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            RecomendationModel model = new RecomendationModel();
            if (id == null || _context.TblRecomendations == null)
            {
                return NotFound();
            }
            var tblRecomendation = await _context.TblRecomendations.FindAsync(id);
            model.RecoId = tblRecomendation.RecoId;
          
            model.Recomendation = tblRecomendation.Recomendation;
            model.CreatedBy = tblRecomendation.CreatedBy;
            model.IsActive = tblRecomendation.IsActive;
            model.Title = tblRecomendation.RecomendationTitle;
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
            model.Laws = _context.TblInspectionLaws.Select(s => new SelectListItem
            {
                Text = s.LawDescription,
                Value = s.LawId.ToString(),
            }).ToList();
            model.LawId=tblRecomendation.LawId;
            model.RecostatusID = tblRecomendation.RecostatusId;
            model.Years = _context.TblYears.Select(s => new SelectListItem
            {
                Text = s.Year,
                Value = s.YearId.ToString(),
            }).ToList();
            model.YearId= tblRecomendation.YearId;
            if (tblRecomendation == null)
            {
                return NotFound();
            }
            return View(model);
        }

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
                    tblRecomendation.YearId = model.YearId;
                    tblRecomendation.LawId = model.LawId;
                    tblRecomendation.RecomendationTitle = model.Title;
                    tblRecomendation.IsActive = model.IsActive;
                  
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Recomendation is successfully created");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Recomendation isn't sucessfully created. Please try again");
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
                        model.Years = _context.TblYears.Select(s => new SelectListItem
                        {
                            Text = s.Year,
                            Value = s.YearId.ToString(),
                        }).ToList();
                        model.RecostatusID = tblRecomendation.RecostatusId;
                        return View(model);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TblRecomendationExists(model.RecoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _notifyService.Error($"Error: {ex.Message}. Please try again");
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
                        model.Years = _context.TblYears.Select(s => new SelectListItem
                        {
                            Text = s.Year,
                            Value = s.YearId.ToString(),
                        }).ToList();
                        model.RecostatusID = tblRecomendation.RecostatusId;
                        return View(model);
                    }
                }
            }
            else
            {
                _notifyService.Error("Recomendation isn't sucessfully created. Please try again");
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
                model.Years = _context.TblYears.Select(s => new SelectListItem
                {
                    Text = s.Year,
                    Value = s.YearId.ToString(),
                }).ToList();
                return View(model);
            }
        }

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
