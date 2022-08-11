using AdministratorApp.Data;
using AdministratorApp.Interfaces;
using AdministratorApp.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdministratorApp.Repository
{
    public class AdministratorAppRepository: IAdministratorAppRepository

    {
        private readonly ApplicationDbContext db;

        public AdministratorAppRepository(ApplicationDbContext context)
        {
            db = context;
        }

        /*Work with "Client"*/
        public void CreateClient(Client cl)
        {
            db.Clients.Add(cl);
            db.SaveChanges();
        }
        public IQueryable<Client> GetClientsListForPage(int? id, string name)
        {
            IQueryable<Client> clients = db.Clients
                   .Include(s => s.ServicesForClients)
                   .ThenInclude(e => e.Service);

            if (id != null && id != 0)
            {
                clients = clients.Where(e => e.ServicesForClients.Any(t=>t.ServiceId==id));
            }

            if (!String.IsNullOrEmpty(name))
            {
                clients = clients.Where(x => EF.Functions.Like(x.Client_Name, $"%{name}%"));
            }

            if (clients != null)
            {
                return clients.OrderBy(o => o.Client_Name);
            }
            else
            {
                return null;
            }
        }

        public async Task<Client> GetClient(int id)
        {
            return await db.Clients.FirstOrDefaultAsync(e => e.Id == id);
        }

        public void UpdateClient(Client client)
        {
            db.Clients.Update(client);
            db.SaveChanges();
        }

        public async Task<Client> GetClientWithServices(int id)
        {
            Client cl = await db.Clients
                   .Include(s => s.ServicesForClients)
                   .ThenInclude(e => e.Service)
                   .AsNoTracking()
                   .FirstOrDefaultAsync(m => m.Id == id);
            return cl;
        }

        public async Task<Client> EditPostClient(Client cl, int[] selectedServices)
        {
            Client client = await db.Clients
                    .Include(s => s.ServicesForClients)
                    .ThenInclude(e => e.Service)
                    .FirstOrDefaultAsync(m => m.Id == cl.Id);

            var clientServices = new HashSet<int>(client.ServicesForClients.Select(c => c.Service.Id));
            var oldClient = client.ServicesForClients.ToList();
            client.Client_Name = cl.Client_Name;
            client.Client_Contact_Email = cl.Client_Contact_Email;
            client.Client_Contact_Person = cl.Client_Contact_Person;
            client.Client_Contact_Phone = cl.Client_Contact_Phone;
            client.ServicesForClients.Clear();

            foreach (var service in db.Services)
            {
                if (selectedServices.Contains(service.Id))
                {
                    if (!clientServices.Contains(service.Id))
                    {
                        ServiceForClient serv = new ServiceForClient
                        {
                            Client = cl,
                            ServiceId = service.Id
                        };
                        client.ServicesForClients.Add(serv);   
                    }
                    else
                    {
                        foreach (var s in oldClient)
                        {
                            client.ServicesForClients.Add(s);
                        }
                    }
                }
                 
            }
            db.SaveChanges();
            return client;
        }

        public async Task<List<Client>> GetClientList()
        {
            return await db.Clients.ToListAsync();
        }

        public void DeleteClient(int? id)
        {
            if (id != null)
            {
                Client cl = new Client { Id = id.Value };
                db.Entry(cl).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public async Task<List<Service>> GetServicesList()
        {
            return await db.Services.ToListAsync();
        }


        /*Work with "Service"*/
        public IQueryable<Service> GetServicesListForPage(int? id, string name)
        {
            IQueryable<Service> services = db.Services
                   .Include(s => s.ServicesForClients)
                   .ThenInclude(e => e.Client);

            if (id != null && id != 0)
            {
                services = services.Where(e => e.ServicesForClients.Any(t=>t.ClientId==id));
            }

            if (!String.IsNullOrEmpty(name))
            {
                services = services.Where(x => EF.Functions.Like(x.Service_Name, $"%{name}%"));
            }

            if (services != null)
            {
                return services.OrderBy(o => o.Service_Name);
            }
            else
            {
                return null;
            }
        }

        public async Task<Service> GetService(int id)
        {
            return await db.Services.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<Service> GetServiceWithClients(int id)
        {
            return await db.Services
                    .Include(s => s.ServicesForClients)
                    .ThenInclude(e => e.Client)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.Id == id);
        }

        public void CreateService(Service serv, bool selectedTime)
        {
            if (selectedTime == true)
            {
                serv.Service_Time_Type_Minutes = true;
            }
            else
            {
                serv.Service_Time_Type_Hours = true;
            }
            db.Services.Add(serv);
            db.SaveChanges();
        }

        public Service EditPostService(Service serv)
        {
            Service service = db.Services.FirstOrDefault(e => e.Id == serv.Id);
            service.Service_Name = serv.Service_Name;
            service.Service_Description = serv.Service_Description;
            service.Service_Time_Type_Minutes = serv.Service_Time_Type_Minutes;
            return service;
        }

        public void UpdateService(Service service)
        {
            db.Services.Update(service);
            db.SaveChanges();
        }

        public void DeleteService(int? id)
        {
            Service serv = new Service { Id = id.Value };
            db.Entry(serv).State = EntityState.Deleted;
            db.SaveChanges();
        }

        /*Work with "ServiceForClient"*/
        public async Task<ServiceForClient> ServiceClientGet(int? client_id, int? service_id)
        {
            Client client = await db.Clients.Include(s => s.ServicesForClients).FirstOrDefaultAsync(s => s.Id == client_id);
            Service service = await db.Services.FirstOrDefaultAsync(c => c.Id == service_id);

            bool time_type = service.Service_Time_Type_Minutes;

            if (client != null && service != null)
            {
                return client.ServicesForClients.FirstOrDefault(sc => sc.ServiceId == service.Id);
            }
            else return null;
        }

        public ServiceForClient ServiceClientEditPost(ServiceForClient serv, int client_id, int service_id)
        {
            Client client = db.Clients.Include(s => s.ServicesForClients).FirstOrDefault(s => s.Id == client_id);
            Service service = db.Services.FirstOrDefault(c => c.Id == service_id);
            bool time_type = service.Service_Time_Type_Minutes;

            if (client != null && service != null)
            {
                ServiceForClient srcl = client.ServicesForClients.FirstOrDefault(sc => sc.ServiceId == service.Id);
                client.ServicesForClients.Remove(srcl);

                DateTime date_start, date_end, deadline;

                if (serv.Service_For_Client_Start_Date.ToString() == "01.01.0001 0:00:00")
                {
                    date_start= DateTime.Today;
                }
                else
                {
                    date_start = serv.Service_For_Client_Start_Date;
                }

                if (serv.Service_For_Client_Actual_End_Date.ToString() == "01.01.0001 0:00:00")
                {
                    date_end = date_start.AddDays(3);
                }
                else
                {
                    date_end = serv.Service_For_Client_Actual_End_Date;
                }

                if (serv.Service_For_Client_Deadline_Date.ToString() == "01.01.0001 0:00:00")
                {
                    deadline = date_start.AddDays(3);
                }
                else
                {
                    deadline = serv.Service_For_Client_Deadline_Date;
                }

                client.ServicesForClients.Add(new ServiceForClient
                    {
                        Service = service,
                        Service_For_Client_Execution_Status = serv.Service_For_Client_Execution_Status,
                        Service_For_Client_Payment_Status = serv.Service_For_Client_Payment_Status,
                        Service_For_Client_Start_Date = date_start,
                        Service_For_Client_Actual_End_Date = date_end,
                        Service_For_Client_Deadline_Date = deadline,
                        Service_For_Client_Payment_Date = serv.Service_For_Client_Payment_Date,
                        Service_For_Client_Time_Type_Minutes = time_type,
                        Service_For_Client_Elapsed_Time = serv.Service_For_Client_Elapsed_Time
                    }
                );
                db.SaveChanges();
                return srcl;
            }
            else return null; 
        }

        public void ServiceClientDelete(int? client_id, int? service_id)
        {
            if (client_id != null && service_id != null)
            {
                Client client = db.Clients.Include(s => s.ServicesForClients).FirstOrDefault(s => s.Id == client_id);
                Service service = db.Services.FirstOrDefault(c => c.Id == service_id);
                if (client != null && service != null)
                {
                    var srcl = client.ServicesForClients.FirstOrDefault(sc => sc.ServiceId == service.Id);
                    client.ServicesForClients.Remove(srcl);
                    db.SaveChanges();
                }
            }
        }
    }
}
