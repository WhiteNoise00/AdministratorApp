using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdministratorApp.Models
{
    public class Service
    {
        public int Id { get; set; }

        //Name of service 
        [Required(ErrorMessage = "Enter service name:")]
        [StringLength(40)]
        [Display(Name = "Name of service:")]
        public string Service_Name { get; set; }

        //Service description
        [StringLength(200)]
        [Display(Name = "Service description:")]
        public string Service_Description { get; set; }

        //Calculate elapsed time in minutes
        [DefaultValue(false)]
        public bool Service_Time_Type_Minutes { get; set; }

        //Calculate elapsed time in hours
        [DefaultValue(true)]
        public bool Service_Time_Type_Hours { get; set; }

        //Link for intermediate table
        public virtual ICollection<ServiceForClient> ServicesForClients { get; set; }
    }
}
