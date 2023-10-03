using Microsoft.AspNetCore.Mvc;
using ATSManagementExternal.Models;
using ATSManagementExternal.IModels;
using Microsoft.EntityFrameworkCore;
using ATSManagementExternal.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ATSManagementExternal.Controllers
{
    public class ExternalRequestsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        public ExternalRequestsController(AtsdbContext context, IHttpContextAccessor contextAccessor, IMailService mail)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            _mail = mail;
        }

        // GET: ExternalRequests
        public async Task<IActionResult> Index()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblExternalRequests.Include(t => t.ExterUser).Include(t => t.Int).Include(s => s.ExternalRequestStatus).Where(x => x.DepId == null);
            return View(await atsdbContext.ToListAsync());
        }


        public async Task<IActionResult> AssignedRequests()
        {

            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblExternalRequests.Include(t => t.ExterUser).Include(t => t.Int).Include(s => s.ExternalRequestStatus).Include(x => x.DepId);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> InpectionRequests()
        {

            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblExternalRequests.Include(t => t.ExterUser).Include(t => t.Int).Include(s => s.ExternalRequestStatus).Where(x => x.DepId == null);
            return View(await atsdbContext.ToListAsync());
        }
        public async Task<IActionResult> LegalStudies()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;

            var atsdbContext = _context.TblLegalStudiesDraftings
               .Include(t => t.AssignedByNavigation)
               .Include(t => t.AssignedToNavigation)
               .Include(t => t.CaseType)
               .Include(t => t.Dep)
               .Include(t => t.Inist)
               .Include(t => t.RequestedByNavigation)
               .Include(x => x.ExternalRequestStatus)
               .Include(t => t.Priority).Where(x => x.Dep.DepCode == "CVA" && x.Inist.Name == instName.Inist.Name);
            return View(await atsdbContext.ToListAsync());


        }
        public async Task<IActionResult> CivilJustice()
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            var atsdbContext = _context.TblCivilJustices
                                                        .Include(t => t.AssignedByNavigation)
                                                        .Include(t => t.AssignedToNavigation)
                                                        .Include(t => t.CaseType)
                                                        .Include(t => t.Dep)
                                                        .Include(t => t.Inist)
                                                        .Include(t => t.RequestedByNavigation)
                                                        .Include(t => t.CreatedByNavigation)
                                                        .Include(x => x.ExternalRequestStatus)
                                                        .Include(t => t.Priority).Where(x => x.Dep.DepCode == "CVA" && x.Inist.Name == instName.Inist.Name);
            return View(await atsdbContext.ToListAsync());
        }

        // GET: ExternalRequests/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || _context.TblExternalRequests == null)
            {
                return NotFound();
            }
            var tblExternalRequest = await _context.TblExternalRequests
                .Include(t => t.ExterUser)
                .Include(t => t.Int)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblExternalRequest == null)
            {
                return NotFound();
            }
            return View(tblExternalRequest);
        }

        // GET: ExternalRequests/Create
        public IActionResult Create()
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var instName = _context.TblExternalUsers.FindAsync(userId).Result;
            model.RequestedDate = DateTime.Now;
            model.ExterUserId = userId;
            model.IntId = instName.InistId;
            model.Deparments = _context.TblDepartments.Select(a => new SelectListItem
            {
                Text=a.DepName,
                Value=a.DepId.ToString()

            }).ToList();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CivilJusticeExternalRequestModel model)
        {

            try
            {
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));


                TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "New" select items).FirstOrDefault();
                Guid statusiD = (from id in _context.TblExternalRequestStatuses where id.StatusName == "New" select id.ExternalRequestStatusId).FirstOrDefault();

                TblExternalRequest requests = new TblExternalRequest();
                TblCivilJustice tblCivilJustice= new TblCivilJustice();
                TblLegalStudiesDrafting drafting= new TblLegalStudiesDrafting();
                TblDepartment department = _context.TblDepartments.FindAsync(model.DepId).Result;
                if (department.DepCode== "CVA")
                {
                    tblCivilJustice.DepId = model.DepId;
                    tblCivilJustice.RequestDetail = model.RequestDetail;
                    tblCivilJustice.InistId = model.IntId;
                    tblCivilJustice.IsUpprovedByUser = false;
                    tblCivilJustice.IsUprovedbyDepartment=false;
                    tblCivilJustice.IsUprovedByDeputy=false;
                    tblCivilJustice.CreatedDate = DateTime.Now;
                    tblCivilJustice.IsUprovedByTeam=false;
                    tblCivilJustice.RequestedBy = userId;
                    tblCivilJustice.CreatedBy = userId;
                    tblCivilJustice.ExternalRequestStatusId = statusiD;
                    _context.TblCivilJustices.Add(tblCivilJustice);

                }
                else if (department.DepCode == "LSDC")
                {
                    drafting.DepId = model.DepId;
                    drafting.CreatedDate = DateTime.Now;
                    drafting.RequestedBy = userId;
                    drafting.RequestDetail = model.RequestDetail;
                    drafting.DepId= model.DepId;
                    drafting.CreatedBy = userId;
                    drafting.RequestedBy= userId;
                    drafting.InistId = model.IntId;
                    drafting.ExternalRequestStatusId= statusiD;
                    tblCivilJustice.IsUpprovedByUser = false;
                    tblCivilJustice.IsUprovedbyDepartment = false;
                    tblCivilJustice.IsUprovedByDeputy = false;
                    tblCivilJustice.IsUprovedByTeam = false;
                    _context.TblCivilJustices.Add(tblCivilJustice);


                }
                else if (department.DepCode == "FLIM")
                {

                }

                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                return View(model);
            }

        }

        // GET: ExternalRequests/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            if (id == null || _context.TblExternalRequests == null)
            {
                return NotFound();
            }

            var tblExternalRequest = await _context.TblExternalRequests.FindAsync(id);
            if (tblExternalRequest == null)
            {
                return NotFound();
            }
            model.ExterUserId = tblExternalRequest.ExterUserId;
            model.IntId = tblExternalRequest.IntId;
            model.RequestedDate = tblExternalRequest.RequestedDate;
            model.RequestId = tblExternalRequest.RequestId;
            model.RequestDetail = tblExternalRequest.RequestDetail;


            return View(model);
        }

        // POST: ExternalRequests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CivilJusticeExternalRequestModel model)
        {
            try
            {
                TblExternalRequest tblExternalRequest = await _context.TblExternalRequests.FindAsync(model.RequestId);
                if (tblExternalRequest == null)
                {
                    return NotFound();
                }
                tblExternalRequest.RequestDetail = model.RequestDetail;

                int updated = await _context.SaveChangesAsync();
                if (updated > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return View(model);
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TblExternalRequestExists(model.RequestId))
                {
                    return NotFound();
                }
                else
                {
                    return View(model);
                }
            }
        }

        // GET: ExternalRequests/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || _context.TblExternalRequests == null)
            {
                return NotFound();
            }

            var tblExternalRequest = await _context.TblExternalRequests
                .Include(t => t.ExterUser)
                .Include(t => t.Int)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (tblExternalRequest == null)
            {
                return NotFound();
            }

            return View(tblExternalRequest);
        }

        // POST: ExternalRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (_context.TblExternalRequests == null)
            {
                return Problem("Entity set 'AtsdbContext.TblExternalRequests'  is null.");
            }
            var tblExternalRequest = await _context.TblExternalRequests.FindAsync(id);
            if (tblExternalRequest != null)
            {
                _context.TblExternalRequests.Remove(tblExternalRequest);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblExternalRequestExists(Guid id)
        {
            return (_context.TblExternalRequests?.Any(e => e.RequestId == id)).GetValueOrDefault();
        }
    }
}
