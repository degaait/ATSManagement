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
    public class AdractivitiesReportsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService _mail;
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        private readonly IHttpContextAccessor _contextAccessor;
        public AdractivitiesReportsController(AtsdbContext context, INotyfService notyfService, IMailService mailService, IHttpContextAccessor contextAccessor, ILogger<HomeController> logger)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mailService;
            _notifyService = notyfService;
            _logger = logger;
        }

        // GET: AdractivitiesReports
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var users=_context.TblInternalUsers.Where(s=>s.UserId == userId).FirstOrDefault();
            if (users.IsDeputy||users.IsDepartmentHead==true||users.IsSuperAdmin==true)
            {
                var atsdbContext = _context.TblAdractivitiesReports.Include(t => t.CreatedByNavigation).Include(t => t.Month).Include(t => t.Type).Include(t => t.Year);
                return View(await atsdbContext.ToListAsync());
            }
            else
            {
                var atsdbContext = _context.TblAdractivitiesReports.Include(t => t.CreatedByNavigation).Include(t => t.Month).Include(t => t.Type).Include(t => t.Year).Where(s=>s.CreatedBy==userId);
                return View(await atsdbContext.ToListAsync());
            }            
        }

        // GET: AdractivitiesReports/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblAdractivitiesReports == null)
            {
                return NotFound();
            }

            var tblAdractivitiesReport = await _context.TblAdractivitiesReports
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Month)
                .Include(t => t.Type)
                .Include(t => t.Year)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblAdractivitiesReport == null)
            {
                return NotFound();
            }

            return View(tblAdractivitiesReport);
        }

        // GET: AdractivitiesReports/Create
        public IActionResult Create()
        {
            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId");
            ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName");
            ViewData["TypeId"] = new SelectList(_context.TblAdreventTypes, "TypeId", "TypeNames");
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TypeId,Womens,Childrens,WomeElders,Hivpositives,Mens,OtherServantType,OutofResponsibilty,Family,Property,AsserinaSerategna,LelochGudayAyine,YediridirGenzebMeten,YeteWosenewuGenzebMeten,YearId,MonthId,CreatedBy")] TblAdractivitiesReport tblAdractivitiesReport)
        {
            try
            {
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var ceoEmail=(from items in _context.TblInternalUsers where items.Dep.DepCode== "CVA" && (items.IsDeputy==true||items.IsDepartmentHead==true) select items.EmailAddress).ToList();

                if (ModelState.IsValid)
                {
                    tblAdractivitiesReport.CreatedBy = userId;
                    _context.Add(tblAdractivitiesReport);
                  int saved=  await _context.SaveChangesAsync();
                    if (saved>0)
                    {
                        _notifyService.Success("Report created successfully");
                        await SendMail(ceoEmail, "Monthly Report from branch offices", "<h3>Monthly report iis added from branch offices.Please check on Task Tracking Dashboard</h3>");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Report isn't created successfully. Please trya again");
                        ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblAdractivitiesReport.CreatedBy);
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblAdractivitiesReport.MonthId);
                        ViewData["TypeId"] = new SelectList(_context.TblAdreventTypes, "TypeId", "TypeNames", tblAdractivitiesReport.TypeId);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblAdractivitiesReport.YearId);
                        return View(tblAdractivitiesReport);
                    }                   
                }
                else
                {
                    ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblAdractivitiesReport.CreatedBy);
                    ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblAdractivitiesReport.MonthId);
                    ViewData["TypeId"] = new SelectList(_context.TblAdreventTypes, "TypeId", "TypeNames", tblAdractivitiesReport.TypeId);
                    ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblAdractivitiesReport.YearId);
                    return View(tblAdractivitiesReport);
                }              
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message + " happened. Please try again.");
                ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblAdractivitiesReport.CreatedBy);
                ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblAdractivitiesReport.MonthId);
                ViewData["TypeId"] = new SelectList(_context.TblAdreventTypes, "TypeId", "TypeNames", tblAdractivitiesReport.TypeId);
                ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblAdractivitiesReport.YearId);
                return View(tblAdractivitiesReport);
            }

         
        }

        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblAdractivitiesReports == null)
            {
                return NotFound();
            }

            var tblAdractivitiesReport = await _context.TblAdractivitiesReports.FindAsync(id);
            if (tblAdractivitiesReport == null)
            {
                return NotFound();
            }

            ViewData["CreatedBy"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblAdractivitiesReport.CreatedBy);
            ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblAdractivitiesReport.MonthId);
            ViewData["TypeId"] = new SelectList(_context.TblAdreventTypes, "TypeId", "TypeNames", tblAdractivitiesReport.TypeId);
            ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblAdractivitiesReport.YearId);

            return View(tblAdractivitiesReport);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TypeId,Womens,Childrens,WomeElders,Hivpositives,Mens,OtherServantType,OutofResponsibilty,Family,Property,AsserinaSerategna,LelochGudayAyine,YediridirGenzebMeten,YeteWosenewuGenzebMeten,YearId,MonthId,CreatedBy")] TblAdractivitiesReport tblAdractivitiesReport)
        {
            if (id != tblAdractivitiesReport.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblAdractivitiesReport);
                  int updated=  await _context.SaveChangesAsync();
                    if (updated>0)
                    {
                        _notifyService.Success("Report is successfully updated.");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {

                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblAdractivitiesReport.MonthId);
                        ViewData["TypeId"] = new SelectList(_context.TblAdreventTypes, "TypeId", "TypeNames", tblAdractivitiesReport.TypeId);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblAdractivitiesReport.YearId);
                        return View(tblAdractivitiesReport);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TblAdractivitiesReportExists(tblAdractivitiesReport.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        _notifyService.Error(ex.Message+". Please try again");
                        ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblAdractivitiesReport.MonthId);
                        ViewData["TypeId"] = new SelectList(_context.TblAdreventTypes, "TypeId", "TypeNames", tblAdractivitiesReport.TypeId);
                        ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblAdractivitiesReport.YearId);
                        return View(tblAdractivitiesReport);
                    }
                }
            }
            else
            {
                _notifyService.Error("Please fill all neccessary fileds");
                ViewData["MonthId"] = new SelectList(_context.TblMonths, "MonthId", "MonthName", tblAdractivitiesReport.MonthId);
                ViewData["TypeId"] = new SelectList(_context.TblAdreventTypes, "TypeId", "TypeNames", tblAdractivitiesReport.TypeId);
                ViewData["YearId"] = new SelectList(_context.TblYears, "YearId", "Year", tblAdractivitiesReport.YearId);
                return View(tblAdractivitiesReport);
            }
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblAdractivitiesReports == null)
            {
                return NotFound();
            }

            var tblAdractivitiesReport = await _context.TblAdractivitiesReports
                .Include(t => t.CreatedByNavigation)
                .Include(t => t.Month)
                .Include(t => t.Type)
                .Include(t => t.Year)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tblAdractivitiesReport == null)
            {
                return NotFound();
            }

            return View(tblAdractivitiesReport);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblAdractivitiesReports == null)
            {
                return Problem("Entity set 'AtsdbContext.TblAdractivitiesReports'  is null.");
            }
            var tblAdractivitiesReport = await _context.TblAdractivitiesReports.FindAsync(id);
            if (tblAdractivitiesReport != null)
            {
                _context.TblAdractivitiesReports.Remove(tblAdractivitiesReport);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblAdractivitiesReportExists(Guid id)
        {
          return (_context.TblAdractivitiesReports?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }

    }
}
