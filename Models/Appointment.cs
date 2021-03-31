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
        public DateTime AppointmentDay { get; set; }
        [NotMapped]
        public DateTime AppointmentTime { get; set; }
        [Required]
        [MinLength(2)]
        [MaxLength(30)]
        public string CustomerName { get; set; }
        [Required]
        [MinLength(5)]
        [MaxLength(25)]
        public string CustomerPhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string CustomerEmail { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
