using CoreAspShop.Data;
using CoreAspShop.Extensions;
using CoreAspShop.Models;
using CoreAspShop.Models.ViewModels;
using CoreAspShop.Utility;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
                Products = _db.Products.ToList(),
                Appointment = new Appointment()
            };
        }
        public async Task<IActionResult> Index()
        {
            var productsId = HttpContext.Session.Get<List<int>>(SD.SessionKey);
            Product product;
            if(productsId != null)
            {
                for (int i = 0; i < productsId.Count; i++)
                {
                    product = await _db.Products.FindAsync(productsId[i]);
                    ShopingCartVM.Products[i] = product;
                }
            }
            return View(ShopingCartVM);
        }
    }
}
