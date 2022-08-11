using AdministratorApp.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdministratorApp.Models;

namespace AdministratorApp.Controllers
{
    public class ServiceForClientController : Controller
    {
        private readonly IAdministratorAppRepository db;

        public ServiceForClientController(IAdministratorAppRepository context)
        {
            db = context;
        }

        [HttpGet]
        [Route("ServiceForClient/EditServiceForClient")]
        public async Task<IActionResult> EditServiceForClient(int? client_id, int? service_id, string service_name)
        {
            ViewBag.Name = service_name;
            var service = await db.GetService(service_id.Value);
            ViewBag.Minutes = service.Service_Time_Type_Minutes;

            var serv = await db.ServiceClientGet(client_id.Value, service_id.Value);
            if (serv != null)
            {
                return View(serv);
            }
            else { return NotFound(); }
        }

        [Route("ServiceForClient/EditServiceForClient")]
        [HttpPost]
        public IActionResult EditServiceForClient(ServiceForClient serv, int client_id, int service_id)
        {
            ServiceForClient servcl = db.ServiceClientEditPost(serv, client_id, service_id);
            if (servcl != null)
            {
                return RedirectToAction("ShowAllClients", "Client");
            }
            else { return NotFound(); }
        }

        [Route("ServiceForClient/DeleteServiceForClient")]
        [HttpGet]
        [ActionName("DeleteServiceForClient")]
        public async Task<IActionResult> ServiceClientConfirmDelete(int? client_id, int? service_id)
        {
            if (client_id != null && service_id != null)
            {
                var srcl = await db.ServiceClientGet(client_id.Value, service_id.Value);
                return View(srcl);
            }
            return NotFound();
        }

        [Route("ServiceForClient/DeleteServiceForClient")]
        [HttpPost]
        [ActionName("DeleteServiceForClient")]
        public IActionResult DeleteServiceForClient(int? client_id, int? service_id)
        {
            if (client_id != null && service_id != null)
            {
                db.ServiceClientDelete(client_id, service_id);
                return RedirectToAction("ShowAllClients", "Client");
            }
            return NotFound();
        }
    }
}
