using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAspShop.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public List<Appointment> Appointments { get; set; }
        public ProductsForAppointment ProductsForAppointment { get; set; }
    }
}
