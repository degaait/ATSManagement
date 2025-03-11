using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.IModels;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class InspectionReportsController :Controller
    {
        private readonly AtsdbContext _context;
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        private readonly IHttpContextAccessor _contextAccessor;
        public InspectionReportsController(AtsdbContext context, IHttpContextAccessor contextAccessor, INotyfService notyfService, IMailService mailService, ILogger<HomeController> logger)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
            _logger = logger;
            _notifyService = notyfService;
        }

        // GET: Recomendations
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblInspectionReports.Include(t => t.Inist).Include(s => s.CreatedByNavigation).Include(s => s.Year);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: Recomendations/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblInspectionReports == null)
            {
                return NotFound();
            }

            var tblRecomendation = await _context.TblInspectionReports
                .Include(t => t.Inist)
                .FirstOrDefaultAsync(m => m.ReportId == id);
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

            TblInspectionReport tblRecomendation = new TblInspectionReport();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            try
            {
                tblRecomendation.ReportId = Guid.NewGuid();
                tblRecomendation.ReportTitle = model.Title;
                tblRecomendation.RecostatusId = model.RecostatusID;
                tblRecomendation.CreatinDate = DateTime.Now;
                tblRecomendation.CreatedBy = userId;
                tblRecomendation.YearId = model.YearId;
                tblRecomendation.InistId = model.InistId;
                tblRecomendation.LawId = model.LawId;                
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                if (model.FinalReport != null)
                {
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    FileInfo fileInfo = new FileInfo(model.FinalReport.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.FinalReport.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.FinalReport.CopyTo(stream);
                    }
                    string dbPath = "/admin/Files/" + fileName;
                    tblRecomendation.ReportPath = dbPath;
                }
                _context.TblInspectionReports.Add(tblRecomendation);
                int save = await _context.SaveChangesAsync();
                if (save > 0)
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
                    model.Laws = _context.TblInspectionLaws.Select(s => new SelectListItem
                    {
                        Text = s.LawDescription,
                        Value = s.LawId.ToString(),
                    }).ToList();
                    return View(model);
                }
            }
            catch(Exception ex)
            {
                _notifyService.Error(ex.Message+" happened. Recomendation isn't successfully Created. Please try again by filling all neccessary fields");
                model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
                {
                    Text = t.Name,
                    Value = t.InistId.ToString(),
                }).ToList();
                model.InistId = model.InistId;            
                model.Laws = _context.TblInspectionLaws.Select(s => new SelectListItem
                {
                    Text = s.LawDescription,
                    Value = s.LawId.ToString(),
                }).ToList();
                model.Years = _context.TblYears.Select(s => new SelectListItem
                {
                    Text = s.Year,
                    Value = s.YearId.ToString(),
                }).ToList();
                return View(model);
            }
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            RecomendationModel model = new RecomendationModel();
            if (id == null || _context.TblInspectionReports == null)
            {
                return NotFound();
            }            
            var tblRecomendation = await _context.TblInspectionReports.FindAsync(id);
            model.RecoId = tblRecomendation.ReportId;
            model.CreatedBy = tblRecomendation.CreatedBy;
            model.Title = tblRecomendation.ReportTitle;
            model.Inistitutions = _context.TblInistitutions.Select(t => new SelectListItem
            {
                Text = t.Name,
                Value = t.InistId.ToString(),
            }).ToList();
            model.InistId = tblRecomendation.InistId;
            model.Laws = _context.TblInspectionLaws.Select(s => new SelectListItem
            {
                Text = s.LawDescription,
                Value = s.LawId.ToString(),
            }).ToList();
            model.LawId = tblRecomendation.LawId;
            model.Years = _context.TblYears.Select(s => new SelectListItem
            {
                Text = s.Year,
                Value = s.YearId.ToString(),
            }).ToList();
            model.YearId = tblRecomendation.YearId;
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
            TblInspectionReport tblRecomendation = await _context.TblInspectionReports.FindAsync(model.RecoId);
            if (model.RecoId == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {                    
                    tblRecomendation.InistId = model.InistId;
                    tblRecomendation.YearId = model.YearId;
                    tblRecomendation.LawId = model.LawId;
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                    if (model.FinalReport != null)
                    {
                        //create folder if not exist
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        //get file extension
                        FileInfo fileInfo = new FileInfo(model.FinalReport.FileName);
                        string fileName = Guid.NewGuid().ToString() + model.FinalReport.FileName;
                        string fileNameWithPath = Path.Combine(path, fileName);
                        using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                        {
                            model.FinalReport.CopyTo(stream);
                        }
                        string dbPath = "/admin/Files/" + fileName;
                    }
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
                      
                        model.Years = _context.TblYears.Select(s => new SelectListItem
                        {
                            Text = s.Year,
                            Value = s.YearId.ToString(),
                        }).ToList();
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
            if (id == null || _context.TblInspectionReports == null)
            {
                return NotFound();
            }
            var tblRecomendation = await _context.TblInspectionReports
                .Include(t => t.Inist)
                .FirstOrDefaultAsync(m => m.ReportId == id);
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
            if (_context.TblInspectionReports == null)
            {
                return Problem("Entity set 'AtsdbContext.TblRecomendations'  is null.");
            }
            var tblRecomendation = await _context.TblInspectionReports.FindAsync(id);
            if (tblRecomendation != null)
            {
                _context.TblInspectionReports.Remove(tblRecomendation);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblRecomendationExists(Guid id)
        {
            return (_context.TblInspectionReports?.Any(e => e.ReportId == id)).GetValueOrDefault();
        }
    }
}
