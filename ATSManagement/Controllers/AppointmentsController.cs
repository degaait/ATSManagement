using NToastNotify;
using ATSManagement.Models;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    public class AppointmentsController : Controller
    {
        private readonly AtsdbContext _context;
        //private readonly IToastNotification _toastNotification;
        private readonly ILogger _logger;
        private readonly INotyfService _notifyService;
        public AppointmentsController(AtsdbContext context, IToastNotification toastNotification, INotyfService notyfService)
        {
           
            _notifyService = notyfService;
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblAppointments.Include(t => t.Inist).Include(t => t.RequestedByNavigation);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblAppointments == null)
            {
                return NotFound();
            }

            var tblAppointment = await _context.TblAppointments
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (tblAppointment == null)
            {
                return NotFound();
            }

            return View(tblAppointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId");
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId");
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AppointmentId,AppointmentDetail,InistId,RequestedBy,DescusionFinalComeup,CreatedDate,AppointmentDate")] TblAppointment tblAppointment)
        {
            if (ModelState.IsValid)
            {
                tblAppointment.AppointmentId = Guid.NewGuid();
                _context.Add(tblAppointment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblAppointment.InistId);
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblAppointment.RequestedBy);
            return View(tblAppointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblAppointments == null)
            {
                return NotFound();
            }

            var tblAppointment = await _context.TblAppointments.FindAsync(id);
            if (tblAppointment == null)
            {
                return NotFound();
            }
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblAppointment.InistId);
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblAppointment.RequestedBy);
            return View(tblAppointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("AppointmentId,AppointmentDetail,InistId,RequestedBy,DescusionFinalComeup,CreatedDate,AppointmentDate")] TblAppointment tblAppointment)
        {
            if (id != tblAppointment.AppointmentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblAppointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblAppointmentExists(tblAppointment.AppointmentId))
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
            ViewData["InistId"] = new SelectList(_context.TblInistitutions, "InistId", "InistId", tblAppointment.InistId);
            ViewData["RequestedBy"] = new SelectList(_context.TblExternalUsers, "ExterUserId", "ExterUserId", tblAppointment.RequestedBy);
            return View(tblAppointment);
        }

        // GET: Appointments/Delete/5

        public async Task<IActionResult> AssignParticipants(Guid? id)
        {
            AppointmentModel app = new AppointmentModel();
            var appointment = _context.TblAppointments.FirstOrDefault(x => x.AppointmentId == id);
            app.Users = _context.TblInternalUsers.Select(a => new SelectListItem
            {
                Text = a.FirstName + " " + a.MidleName,
                Value = a.UserId.ToString()
            }).ToList();
            
            app.AppointmentID = id;
            app.AppointmentDetail = appointment.AppointmentDetail;
            app.Institution = _context.TblInistitutions.Where(a => a.InistId == appointment.InistId).Select(s => s.Name).FirstOrDefault();
           
            return View(app);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignParticipants(AppointmentModel model)
        {
            List<TblAppointmentParticipant> tblAppointmentParticipants;
            TblAppointment tblAppointment = _context.TblAppointments.Find(model.AppointmentID);
            if (tblAppointment == null)
            {
                return NotFound();
            }
            tblAppointment.AppointmentDate = model.AppointmentDate;
            if (model.UserId.Length > 0)
            {
                tblAppointmentParticipants = new List<TblAppointmentParticipant>();
                foreach (var item in model.UserId)
                {
                    tblAppointmentParticipants.Add(new TblAppointmentParticipant { UserId = item, AppointmentId = model.AppointmentID });
                }
                tblAppointment.TblAppointmentParticipants = tblAppointmentParticipants;
            }
            tblAppointment.AppointmentDate=model.AppointmentDate;
            int updated = await _context.SaveChangesAsync();
            if (updated>0)
            {
                _notifyService.Success("Appointment Participants are assigned succeesfully. Email notification is will be sent to respective institution focal person.");
                return RedirectToAction(nameof(Index));

            }
            else
            {

            }

            return View();
        }

        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblAppointments == null)
            {
                return NotFound();
            }

            var tblAppointment = await _context.TblAppointments
                .Include(t => t.Inist)
                .Include(t => t.RequestedByNavigation)
                .FirstOrDefaultAsync(m => m.AppointmentId == id);
            if (tblAppointment == null)
            {
                return NotFound();
            }

            return View(tblAppointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblAppointments == null)
            {
                return Problem("Entity set 'AtsdbContext.TblAppointments'  is null.");
            }
            var tblAppointment = await _context.TblAppointments.FindAsync(id);
            if (tblAppointment != null)
            {
                _context.TblAppointments.Remove(tblAppointment);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblAppointmentExists(Guid id)
        {
            return (_context.TblAppointments?.Any(e => e.AppointmentId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> AddFinalOutCome(Guid? id)
        {
            AppointmentModel app = new AppointmentModel();
            TblAppointment tblAppointment = await _context.TblAppointments.FindAsync(id);
            app.AppointmentDetail = tblAppointment.AppointmentDetail;
            app.CreatedDate=tblAppointment.CreatedDate;
            app.AppointmentDate=tblAppointment?.AppointmentDate;
            return View(app);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddFinalOutCome(AppointmentModel model)
        {
            if (model.AppointmentDate>DateTime.Now)
            {
                _notifyService.Error("Final meeting outcome can't be added before appointment date");
                return View(model);
            }
            TblAppointment tblAppointment = await _context.TblAppointments.FindAsync(model.AppointmentID);
            tblAppointment.DescusionFinalComeup = model.FinalOutCome;
            int saved=await _context.SaveChangesAsync();
            if (saved>0)
            {
                _notifyService.Success("Successfully saved.");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notifyService.Error("Data isn't added successfully. Please try again");
                return View(model);
            }            
           
        }

    }
}
