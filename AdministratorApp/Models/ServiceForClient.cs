using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdministratorApp.Models
{
    public class ServiceForClient
    {
        public int Id { get; set; }

        //Service execution status
        [Display(Name = "Service completed: ")]
        public bool Service_For_Client_Execution_Status { get; set; }

        //Service payment status
        [Display(Name = "Service paid for:")]
        public bool Service_For_Client_Payment_Status { get; set; }

        //Elapsed time
        [Display(Name = "Elapsed time:")]
        public double Service_For_Client_Elapsed_Time { get; set; }

        //Service is measured in minutes
        [DefaultValue(false)]
        public bool Service_For_Client_Time_Type_Minutes { get; set; }

        //Service start date
        [Display(Name = "Service start date:")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Service_For_Client_Start_Date { get; set; }

        //Date of actual completion of the service
        [Display(Name = "Date of actual completion of the service:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Service_For_Client_Actual_End_Date { get; set; }

        //Deadline date
        [Display(Name = "Service deadline date:")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Service_For_Client_Deadline_Date { get; set; }

        //Service payment date
        [Display(Name = "Service payment date:")]
        [DisplayFormat(DataFormatString = "{YYYY-MM-DD}", ApplyFormatInEditMode = true)]
        public DateTime Service_For_Client_Payment_Date { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }
        public int ServiceId { get; set; }
        public Service Service { get; set; }
    }
}
