using System;
using System.Linq;
using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class InstotutionMonitoringReportsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        private readonly IHttpContextAccessor _contextAccessor;
        public InstotutionMonitoringReportsController(AtsdbContext context, INotyfService notyfService, IMailService mailService, IHttpContextAccessor contextAccessor, ILogger<HomeController> logger)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
            _notifyService = notyfService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true)
            {
                var atsdbContext = _context.TblInstotutionMonitoringReports.Include(t => t.CreatedByNavigation).Include(t => t.Month).Include(t => t.Year);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                var atsdbContext = _context.TblInstotutionMonitoringReports.Include(t => t.CreatedByNavigation).Include(t => t.Month).Include(t => t.Year).Where(s=>s.CreatedBy==userId);
                return View(await atsdbContext.ToListAsync());
            }                
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblInstotutionMonitoringReports == null)
            {
                return NotFound();
            }

            var tblInstotutionMonitoringReport = await _context.TblInstotutionMonitoringReports
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Month)
                .Include(t => t.Year)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblInstotutionMonitoringReport == null)
            {
                return NotFound();
            }

            return View(tblInstotutionMonitoringReport);
        }
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName");
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MoniteredOffice,ContractNumber,ContractMoneyAmount,Adrnumber,AdrmoneyAmount,DebateNumber,DebateMoneyAmmount,Investigation,YearId,MonthId,CreatedBy")] TblInstotutionMonitoringReport tblInstotutionMonitoringReport)
        {
            try
            {
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var ceoEmail = (from items in _context.TblInternalUsers where items.Dep.DepCode == "CVA" && (items.IsDeputy == true || items.IsDepartmentHead == true) select items.EmailAddress).ToList();
                if (ModelState.IsValid)
                {
                    tblInstotutionMonitoringReport.CreatedBy=userId;
                    _context.Add(tblInstotutionMonitoringReport);
                   int saved= await _context.SaveChangesAsync();
                    if (saved>0)
                    {
                        _notifyService.Success("Report Successfully added");
                        await SendMail(ceoEmail, "Monthly Report from branch offices", "<h3>Monthly report iis added from branch offices.Please check on Task Tracking Dashboard</h3>");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Report isn't added successfully!. Please try again");
                        ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInstotutionMonitoringReport.CreatedBy);
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblInstotutionMonitoringReport.MonthId);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblInstotutionMonitoringReport.YearId);
                        return View(tblInstotutionMonitoringReport);
                    }                    
                }
                else
                {
                    _notifyService.Error("Report isn't added successfully!. Please try again");
                    ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInstotutionMonitoringReport.CreatedBy);
                    ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblInstotutionMonitoringReport.MonthId);
                    ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblInstotutionMonitoringReport.YearId);
                    return View(tblInstotutionMonitoringReport);
                }               
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message}");
                ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInstotutionMonitoringReport.CreatedBy);
                ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblInstotutionMonitoringReport.MonthId);
                ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblInstotutionMonitoringReport.YearId);
                return View(tblInstotutionMonitoringReport);
            }          
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblInstotutionMonitoringReports == null)
            {
                return NotFound();
            }
            var tblInstotutionMonitoringReport = await _context.TblInstotutionMonitoringReports.FindAsync(id);
            if (tblInstotutionMonitoringReport == null)
            {
                return NotFound();
            }
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInstotutionMonitoringReport.CreatedBy);
            ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblInstotutionMonitoringReport.MonthId);
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblInstotutionMonitoringReport.YearId);
            return View(tblInstotutionMonitoringReport);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,MoniteredOffice,ContractNumber,ContractMoneyAmount,Adrnumber,AdrmoneyAmount,DebateNumber,DebateMoneyAmmount,Investigation,YearId,MonthId,CreatedBy")] TblInstotutionMonitoringReport tblInstotutionMonitoringReport)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (id != tblInstotutionMonitoringReport.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    tblInstotutionMonitoringReport.CreatedBy = userId;
                    _context.Update(tblInstotutionMonitoringReport);
                   int updated= await _context.SaveChangesAsync();
                    if (updated>0)
                    {
                        _notifyService.Success("Report is successfully added");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Report isn't edited. Please try again.");
                        ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInstotutionMonitoringReport.CreatedBy);
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblInstotutionMonitoringReport.MonthId);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblInstotutionMonitoringReport.YearId);
                        return View(tblInstotutionMonitoringReport);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TblInstotutionMonitoringReportExists(tblInstotutionMonitoringReport.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _notifyService.Error($"Error: {ex.Message}");
                        ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInstotutionMonitoringReport.CreatedBy);
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthId", tblInstotutionMonitoringReport.MonthId);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblInstotutionMonitoringReport.YearId);
                        return View(tblInstotutionMonitoringReport);
                    }
                }
            }
            else
            {
                _notifyService.Error("Report isn't submitted successfully. Please fill all neccessary fields");
                ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblInstotutionMonitoringReport.CreatedBy);
                ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthId", tblInstotutionMonitoringReport.MonthId);
                ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblInstotutionMonitoringReport.YearId);
                return View(tblInstotutionMonitoringReport);
            }
         
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblInstotutionMonitoringReports == null)
            {
                return NotFound();
            }

            var tblInstotutionMonitoringReport = await _context.TblInstotutionMonitoringReports
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Month)
                .Include(t => t.Year)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblInstotutionMonitoringReport == null)
            {
                return NotFound();
            }

            return View(tblInstotutionMonitoringReport);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblInstotutionMonitoringReports == null)
            {
                return Problem("Entity set 'AtsdbContext.TblInstotutionMonitoringReports'  is null.");
            }
            var tblInstotutionMonitoringReport = await _context.TblInstotutionMonitoringReports.FindAsync(id);
            if (tblInstotutionMonitoringReport != null)
            {
                _context.TblInstotutionMonitoringReports.Remove(tblInstotutionMonitoringReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblInstotutionMonitoringReportExists(Guid id)
        {
          return (_context.TblInstotutionMonitoringReports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
    }
}
