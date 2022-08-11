using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AdministratorApp.Models
{
    public class Client
    {
        public int Id { get; set; }

        //Name of company
        [Required(ErrorMessage = "Enter the name of the client organization")]
        [StringLength(50)]
        [Display(Name = "Name of company:")]
        public string Client_Name { get; set; }

        //Contact person 
        [Required(ErrorMessage = "Enter contact name")]
        [StringLength(100)]
        [Display(Name = "Contact person:")]
        public string Client_Contact_Person { get; set; }

        //E-mail  
        [Required(ErrorMessage = "Enter your email address")]
        [Display(Name = "E-mail address:")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Client_Contact_Email { get; set; }

        //Feedback phone number 
        [Required(ErrorMessage = "Enter phone number")]
        [Display(Name = "Phone number for feedback:")]
        [StringLength(12)]
        public string Client_Contact_Phone { get; set; }

        //Link for intermediate table
        public virtual ICollection<ServiceForClient> ServicesForClients { get; set; }

    }
}
