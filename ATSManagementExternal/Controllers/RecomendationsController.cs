﻿using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using ATSManagementExternal.Models;
using Microsoft.EntityFrameworkCore;
using ATSManagementExternal.ViewModels;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagementExternal.Controllers
{
    public class RecomendationsController : Controller
    {
        private readonly AtsdbContext _context;
        public RecomendationsController(AtsdbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            ViewBag.openRecos = _context.TblRecomendations.Include(s => s.Recostatus).Where(s => s.Recostatus.Status == "Open").ToList().Count();
            ViewBag.closedStatus = _context.TblRecomendations.Include(s => s.Recostatus).Where(s => s.Recostatus.Status == "Closed").ToList().Count();
            CollectionModel collectionModel = GetModel();
            ViewBag.RecoId = Guid.Empty.ToString();
            List<DataPoint> dataPointss = new List<DataPoint>();

            var recomendations = (from item in _context.TblRecomendations
                                  join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                  join years in _context.TblYears on item.YearId equals years.YearId
                                  join status in _context.TblRecomendationStatuses on item.RecostatusId equals status.RecostatusId
                                  group item by item.Inist.Name into g
                                  select new
                                  {
                                      Instition = g.Key,
                                      Number = g.Count(),
                                  }).ToList();
            foreach (var item in recomendations)
            {
                dataPointss.Add(new DataPoint(item.Instition, item.Number));
            }
            ViewBag.Datas = dataPointss;
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPointss);
            return View(collectionModel);
        }
        private CollectionModel GetModel()
        {
            CollectionModel collectionModel = new CollectionModel();
            collectionModel.tblYears = _context.TblYears.Select(s => new SelectListItem
            {
                Value = s.YearId.ToString(),
                Text = s.Year
            }).ToList();

            collectionModel.tblInistitutions = _context.TblInistitutions.Select(s => new SelectListItem
            {
                Value = s.InistId.ToString(),
                Text = s.Name
            }).ToList();
            collectionModel.tblInspectionLaws = _context.TblInspectionLaws.Select(s => new SelectListItem
            {
                Text = s.LawDescription,
                Value = s.LawId.ToString()
            }).ToList();
            collectionModel.tblRecomendationsStatus = _context.TblRecomendationStatuses.Select(s => new SelectListItem
            {
                Text = s.Status,
                Value = s.RecostatusId.ToString()
            }).ToList();
            collectionModel.tblRecomendations = _context.TblRecomendations.Include(s => s.Inist).Include(s => s.Year).Include(s => s.Recostatus).ToList();
            collectionModel.tblRecomendings = _context.TblRecomendations.Where(S => S.IsActive == true).Select(s => new SelectListItem
            {
                Value = s.RecoId.ToString(),
                Text = s.RecomendationTitle
            }).ToList();
            return collectionModel;
        }

        [HttpPost]
        public async Task<IActionResult> Index(CollectionModel collectionModel)
        {
            ViewBag.openRecos = _context.TblRecomendations.Include(s => s.Recostatus).Where(s => s.Recostatus.Status == "Open").ToList().Count();
            ViewBag.closedStatus = _context.TblRecomendations.Include(s => s.Recostatus).Where(s => s.Recostatus.Status == "Closed").ToList().Count();
            CollectionModel model = GetModel();
            List<DataPoint> dataPointss = new List<DataPoint>();

            if (collectionModel.RecostatusId != Guid.Empty)
            {
                
                if (collectionModel.InistId == Guid.Empty &&
                    collectionModel.LawId == Guid.Empty)
                {

                    var recomendationss = _context.TblRecomendations
                        .Include(s => s.Inist)
                        .Include(s => s.Recostatus)
                        .Include(s => s.Law)
                        .Where(s => s.RecostatusId == collectionModel.RecostatusId).ToList();
                    model.tblRecomendations = recomendationss;

                    var recomendations = (from item in _context.TblRecomendations
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join status in _context.TblRecomendationStatuses on item.RecostatusId equals status.RecostatusId
                                          where status.RecostatusId == collectionModel.RecostatusId
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                   
                }
                else if (
                    collectionModel.InistId != Guid.Empty &&
                    collectionModel.LawId == Guid.Empty)
                {
                    var recomendationss = _context.TblRecomendations
                        .Include(s => s.Inist)
                        .Include(s => s.Recostatus)
                        .Include(s => s.Law)
                        .Where(s => s.RecostatusId == collectionModel.RecostatusId
                        &&s.InistId==collectionModel.InistId).ToList();
                    model.tblRecomendations = recomendationss;

                    var recomendations = (from item in _context.TblRecomendations
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join years in _context.TblYears on item.YearId equals years.YearId
                                          join status in _context.TblRecomendationStatuses on item.RecostatusId equals status.RecostatusId
                                          where status.RecostatusId == collectionModel.RecostatusId && ints.InistId == collectionModel.InistId 
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }
                else if (collectionModel.InistId != Guid.Empty &&
                    collectionModel.LawId != Guid.Empty)
                {
                    var recomendationss = _context.TblRecomendations
                       .Include(s => s.Inist)
                       .Include(s => s.Recostatus)
                       .Include(s => s.Law)
                       .Where(s => s.RecostatusId == collectionModel.RecostatusId
                       && s.InistId == collectionModel.InistId&&s.LawId==collectionModel.LawId).ToList();
                    model.tblRecomendations = recomendationss;
                    var recomendations = (from item in _context.TblRecomendations
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join status in _context.TblRecomendationStatuses on item.RecostatusId equals status.RecostatusId
                                          where status.RecostatusId == collectionModel.RecostatusId &&
                                          ints.InistId == collectionModel.InistId && item.LawId == collectionModel.LawId
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }             
            }
            else
            {
                if (collectionModel.InistId == Guid.Empty &&
                    collectionModel.LawId == Guid.Empty)
                {
                    var recomendationss = _context.TblRecomendations
                       .Include(s => s.Inist)
                       .Include(s => s.Recostatus)
                       .Include(s => s.Law).ToList();
                    model.tblRecomendations = recomendationss;

                    var recomendations = (from item in _context.TblRecomendations
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join years in _context.TblYears on item.YearId equals years.YearId
                                          join status in _context.TblRecomendationStatuses on item.RecostatusId equals status.RecostatusId
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }
                else if (collectionModel.InistId != Guid.Empty &&
                    collectionModel.LawId == Guid.Empty)
                {
                    var recomendationss = _context.TblRecomendations
                       .Include(s => s.Inist)
                       .Include(s => s.Recostatus)
                       .Include(s => s.Law)
                       .Where(s=>s.InistId == collectionModel.InistId).ToList();
                    model.tblRecomendations = recomendationss;
                    var recomendations = (from item in _context.TblRecomendations
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join years in _context.TblYears on item.YearId equals years.YearId
                                          join status in _context.TblRecomendationStatuses on item.RecostatusId equals status.RecostatusId
                                          where ints.InistId == collectionModel.InistId
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }
                else if (
                    collectionModel.InistId != Guid.Empty &&
                    collectionModel.LawId != Guid.Empty)
                {
                    var recomendationss = _context.TblRecomendations
                       .Include(s => s.Inist)
                       .Include(s => s.Recostatus)
                       .Include(s => s.Law)
                       .Where(s => s.LawId == collectionModel.LawId
                       && s.InistId == collectionModel.InistId).ToList();
                    model.tblRecomendations = recomendationss;
                    var recomendations = (from item in _context.TblRecomendations
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join status in _context.TblRecomendationStatuses on item.RecostatusId equals status.RecostatusId
                                          where item.InistId == collectionModel.InistId && item.LawId == collectionModel.LawId
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }

            }

            ViewBag.Datas = dataPointss;
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPointss);
            return View(model);
        }

        public async Task<IActionResult> MonitoringReport()
        {
            ViewBag.TotalReports = _context.TblInspectionReports.ToList().Count();
            CollectionModel collectionModel = GetModel();
            List<DataPoint> dataPointss = new List<DataPoint>();

            var recomendations = (from item in _context.TblInspectionReports
                                  join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                  join years in _context.TblYears on item.YearId equals years.YearId
                                  group item by item.Inist.Name into g
                                  select new
                                  {
                                      Instition = g.Key,
                                      Number = g.Count(),
                                  }).ToList();
            foreach (var item in recomendations)
            {
                dataPointss.Add(new DataPoint(item.Instition, item.Number));
            }
            ViewBag.Datas = dataPointss;
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPointss);
            return View(collectionModel);
        }
        [HttpPost]
        public async Task<IActionResult> MonitoringReport(CollectionModel collectionModel)
        {
            ViewBag.TotalReports = _context.TblInspectionReports.ToList().Count(); CollectionModel model = GetModel();
            List<DataPoint> dataPointss = new List<DataPoint>();

            if (collectionModel.YearId != Guid.Empty)
            {               
                if (
                    collectionModel.InistId == Guid.Empty &&
                    collectionModel.LawId == Guid.Empty)
                {
                    var recomendations = (from item in _context.TblInspectionReports
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join years in _context.TblYears on item.YearId equals years.YearId
                                          where years.YearId == collectionModel.YearId 
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }
                else if (collectionModel.InistId != Guid.Empty &&
                    collectionModel.LawId == Guid.Empty)
                {
                    var recomendations = (from item in _context.TblInspectionReports
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join years in _context.TblYears on item.YearId equals years.YearId
                                          where years.YearId == collectionModel.YearId && ints.InistId == collectionModel.InistId
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }
                else if (collectionModel.InistId != Guid.Empty &&
                    collectionModel.LawId != Guid.Empty)
                {
                    var recomendations = (from item in _context.TblInspectionReports
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join years in _context.TblYears on item.YearId equals years.YearId
                                          where years.YearId == collectionModel.YearId &&
                                          item.LawId == collectionModel.LawId && 
                                          ints.InistId == collectionModel.InistId
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }
                
            }
            else
            {
                
                if ( collectionModel.InistId == Guid.Empty &&
                    collectionModel.LawId == Guid.Empty)
                {
                    var recomendations = (from item in _context.TblInspectionReports
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join years in _context.TblYears on item.YearId equals years.YearId                                          
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }
                else if (
                    collectionModel.InistId != Guid.Empty &&
                    collectionModel.LawId == Guid.Empty)
                {
                    var recomendations = (from item in _context.TblInspectionReports
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join years in _context.TblYears on item.YearId equals years.YearId
                                          where  ints.InistId == collectionModel.InistId
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }
                else if (collectionModel.InistId != Guid.Empty &&
                    collectionModel.LawId != Guid.Empty)
                {
                    var recomendations = (from item in _context.TblInspectionReports
                                          join ints in _context.TblInistitutions on item.InistId equals ints.InistId
                                          join years in _context.TblYears on item.YearId equals years.YearId
                                          where item.LawId == collectionModel.LawId && ints.InistId == collectionModel.InistId
                                          group item by item.Inist.Name into g
                                          select new
                                          {
                                              Instition = g.Key,
                                              Number = g.Count(),
                                          }).ToList();
                    foreach (var item in recomendations)
                    {
                        dataPointss.Add(new DataPoint(item.Instition, item.Number));
                    }
                }

            }

            ViewBag.Datas = dataPointss;
            ViewBag.DataPoints = JsonConvert.SerializeObject(dataPointss);
            return View(model);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblRecomendations == null)
            {
                return NotFound();
            }
            var tblRecomendation = await _context.TblRecomendations
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Inist)
                .Include(t => t.Recostatus)
                .FirstOrDefaultAsync(m => m.RecoId == id);
            if (tblRecomendation == null)
            {
                return NotFound();
            }

            return View(tblRecomendation);
        }
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId");
            ViewData["RecostatusId"] = new SelectList(_context.TblRecomendationStatuses, "RecostatusId", "RecostatusId");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RecoId,Recomendation,InistId,RecostatusId,EvaluationYear,CreatedBy,CreatinDate,ModifyDate,IsActive")] TblRecomendation tblRecomendation)
        {
            if (ModelState.IsValid)
            {
                tblRecomendation.RecoId = Guid.NewGuid();
                _context.Add(tblRecomendation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRecomendation.CreatedBy);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblRecomendation.InistId);
            ViewData["RecostatusId"] = new SelectList(_context.TblRecomendationStatuses, "RecostatusId", "RecostatusId", tblRecomendation.RecostatusId);
            return View(tblRecomendation);
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblRecomendations == null)
            {
                return NotFound();
            }

            var tblRecomendation = await _context.TblRecomendations.FindAsync(id);
            if (tblRecomendation == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRecomendation.CreatedBy);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblRecomendation.InistId);
            ViewData["RecostatusId"] = new SelectList(_context.TblRecomendationStatuses, "RecostatusId", "RecostatusId", tblRecomendation.RecostatusId);
            return View(tblRecomendation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("RecoId,Recomendation,InistId,RecostatusId,EvaluationYear,CreatedBy,CreatinDate,ModifyDate,IsActive")] TblRecomendation tblRecomendation)
        {
            if (id != tblRecomendation.RecoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblRecomendation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblRecomendationExists(tblRecomendation.RecoId))
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
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblRecomendation.CreatedBy);
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblRecomendation.InistId);
            ViewData["RecostatusId"] = new SelectList(_context.TblRecomendationStatuses, "RecostatusId", "RecostatusId", tblRecomendation.RecostatusId);
            return View(tblRecomendation);
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblRecomendations == null)
            {
                return NotFound();
            }

            var tblRecomendation = await _context.TblRecomendations
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Inist)
                .Include(t => t.Recostatus)
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

        public ActionResult DocumentViewer(string path, string? methodController, string? method)
        {
            ViewBag.path = path;
            ViewBag.controller = methodController;
            ViewBag.action = method;
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> DownloadEvidenceFile(string path, string? methodController, string? method)
        {
            string filename = path.Substring(7);
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "admin\\", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }
            if (!System.IO.File.Exists(filepath))
            {
                return RedirectToAction(nameof(FileNotFound), new { path = path, methodController = methodController, method = method });
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));

        }

        public async Task<IActionResult>? FileNotFound(string path, string? methodController, string? method)
        {
            ViewBag.controller = methodController;
            ViewBag.action = method;
            return View();
        }

    }
}
