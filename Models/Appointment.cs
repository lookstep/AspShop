using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAspShop.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        [Display(Name = "Appointment day")]
        public DateTime AppointmentDay { get; set; }
        [NotMapped]
        [Display(Name = "Appointment time")]
        public DateTime AppointmentTime { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(25)]
        [Display(Name = "Customer phone number")]
        public string CustomerPhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Customer email")]
        public string CustomerEmail { get; set; }
        [Display(Name = "Confirmed")]
        public bool IsConfirmed { get; set; }
    }
}
