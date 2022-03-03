using CoreAspShop.Data;
using CoreAspShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAspShop.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AppointmentViewModel AppointmentVM { get; set; } = new();
        public AppointmentsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(string searchKey)
        {
            if (searchKey is null)
                AppointmentVM.Appointments = await _db.Appointments.ToListAsync();
            else            
                AppointmentVM.Appointments = (await _db.Appointments.Where(x => x.CustomerName.ToLower().Contains(searchKey.ToLower()) ||
                                                                                x.CustomerEmail.ToLower().Contains(searchKey.ToLower()) ||
                                                                                x.CustomerPhoneNumber.ToLower().Contains(searchKey.ToLower())).ToListAsync());        
            return View(AppointmentVM);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();

            return View();
        }
    }
}
