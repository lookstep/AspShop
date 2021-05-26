using CoreAspShop.Data;
using CoreAspShop.Extensions;
using CoreAspShop.Models;
using CoreAspShop.Models.ViewModels;
using CoreAspShop.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAspShop.Areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class ShopingCartController : Controller
    {
        private readonly ApplicationDbContext _db;
        [BindProperty]
        public ShopingCartViewModel ShopingCartVM { get; set; }

        public ShopingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShopingCartVM = new ShopingCartViewModel()
            {
                Products = new List<Product>()
            };
        }
        public async Task<IActionResult> Index()
        {
            var productsId = HttpContext.Session.Get<List<int>>(SD.SessionKey);
            if(productsId != null)
            {
                foreach(var el in productsId)
                {
                    var product = await _db.Products
                        .Include(x => x.SpecialTag)
                        .Include(x => x.ProductType)
                        .FirstOrDefaultAsync(x => x.Id == el);

                    ShopingCartVM.Products.Add(product);
                }
            }
            return View(ShopingCartVM);
        }
        [ActionName("Index")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PostIndex()
        {
            var productsId = HttpContext.Session.Get<List<int>>(SD.SessionKey);

            ShopingCartVM.Appointment.AppointmentDay = ShopingCartVM.Appointment.AppointmentDay
                                                                    .AddHours(ShopingCartVM.Appointment.AppointmentTime.Hour)
                                                                    .AddMinutes(ShopingCartVM.Appointment.AppointmentTime.Minute);

            //appointmentData.CustomerEmail = ShopingCartVM.Appointment.CustomerEmail;
            //appointmentData.CustomerName = ShopingCartVM.Appointment.CustomerName;
            //appointmentData.CustomerPhoneNumber = ShopingCartVM.Appointment.CustomerPhoneNumber;

            Appointment appointmentData = ShopingCartVM.Appointment;

            await _db.Appointments.AddAsync(appointmentData);
            await _db.SaveChangesAsync();

            int appointmentId = appointmentData.Id;

            foreach (var el in productsId)
            {
                //var appointment = _db.ProductsForAppointments
                //                    .Include(x => x.Appointment)
                //                    .Include(x => x.Product)
                //                    .FirstOrDefault();
                //appointment.AppointmentId = appointmentData.Id;
                //appointment.ProductId = el;

                var appointment = new ProductsForAppointment()
                {
                    AppointmentId = appointmentId,
                    ProductId = el
                };
                _db.ProductsForAppointments.Add(appointment);
            }
            await _db.SaveChangesAsync();

            var clearList = new List<int>();
            HttpContext.Session.Set(SD.SessionKey, clearList);
            return RedirectToAction(nameof(AppointmentConfirmation), new { id = appointmentId });
        }
        public IActionResult Remove(int id)
        {
            var ShopingCartList = HttpContext.Session.Get<List<int>>(SD.SessionKey);
            if(ShopingCartList.Count > 0 && ShopingCartList.Contains(id))           
                ShopingCartList.Remove(id);
            HttpContext.Session.Set(SD.SessionKey, ShopingCartList);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AppointmentConfirmation(int id)
        {
            ShopingCartVM.Appointment = await _db.Appointments.FindAsync(id);
            var productListObject = await _db.ProductsForAppointments.Where(x => x.Appointment.Id == id).ToListAsync();
            foreach(var el in productListObject)
            {
                ShopingCartVM.Products.Add(await _db.Products
                                                     .Include(x => x.ProductType)
                                                     .Include(x => x.SpecialTag)
                                                     .FirstOrDefaultAsync(x => x.Id == el.ProductId));
            }
            return View(ShopingCartVM);
        }
    }
}
