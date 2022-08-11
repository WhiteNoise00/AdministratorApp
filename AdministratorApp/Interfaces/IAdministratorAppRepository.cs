using AdministratorApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdministratorApp.Interfaces
{
    public interface IAdministratorAppRepository
    {

        void CreateClient(Client cl);
        IQueryable<Client> GetClientsListForPage(int? id, string name);
        Task<List<Client>> GetClientList();
        Task<Client> GetClientWithServices(int id);
        Task<Client> EditPostClient(Client cl, int[] selectedServices);
        void UpdateClient(Client client);
        void DeleteClient(int? id);
        Task<Client> GetClient(int id);

        Task<List<Service>> GetServicesList();
        void CreateService(Service serv, bool selectedTime);
        IQueryable<Service> GetServicesListForPage(int? id, string name);
        Task<Service> GetServiceWithClients(int id);
        Task<Service> GetService(int id);
        Service EditPostService(Service serv);
        void UpdateService(Service service);
        void DeleteService(int? id);
        
         Task<ServiceForClient> ServiceClientGet(int? client_id, int? service_id);
         ServiceForClient ServiceClientEditPost(ServiceForClient serv, int client_id, int service_id);
         void ServiceClientDelete(int? client_id, int? service_id);
    }
}
