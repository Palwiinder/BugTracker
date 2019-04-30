using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Mvc2.Models;
using Mvc2.Models.Domain;
using Mvc2.Models.Filters;
using Mvc2.Models.Helpers;
using Mvc2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Mvc2.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext DbContext;
        private ProjectHelper ProjectHelper;

        public TicketsController()
        {
            DbContext = new ApplicationDbContext();
            ProjectHelper = new ProjectHelper(DbContext);
        }
        public ActionResult Index()
        {
            return RedirectToAction(nameof(Tickets));

        }

        public ActionResult Tickets()
        {
            var userId = User.Identity.GetUserId();
            var model = DbContext.TicketsDatabase
                .Where(p => p.Project.Archive == false)
                .Where(ticket => ticket.CreatedById == userId) 
               .Select(p => new TicketsViewModel
               {
                   TicketId = p.Id,
                   Title = p.Title,
                   Description = p.Description,
                   Priority = p.TicketPriority.Name,
                   Type = p.TicketType.Name,
                   DateCreated = p.DateCreated,
                   DateUpdated = p.DateUpdated,
               }).ToList();

            return View(model);
        }

        [AuthorizeFilter(Roles = "Admin,ProjectManager")]
        
        public ActionResult AllTickets()
        {
           
            var model = DbContext.TicketsDatabase.ToList()
                .Where(p => p.Project.Archive == false)
               .Select(p => new TicketsViewModel
               {
                   TicketId = p.Id,
                   Title = p.Title,
                   Description = p.Description,
                   Priority = p.TicketPriority.Name,
                   Type = p.TicketType.Name,
                   DateCreated = p.DateCreated,
                   DateUpdated = p.DateUpdated,
               }).ToList();
               

            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "Submitter")]
        public ActionResult CreateTicket(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = User.Identity.GetUserId();
            //var userName = User.Identity.GetUserName();
            //string userName = DbContext.Users.ToList()[0].UserName;
            var ticketType = DbContext.TicketsTypeDatabase.ToList();
            var ticketPriority = DbContext.TicketsPriorityDatabase.ToList();
            var ticketAttachment = new CreateAttachmetsViewModel() { };
            var model = new CreateTicketViewModel
            {
                TicketType = ticketType.Select(p => new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),
                }).ToList(),
                TicketPriority = ticketPriority.Select(p => new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),
                }).ToList(),
                ProjectId = id.Value,
                
                //UserName = userName,

            };

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Submitter")]
        public ActionResult CreateTicket(CreateTicketViewModel formData)
        {
            return SaveTicket(null, formData);
        }

        private ActionResult SaveTicket(int? id, CreateTicketViewModel formData)
        {
            var userId = User.Identity.GetUserId();
            var ticketPriority = DbContext.TicketsPriorityDatabase.ToList();

            if (!ModelState.IsValid)
            {
                //var userName = User.Identity.GetUserName();
                //string userName = DbContext.Users.ToList()[0].UserName;
                var ticketType = DbContext.TicketsTypeDatabase.ToList();
                var ticketAttachment = new CreateAttachmetsViewModel() { };
                var model = new CreateTicketViewModel
                {
                    TicketType = ticketType.Select(p => new SelectListItem()
                    {
                        Text = p.Name,
                        Value = p.Id.ToString(),
                    }).ToList(),
                    TicketPriority = ticketPriority.Select(p => new SelectListItem()
                    {
                        Text = p.Name,
                        Value = p.Id.ToString(),
                    }).ToList(),
                    ProjectId = id.HasValue ? id.Value : formData.ProjectId,
                    
                    
                    //UserName = userName,

                };

                return View(model);
            }

            string fileExtension;

            //Validating file upload
            if (formData.Media != null)
            {
                fileExtension = Path.GetExtension(formData.Media.FileName);

                if (!Constants.AllowedFileExtensions.Contains(fileExtension.ToLower()))
                {
                    ModelState.AddModelError("", "File extension is not allowed.");

                    return RedirectToAction(nameof(AllTickets));
                }
            }

            Ticket ticket;
            //var userId = User.Identity.GetUserId();
            if (!id.HasValue)
            {
                ticket = new Ticket();
                var applicationUser = DbContext.Users.FirstOrDefault(user => user.Id == userId);
                if (applicationUser == null)
                {
                    return RedirectToAction(nameof(HomeController.Index));
                }
                ticket.CreatedBy = applicationUser;
                ticket.ProjectId = formData.ProjectId;
                ticket.TicketStatusId = DbContext.TicketsStatusDatabase.First(p => p.Name == "Open").Id;
                var userM = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                userM.SendEmailAsync(userId, "Notification", "There is a new Ticket Created for project You Belong To");
                DbContext.TicketsDatabase.Add(ticket);
            }
            else
            {
                ticket = DbContext.TicketsDatabase.FirstOrDefault(p => p.Id == id);
                ticket.DateUpdated = DateTime.Now;

                var status = DbContext.TicketsStatusDatabase.FirstOrDefault(p => p.Id == formData.StatusId);
                //ticket.TicketStatusId = status.Id;
                TicketHistory(ticket);
                //if (User.IsInRole ("Admin,ProjectManager") && ticket.SendNotification == true)
                //{
                //var userM = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //userM.SendEmailAsync(userId, "Notification", "There is a new Attachment for Ticket You Belong To");
                //}
                DbContext.SaveChanges();
                if (ticket == null)
                {
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            if (formData.Media != null)
            {
                if (!Directory.Exists(Constants.MappedUploadFolder))
                {
                    Directory.CreateDirectory(Constants.MappedUploadFolder);
                }

                var fileName = formData.Media.FileName;
                var fullPathWithName = Constants.MappedUploadFolder + fileName;

                formData.Media.SaveAs(fullPathWithName);
                var media = DbContext.TicketsAttachmentsDatabase.FirstOrDefault(p => p.TicketId == ticket.Id);



                TicketAttachments attachment;
                if (media == null)
                {
                    attachment = new TicketAttachments()
                    {
                        MediaUrl = Constants.UploadFolder + fileName,
                        TicketId = ticket.Id,
                        UserId = ticket.CreatedById,

                    };
                    var userM = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                    userM.SendEmailAsync(userId, "Notification", "There is a new Attachment for Ticket You Belong To");
                    DbContext.TicketsAttachmentsDatabase.Add(attachment);
                    ticket.Attachments.Add(attachment);
                }
            }
            var tp = DbContext.TicketsPriorityDatabase.FirstOrDefault(p => p.Id == formData.PriorityId);
            var type = DbContext.TicketsTypeDatabase.FirstOrDefault(p => p.Id == formData.TypeId);

            ticket.Title = formData.Title;
            ticket.Description = formData.Description;
            ticket.TicketPriorityId = tp.Id;
            ticket.TicketPriorityId = formData.PriorityId;
            ticket.TicketTypeId = formData.TypeId;
            ticket.TicketTypeId = type.Id;
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

            userManager.SendEmailAsync(userId, "Notification", "Some changes have been made to a Ticket that You Are Assigned to");
            DbContext.SaveChanges();

            return RedirectToAction(nameof(TicketsController.Tickets));
        }




        [HttpGet]
        [Authorize(Roles = "Admin,ProjectManager,Submitter")]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketsController.Tickets));
            }

            var ticket = DbContext.TicketsDatabase.FirstOrDefault(p => p.Id == id);

            if (ticket == null)
            {
                return RedirectToAction(nameof(TicketsController.Tickets));
            }


            var userId = User.Identity.GetUserId();
            var ticketType = DbContext.TicketsTypeDatabase.ToList();
            var ticketPriority = DbContext.TicketsPriorityDatabase.ToList();
            var ticketStatus = DbContext.TicketsStatusDatabase.ToList();
            if (!(User.IsInRole("Admin") || User.IsInRole("Project Manager")) && User.IsInRole("Submitter"))
            {
                ticketStatus.Clear();
            }
            var model = new CreateTicketViewModel
            {
                Title = ticket.Title,
                Description = ticket.Description,
                AssignedTo = ticket.AssignedTo,
                MediaUrl = ticket.MediaUrl,
                TicketType = ticketType.Select(p => new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),
                }).ToList(),
                TicketPriority = ticketPriority.Select(p => new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),
                }).ToList(),
                TicketStatus = ticketStatus.Select(p => new SelectListItem()
                {
                    Text = p.Name,
                    Value = p.Id.ToString(),
                }).ToList(),
                ProjectId = id.Value,

                //Users = p.Users ?? new List<ApplicationUser>(),
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager,Submitter")]
        public ActionResult Edit(int id, CreateTicketViewModel formData)
        {
            return SaveTicket(id, formData);
        }

        [HttpGet]
        [ExceptionFilter]
        public ActionResult Detail(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketsController.Tickets));
            }

            var ticket = DbContext.TicketsDatabase.Where(p => p.Project.Archive == false).FirstOrDefault(p => p.Id == id);
            if (ticket == null)
            {
                return RedirectToAction(nameof(TicketsController.Tickets));
            }


            var model = new ViewTicketViewModel()
            {
                Title = ticket.Title,
                Description = ticket.Description,
                Id = ticket.Id,
                TicketHistory = ticket.TicketHistory,
                Comments = ticket.Comments,
                Attachments = ticket.Attachments.ToList(),
                CreatedBy = DbContext.Users.FirstOrDefault(p => p.Id == ticket.CreatedById),
                Project = ticket.Project.ProjectName,
                AssignedTo = DbContext.Users.FirstOrDefault(p => p.Id == ticket.AssignedToId),
                Type = DbContext.TicketsTypeDatabase.First(p => p.Id == ticket.TicketTypeId).Name,
                Status = DbContext.TicketsStatusDatabase.First(p => p.Id == ticket.TicketStatusId).Name,
                Priority = DbContext.TicketsPriorityDatabase.First(p => p.Id == ticket.TicketPriorityId).Name,
                DateCreated = ticket.DateCreated,
                DateUpdated = ticket.DateUpdated,


            };
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateComment(CreateCommentViewModel formData)
        {
            return SaveComment(null, formData);
        }
        private ActionResult SaveComment(int? id, CreateCommentViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            TicketComment comment;
            if (!id.HasValue)
            {
                var userId = User.Identity.GetUserId();
                var user = DbContext.Users.FirstOrDefault(p => p.Id == userId);
                var ticket = DbContext.TicketsDatabase.FirstOrDefault(p => p.Id == formData.TicketId);

                if (ticket == null)
                {
                    return View();
                }

                comment = new TicketComment();
                comment.Comment = formData.Comment;
                comment.DateCreated = DateTime.Now;
                comment.UserId = userId;
                comment.User = user;
                comment.Ticket = ticket;
                comment.TicketId = ticket.Id;
                ticket.Comments.Add(comment);
                DbContext.TicketsCommentsDatabase.Add(comment);
                DbContext.SaveChanges();
            }
            else
            {
                var userId = User.Identity.GetUserId();
                var user = DbContext.Users.FirstOrDefault(p => p.Id == userId);
                comment = DbContext.TicketsCommentsDatabase.FirstOrDefault(p => p.Id == id);
                if (comment == null)
                {
                    return RedirectToAction(nameof(TicketsController.Tickets));
                }
                comment.Comment = formData.Comment;
                comment.DateUpdated = DateTime.Now;
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                userManager.SendEmailAsync(userId, "Notification", "You are assigned to a new Project");
                DbContext.SaveChanges();
            }
            return RedirectToAction(nameof(TicketsController.Tickets));
        }

        [HttpGet]
        [Authorize]
        public ActionResult EditComment(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketsController.Tickets));
            }
            string userId = User.Identity.GetUserId();
            var comment = DbContext.TicketsCommentsDatabase.FirstOrDefault(p => p.Id == id);

            if (comment == null)
            {
                return RedirectToAction(nameof(TicketsController.Tickets));
            }

            var model = new CreateCommentViewModel();
            model.Comment = comment.Comment;
            model.DateUpdated = comment.DateUpdated;
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EditComment(int id, CreateCommentViewModel formData)
        {
            return SaveComment(id, formData);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketsController.Tickets));
            }
            string userId = User.Identity.GetUserId();
            var comment = DbContext.TicketsCommentsDatabase.FirstOrDefault(p => p.Id == id && p.UserId == userId);
            if (comment != null)
            {
                DbContext.TicketsCommentsDatabase.Remove(comment);
                DbContext.SaveChanges();
            }
            return RedirectToAction(nameof(TicketsController.Tickets));
        }


        [HttpPost]
        public ActionResult Attachments(CreateAttachmetsViewModel formData)
        {
            string fileExtension;

            //Validating file upload
            if (formData.Media != null)
            {
                fileExtension = Path.GetExtension(formData.Media.FileName);

                if (!Constants.AllowedFileExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("", "File extension is not allowed.");

                    return View();
                }
            }
            return RedirectToAction(nameof(TicketsController.Tickets));
        }

        public ActionResult AssignTickets(int? TicketId)
        {

            if (!TicketId.HasValue)
            {
                return RedirectToAction(nameof(TicketsController.Tickets));
            }
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(DbContext));
            var model = new AssignTicketsViewModel();
            var ticket = DbContext.TicketsDatabase.FirstOrDefault(p => p.Id == TicketId);
            model.TicketId = ticket.Id;
            var users = DbContext.Users.ToList();
            var developerRoleId = roleManager.Roles.First(role => role.Name == "developer").Id;
            var developer = DbContext.Users.Where(user => user.Roles.Any(role => role.RoleId == developerRoleId)).ToList();

            model.AddUsers = new SelectList(developer, "Id", "Email");

            return View(model);
        }

        public ActionResult DeleteAttachments(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(TicketsController.Tickets));
            }
            string userId = User.Identity.GetUserId();
            var attachment = DbContext.TicketsAttachmentsDatabase.FirstOrDefault(p => p.Id == id && p.UserId == userId);
            if (attachment != null)
            {
                DbContext.TicketsAttachmentsDatabase.Remove(attachment);
                DbContext.SaveChanges();
            }
            return RedirectToAction(nameof(TicketsController.Tickets));
        }

        [HttpPost]
        [AuthorizeFilter(Roles = "Admin,ProjectManager")]
        public ActionResult AssignTickets(AssignTicketsViewModel model)
        {
            var ticket = DbContext.TicketsDatabase.FirstOrDefault(p => p.Id == model.TicketId);
            if (ticket == null)
            {
                return RedirectToAction(nameof(TicketsController.Tickets));
            }

            if (model.SelectedRemoveUsers != null)
            {
                //    DbContext.Users.First(user => ticket.AssignedToId == user.Id).AssignedTickets.Remove(ticket);

                ticket.AssignedTo = null;
                ticket.AssignedToId = null;
            }

            if (model.SelectedAddUsers != null)
            {
                var assignedUser = DbContext.Users.FirstOrDefault(p => p.Id == model.SelectedAddUsers);
                ticket.AssignedTo = assignedUser;
                var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

                userManager.SendEmailAsync(assignedUser.Id, "Notification", "You are assigned to a new Project");
                DbContext.SaveChanges();
            }
            return RedirectToAction("Index");

        }

        public  void TicketHistory(Ticket ticket)
        {
            var userId = User.Identity.GetUserId();
            var ticketHistory = new List<TicketHistory>();
            var originalValues = DbContext.Entry(ticket).OriginalValues;
            var currentValues = DbContext.Entry(ticket).CurrentValues;

            foreach (var property in originalValues.PropertyNames)
            {
                var originalValue = originalValues[property]?.ToString();
                var currentValue = currentValues[property]?.ToString();
                if (originalValue != currentValue)
                {
                    var history = new TicketHistory();
                    history.DateChanged = DateTime.Now;
                    history.NewValue = currentValue;
                    history.OldValue = originalValue;
                    history.Property = property;
                    history.Id = ticket.Id;
                    history.UserId = userId;
                    ticketHistory.Add(history);
                }
            }
            DbContext.TicketsHistoryDatabase.AddRange(ticketHistory);
        }
    }
}
