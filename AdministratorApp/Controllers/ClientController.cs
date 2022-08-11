using AdministratorApp.Interfaces;
using AdministratorApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdministratorApp.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IAdministratorAppRepository db;

        public ClientController(IAdministratorAppRepository context)
        {
            db = context;
        }

        public async Task<IActionResult> ShowAllClients(int? id, string name, int page = 1)
        {
            var clients= db.GetClientsListForPage(id, name);
            List<Service> service_values = await db.GetServicesList();
            service_values.Insert(0, new Service { Service_Name = "All", Id = 0 });
            SelectList services = new SelectList(service_values, "Id", "Service_Name");
            ViewBag.ServicesList = services;

            int pageSize = 4;
            int count = clients.Count();
            var items = await clients.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Clients = items
            };

            if (clients != null)
            {
                return View(viewModel);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("Client/EditClient")]
        [ActionName("EditClient")]
        public async Task<IActionResult> EditClient(int? id)
        {
            if (id != null)
            {
                Client cl = await db.GetClientWithServices(id.Value);
                List<Service> serv = await db.GetServicesList();
                SelectList services = new SelectList(serv, "Id", "Service_Name");
                ViewBag.ServicesList = services;
                ViewBag.Services = await db.GetServicesList();
                return View(cl);
            }
            return NotFound();
        }

        [Route("Client/EditClient")]
        [HttpPost]
        [ActionName("EditClient")]
        public async Task<IActionResult> EditClient(Client cl, int[] selectedServices)
        {
            Client client = await db.EditPostClient(cl, selectedServices);
           
            if (ModelState.IsValid)
            {
                db.UpdateClient(client);
                return RedirectToAction("ShowAllClients");
            }
            else
            {
                ViewBag.Services = await db.GetServicesList();
                Client clients = await db.GetClientWithServices(cl.Id);
                return View(clients);
            }
        }

        [Route("Client/CreateClient")]
        public async Task<IActionResult> CreateClient()
        {
            var ServicesValue = await db.GetServicesList();
            if (ServicesValue != null)
            {
                List<Service> serv = ServicesValue;
                SelectList services = new SelectList(serv, "Id", "Service_Name");
                ViewBag.ServiceList = services;
                return View();
            }
            else
            {
                return NotFound();
            }
        }

        [Route("Client/CreateClient")]
        [HttpPost]
        public IActionResult CreateClient(Client cl)
        {
            if (ModelState.IsValid)
            {
                db.CreateClient(cl);
                return RedirectToAction("ShowAllClients");
            }
            return View();
        }

        [Route("Client/DeleteClient/{id?}")]
        [HttpGet]
        [ActionName("DeleteClient")]
        public async Task<IActionResult> ClientConfirmDelete(int? id)
        {
            if (id != null)
            {
                Client cl = await db.GetClient(id.Value);
                if (cl != null) return View(cl);
            }
            return NotFound();
        }

        [Route("Client/DeleteClient/{id?}")]
        [HttpPost]
        [ActionName("DeleteClient")]
        public IActionResult DeleteEntity(int? id)
        {
            if (id != null)
            {
                db.DeleteClient(id);
                return RedirectToAction("ShowAllClients");
            }
            return NotFound();
        }

        [Route("Client/ClientDetails/{id?}")]
        public async Task<IActionResult> ClientDetails(int? id)
        {
            if (id != null)
            {
                Client cl = await db.GetClientWithServices(id.Value);
                if (cl != null)
                {
                    return View(cl);
                }
            }
            return NotFound();
        }
    }
}
