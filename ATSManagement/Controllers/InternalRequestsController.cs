using ATSManagement.Models;
using ATSManagement.IModels;
using ATSManagement.Filters;
using Microsoft.AspNetCore.Mvc;
using ATSManagement.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace ATSManagement.Controllers
{
    [CheckSessionIsAvailable]
    public class InternalRequestsController : Controller
    {
        private readonly AtsdbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMailService _mail;
        private readonly INotyfService _notifyService;
        private readonly INotificationService _notificationService;
        public InternalRequestsController(AtsdbContext atsdbContext,IMailService mailService, IHttpContextAccessor httpContext
           , INotyfService notyfService, INotificationService notification) 
        {
            _context = atsdbContext;
            _contextAccessor = httpContext;
            _mail = mailService;
            _notifyService = notyfService;
            _notificationService = notification;        
        }
        public IActionResult Index()
        {
            List<TblInternalRequest> requests = new List<TblInternalRequest>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var user = _context.TblInternalUsers.Find(userId);
            
            if (user.IsDepartmentHead==true)
            {
                requests = _context.TblInternalRequests
                              .Include(x => x.AssignedByNavigation)
                              .Include(s => s.RequestStatus)
                              .Include(s => s.ServiceType)
                              .Include(s => s.RequestedByNavigation).ToList();
            }
            else
            {
                requests = _context.TblInternalRequests
              .Include(x => x.AssignedByNavigation)
              .Include(s => s.RequestStatus)
              .Include(s => s.ServiceType)
              .Include(s => s.RequestedByNavigation).Where(s=>s.RequestedBy==userId).ToList();
            }
            return View(requests);
        }
        public IActionResult DepIndex()
        {
            List<TblInternalRequest> requests = new List<TblInternalRequest>();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var user = _context.TblInternalUsers.Find(userId);

            if (user.IsDepartmentHead == true)
            {
                requests = _context.TblInternalRequests
                              .Include(x => x.AssignedByNavigation)
                              .Include(s => s.RequestStatus)
                              .Include(s => s.ServiceType)
                              .Include(s => s.RequestedByNavigation).ToList();
            }
            else
            {
                requests = _context.TblInternalRequests
              .Include(x => x.AssignedByNavigation)
              .Include(s => s.RequestStatus)
              .Include(s => s.ServiceType)
              .Include(s => s.RequestedByNavigation).Where(s => s.RequestedBy == userId).ToList();
            }
            return View(requests);
        }
        public IActionResult Create()
        {
            InternalRequestModel model= new InternalRequestModel();
         
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            model.CreatedDate = DateTime.Now;
            if (cultur== "am")
            {
                model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeNameAmharic.ToString()
                }).ToList();
            }
            else
            {
                model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName.ToString()
                }).ToList();
            }
            List<CurrencyModel> currencyModels;
            currencyModels = new List<CurrencyModel>()
            {
                new CurrencyModel
                {
                    CurrencyId="USD",
                    CurrencyName="የአሜሪካን ዶላር"
                },
                new CurrencyModel
                {
                    CurrencyId="ETB",
                    CurrencyName="ብር"
                },
                new CurrencyModel
                {
                    CurrencyId="Euro",
                    CurrencyName="ዩሮ"
                },
                new CurrencyModel
                {
                    CurrencyId="Pound",
                    CurrencyName="የእግሊዝ ፓውንድ"
                },
                new CurrencyModel
                {
                    CurrencyId="Other",
                    CurrencyName="ሌላ..."
                },
            };
            model.Currency = currencyModels.Select(s => new SelectListItem
            {
                Value = s.CurrencyId.ToString(),
                Text=s.CurrencyName
            }).ToList();

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult Create(InternalRequestModel model)
        {
            try
            {
                TblInternalRequest request = new TblInternalRequest();

                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var status = _context.TblInternalRequestStatuses.Where(s => s.StatusName == "New").FirstOrDefault();
                List<TblInternalDocumentHistory> documentHistory = new List<TblInternalDocumentHistory>();
                request.CreatedDate = DateTime.Now;
                request.RequestDetail = model.RequestDetail;
                request.ServiceTypeId = model.ServiceTypeId;
                request.RequestedBy = userId;
                request.IsAssignedToexpert = false;
                request.RequestStatusId = status.RequestStatusId;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                if (model.MultipleFiles!=null)
                {
                    foreach (var item in model.MultipleFiles)
                    {
                        if (item.Length > 0)
                        {
                            if (item.FileName != null)
                            {
                                if (!Directory.Exists(path))
                                    Directory.CreateDirectory(path);
                                FileInfo fileInfo = new FileInfo(item.FileName);
                                string ExactFileName = item.FileName;
                                string fileName = Guid.NewGuid().ToString() + item.FileName;
                                string fileNameWithPath = Path.Combine(path, fileName);
                                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                                {
                                    item.CopyTo(stream);
                                }
                                string dbPath = "/admin/Files/" + fileName;
                                documentHistory.Add(new TblInternalDocumentHistory { DocPath = dbPath, RequestId = request.RequestId, ExactFileName = ExactFileName, FileDescription = ExactFileName });

                            }
                        }
                    }
                    request.TblInternalDocumentHistories = documentHistory;
                }                
                _context.TblInternalRequests.Add(request);
                int saved = _context.SaveChanges();
                if (saved > 0)
                {
                  
                    _notifyService.Success("Your request is submitted Successfully. Responsive body is notified by emailSuccessfully submitted!");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
                    if (cultur == "am")
                    {
                        model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeNameAmharic.ToString()
                        }).ToList();
                    }
                    else
                    {
                        model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName.ToString()
                        }).ToList();
                    }
                    List<CurrencyModel> currencyModels;
                    currencyModels = new List<CurrencyModel>()
            {
                new CurrencyModel
                {
                    CurrencyId="USD",
                    CurrencyName="የአሜሪካን ዶላር"
                },
                new CurrencyModel
                {
                    CurrencyId="ETB",
                    CurrencyName="ብር"
                },
                new CurrencyModel
                {
                    CurrencyId="Euro",
                    CurrencyName="ዩሮ"
                },
                new CurrencyModel
                {
                    CurrencyId="Pound",
                    CurrencyName="የእግሊዝ ፓውንድ"
                },
                new CurrencyModel
                {
                    CurrencyId="Other",
                    CurrencyName="ሌላ..."
                },
            };
                    model.Currency = currencyModels.Select(s => new SelectListItem
                    {
                        Value = s.CurrencyId.ToString(),
                        Text = s.CurrencyName
                    }).ToList();
                    _notifyService.Error("Your request isn't submitted successfully. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {

                var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
                if (cultur == "am")
                {
                    model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                    {
                        Value = s.ServiceTypeId.ToString(),
                        Text = s.ServiceTypeNameAmharic.ToString()
                    }).ToList();
                }
                else
                {
                    model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                    {
                        Value = s.ServiceTypeId.ToString(),
                        Text = s.ServiceTypeName.ToString()
                    }).ToList();
                }
                _notifyService.Error(ex.Message+" happened. Because of this your request isn't submitted successfully. Please try again");
                List<CurrencyModel> currencyModels;
                currencyModels = new List<CurrencyModel>()
            {
                new CurrencyModel
                {
                    CurrencyId="USD",
                    CurrencyName="የአሜሪካን ዶላር"
                },
                new CurrencyModel
                {
                    CurrencyId="ETB",
                    CurrencyName="ብር"
                },
                new CurrencyModel
                {
                    CurrencyId="Euro",
                    CurrencyName="ዩሮ"
                },
                new CurrencyModel
                {
                    CurrencyId="Pound",
                    CurrencyName="የእግሊዝ ፓውንድ"
                },
                new CurrencyModel
                {
                    CurrencyId="Other",
                    CurrencyName="ሌላ..."
                },
            };
                model.Currency = currencyModels.Select(s => new SelectListItem
                {
                    Value = s.CurrencyId.ToString(),
                    Text = s.CurrencyName
                }).ToList();
                return View(model);
            }
     
           
        }
        public async Task<IActionResult> AssignToUser(Guid id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            InternalRequestModel model = new InternalRequestModel();
            TblInternalRequest drafting = await _context.TblInternalRequests.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            model.RequestDetail = drafting.RequestDetail;
            model.RequestId = id;
            model.AssignedBy = userId;
            model.AssignedDate = DateTime.Now;
            if (cultur == "am")
            {
                model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeNameAmharic
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeTypeAmharic.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Text = s.TeamNameAmharic,
                    Value = s.TeamId.ToString(),
                }).ToList();
            }
            else
            {
                model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeType.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Text = s.TeamName,
                    Value = s.TeamId.ToString(),
                }).ToList();
            }

            model.ServiceTypeId = drafting.ServiceTypeId;
            model.AssignedTos = _context.TblInternalUsers.Where(s => s.Dep.DepCode == "CVA").Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(),
                Text = x.FirstName.ToString() + " " + x.MidleName

            }).ToList();
            model.DueDate = DateTime.Now.AddDays(10);

            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AssignToUser(InternalRequestModel model)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();

            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            List<Guid> cretatedTos = new List<Guid>();
            List<string>? emails = new List<string>();
            List<TblInternalRequestAssignee> assignees;
            if (model.RequestId == null)
            {
                return NotFound();
            }
            TblInternalRequest drafting = await _context.TblInternalRequests.FindAsync(model.RequestId);
            if (drafting == null)
            {
                return NotFound();
            }
            try
            {
                var topstatus = (from tops in _context.TblTopStatuses where tops.StatusName == "In Progress" select tops).FirstOrDefault();
                if (model.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847"))
                {
                    if (model.TeamId == null)
                    {
                        _notifyService.Error("Since assignment type is team , You should select one the team");

                        if (cultur == "am")
                        {
                            model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                            {
                                Value = s.ServiceTypeId.ToString(),
                                Text = s.ServiceTypeNameAmharic
                            }).ToList();
                            model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                            {
                                Text = s.AssigneeTypeAmharic.ToString(),
                                Value = s.AssigneeTypeId.ToString(),
                            }).ToList();
                            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                            {
                                Text = s.TeamNameAmharic,
                                Value = s.TeamId.ToString(),
                            }).ToList();
                        }
                        else
                        {
                            model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                            {
                                Value = s.ServiceTypeId.ToString(),
                                Text = s.ServiceTypeName
                            }).ToList();
                            model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                            {
                                Text = s.AssigneeType.ToString(),
                                Value = s.AssigneeTypeId.ToString(),
                            }).ToList();
                            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                            {
                                Text = s.TeamName,
                                Value = s.TeamId.ToString(),
                            }).ToList();
                        }

                        model.ServiceTypeId = drafting.ServiceTypeId;                       
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        return View(model);
                    }

                    TblInternalRequestStatus status = (from items in _context.TblInternalRequestStatuses where items.StatusName == "Assigned to team" select items).FirstOrDefault();
                    var TeamEmail = _context.TblInternalUsers.Where(x => x.TeamId == model.TeamId && x.IsTeamLeader == true).Select(s => s.EmailAddress).ToList();                  
                    drafting.AssingmentRemark = model.AssingmentRemark;
                    drafting.RequestStatusId = status.RequestStatusId;
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Request is successfully assigned to team");
                        await SendMail(TeamEmail, "Task is assign notification", "<h3>Some tasks are assigned to your team via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Assignment isn't successfull. Please try again");
                        if (cultur == "am")
                        {
                            model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                            {
                                Value = s.ServiceTypeId.ToString(),
                                Text = s.ServiceTypeNameAmharic
                            }).ToList();
                            model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                            {
                                Text = s.AssigneeTypeAmharic.ToString(),
                                Value = s.AssigneeTypeId.ToString(),
                            }).ToList();
                            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                            {
                                Text = s.TeamNameAmharic,
                                Value = s.TeamId.ToString(),
                            }).ToList();
                        }
                        else
                        {
                            model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                            {
                                Value = s.ServiceTypeId.ToString(),
                                Text = s.ServiceTypeName
                            }).ToList();
                            model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                            {
                                Text = s.AssigneeType.ToString(),
                                Value = s.AssigneeTypeId.ToString(),
                            }).ToList();
                            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                            {
                                Text = s.TeamName,
                                Value = s.TeamId.ToString(),
                            }).ToList();
                        }
                        model.ServiceTypeId = drafting.ServiceTypeId;
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        return View(model);
                    }
                }
                else
                {
                    TblInternalRequestStatus status = (from items in _context.TblInternalRequestStatuses where items.StatusName == "Assigned to user" select items).FirstOrDefault();
                    foreach (var item in model.AssignedTo)
                    {
                        var userEmails = (from user in _context.TblInternalUsers where user.UserId == item select user.EmailAddress).FirstOrDefault();
                        emails.Add(userEmails);
                    }
                    if (model.AssignedTo.Length == 0)
                    {
                        _notifyService.Error("Since assignment type is Expert , You should select one the experts");
                        if (cultur == "am")
                        {
                            model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                            {
                                Value = s.ServiceTypeId.ToString(),
                                Text = s.ServiceTypeNameAmharic
                            }).ToList();
                            model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                            {
                                Text = s.AssigneeTypeAmharic.ToString(),
                                Value = s.AssigneeTypeId.ToString(),
                            }).ToList();
                            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                            {
                                Text = s.TeamNameAmharic,
                                Value = s.TeamId.ToString(),
                            }).ToList();
                        }
                        else
                        {
                            model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                            {
                                Value = s.ServiceTypeId.ToString(),
                                Text = s.ServiceTypeName
                            }).ToList();
                            model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                            {
                                Text = s.AssigneeType.ToString(),
                                Value = s.AssigneeTypeId.ToString(),
                            }).ToList();
                            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                            {
                                Text = s.TeamName,
                                Value = s.TeamId.ToString(),
                            }).ToList();
                        }
                        model.ServiceTypeId = drafting.ServiceTypeId;
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);

                        return View(model);
                    }
                    drafting.DueDate = model.DueDate;
                    drafting.AssignedDate = model.AssignedDate;
                    drafting.AssignedBy = model.AssignedBy;
                    drafting.AssingmentRemark = model.AssingmentRemark;
                    drafting.RequestStatusId = status.RequestStatusId;
                    if (model.AssignedTo.Length > 0)
                    {
                        assignees = new List<TblInternalRequestAssignee>();
                        foreach (var item in model.AssignedTo)
                        {
                            var ifExists = _context.TblRequestAssignees.Where(x => x.RequestId == model.RequestId && x.UserId == item).FirstOrDefault();
                            if (ifExists == null)
                            {
                                cretatedTos.Add(item);
                                assignees.Add(new TblInternalRequestAssignee { UserId = item, RequestId = model.RequestId });
                            }
                        }
                        drafting.TblInternalRequestAssignees = assignees;
                    }
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Request is successfully assigned to team");

                        _notificationService.saveNotification(userId, cretatedTos, "New request assigned.");
                        await SendMail(emails, "Task is assign notification", "<h3>Some tasks are assigned to you via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        if (cultur == "am")
                        {
                            model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                            {
                                Value = s.ServiceTypeId.ToString(),
                                Text = s.ServiceTypeNameAmharic
                            }).ToList();
                            model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                            {
                                Text = s.AssigneeTypeAmharic.ToString(),
                                Value = s.AssigneeTypeId.ToString(),
                            }).ToList();
                            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                            {
                                Text = s.TeamNameAmharic,
                                Value = s.TeamId.ToString(),
                            }).ToList();
                        }
                        else
                        {
                            model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                            {
                                Value = s.ServiceTypeId.ToString(),
                                Text = s.ServiceTypeName
                            }).ToList();
                            model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                            {
                                Text = s.AssigneeType.ToString(),
                                Value = s.AssigneeTypeId.ToString(),
                            }).ToList();
                            model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                            {
                                Text = s.TeamName,
                                Value = s.TeamId.ToString(),
                            }).ToList();
                        }

                        model.ServiceTypeId = drafting.ServiceTypeId;
                       model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);

                        return View(model);

                    }
                }
            }
            catch (Exception ex)
            {
                if (cultur == "am")
                {
                    model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                    {
                        Value = s.ServiceTypeId.ToString(),
                        Text = s.ServiceTypeNameAmharic
                    }).ToList();
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Text = s.AssigneeTypeAmharic.ToString(),
                        Value = s.AssigneeTypeId.ToString(),
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                    {
                        Text = s.TeamNameAmharic,
                        Value = s.TeamId.ToString(),
                    }).ToList();
                }
                else
                {
                    model.ServiceTypes = _context.TblInternalServiceTypes.Select(s => new SelectListItem
                    {
                        Value = s.ServiceTypeId.ToString(),
                        Text = s.ServiceTypeName
                    }).ToList();
                    model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                    {
                        Text = s.AssigneeType.ToString(),
                        Value = s.AssigneeTypeId.ToString(),
                    }).ToList();
                    model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                    {
                        Text = s.TeamName,
                        Value = s.TeamId.ToString(),
                    }).ToList();
                }
                model.ServiceTypeId = drafting.ServiceTypeId;
                model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                {
                    Value = x.UserId.ToString(),
                    Text = x.FirstName.ToString() + " " + x.MidleName

                }).ToList();
                model.DueDate = DateTime.Now.AddDays(10);
                return View(model);
            }
        }

        public async Task<IActionResult> ReAssignToUser(Guid id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            LegalStudiesDraftingModel model = new LegalStudiesDraftingModel();
            TblRequest drafting = await _context.TblRequests.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            if (drafting.RequestDetail != null)
            {
                model.RequestDetail = drafting.RequestDetail;
            }
            model.RequestId = id;
            model.AssignedBy = userId;
            model.AssignedDate = DateTime.Now;
            model.DocId = drafting.DocTypeId;
            model.ServiceTypeID = drafting.ServiceTypeId;
            if (cultur == "am")
            {
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeTypeAmharic.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeNameAmharic
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Text = s.TeamNameAmharic,
                    Value = s.TeamId.ToString(),
                }).ToList();

                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityNameAmharic.ToString()

                }).ToList();
                model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
                {
                    Value = x.DocId.ToString(),
                    Text = x.DocNameAmharic
                }).ToList();
            }
            else
            {
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeType.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
                {
                    Value = x.ServiceTypeId.ToString(),
                    Text = x.ServiceTypeName
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Text = s.TeamName,
                    Value = s.TeamId.ToString(),
                }).ToList();

                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityName.ToString()

                }).ToList();
                model.LegalStadiesDocumenttypes = _context.TblLegalDraftingDocTypes.Select(x => new SelectListItem
                {
                    Value = x.DocId.ToString(),
                    Text = x.DocName
                }).ToList();
            }
            model.AssignedTos = _context.TblInternalUsers.Where(x => x.Dep.DepCode == "CVA").Select(x => new SelectListItem
            {
                Value = x.UserId.ToString(),
                Text = x.FirstName.ToString() + " " + x.MidleName

            }).ToList();
            model.DueDate = DateTime.Now.AddDays(10);
            model.PriorityId = drafting.PriorityId;
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ReAssignToUser(LegalStudiesDraftingModel model)
        {
            List<string>? emails = new List<string>();
            List<TblRequestAssignee> assignees;
            if (model.RequestId == null)
            {
                return NotFound();
            }
            TblRequest drafting = await _context.TblRequests.FindAsync(model.RequestId);
            TblRequestDepartmentRelation relation = await _context.TblRequestDepartmentRelations.Where(s => s.RequestId == model.RequestId).FirstOrDefaultAsync();
            if (drafting == null)
            {
                return NotFound();
            }
            try
            {

                if (model.AssigneeTypeId == Guid.Parse("bdfb6c89-fb2a-45f9-82f1-d56a3a396847"))
                {
                    if (model.TeamId == null)
                    {
                        _notifyService.Error("Since assignment type is team , You should select one the team");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);
                    }
                    TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "Assigned to team" select items).FirstOrDefault();
                    var TeamEmail = _context.TblInternalUsers.Where(x => x.TeamId == model.TeamId && x.IsTeamLeader == true).Select(s => s.EmailAddress).ToList();
                    relation.AssigneeTypeId = model.AssigneeTypeId;
                    relation.TeamId = model.TeamId;
                    relation.IsAssingedToUser = false;

                    drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        _notifyService.Success("Request is successfully assigned to team");
                        await SendMail(TeamEmail, "Task is assign notification", "<h3>Some tasks are assigned to your team via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        _notifyService.Error("Assignment isn't successfull. Please try again");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);
                    }
                }
                else
                {
                    TblExternalRequestStatus status = (from items in _context.TblExternalRequestStatuses where items.StatusName == "Assigned to user" select items).FirstOrDefault();
                    foreach (var item in model.AssignedTo)
                    {
                        var userEmails = (from user in _context.TblInternalUsers where user.UserId == item select user.EmailAddress).FirstOrDefault();
                        emails.Add(userEmails);
                    }
                    if (model.AssignedTo.Length == 0)
                    {
                        _notifyService.Error("Since assignment type is Expert , You should select one the experts");
                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);
                    }
                    relation.IsAssingedToUser = true;
                    drafting.DueDate = model.DueDate;
                    drafting.AssignedDate = model.AssignedDate;
                    drafting.PriorityId = model.PriorityId;
                    drafting.AssignedBy = model.AssignedBy;
                    drafting.AssingmentRemark = model.AssingmentRemark;
                    drafting.CreatedBy = model.CreatedBy;
                    drafting.ExternalRequestStatusId = status.ExternalRequestStatusId;
                    if (model.AssignedTo.Length > 0)
                    {
                        assignees = new List<TblRequestAssignee>();
                        foreach (var item in model.AssignedTo)
                        {
                            var ifExists = _context.TblRequestAssignees.Where(x => x.RequestId == model.RequestId && x.UserId == item).FirstOrDefault();
                            if (ifExists == null)
                            {
                                assignees.Add(new TblRequestAssignee { UserId = item, RequestId = model.RequestId });
                            }
                        }
                        drafting.TblRequestAssignees = assignees;
                    }
                    int updated = await _context.SaveChangesAsync();
                    if (updated > 0)
                    {
                        await SendMail(emails, "Task is assign notification", "<h3>Some tasks are assigned to you via <strong> Task tacking Dashboard</strong>. Please check on the system and reply!. </h3");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {

                        model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Value = s.ServiceTypeId.ToString(),
                            Text = s.ServiceTypeName
                        }).ToList();
                        model.ServiceTypeID = drafting.ServiceTypeId;
                        model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                        {
                            Text = s.AssigneeType.ToString(),
                            Value = s.AssigneeTypeId.ToString(),
                        }).ToList();
                        model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "CVA").Select(s => new SelectListItem
                        {
                            Text = s.TeamName,
                            Value = s.TeamId.ToString(),
                        }).ToList();
                        model.AssignedTos = _context.TblInternalUsers.Select(x => new SelectListItem
                        {
                            Value = x.UserId.ToString(),
                            Text = x.FirstName.ToString() + " " + x.MidleName

                        }).ToList();
                        model.DueDate = DateTime.Now.AddDays(10);
                        model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                        {
                            Value = x.PriorityId.ToString(),
                            Text = x.PriorityName.ToString()

                        }).ToList();
                        model.PriorityId = drafting.PriorityId;
                        return View(model);

                    }
                }
            }
            catch (Exception)
            {
                model.ServiceTypes = _context.TblServiceTypes.Where(x => x.Dep.DepCode == "CVA").Select(s => new SelectListItem
                {
                    Value = s.ServiceTypeId.ToString(),
                    Text = s.ServiceTypeName
                }).ToList();
                model.ServiceTypeID = drafting.ServiceTypeId;
                model.AssignmentTypes = _context.TblAssignementTypes.Select(s => new SelectListItem
                {
                    Text = s.AssigneeType.ToString(),
                    Value = s.AssigneeTypeId.ToString(),
                }).ToList();
                model.Teams = _context.TblTeams.Where(s => s.Dep.DepCode == "LSDC").Select(s => new SelectListItem
                {
                    Text = s.TeamName,
                    Value = s.TeamId.ToString(),
                }).ToList();
                model.AssignedTos = _context.TblInternalUsers.Where(x => x.Dep.DepCode == "LSDC").Select(x => new SelectListItem
                {
                    Value = x.UserId.ToString(),
                    Text = x.FirstName.ToString() + " " + x.MidleName

                }).ToList();
                model.DueDate = DateTime.Now.AddDays(10);
                model.Priorities = _context.TblPriorities.Select(x => new SelectListItem
                {
                    Value = x.PriorityId.ToString(),
                    Text = x.PriorityName.ToString()

                }).ToList();
                model.PriorityId = drafting.PriorityId;
                return View(model);
            }
        }
        private async Task SendMail(List<string> to, string subject, string body)
        {
            var companyEmail = _context.TblCompanyEmails.Where(x => x.IsActive == true).FirstOrDefault();
            MailData data = new MailData(to, subject, body, companyEmail.EmailAdress);
            bool sentResult = await _mail.SendAsync(data, new CancellationToken());
        }

        public async Task<IActionResult> AssignedRequests()
        {
            List<TblInternalRequest>? atsdbContext = new List<TblInternalRequest>();
            TblInternalRequest Request;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
            var assignedReq = _context.TblInternalRequestAssignees.Where(x => x.UserId == userId).ToList();
            foreach (var item in assignedReq)
            {
                Request = new TblInternalRequest();
                Request = _context.TblInternalRequests
                                 .Include(t => t.AssignedByNavigation)
                                 .Include(t => t.ServiceType)
                                 .Include(t => t.RequestedByNavigation)
                                 .Include(x => x.RequestStatus)
                                 .Where(a => a.RequestId == item.RequestId).FirstOrDefault();
                if (Request != null)
                {
                    atsdbContext.Add(Request);
                }
            }
            var sortedLists = atsdbContext.OrderByDescending(s => s.OrderId);
            return View(sortedLists);
        }
        public async Task<IActionResult> UpploadFinalReport(Guid id)
        {
            InternalRequestModel model = new InternalRequestModel();
            var detail = _context.TblInternalRequests.FindAsync(id).Result;
            model.RequestId = id;
            model.CreatedDate = DateTime.UtcNow;
            model.RequestDetail = detail.RequestDetail;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> UpploadFinalReport(InternalRequestModel model)
        {
            TblInternalRequest civilJustice = await _context.TblInternalRequests.FindAsync(model.RequestId);
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");

            //create folder if not exist
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            //get file extension
            FileInfo fileInfo = new FileInfo(model.finalReport.FileName);
            string fileName = Guid.NewGuid().ToString() + model.finalReport.FileName;
            string fileNameWithPath = Path.Combine(path, fileName);
            using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
            {
                model.finalReport.CopyTo(stream);
            }
            string dbPath = "/admin/Files/" + fileName;
            civilJustice.FinalReport = dbPath;
            civilJustice.FinalReportSummary = model.FinalReportSummary;
            int updated = _context.SaveChanges();
            if (updated > 0)
            {
                _notifyService.Success("Report successfully upploaded");
                return RedirectToAction(nameof(AssignedRequests));
            }
            else
            {
                _notifyService.Error("Report isn't successfully uploaded. Please try again");
                return View(model);
            }
        }
        public IActionResult NewInternalRequests()
        {
            var requests = _context.TblInternalRequests
                .Include(x => x.AssignedByNavigation)
                .Include(s => s.TblInternalRequestAssignees)
                .Include(s => s.RequestedByNavigation).Where(s => s.IsAssignedToexpert == false).ToList();
            return View(requests);
        }
        public async Task<IActionResult> RequestChats(Guid? id, string actionMethod, string controller, string type)
        {
            RequestChatModel model = new RequestChatModel();
            if (type != null)
            {
                if (type == "minister")
                {
                    model.ForStateMinister = true;
                }
                else if (type == "expert")
                {
                    model.ForStateMinister = false;
                }
            }
            var appointment = _context.TblSpecificPlans.Find(id);
            model.RequestId = id;
            ViewBag.actionMethod = actionMethod;
            ViewBag.controller = controller;
            _contextAccessor.HttpContext.Session.SetString("actionMethod", actionMethod);
            _contextAccessor.HttpContext.Session.SetString("controller", controller);
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RequestChats(RequestChatModel chatModel)
        {
            try
            {
                var depHeadEmail = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "CVA" && s.IsDepartmentHead == true).Select(s => s.EmailAddress).ToList();
                string actionMethod = _contextAccessor.HttpContext.Session.GetString("actionMethod");
                string controller = _contextAccessor.HttpContext.Session.GetString("controller");
                Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                var user = _context.TblInternalUsers.Find(userId);
                TblInternalRequestChat chat = new TblInternalRequestChat();
                if (user.IsDepartmentHead == true)
                {
                    chat.SendBy = user.UserId;
                    chat.IsDephead = true;
                    chat.IsExpert = true;
                    if (chatModel.ForStateMinister == true)
                    {
                        chat.IsExpert = false;
                    }
                    else
                    {
                        chat.IsExpert = true;
                    }
                }
                
                else
                {
                    chat.SendTo = user.UserId;
                    chat.IsDephead = true;
                    chat.IsExpert = true;
                }
                chat.RequestId = chatModel.RequestId;
                chat.ChatContent = chatModel.ChatContent;
                chat.Datetime = DateTime.Now;
                chat.UserId = userId;
                chat.IsSeen = false;
                string? dbPath = null;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (chatModel.FilePath != null)
                {
                    FileInfo fileInfo = new FileInfo(chatModel.FilePath.FileName);
                    string fileName = Guid.NewGuid().ToString() + chatModel.FilePath.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        chatModel.FilePath.CopyTo(stream);
                    }
                    dbPath = "/admin/Files/" + fileName;
                    chat.FilePath = dbPath;
                }
                _context.TblInternalRequestChats.Add(chat);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    if (user.IsDefaultUser == true)
                    {
                        await SendMail(depHeadEmail, "Request update", "<h3>Expert dropped message on dashboard please check it.</h3>");

                    }
                    _notifyService.Success("Sent");
                    return RedirectToAction(nameof(RequestChats), new { id = chatModel.RequestId, actionMethod = actionMethod, controller = controller });
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
        public async Task<IActionResult> UppdateProgressStatus(Guid? id)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblInternalRequest tblCivilJustice = await _context.TblInternalRequests.FindAsync(id);
            model.RequestDetail = tblCivilJustice.RequestDetail;
            model.RequestId = tblCivilJustice.RequestId;
            if (cultur == "am")
            {
                model.ExternalStatus = _context.TblInternalRequestStatuses.Where(x => x.StatusName != "New" && x.StatusName != "Assigned to user"
                && x.StatusName != "Assigned to team" && x.StatusName != "In Progress").Select(x => new SelectListItem
                {
                    Text = x.StatusNameAmharic,
                    Value = x.RequestStatusId.ToString()
                }).ToList();
            }
            else
            {
                model.ExternalStatus = _context.TblInternalRequestStatuses.Where(x => x.StatusName != "New" && x.StatusName != "Assigned to user"
                && x.StatusName != "Assigned to team" && x.StatusName != "In Progress").Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.RequestStatusId.ToString()
                }).ToList();
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UppdateProgressStatus(CivilJusticeExternalRequestModel model)
        {
            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            List<Guid> cretatedTos = new List<Guid>();
            cretatedTos = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "LSDC").Select(s => s.UserId).ToList();
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));

            TblInternalRequest tblCivilJustice = await _context.TblInternalRequests.FindAsync(model.RequestId);
            TblDecisionStatus status = _context.TblDecisionStatuses.Where(x => x.StatusName == "Waiting for Upproval").FirstOrDefault();
            if (model.ExternalRequestStatusID == Guid.Parse("2521c2b7-a886-439b-b4ba-6c0167d74940") && tblCivilJustice.FinalReport == null)
            {
                _notifyService.Error("Before you make complete status. Please uppload final report");
                if (cultur == "am")
                {
                    model.ExternalStatus = _context.TblInternalRequestStatuses.Where(x => x.StatusName != "New" && x.StatusName != "Assigned to user"
               && x.StatusName != "Assigned to team" && x.StatusName != "In Progress").Select(x => new SelectListItem
               {
                   Text = x.StatusNameAmharic,
                   Value = x.RequestStatusId.ToString()
               }).ToList();
                }
                else
                {
                    model.ExternalStatus = _context.TblInternalRequestStatuses.Where(x => x.StatusName != "New" && x.StatusName != "Assigned to user"
               && x.StatusName != "Assigned to team" && x.StatusName != "In Progress").Select(x => new SelectListItem
               {
                   Text = x.StatusName,
                   Value = x.RequestStatusId.ToString()
               }).ToList();
                }
                model.RequestDetail = tblCivilJustice.RequestDetail;
                return View(model);
            }
            tblCivilJustice.RequestStatusId = model.ExternalRequestStatusID;
            tblCivilJustice.TeamUpprovalStatus = status.DesStatusId;
            tblCivilJustice.DepartmentUpprovalStatus = status.DesStatusId;
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Status successfully updated");
                _notificationService.saveNotification(userId, cretatedTos, "New request is assigned. Please check");

                return RedirectToAction(nameof(AssignedRequests));
            }
            return View(model);
        }
        public async Task<IActionResult> UppdateDesicionStatus(Guid? id)
        {

            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            CivilJusticeExternalRequestModel model = new CivilJusticeExternalRequestModel();
            TblInternalRequest tblCivilJustice = await _context.TblInternalRequests.FindAsync(id);
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser user = await _context.TblInternalUsers.FindAsync(userId);
            if (user.IsDepartmentHead == true)
            {
                ViewBag.visible = "visible";
            }
            else
            {
                ViewBag.visible = "none";
            }
            model.IsDeputyApprovalNeeded = false;
            model.RequestDetail = tblCivilJustice.RequestDetail;
            model.RequestId = tblCivilJustice.RequestId;
            if (cultur == "am")
            {
                model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                {
                    Text = x.StatusNameAmharic,
                    Value = x.DesStatusId.ToString()
                }).ToList();
            }
            else
            {
                model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.DesStatusId.ToString()
                }).ToList();
            }

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UppdateDesicionStatus(CivilJusticeExternalRequestModel model)
        {
            TblInternalRequestChat chat = new TblInternalRequestChat();
            List<Guid> ids = new List<Guid>();
            List<string> emails = new List<string>();
            var exterStatu = _context.TblExternalRequestStatuses.Where(s => s.StatusName == "Reviewed by Department Head").FirstOrDefault();

            var cultur = _contextAccessor.HttpContext.Session.GetString("culture").ToString();
            TblInternalRequest? tblCivilJustice = await _context.TblInternalRequests.FindAsync(model.RequestId);
            var users = (from items in _context.TblInternalUsers
                         join assigne in _context.TblRequestAssignees on items.UserId equals assigne.UserId
                         where assigne.RequestId == model.RequestId
                         select new { UserId = items.UserId, email = items.EmailAddress }).ToList();
            var Chatusers = (from items in _context.TblInternalUsers
                             join assigne in _context.TblRequestAssignees on items.UserId equals assigne.UserId
                             where assigne.RequestId == model.RequestId
                             select new { UserId = items.UserId, email = items.EmailAddress }).FirstOrDefault();
            foreach (var item in users)
            {
                ids.Add(item.UserId);
                emails.Add(item.email);
            }
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser? user = await _context.TblInternalUsers.FindAsync(userId);
            TblTopStatus? topStatus = _context.TblTopStatuses.Where(s => s.StatusName == "Completed").FirstOrDefault();
            TblDecisionStatus? decisionStatus = await _context.TblDecisionStatuses.FindAsync(model.DesStatusId);
            if (user.IsTeamLeader == true)
            {
                tblCivilJustice.TeamUpprovalStatus = model.DesStatusId;
                tblCivilJustice.TeamDesicionRemark = model.DescissionRemark;
            }
            else if (user.IsDefaultUser == true)
            {
                tblCivilJustice.UserUpprovalStatus = model.DesStatusId;
            }
            else if (user.IsDepartmentHead == true)
            {
                tblCivilJustice.DepartmentUpprovalStatus = model.DesStatusId;
                tblCivilJustice.DepartmentDesicionRemark = model.DescissionRemark;
                if (model.IsDeputyApprovalNeeded == true)
                {
                    tblCivilJustice.TopStatusId = topStatus.TopStatusId;
                }
                if (decisionStatus.StatusName == "Upproved")
                {
                    tblCivilJustice.TeamUpprovalStatus = model.DesStatusId;
                }
            }
            else if (user.IsDeputy == true)
            {
                tblCivilJustice.TopStatusId = topStatus.TopStatusId;
                if (decisionStatus.StatusName == "Upproved")
                {
                    tblCivilJustice.TeamUpprovalStatus = model.DesStatusId;
                    tblCivilJustice.DepartmentUpprovalStatus = model.DesStatusId;
                }
            }
            else
            {
                if (cultur == "am")
                {
                    model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                    {
                        Text = x.StatusNameAmharic,
                        Value = x.DesStatusId.ToString()
                    }).ToList();
                }
                else
                {
                    model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                    {
                        Text = x.StatusName,
                        Value = x.DesStatusId.ToString()
                    }).ToList();
                }
                _notifyService.Error("Opperation isn't successfull. Please try again");
                return View(model);
            }

            if (model.finalReport != null)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
                //create folder if not exist
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                //get file extension
                FileInfo fileInfo = new FileInfo(model.finalReport.FileName);
                string fileName = Guid.NewGuid().ToString() + model.finalReport.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.finalReport.CopyTo(stream);
                }
                string dbPath = "/admin/Files/" + fileName;
                tblCivilJustice.FinalReport = dbPath;
            }
            tblCivilJustice.RequestStatusId = exterStatu.ExternalRequestStatusId;
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                _notifyService.Success("Upproval status changed successfully!");
                chat.UserId = userId;
                chat.RequestId = model.RequestId;
                chat.ChatContent = model.DescissionRemark;
                chat.Datetime = DateTime.Now;
                chat.IsDephead = true;
                chat.SendBy = userId;
                chat.SendTo = Chatusers.UserId;
                _context.TblInternalRequestChats.Add(chat);
                _context.SaveChanges();
                await SendMail(emails, "Request update status", "Upproval status changed successfully!. You can check detail on request chat section");
                _notificationService.saveNotification(userId, ids, "Your request status is changed by Department head");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (user.IsDepartmentHead == true)
                {
                    ViewBag.visible = true;
                }
                else
                {
                    ViewBag.visible = false;
                }
                _notifyService.Error("Upproval status isn't updated. Please try again");
                model.DesicionStatus = _context.TblDecisionStatuses.Where(x => x.StatusName == "Upproved" || x.StatusName == "Rejected").Select(x => new SelectListItem
                {
                    Text = x.StatusName,
                    Value = x.DesStatusId.ToString()
                }).ToList();
                return View(model);
            }
        }
        public async Task<IActionResult> CompletedRequests()
        {
            TblTopStatus tblTopStatus = _context.TblTopStatuses.Where(x => x.StatusName == "Completed").FirstOrDefault();

            List<TblInternalRequest>? atsdbContext = new List<TblInternalRequest>();
            TblInternalRequest tblRequest;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
            var moreDeps = _context.TblRequestDepartmentRelations.Where(x => x.Dep.DepCode == "CVA").Select(a => a.RequestId).ToList();
            if (tblInternalUser.IsDeputy == true || tblInternalUser.IsSuperAdmin == true || tblInternalUser.IsDepartmentHead == true)
            {
                    atsdbContext = _context.TblInternalRequests
                              .Include(x => x.AssignedByNavigation)
                              .Include(s => s.RequestStatus)
                              .Include(s => s.ServiceType)
                              .Include(s => s.TblInternalRequestAssignees)
                              .Include(s => s.RequestedByNavigation).Where(x => x.TopStatusId == tblTopStatus.TopStatusId ).ToList(); 
            }
            else
            {
                var assignedReq = _context.TblInternalRequestAssignees.Where(x => x.UserId == userId).ToList();
                foreach (var item in assignedReq)
                {
                    tblRequest = _context.TblInternalRequests
                              .Include(x => x.AssignedByNavigation)
                              .Include(s => s.RequestStatus)
                              .Include(s => s.ServiceType)
                              .Include(s => s.TblInternalRequestAssignees)
                              .Include(s => s.RequestedByNavigation).Where(x => x.RequestId == item.RequestId && x.TopStatusId == tblTopStatus.TopStatusId).FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }
                }
            }
            var sortedLists = atsdbContext.OrderByDescending(s => s.OrderId);
            return View(sortedLists);
        }
        public async Task<IActionResult> PendingRequests()
        {
            List<TblInternalRequest>? atsdbContext = new List<TblInternalRequest>();
            TblInternalRequest tblRequest;
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblInternalUser tblInternalUser = await _context.TblInternalUsers.FindAsync(userId);
           if (tblInternalUser.IsDeputy == true || tblInternalUser.IsSuperAdmin == true || tblInternalUser.IsDepartmentHead == true)
            {
               
                    atsdbContext =  _context.TblInternalRequests
                              .Include(x => x.AssignedByNavigation)
                              .Include(s => s.RequestStatus)
                              .Include(s => s.ServiceType)
                              .Include(s => s.TblInternalRequestAssignees)
                              .Include(s => s.RequestedByNavigation).Where(x => x.TopStatus.StatusName == "In Progress").ToList();
                    
            }
            else
            {

                var assignedReq = _context.TblInternalRequestAssignees.Where(x => x.UserId == userId).ToList();
                foreach (var item in assignedReq)
                {
                    tblRequest = _context.TblInternalRequests
                              .Include(x => x.AssignedByNavigation)
                              .Include(s => s.RequestStatus)
                              .Include(s => s.ServiceType)
                              .Include(s => s.TblInternalRequestAssignees)
                              .Include(s => s.RequestedByNavigation).Where(x => x.RequestId == item.RequestId && x.RequestStatus.StatusName != "New" && x.RequestStatus.StatusName != "Completed").FirstOrDefault();
                    if (tblRequest != null)
                    {
                        atsdbContext.Add(tblRequest);
                    }
                }
            }
            var sortedLists = atsdbContext.OrderByDescending(s => s.OrderId);
            return View(sortedLists);
        }
        public async Task<IActionResult> Replies(Guid? id)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var replays = await _context.TblInternalRequestReplays.Where(a => a.RequestId == id).ToListAsync();
            RepliesModel replies = new RepliesModel();
            replies.RequestId = id;
            replies.ReplyDate = DateTime.UtcNow;
            replies.InternalReplayedBy = userId;
            replies.IsSent = false;
            ViewData["Replies"] = _context.TblInternalRequestReplays
                .Include(x => x.InternalReplayedByNavigation)
                .Include(x => x.Request)
                .Where(_context => _context.RequestId == id).ToList();
            return View(replies);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Replies(RepliesModel model)
        {
            try
            {
                TblInternalRequestReplay replay = new TblInternalRequestReplay();
                replay.ReplyDate = DateTime.Now;
                replay.InternalReplayedBy = model.InternalReplayedBy;
                replay.RequestId = model.RequestId;
                replay.ReplayDetail = model.ReplayDetail;
                replay.IsInternalUser = false;
                replay.IsInternal = true;
                if (model.IsSent == true)
                {
                    replay.IsSent = true;
                }
                else
                {
                    replay.IsSent = false;
                }
                _context.TblInternalRequestReplays.Add(replay);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply successfully added.");
                    return RedirectToAction("Replies", new { id = model.RequestId });
                }
                else
                {
                    _notifyService.Error("Reply isn't added. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again.");
                return View(model);
            }
        }
        public async Task<IActionResult> EditReplies(Guid? ReplyId)
        {
            var reply = _context.TblInternalRequestReplays.Where(s => s.ReplyId == ReplyId).FirstOrDefault();
            RepliesModel repliesModel = new RepliesModel();
            repliesModel.ReplyId = reply.ReplyId;
            repliesModel.RequestId = reply.RequestId;
            if (reply.IsSent == null || reply.IsSent == false)
            {
                repliesModel.IsSent = false;
            }
            else
            {
                repliesModel.IsSent = true;
            }
            repliesModel.ReplayDetail = reply.ReplayDetail;
            return View(repliesModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> EditReplies(RepliesModel model)
        {
            try
            {
                var DepheadId = _context.TblInternalUsers.Include(s => s.Dep).Where(s => s.Dep.DepCode == "CVA" && s.IsDepartmentHead == true).FirstOrDefault();
                TblInternalRequestReplay replay = _context.TblInternalRequestReplays.Find(model.ReplyId);
                replay.ReplayDetail = model.ReplayDetail;
                replay.InternalReplayedBy = DepheadId.UserId;
                if (model.IsSent == true)
                {
                    replay.IsSent = true;
                }
                else
                {
                    replay.IsSent = false;
                }
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply successfully added.");
                    return RedirectToAction("Replies", new { id = model.RequestId });
                }
                else
                {
                    _notifyService.Error("Reply isn't added. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again.");
                return View(model);
            }

        }
        public async Task<IActionResult> SendReply(Guid? ReplyId)
        {
            TblInternalRequestReplay replay = _context.TblInternalRequestReplays.Find(ReplyId);
            try
            {
                Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
                replay.IsSent = true;
                replay.InternalReplayedBy = userId;
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply successfully sent.");
                    return RedirectToAction("Replies", new { id = replay.RequestId });
                }
                else
                {
                    _notifyService.Error("Reply isn't sent. Please try again");
                    return RedirectToAction("Replies", new { id = replay.RequestId });
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error($"Error: {ex.Message} happened. Please try again.");
                return RedirectToAction("Replies", new { id = replay.RequestId });
            }

        }


        public async Task<IActionResult> InternalUserReplies(Guid? id)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var replays = await _context.TblInternalRequestReplays.Where(a => a.RequestId == id && a.IsSent == true).ToListAsync();
            RepliesModel model = new RepliesModel
            {
                RequestId = id,
                ReplyDate = DateTime.Now,
                ExternalReplayedBy = userId,
            };
            ViewData["Replies"] = _context.TblInternalRequestReplays
                .Include(x => x.InternalReplayedByNavigation)
                .Include(x => x.Request)
                .Where(y => y.RequestId == id && y.IsSent == true).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> InternalUserReplies(RepliesModel model)
        {
            try
            {
                TblInternalRequestReplay replay = new TblInternalRequestReplay();
                replay.ReplyDate = DateTime.Now;
                replay.InternalReplayedBy = model.ExternalReplayedBy;
                replay.RequestId = model.RequestId;
                replay.ReplayDetail = model.ReplayDetail;
                replay.IsInternalUser = true;
                replay.IsInternal = false;
                replay.IsSent = true;
                string? dbPath = null;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "admin/Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (model.Attachement != null)
                {
                    FileInfo fileInfo = new FileInfo(model.Attachement.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.Attachement.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.Attachement.CopyTo(stream);
                    }
                    dbPath = "/admin/Files/" + fileName;
                    replay.Attachment = dbPath;
                }
                _context.TblInternalRequestReplays.Add(replay);
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply submitted");
                    return RedirectToAction("Replies", new { id = model.RequestId });
                }
                else
                {
                    _notifyService.Error("Reply isn't subbmitted. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error("Reply isn't subbmitted because of " + ex.Message + ". Please try again");
                return View(model);
            }
        }
        public async Task<IActionResult> InternalUserEditReplies(Guid? ReplyId)
        {
            Guid? userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            var reply = _context.TblInternalRequestReplays.Find(ReplyId);
            RepliesModel model = new RepliesModel
            {
                ReplyId = ReplyId,
                RequestId = reply.RequestId,
                ReplayDetail = reply.ReplayDetail,
                ReplyDate = DateTime.Now,
                ExternalReplayedBy = userId,
            };
            ViewData["Replies"] = _context.TblInternalRequestReplays
                .Include(x => x.InternalReplayedByNavigation)
                .Include(x => x.Request)
                .Where(y => y.RequestId == reply.RequestId && y.IsSent == true).ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> InternalUserEditReplies(RepliesModel model)
        {
            try
            {
                TblInternalRequestReplay replay = _context.TblInternalRequestReplays.Find(model.ReplyId);
                replay.ReplyDate = DateTime.Now;
                replay.ReplayDetail = model.ReplayDetail;
                string? dbPath = null;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "admin/Files");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                if (model.Attachement != null)
                {
                    FileInfo fileInfo = new FileInfo(model.Attachement.FileName);
                    string fileName = Guid.NewGuid().ToString() + model.Attachement.FileName;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        model.Attachement.CopyTo(stream);
                    }
                    dbPath = "/admin/Files/" + fileName;
                    replay.Attachment = dbPath;
                }
                int saved = await _context.SaveChangesAsync();
                if (saved > 0)
                {
                    _notifyService.Success("Reply updated");
                    return RedirectToAction("Replies", new { id = model.RequestId });
                }
                else
                {
                    _notifyService.Error("Reply isn't subbmitted. Please try again");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _notifyService.Error("Reply isn't subbmitted because of " + ex.Message + ". Please try again");
                return View(model);
            }
        }
        public async Task<IActionResult> Details(Guid RequestId)
        {
            var internalRequest = _context.TblInternalRequests
                .Include(s=>s.ServiceType)
                .Include(s=>s.AssignedByNavigation)
                .Include(s=>s.RequestStatus)
                .Include(s=>s.RequestedByNavigation).Where(s=>s.RequestId==RequestId).FirstOrDefault();
            return View(internalRequest);
        }

        public async Task<IActionResult> AddHistory(Guid? id)
        {
            DocumentHistoryModel model = await historyModel(id);

            return View(model);
        }
        private async Task<DocumentHistoryModel> historyModel(Guid? id)
        {
            Guid userId = Guid.Parse(_contextAccessor.HttpContext.Session.GetString("userId"));
            TblDocumentHistory tblDocument = _context.TblDocumentHistories.Include(x => x.Request).Where(x => x.RequestId == id).OrderBy(x => x.Round).Last();
            DocumentHistoryModel model = new DocumentHistoryModel();
            ViewData["histories"] = _context.TblDocumentHistories.Where(x => x.RequestId == id).ToList();
            model.RequestId = id;
            model.ExternalRepliedBy = userId;
            if (tblDocument.Round == null)
            {
                model.Round = 1;
            }
            else
            {
                model.Round = Convert.ToInt32(tblDocument.Round) + 1;
            }
            return model;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> AddHistory(DocumentHistoryModel? model)
        {
            List<string> emails = new List<string>();


            var users = (from user in _context.TblRequestAssignees
                         join assignees in _context.TblInternalUsers on user.UserId equals assignees.UserId
                         where (assignees.IsDepartmentHead == true || assignees.IsDeputy == true) && user.RequestId == model.RequestId
                         select assignees.EmailAddress).ToList();
            if (users.Count != 0)
            {
                emails = users;
            }
            else
            {
                emails = (from assignees in _context.TblInternalUsers
                          where assignees.IsDepartmentHead == true || assignees.IsDeputy == true
                          select assignees.EmailAddress).ToList();
            }
            var institutionName = (from items in _context.TblRequests where items.RequestId == model.RequestId select items.Inist.Name).FirstOrDefault();

            TblDocumentHistory history = new TblDocumentHistory();
            history.ExternalRepliedBy = model.ExternalRepliedBy;
            history.Round = model.Round;
            history.Description = model.Description;
            history.FileDescription = model.FileDescription;
            history.RequestId = model.RequestId;
            history.CreatedDate = DateTime.Now;
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Files");
            if (model.DocPath != null)
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                FileInfo fileInfo = new FileInfo(model.DocPath.FileName);
                string fileName = Guid.NewGuid().ToString() + model.DocPath.FileName;
                string fileNameWithPath = Path.Combine(path, fileName);
                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    model.DocPath.CopyTo(stream);
                }
                string dbPath = "/Files/" + fileName;
                history.DocPath = dbPath;
            }

            _context.TblDocumentHistories.Add(history);
            int saved = await _context.SaveChangesAsync();
            if (saved > 0)
            {
                await SendMail(users, "Request notifications from " + institutionName, "<h3>Please review the recquest on the system and reply for the institution accordingly</h3>");
                _notifyService.Success("Document added successfully!");
                return RedirectToAction(nameof(AddHistory));
            }
            else
            {
                _notifyService.Error("Document is not successfully added. Please try again");
                return View(model);
            }
        }


    }
}
