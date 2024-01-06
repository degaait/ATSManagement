using System;
using System.Linq;
using NToastNotify;
using ATSManagement.Models;
using ATSManagement.IModels;
using System.Threading.Tasks;
using ATSManagement.Services;
using Microsoft.AspNetCore.Mvc;
using ATSManagement.ViewModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    public class PlanCatagoriesController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        private LanguageService _localization;
        public PlanCatagoriesController(ILogger<HomeController> logger, IMailService mail, AtsdbContext atsdbContext, INotyfService notyfService, LanguageService localization)
        {
            _notifyService = notyfService;
            _logger = logger;
            _mail = mail;
            _context = atsdbContext;
            _localization = localization;
        }

        public async Task<IActionResult> Index(Guid? InspectionPlanId)
       {
            ViewBag.InspectionPlanId= InspectionPlanId;
            var atsdbContext = _context.TblPlanCatagories.Include(t => t.InspectionPlan).Where(s=>s.InspectionPlanId == InspectionPlanId);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: PlanCatagories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblPlanCatagories == null)
            {
                return NotFound();
            }
            var tblPlanCatagory = await _context.TblPlanCatagories
                .Include(t => t.InspectionPlan)
                .FirstOrDefaultAsync(m => m.PlanCatId == id);
            if (tblPlanCatagory == null)
            {
                return NotFound();
            }

            return View(tblPlanCatagory);
        }

        // GET: PlanCatagories/Create
        public IActionResult Create(Guid? InspectionPlanId)
        {
            AnnualPlanCatagory model= new AnnualPlanCatagory();
            model.InspectionPlanId = InspectionPlanId;
             return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AnnualPlanCatagory model)
        {
            try
            {
                TblPlanCatagory item = new TblPlanCatagory();
                item.InspectionPlanId=model.InspectionPlanId;
                item.CatTitle = model.CatTitle;
                item.CatDescription = model.CatDescription;
                _context.TblPlanCatagories.Add(item);
                int save=_context.SaveChanges();
                if (save>0)
                {
                    _notifyService.Success("Annua plan category successfully added");
                    return RedirectToAction(nameof(Index), new { InspectionPlanId = model.InspectionPlanId });
                }
                else
                {
                    _notifyService.Error("Annual plan category isn't successfully Added.");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error:{ex.Message} happened. Annual plan category isn't successfully Added.");
                return View(model);
            }        
        }        
        public async Task<IActionResult> Edit(int PlanCatId)
        {
            TblPlanCatagory Categories = _context.TblPlanCatagories.Find(PlanCatId);
            AnnualPlanCatagory category= new AnnualPlanCatagory();
            category.PlanCatId = PlanCatId;
            category.CatTitle=category.CatTitle;
            category.CatDescription=category.CatDescription;
            if (Categories == null)
            {
                return NotFound();
            }
            return View(category);
        }      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AnnualPlanCatagory model)
        {
                if (model.PlanCatId!=null)
                {
                    return NotFound();
                }            
                try
                {
                    TblPlanCatagory Categories = _context.TblPlanCatagories.Find(model.PlanCatId);
                    Categories.CatTitle=model.CatTitle;
                    Categories.CatDescription=model.CatDescription;                    
                  int updated=  await _context.SaveChangesAsync();
                    if (updated>0)
                    {
                        _notifyService.Success("Plan category is successfully.");
                    return RedirectToAction(nameof(Index), new { InspectionPlanId = model.InspectionPlanId });
                }
                    else
                    {
                        _notifyService.Error("Annual plan category isn't successfully uppdated.");
                        return View(model);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TblPlanCatagoryExists(model.PlanCatId))
                    {
                        return NotFound();
                    }
                    else
                    {
                    _notifyService.Error($"Error:{ex.Message}. Annual plan category isn't successfully uppdated.");
                    return View(model);
                    }
                }      
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblPlanCatagories == null)
            {
                return NotFound();
            }
            var tblPlanCatagory = await _context.TblPlanCatagories
                .Include(t => t.InspectionPlan)
                .FirstOrDefaultAsync(m => m.PlanCatId == id);
            if (tblPlanCatagory == null)
            {
                return NotFound();
            }
            return View(tblPlanCatagory);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblPlanCatagories == null)
            {
                return Problem("Entity set 'AtsdbContext.TblPlanCatagories'  is null.");
            }
            var tblPlanCatagory = await _context.TblPlanCatagories.FindAsync(id);
            if (tblPlanCatagory != null)
            {
                _context.TblPlanCatagories.Remove(tblPlanCatagory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblPlanCatagoryExists(int id)
        {
          return (_context.TblPlanCatagories?.Any(e => e.PlanCatId == id)).GetValueOrDefault();
        }
    }
}
