using System;
using System.Linq;
using ATSManagement.Models;
using ATSManagement.Filters;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class ContactRequestsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly INotyfService _notifyService;
        public ContactRequestsController(AtsdbContext context, INotyfService notifyService)
        {
            _context = context;
            _notifyService = notifyService;
        }

        // GET: ContactRequests
        public async Task<IActionResult> Index()
        {
              return _context.TblContactInformations != null ? 
                          View(await _context.TblContactInformations.ToListAsync()) :
                          Problem("Entity set 'AtsdbContext.TblContactInformations'  is null.");
        }

        // GET: ContactRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblContactInformations == null)
            {
                return NotFound();
            }

            var tblContactInformation = await _context.TblContactInformations
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (tblContactInformation == null)
            {
                return NotFound();
            }

            return View(tblContactInformation);
        }

        // GET: ContactRequests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ContactRequests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactId,ContactDetaiMessage,ContactEmail,ContactPhoneNumber,ContactCountry")] TblContactInformation tblContactInformation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblContactInformation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblContactInformation);
        }

        // GET: ContactRequests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || _context.TblContactInformations == null)
            {
                return NotFound();
            }

            var tblContactInformation = await _context.TblContactInformations.FindAsync(id);
            if (tblContactInformation == null)
            {
                return NotFound();
            }
            return View(tblContactInformation);
        }

        // POST: ContactRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContactId,ContactDetaiMessage,ContactEmail,ContactPhoneNumber,ContactCountry")] TblContactInformation tblContactInformation)
        {
            if (id != tblContactInformation.ContactId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblContactInformation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblContactInformationExists(tblContactInformation.ContactId))
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
            return View(tblContactInformation);
        }

        // GET: ContactRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblContactInformations == null)
            {
                return NotFound();
            }

            var tblContactInformation = await _context.TblContactInformations
                .FirstOrDefaultAsync(m => m.ContactId == id);
            if (tblContactInformation == null)
            {
                return NotFound();
            }

            return View(tblContactInformation);
        }

        // POST: ContactRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblContactInformations == null)
            {
                return Problem("Entity set 'AtsdbContext.TblContactInformations'  is null.");
            }            var tblContactInformation = await _context.TblContactInformations.FindAsync(id);
            if (tblContactInformation != null)
            {
                _context.TblContactInformations.Remove(tblContactInformation);
            }            
            int deleted=await _context.SaveChangesAsync();
            if (deleted>0)
            {
                _notifyService.Success("Contact request successfully deleted!");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Delete), new {id=tblContactInformation.ContactId});
            }           
        }
        private bool TblContactInformationExists(int? id)
        {
          return (_context.TblContactInformations?.Any(e => e.ContactId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> DownloadFile(string path)
        {
            string filename = path.Substring(7);
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "admin\\", filename);
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filepath, out var contenttype))
            {
                contenttype = "application/octet-stream";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, Path.GetFileName(filepath));
        }
    }
}
