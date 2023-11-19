using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class DebateWorkPerformanceReportsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        private readonly IHttpContextAccessor _contextAccessor;
        public DebateWorkPerformanceReportsController(AtsdbContext context, INotyfService notyfService, IMailService mailService, IHttpContextAccessor contextAccessor, ILogger<HomeController> logger)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
            _notifyService = notyfService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["DebateTypes"]=_context.TblDebatePerformances.ToList();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users = _context.TblInternalUsers.Where(s => s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy || users.IsDepartmentHead == true || users.IsSuperAdmin == true)
            {
                var atsdbContext = _context.TblDebateWorkPerformanceReports.Include(t => t.CreatedByNavigation).Include(t => t.IdNavigation).Include(t => t.Month).Include(t => t.Year);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                var atsdbContext = _context.TblDebateWorkPerformanceReports.Include(t => t.CreatedByNavigation).Include(t => t.IdNavigation).Include(t => t.Month).Include(t => t.Year).Where(s => s.CreatedBy == userId);
                return View(await atsdbContext.ToListAsync());
            }

        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblDebateWorkPerformanceReports == null)
            {
                return NotFound();
            }

            var tblDebateWorkPerformanceReport = await _context.TblDebateWorkPerformanceReports
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.IdNavigation)
                .Include(t => t.Month)
                .Include(t => t.Year)
                .FirstOrDefaultAsync(m => m.WorkPerformId == id);
            if (tblDebateWorkPerformanceReport == null)
            {
                return NotFound();
            }

            return View(tblDebateWorkPerformanceReport);
        }
        public IActionResult Create(Guid? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            _contextAccessor.HttpContext.Session.SetString("debateTypeId", id.ToString());
            ViewData["Id"] = new SelectList(_context.TblDebatePerformanceEventTypes, "Id", "IWorkPerformanceEventTyped");
            ViewData["subDebateTypes"]= new SelectList(_context.TblSubDebatePerformances.Where(x=>x.PerformanceId==id), "SubPerformanceId", "SubPerformanceName");
            ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName");
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubPerformanceId,WorkPerformId,Womens,Childrens,WomenElders,Hivpositives,Mens,OtherServants,TotalServant,OutofContact,Family,Property,WorkDebate,OtherCaseTypes,JudgementMoneyAmmount,JudgementVerifiedAmmount,Id,YearId,MonthId,CreatedBy")] TblDebateWorkPerformanceReport tblDebateWorkPerformanceReport)
        {
            Guid id = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("debateTypeId"));
            if (tblDebateWorkPerformanceReport.Id==Guid.Empty)
            {
                _notifyService.Information("ክንውን አልተመረጠም። እባኮት ክንውን መርጠው እንደገና ይሞክሩ");
                ViewData["subDebateTypes"] = new SelectList(_context.TblSubDebatePerformances.Where(x => x.PerformanceId == id), "SubPerformanceId", "SubPerformanceName");
                ViewData["Id"] = new SelectList(_context.TblDebatePerformanceEventTypes, "Id", "WorkPerformanceEventType", tblDebateWorkPerformanceReport.Id);
                ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDebateWorkPerformanceReport.MonthId);
                ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDebateWorkPerformanceReport.YearId);
                return View(tblDebateWorkPerformanceReport);
            }
            try
            {
               
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var ceoEmail = (from items in _context.TblInternalUsers where items.Dep.DepCode == "CVA" && (items.IsDeputy == true || items.IsDepartmentHead == true) select items.EmailAddress).ToList();
                var isExits = _context.TblDebateWorkPerformanceReports.Where(s => s.Month == tblDebateWorkPerformanceReport.Month && s.Year == tblDebateWorkPerformanceReport.Year && s.Id == tblDebateWorkPerformanceReport.Id).FirstOrDefault();
                if (isExits != null)
                {
                    _notifyService.Information("This report already exists. Please atleast change one of these fields(Year, Month, Servant type)");
                    ViewData["subDebateTypes"] = new SelectList(_context.TblSubDebatePerformances.Where(x => x.PerformanceId == id), "SubPerformanceId", "SubPerformanceName");
                    ViewData["Id"] = new SelectList(_context.TblDebatePerformanceEventTypes, "Id", "WorkPerformanceEventType", tblDebateWorkPerformanceReport.Id);
                    ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDebateWorkPerformanceReport.MonthId);
                    ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDebateWorkPerformanceReport.YearId);
                    return View(tblDebateWorkPerformanceReport);
                }
                if (ModelState.IsValid)
                {
                    tblDebateWorkPerformanceReport.WorkPerformId = Guid.NewGuid();
                    tblDebateWorkPerformanceReport.CreatedBy = userId;
                    _context.Add(tblDebateWorkPerformanceReport);
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
                        ViewData["subDebateTypes"] = new SelectList(_context.TblSubDebatePerformances.Where(x => x.PerformanceId == id), "SubPerformanceId", "SubPerformanceName");
                        ViewData["Id"] = new SelectList(_context.TblDebatePerformanceEventTypes, "Id", "WorkPerformanceEventType", tblDebateWorkPerformanceReport.Id);
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDebateWorkPerformanceReport.MonthId);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDebateWorkPerformanceReport.YearId);
                        return View(tblDebateWorkPerformanceReport);
                    }
                }
                else
                {
                    ViewData["subDebateTypes"] = new SelectList(_context.TblSubDebatePerformances.Where(x => x.PerformanceId == id), "SubPerformanceId", "SubPerformanceName");
                    ViewData["Id"] = new SelectList(_context.TblDebatePerformanceEventTypes, "Id", "IWorkPerformanceEventTyped", tblDebateWorkPerformanceReport.Id);
                    ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDebateWorkPerformanceReport.MonthId);
                    ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDebateWorkPerformanceReport.YearId);
                    return View(tblDebateWorkPerformanceReport);
                }     
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message}");
                ViewData["subDebateTypes"] = new SelectList(_context.TblSubDebatePerformances.Where(x => x.PerformanceId == id), "SubPerformanceId", "SubPerformanceName");
                ViewData["Id"] = new SelectList(_context.TblDebatePerformanceEventTypes, "Id", "WorkPerformanceEventType", tblDebateWorkPerformanceReport.Id);
                ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDebateWorkPerformanceReport.MonthId);
                ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDebateWorkPerformanceReport.YearId);
                return View(tblDebateWorkPerformanceReport);
            }
          
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblDebateWorkPerformanceReports == null)
            {
                return NotFound();
            }

            var tblDebateWorkPerformanceReport = await _context.TblDebateWorkPerformanceReports.FindAsync(id);
            if (tblDebateWorkPerformanceReport == null)
            {
                return NotFound();
            }
            var eventType=_context.TblDebatePerformanceEventTypes.Where(x=>x.Id== tblDebateWorkPerformanceReport.Id).FirstOrDefault();           
            ViewData["Id"] = new SelectList(_context.TblDebatePerformanceEventTypes.Where(s=>s.SubPerformanceId== eventType.SubPerformanceId), "Id", "WorkPerformanceEventType", tblDebateWorkPerformanceReport.Id);
            ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDebateWorkPerformanceReport.MonthId);
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDebateWorkPerformanceReport.YearId);
            return View(tblDebateWorkPerformanceReport);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("WorkPerformId,Womens,Childrens,WomenElders,Hivpositives,Mens,OtherServants,TotalServant,OutofContact,Family,Property,WorkDebate,OtherCaseTypes,JudgementMoneyAmmount,JudgementVerifiedAmmount,Id,YearId,MonthId,CreatedBy")] TblDebateWorkPerformanceReport tblDebateWorkPerformanceReport)
        {
           
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblDebateWorkPerformanceReport);
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Report is successfully uppdated");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Report isn't edited. Please try again.");
                        ViewData["Id"] = new SelectList(_context.TblDebatePerformanceEventTypes, "Id", "WorkPerformanceEventType", tblDebateWorkPerformanceReport.Id);
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDebateWorkPerformanceReport.MonthId);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDebateWorkPerformanceReport.YearId);
                        return View(tblDebateWorkPerformanceReport);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TblDebateWorkPerformanceReportExists(tblDebateWorkPerformanceReport.WorkPerformId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _notifyService.Error($"Error: {ex.Message}");
                        ViewData["Id"] = new SelectList(_context.TblDebatePerformanceEventTypes, "Id", "WorkPerformanceEventType", tblDebateWorkPerformanceReport.Id);
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDebateWorkPerformanceReport.MonthId);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDebateWorkPerformanceReport.YearId);
                        return View(tblDebateWorkPerformanceReport);
                    }
                }
            }
            else
            {
                _notifyService.Error("Report isn't submitted successfully. Please fill all neccessary fields");
                ViewData["Id"] = new SelectList(_context.TblDebatePerformanceEventTypes, "Id", "WorkPerformanceEventType", tblDebateWorkPerformanceReport.Id);
                ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblDebateWorkPerformanceReport.MonthId);
                ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblDebateWorkPerformanceReport.YearId);
                return View(tblDebateWorkPerformanceReport);
            }
           
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblDebateWorkPerformanceReports == null)
            {
                return NotFound();
            }

            var tblDebateWorkPerformanceReport = await _context.TblDebateWorkPerformanceReports
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.IdNavigation)
                .Include(t => t.Month)
                .Include(t => t.Year)
                .FirstOrDefaultAsync(m => m.WorkPerformId == id);
            if (tblDebateWorkPerformanceReport == null)
            {
                return NotFound();
            }

            return View(tblDebateWorkPerformanceReport);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblDebateWorkPerformanceReports == null)
            {
                return Problem("Entity set 'AtsdbContext.TblDebateWorkPerformanceReports'  is null.");
            }
            var tblDebateWorkPerformanceReport = await _context.TblDebateWorkPerformanceReports.FindAsync(id);
            if (tblDebateWorkPerformanceReport != null)
            {
                _context.TblDebateWorkPerformanceReports.Remove(tblDebateWorkPerformanceReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblDebateWorkPerformanceReportExists(Guid id)
        {
          return (_context.TblDebateWorkPerformanceReports?.Any(e => e.WorkPerformId == id)).GetValueOrDefault();
        }
        public JsonResult GetEventTypes(Guid? SubPerformanceId)
        {
            List<TblDebatePerformanceEventType> subcategoryModels = new List<TblDebatePerformanceEventType>();
            subcategoryModels = (from items in _context.TblDebatePerformanceEventTypes where items.SubPerformanceId == SubPerformanceId select items).ToList();
            return Json(new SelectList(subcategoryModels, "Id", "WorkPerformanceEventType"));
        }
    }
}
