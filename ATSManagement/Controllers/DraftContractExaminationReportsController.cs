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
    public class DraftContractExaminationReportsController : Controller
    {
        private readonly AtsdbContext _context;

        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        private readonly IHttpContextAccessor _contextAccessor;
        public DraftContractExaminationReportsController(AtsdbContext context,INotyfService notyfService,IMailService mailService,IHttpContextAccessor httpContext, ILogger<HomeController> logger)
        {
            _context = context;
            _contextAccessor = httpContext;
            _mail = mailService;
            _logger= logger;
            _notifyService = notyfService;
        }

        // GET: DraftContractExaminationReports
        public async Task<IActionResult> Index()
        {       

            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true)
            {
                var atsdbContext = _context.TblDraftContractExaminationReports.Include(t => t.SubmittedByNavigation).Include(t => t.Month).Include(t => t.Year);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                var atsdbContext = _context.TblDraftContractExaminationReports.Include(t => t.SubmittedByNavigation).Include(t => t.Month).Include(t => t.Year).Where(s => s.SubmittedBy == userId);
                return View(await atsdbContext.ToListAsync());
            }
        }

        // GET: DraftContractExaminationReports/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblDraftContractExaminationReports == null)
            {
                return NotFound();
            }

            var tblDraftContractExaminationReport = await _context.TblDraftContractExaminationReports
                .Include(t => t.Month)
                .Include(t => t.SubmittedByNavigation)
                .Include(t => t.Year)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblDraftContractExaminationReport == null)
            {
                return NotFound();
            }

            return View(tblDraftContractExaminationReport);
        }

        // GET: DraftContractExaminationReports/Create
        public IActionResult Create()
        {
            ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName");
            ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,QuestionSubmitter,CaseType,FullMoneyAmmount,Investigation,YearId,MonthId,SubmittedBy")] TblDraftContractExaminationReport tblDraftContractExaminationReport)
        {
            try
            {
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var ceoEmail = (from items in _context.TblInternalUsers where items.Dep.DepCode == "CVA" && (items.IsDeputy == true || items.IsDepartmentHead == true) select items.EmailAddress).ToList();
                if (ModelState.IsValid)
                {
                    tblDraftContractExaminationReport.SubmittedBy = userId;
                    _context.Add(tblDraftContractExaminationReport);
                    int saved=await _context.SaveChangesAsync();
                    if (saved>0)
                    {
                        _notifyService.Success("Report Successfully added");
                        await SendMail(ceoEmail, "Monthly Report from branch offices", "<h3>Monthly report iis added from branch offices.Please check on Task Tracking Dashboard</h3>");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Report isn't added successfully!. Please try again");
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDraftContractExaminationReport.MonthId);
                        ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblDraftContractExaminationReport.SubmittedBy);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDraftContractExaminationReport.YearId);
                        return View(tblDraftContractExaminationReport);
                    }
                   
                }
                else
                {
                    _notifyService.Error("Report isn't added successfully!. Please try again");
                    ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDraftContractExaminationReport.MonthId);
                    ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblDraftContractExaminationReport.SubmittedBy);
                    ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "YYearearId", tblDraftContractExaminationReport.YearId);
                    return View(tblDraftContractExaminationReport);
                }               
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message}");
                ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDraftContractExaminationReport.MonthId);
                ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblDraftContractExaminationReport.SubmittedBy);
                ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDraftContractExaminationReport.YearId);
                return View(tblDraftContractExaminationReport);
            }
           
        }

        // GET: DraftContractExaminationReports/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblDraftContractExaminationReports == null)
            {
                return NotFound();
            }

            var tblDraftContractExaminationReport = await _context.TblDraftContractExaminationReports.FindAsync(id);
            if (tblDraftContractExaminationReport == null)
            {
                return NotFound();
            }
            ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDraftContractExaminationReport.MonthId);
            ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblDraftContractExaminationReport.SubmittedBy);
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDraftContractExaminationReport.YearId);
            return View(tblDraftContractExaminationReport);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,QuestionSubmitter,CaseType,FullMoneyAmmount,Investigation,YearId,MonthId,SubmittedBy")] TblDraftContractExaminationReport tblDraftContractExaminationReport)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (id != tblDraftContractExaminationReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    tblDraftContractExaminationReport.SubmittedBy = userId;
                    _context.Update(tblDraftContractExaminationReport);
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Report is successfully added");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Report isn't added. Please try again.");
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDraftContractExaminationReport.MonthId);
                        ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblDraftContractExaminationReport.SubmittedBy);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDraftContractExaminationReport.YearId);
                        return View(tblDraftContractExaminationReport);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TblDraftContractExaminationReportExists(tblDraftContractExaminationReport.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _notifyService.Error($"Error: {ex.Message}");
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDraftContractExaminationReport.MonthId);
                        ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblDraftContractExaminationReport.SubmittedBy);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDraftContractExaminationReport.YearId);
                        return View(tblDraftContractExaminationReport);
                    }
                }
            }
            else
            {
                ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDraftContractExaminationReport.MonthId);
                ViewData["SubmittedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblDraftContractExaminationReport.SubmittedBy);
                ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDraftContractExaminationReport.YearId);
                return View(tblDraftContractExaminationReport);
            }
           
        }

        // GET: DraftContractExaminationReports/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblDraftContractExaminationReports == null)
            {
                return NotFound();
            }

            var tblDraftContractExaminationReport = await _context.TblDraftContractExaminationReports
                .Include(t => t.Month)
                .Include(t => t.SubmittedByNavigation)
                .Include(t => t.Year)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblDraftContractExaminationReport == null)
            {
                return NotFound();
            }

            return View(tblDraftContractExaminationReport);
        }

        // POST: DraftContractExaminationReports/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblDraftContractExaminationReports == null)
            {
                return Problem("Entity set 'AtsdbContext.TblDraftContractExaminationReports'  is null.");
            }
            var tblDraftContractExaminationReport = await _context.TblDraftContractExaminationReports.FindAsync(id);
            if (tblDraftContractExaminationReport != null)
            {
                _context.TblDraftContractExaminationReports.Remove(tblDraftContractExaminationReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
        private bool TblDraftContractExaminationReportExists(Guid id)
        {
          return (_context.TblDraftContractExaminationReports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
