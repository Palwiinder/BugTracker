using Microsoft.AspNet.Identity;
using Mvc2.Models;
using Mvc2.Models.Domain;
using Mvc2.Models.Helpers;
using Mvc2.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            return View();
        }


        public ActionResult Tickets()
        {
            var userId = User.Identity.GetUserId();
            var model = DbContext.TicketsDatabase
                .Where(ticket => ticket.CreatedById == userId)
               .Select(p => new TicketsViewModel
               {
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
        public ActionResult CreateTicket(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(Index));
            }

            var userId = User.Identity.GetUserId();
            var ticketType = DbContext.TicketsTypeDatabase.ToList();
            var ticketPriority = DbContext.TicketsPriorityDatabase.ToList();
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
                //Users = p.Users ?? new List<ApplicationUser>(),
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult CreateTicket(CreateTicketViewModel formData)
        {
            return SaveTicket(null, formData);
        }

        private ActionResult SaveTicket(int? id, CreateTicketViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            Ticket ticket;
            var userId = User.Identity.GetUserId();
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
                DbContext.TicketsDatabase.Add(ticket);
            }
            else
            {
                ticket = DbContext.TicketsDatabase.FirstOrDefault(p => p.Id == id);
                ticket.DateUpdated = DateTime.Now;

                var status = DbContext.TicketsStatusDatabase.FirstOrDefault(p => p.Id == formData.StatusId);
                ticket.TicketStatusId = status.Id;

                if (ticket == null)
                {
                    return RedirectToAction(nameof(HomeController.Index));
                }
            }
            var tp = DbContext.TicketsPriorityDatabase.FirstOrDefault(p => p.Id == formData.PriorityId);
            var type = DbContext.TicketsTypeDatabase.FirstOrDefault(p => p.Id == formData.TypeId);
            ticket.Title = formData.Title;
            ticket.Description = formData.Description;
            ticket.TicketPriorityId = tp.Id;
            ticket.TicketTypeId = type.Id;
            DbContext.SaveChanges();
            return RedirectToAction(nameof(TicketsController.Tickets));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,ProjectManager,Submitter")]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HomeController.Index));
            }

            var userId = User.Identity.GetUserId();
            var ticketType = DbContext.TicketsTypeDatabase.ToList();
            var ticketPriority = DbContext.TicketsPriorityDatabase.ToList();
            var ticketStatus = DbContext.TicketsStatusDatabase.ToList();
            if (User.IsInRole("Submitter"))
            {
                ticketStatus.Clear();
            }
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
        [Authorize(Roles = "Admin,ProjectManager,submitter")]
        public ActionResult Edit(int id, CreateTicketViewModel formData)
        {
            return SaveTicket(id, formData);

        }

    }
}