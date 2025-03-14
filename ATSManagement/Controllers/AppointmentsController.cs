﻿using ATSManagement.Models;
using ATSManagement.Filters;
using ATSManagement.IModels;
using ATSManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class AppointmentsController : Controller
    {
        private readonly AtsdbContext _context;
        //private readonly IToastNotification _toastNotification;
        // private readonly ILogger _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly INotyfService _notifyService;
        private readonly IMailService _mail;
        public AppointmentsController(AtsdbContext context, INotyfService notyfService, IMailService mail, IHttpContextAccessor contextAccessor)
        {
            _notifyService = notyfService;
            _context = context;
            _mail = mail;
            _contextAccessor = contextAccessor;
        }

        // GET: Appointments
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblAppointments.Include(t => t.Inist).Include(t => t.RequestedByNavigation).OrderBy(s => s.CreatedDate);
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
            app.Users = _context.TblInternalUsers.Where(s => s.IsDepartmentHead == true).Select(a => new SelectListItem
            {
                Text = a.FirstName + " " + a.MidleName,
                Value = a.UserId.ToString()
            }).ToList();
            app.CreatedDate = appointment.CreatedDate;
            app.AppointmentDate = appointment.AppointmentDate;
            app.AppointmentID = id;
            app.Remark = appointment.Remark;
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
            var externalUser = _context.TblExternalUsers.Where(x => x.InistId == tblAppointment.InistId).Select(x => x.Email).ToList();
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
            tblAppointment.AppointmentDate = model.AppointmentDate;
            int updated = await _context.SaveChangesAsync();
            if (updated > 0)
            {
                _notifyService.Success("Appointment Participants are assigned succeesfully. Email notification is will be sent to respective institution focal person.");
                await SendMail(externalUser, "Appointment request from " + tblAppointment.Inist.Name, "<h3>Please review the detail on Task Tracking Dashboard and reply</h3>");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notifyService.Error("Appointment isn't successfull.Please try again.");
                return View(model);
            }

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
            app.CreatedDate = tblAppointment.CreatedDate;
            app.AppointmentID = tblAppointment.AppointmentId;
            app.AppointmentDate = tblAppointment?.AppointmentDate;
            return View(app);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddFinalOutCome(AppointmentModel model)
        {
            if (model.AppointmentDate > DateTime.Now)
            {
                _notifyService.Error("Final meeting outcome can't be added before appointment date");
                return View(model);
            }
            TblAppointment tblAppointment = await _context.TblAppointments.FindAsync(model.AppointmentID);
            tblAppointment.DescusionFinalComeup = model.FinalOutCome;
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
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
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }

        public async Task<IActionResult> ReplyBack(Guid? id)
        {
            AppointmentModel app = new AppointmentModel();
            TblAppointment tblAppointment = await _context.TblAppointments.FindAsync(id);
            app.AppointmentDetail = tblAppointment.AppointmentDetail;
            app.CreatedDate = tblAppointment.CreatedDate;
            app.AppointmentID = tblAppointment.AppointmentId;
            app.AppointmentDate = tblAppointment?.AppointmentDate;
            app.AllowedAppointDate=DateTime.Now.AddDays(2);
            app.Remark=tblAppointment?.Remark;
            return View(app);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> ReplyBack(AppointmentModel model)
        {

            if (model.AllowedAppointDate<DateTime.Now)
            {
                _notifyService.Error("Approved appointment date shouldn't be past date");
                return View(model);
            }

            TblAppointment tblAppointment = await _context.TblAppointments.FindAsync(model.AppointmentID);
            var externalUser = _context.TblExternalUsers.Include(x => x.Inist).Where(x => x.InistId == tblAppointment.InistId).Select(x => x.Email).ToList();
            var instname = _context.TblInistitutions.Where(x => x.InistId == tblAppointment.InistId).Select(x => x.Name).FirstOrDefault();
            tblAppointment.Remark = model.Remark;
            tblAppointment.AllowedAppointDate=model.AllowedAppointDate;
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Successfully saved.");
                await SendMail(externalUser, "Appointment Reply" + instname, "<h3>Please review the detail on Task Tracking Dashboard</h3>");

                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notifyService.Error("Data isn't added successfully. Please try again");
                return View(model);
            }

        }

        public async Task<IActionResult> Adjournments()
        {
            ViewData["Adjornies"] = _context.TblAdjornments.Include(X => X.Request).ToList();
            return View();
        }

        public async Task<IActionResult> AppointmentChats(Guid? id, string actionMethod, string controller)
        {
            AppointmentChatModel model = new AppointmentChatModel();
            var appointment=_context.TblAppointments.Find(id);
            model.AppointmentId = id;
            model.RequestedBy = appointment.RequestedBy;
            ViewBag.ActionMethod = actionMethod;
            ViewBag.Controller = controller;
            _contextAccessor.HttpContext.Session.SetString("actionMethod", actionMethod);
            _contextAccessor.HttpContext.Session.SetString("controller", controller);
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AppointmentChats(AppointmentChatModel chatModel)
        {
            try
            {
                string actionMethod = _contextAccessor.HttpContext.Session.GetString("actionMethod");
                string controller = _contextAccessor.HttpContext.Session.GetString("controller");

                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                TblAppointmentChat chat = new TblAppointmentChat();
                chat.AppointmentId = chatModel.AppointmentId;
                chat.ChatContent = chatModel.ChatContent;
                chat.Datetime=DateTime.Now;
                chat.UserId = userId;
                chat.ExterUserId=chatModel.RequestedBy;
                chat.IsInternal = true;
                chat.IsEnternal = false;
                _context.TblAppointmentChats.Add(chat);
                int saved=await _context.SaveChangesAsync();
                if (saved>0)
                {
                    _notifyService.Success("Sent");
                    return RedirectToAction(nameof(AppointmentChats), new {id=chatModel.AppointmentId, actionMethod = actionMethod, controller = controller });
                }
                else
                {
                    _notifyService.Error("Not sent. Please try again");
                    return View(chatModel);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message+" happened. Please try again");
                return View(chatModel);
            }
        }

        public async Task<IActionResult> AdjournementChats(Guid AdjoryId, string actionMethod, string controller)
        {
            AdjornmentChatModel model = new AdjornmentChatModel();
            var appointment = _context.TblAdjornments.Find(AdjoryId);
            model.AdjoryId = AdjoryId;  
            ViewBag.ActionMethod = actionMethod;
            ViewBag.Controller = controller;
            _contextAccessor.HttpContext.Session.SetString("actionMethod", actionMethod);
            _contextAccessor.HttpContext.Session.SetString("controller", controller);
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdjournementChats(AdjornmentChatModel chatModel)
        {
            try
            {
               string actionMethod=  _contextAccessor.HttpContext.Session.GetString("actionMethod");
               string controller= _contextAccessor.HttpContext.Session.GetString("controller");
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
               var user=_context.TblInternalUsers.Find(userId);
                TblAdjournmentChat chat = new TblAdjournmentChat();
                chat.AdjoryId = chatModel.AdjoryId;
                chat.ChatContent = chatModel.ChatContent;
                chat.Datetime = DateTime.Now;
                chat.UserId = userId;
                if (user.IsDepartmentHead==true)
                {
                    chat.IsDephead = true;
                    chat.IsExpert = false;
                }
                else
                {
                    chat.IsDephead = false;
                    chat.IsExpert = true;
                }
                chat.UserId = userId;
                _context.TblAdjournmentChats.Add(chat);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Sent");
                    return RedirectToAction(nameof(AdjournementChats), new { AdjoryId = chatModel.AdjoryId, actionMethod= actionMethod,controller=controller });
                }
                else
                {
                    _notifyService.Error("Not sent. Please try again");
                    return View(chatModel);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error(ex.Message + " happened. Please try again");
                return View(chatModel);
            }
        }
    }
}
