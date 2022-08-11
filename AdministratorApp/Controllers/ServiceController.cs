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
    public class ServiceController : Controller
    {
        private readonly IAdministratorAppRepository db;

        public ServiceController(IAdministratorAppRepository context)
        {
            db = context;
        }

        public async Task<IActionResult> ShowAllServices(int? id, string name, int page = 1)
        {
            var services = db.GetServicesListForPage(id, name);
            int pageSize = 4;
            var count = services.Count();
            var items = await services.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            List<Client> client_values = await db.GetClientList();
            client_values.Insert(0, new Client { Client_Name = "All", Id = 0 });
            SelectList clients = new SelectList(client_values, "Id", "Client_Name");
            ViewBag.ClientList = clients;

            PageViewModel pageViewModel = new PageViewModel(count, page, pageSize);
            IndexViewModel viewModel = new IndexViewModel
            {
                PageViewModel = pageViewModel,
                Services = items
            };

            if (services != null)
            {
                return View(viewModel);
            }
            else
            { 
                return NotFound(); 
            }
        }

        [Route("Service/CreateService")]
        public IActionResult CreateService()
        {
            return View();
        }

        [Route("Service/CreateService")]
        [HttpPost]
        public IActionResult CreateService(Service serv, bool selectedTime)
        {
            if (ModelState.IsValid)
            {
                db.CreateService(serv, selectedTime);
                return RedirectToAction("ShowAllServices");
            }
            return View();
        }

        [HttpGet]
        [Route("Service/EditService")]
        public async Task<IActionResult> EditService(int? id)
        {
            if (id != null)
            {
                Service serv = await db.GetServiceWithClients(id.Value);
                return View(serv);
            }
            return NotFound();
        }

        [Route("Service/EditService")]
        [HttpPost]
        public IActionResult EditService(Service serv)
        {
            Service service = db.EditPostService(serv);
            service.Service_Name = serv.Service_Name;
            service.Service_Description = serv.Service_Description;
            service.Service_Time_Type_Minutes = serv.Service_Time_Type_Minutes;
            service.Service_Time_Type_Hours = serv.Service_Time_Type_Hours;

            if (ModelState.IsValid)
            {
                db.UpdateService(service);
                return RedirectToAction("ShowAllServices");
            }
            else
            {
                return View(service);
            }
        }

        [Route("Service/DeleteService/{id?}")]
        [HttpGet]
        [ActionName("DeleteService")]
        public async Task<IActionResult> ServiceConfirmDelete(int? id)
        {
            if (id != null)
            {
                Service serv = await db.GetService(id.Value);
                if (serv != null) return View(serv);
            }
            return NotFound();
        }

        [Route("Service/DeleteService/{id?}")]
        [HttpPost]
        public IActionResult ServiceDelete(int? id)
        {
            if (id != null)
            {
                db.DeleteService(id.Value);
                return RedirectToAction("ShowAllServices");
            }
            return NotFound();
        }
    }
}
