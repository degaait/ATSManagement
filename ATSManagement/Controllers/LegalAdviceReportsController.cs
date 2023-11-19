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
    public class LegalAdviceReportsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        private readonly IHttpContextAccessor _contextAccessor;

        public LegalAdviceReportsController(AtsdbContext context, INotyfService notyfService, IMailService mailService, IHttpContextAccessor contextAccessor, ILogger<HomeController> logger)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
            _notifyService = notyfService;
            _logger = logger;
        }

        // GET: LegalAdviceReports
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true)
            {
                var atsdbContext = _context.TblLegalAdviceReports.Include(t => t.ReportedByNavigation).Include(t => t.MonthNavigation).Include(t => t.YearNavigation).Include(t=>t.IdNavigation);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                var atsdbContext = _context.TblLegalAdviceReports.Include(t => t.ReportedByNavigation).Include(t => t.MonthNavigation).Include(t => t.YearNavigation).Include(s=>s.IdNavigation).Where(s => s.ReportedBy == userId);
                return View(await atsdbContext.ToListAsync());
            }
        }

        // GET: LegalAdviceReports/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblLegalAdviceReports == null)
            {
                return NotFound();
            }

            var tblLegalAdviceReport = await _context.TblLegalAdviceReports
                .Include(t => t.MonthNavigation)
                .Include(t => t.ReportedByNavigation)
                .Include(t => t.YearNavigation)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (tblLegalAdviceReport == null)
            {
                return NotFound();
            }

            return View(tblLegalAdviceReport);
        }

        // GET: LegalAdviceReports/Create
        public IActionResult Create()
        {
            ViewData["Month"] = new SelectList(_context.TblMonths, "MonthId", "MonthName");
            ViewData["ServantTypes"] = new SelectList(_context.TblLegalAdviceServantTypes, "Id", "ServantTypeName");
            ViewData["Year"] = new SelectList(_context.TblYears, "YearId", "Year");
            return View();
        }

        // POST: LegalAdviceReports/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReportId,Id,Amount,Investigation,Month,Year,ReportedBy")] TblLegalAdviceReport tblLegalAdviceReport)
        {
            try
            {
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var ceoEmail = (from items in _context.TblInternalUsers where items.Dep.DepCode == "CVA" && (items.IsDeputy == true || items.IsDepartmentHead == true) select items.EmailAddress).ToList();
                var isExits=_context.TblLegalAdviceReports.Where(s=>s.Month==tblLegalAdviceReport.Month&&s.Year==tblLegalAdviceReport.Year&&s.Id==tblLegalAdviceReport.Id).FirstOrDefault();
                if (isExits != null)
                {
                    _notifyService.Information("This report already exists. Please atleast change one of these fields(Year, Month, Servant type)");
                    ViewData["Month"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblLegalAdviceReport.Month);
                    ViewData["ServantTypes"] = new SelectList(_context.TblLegalAdviceServantTypes, "Id", "ServantTypeName", tblLegalAdviceReport.Id);
                    ViewData["Year"] = new SelectList(_context.TblYears, "YearId", "Year", tblLegalAdviceReport.Year);
                    return View(tblLegalAdviceReport);
                }
                if (ModelState.IsValid)
                {
                    tblLegalAdviceReport.ReportedBy = userId;
                    _context.Add(tblLegalAdviceReport);
                    int saved = await _context.SaveChangesAsync();
                    if (saved > 0)
                    {
                        _notifyService.Success("Report Successfully added");
                        await SendMail(ceoEmail, "Monthly Report from branch offices", "<h3>Monthly report iis added from branch offices.Please check on Task Tracking Dashboard</h3>");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Report isn't added successfully!. Please try again");
                        ViewData["Month"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblLegalAdviceReport.Month);
                        ViewData["ServantTypes"] = new SelectList(_context.TblLegalAdviceServantTypes, "Id", "ServantTypeName", tblLegalAdviceReport.Id);
                        ViewData["Year"] = new SelectList(_context.TblYears, "YearId", "Year", tblLegalAdviceReport.Year);
                        return View(tblLegalAdviceReport);
                    }
                }
                else
                {
                    ViewData["Month"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblLegalAdviceReport.Month);
                    ViewData["ServantTypes"] = new SelectList(_context.TblLegalAdviceServantTypes, "Id", "ServantTypeName", tblLegalAdviceReport.Id);
                    ViewData["Year"] = new SelectList(_context.TblYears, "YearId", "Year", tblLegalAdviceReport.Year);
                    return View(tblLegalAdviceReport);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message}");
                ViewData["Month"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblLegalAdviceReport.Month);
                ViewData["ServantTypes"] = new SelectList(_context.TblLegalAdviceServantTypes, "Id", "ServantTypeName", tblLegalAdviceReport.Id);
                ViewData["Year"] = new SelectList(_context.TblYears, "YearId", "Year", tblLegalAdviceReport.Year);
                return View(tblLegalAdviceReport);
            }
           
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
        // GET: LegalAdviceReports/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblLegalAdviceReports == null)
            {
                return NotFound();
            }

            var tblLegalAdviceReport = await _context.TblLegalAdviceReports.FindAsync(id);
            if (tblLegalAdviceReport == null)
            {
                return NotFound();
            }
            ViewData["Month"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblLegalAdviceReport.Month);
            ViewData["ServantTypes"] = new SelectList(_context.TblLegalAdviceServantTypes, "Id", "ServantTypeName", tblLegalAdviceReport.Id);
            ViewData["Year"] = new SelectList(_context.TblYears, "YearId", "Year", tblLegalAdviceReport.Year);
            return View(tblLegalAdviceReport);
        }

        // POST: LegalAdviceReports/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ReportId,Id,Amount,Investigation,Month,Year,ReportedBy")] TblLegalAdviceReport tblLegalAdviceReport)
        {
            if (tblLegalAdviceReport.ReportId.ToString()==null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblLegalAdviceReport);
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Report is successfully edited");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Report isn't edited. Please try again.");
                        ViewData["Month"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblLegalAdviceReport.Month);
                        ViewData["ServantTypes"] = new SelectList(_context.TblLegalAdviceServantTypes, "Id", "ServantTypeName", tblLegalAdviceReport.Id);
                        ViewData["Year"] = new SelectList(_context.TblYears, "YearId", "YearId", tblLegalAdviceReport.Year);
                        return View(tblLegalAdviceReport);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TblLegalAdviceReportExists(tblLegalAdviceReport.ReportId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _notifyService.Error($"Error: {ex.Message}");
                        ViewData["Month"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblLegalAdviceReport.Month);
                        ViewData["ServantTypes"] = new SelectList(_context.TblLegalAdviceServantTypes, "Id", "ServantTypeName", tblLegalAdviceReport.Id);
                        ViewData["Year"] = new SelectList(_context.TblYears, "YearId", "YearId", tblLegalAdviceReport.Year);
                        return View(tblLegalAdviceReport);
                    }
                }
            }
            else
            {
                _notifyService.Error("Report isn't submitted successfully. Please fill all neccessary fields");
                ViewData["Month"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblLegalAdviceReport.Month);
                ViewData["ServantTypes"] = new SelectList(_context.TblLegalAdviceServantTypes, "Id", "ServantTypeName", tblLegalAdviceReport.Id);
                ViewData["Year"] = new SelectList(_context.TblYears, "YearId", "YearId", tblLegalAdviceReport.Year);
                return View(tblLegalAdviceReport);
            }
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblLegalAdviceReports == null)
            {
                return NotFound();
            }

            var tblLegalAdviceReport = await _context.TblLegalAdviceReports
                .Include(t => t.MonthNavigation)
                .Include(t => t.ReportedByNavigation)
                .Include(t => t.YearNavigation)
                .FirstOrDefaultAsync(m => m.ReportId == id);
            if (tblLegalAdviceReport == null)
            {
                return NotFound();
            }

            return View(tblLegalAdviceReport);
        }

        // POST: LegalAdviceReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblLegalAdviceReports == null)
            {
                return Problem("Entity set 'AtsdbContext.TblLegalAdviceReports'  is null.");
            }
            var tblLegalAdviceReport = await _context.TblLegalAdviceReports.FindAsync(id);
            if (tblLegalAdviceReport != null)
            {
                _context.TblLegalAdviceReports.Remove(tblLegalAdviceReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblLegalAdviceReportExists(Guid id)
        {
          return (_context.TblLegalAdviceReports?.Any(e => e.ReportId == id)).GetValueOrDefault();
        }
    }
}
