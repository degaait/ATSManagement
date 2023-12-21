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
    public class TeamsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        private readonly IMailService _mail;
        public TeamsController(AtsdbContext context,INotyfService notyfService,IMailService mailService)
        {
            _notifyService = notyfService;
            _mail= mailService;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var atsdbContext = _context.TblTeams.Include(t => t.Dep).Include(t => t.TeamLeader);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblTeams == null)
            {
                return NotFound();
            }

            var tblTeam = await _context.TblTeams
                .Include(t => t.Dep)
                .Include(t => t.TeamLeader)
                .FirstOrDefaultAsync(m => m.TeamId == id);
            if (tblTeam == null)
            {
                return NotFound();
            }

            return View(tblTeam);
        }
        public IActionResult Create()
        {
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName");
           
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamId,TeamName,DepId,TeamLeaderId,TeamDescription,TeamNameAmharic")] TblTeam tblTeam)
        {
            if (ModelState.IsValid)
            {
                tblTeam.TeamId = Guid.NewGuid();
                _context.Add(tblTeam);
               int saved= await _context.SaveChangesAsync();
                if (saved>0)
                {
                    _notifyService.Success("Team successfully created");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _notifyService.Error("Team creation isn't successfull. Please try again");
                    ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepName", "DepId", tblTeam.DepId);
                    return View(tblTeam);
                }              
            }
            else
            {
                _notifyService.Error("Model isn't valid. Please try again");
                ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepName", "DepId", tblTeam.DepId);
                return View(tblTeam);
            }
           
        }
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblTeams == null)
            {
                return NotFound();
            }

            var tblTeam = await _context.TblTeams.FindAsync(id);
            if (tblTeam == null)
            {
                return NotFound();
            }
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepName", tblTeam.DepId);
            return View(tblTeam);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TeamId,TeamName,DepId,TeamLeaderId,TeamDescription,TeamNameAmharic")] TblTeam tblTeam)
        {
            if (id != tblTeam.TeamId)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblTeam);
                  int updated=  await _context.SaveChangesAsync();
                    if (updated>0)
                    {
                        _notifyService.Success("Successfully Updated");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepId", tblTeam.DepId);
                        ViewData["TeamLeaderId"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblTeam.TeamLeaderId);
                        return View(tblTeam);
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!TblTeamExists(tblTeam.TeamId))
                    {
                        return NotFound();
                    }
                    else
                    {
                       _notifyService.Error(ex.Message+" happened. Please try again");
                        ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepId", tblTeam.DepId);
                        ViewData["TeamLeaderId"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblTeam.TeamLeaderId);
                        return View(tblTeam);
                    }
                }              
            }
            _notifyService.Error("Model isn't valid. Please fill all neccessary fields and try again.");
            ViewData["DepId"] = new SelectList(_context.TblDepartments, "DepId", "DepId", tblTeam.DepId);
            ViewData["TeamLeaderId"] = new SelectList(_context.TblInternalUsers, "UserId", "UserId", tblTeam.TeamLeaderId);
            return View(tblTeam);
        }
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblTeams == null)
            {
                return NotFound();
            }

            var tblTeam = await _context.TblTeams
                .Include(t => t.Dep)
                .Include(t => t.TeamLeader)
                .FirstOrDefaultAsync(m => m.TeamId == id);
            if (tblTeam == null)
            {
                return NotFound();
            }

            return View(tblTeam);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblTeams == null)
            {
                return Problem("Entity set 'AtsdbContext.TblTeams'  is null.");
            }
            var tblTeam = await _context.TblTeams.FindAsync(id);
            if (tblTeam != null)
            {
                _context.TblTeams.Remove(tblTeam);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool TblTeamExists(Guid id)
        {
          return (_context.TblTeams?.Any(e => e.TeamId == id)).GetValueOrDefault();
        }
        public JsonResult GetSubCategory(Guid DepId)
        {
            List<TblTeam> subcategoryModels = new List<TblTeam>();
            subcategoryModels = (from items in _context.TblTeams where items.DepId == DepId select items).ToList();
            return Json(new SelectList(subcategoryModels, "TeamID", "TeamName"));
        }
    }
}
