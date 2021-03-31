using CoreAspShop.Data;
using CoreAspShop.Extensions;
using CoreAspShop.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CoreAspShop.Areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.Products
                .Include(x => x.ProductType)
                .Include(x => x.SpecialTag).ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id is null)
                return NotFound();
            var product = await _db.Products
                .Include(x => x.ProductType)
                .Include(x => x.SpecialTag)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product is null)
                return NotFound();
            return View(product);
        }
        [HttpPost]
        [ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id)
        {
            // Создать массив типа int и записать в него десериализованные данные из сессии
            List<int> listOfShoppingCart = HttpContext.Session.Get<List<int>>(SD.SessionKey);

            // Проверяем, если массив null, то создаём новый экземпляр массива
            if (listOfShoppingCart is null)
                listOfShoppingCart = new();

            // Добавляем полученный через параметр ID товара в массив
            listOfShoppingCart.Add(id);

            // Сериализуем и записываем в сессию массив с ID товаров
            HttpContext.Session.Set(SD.SessionKey, listOfShoppingCart);

            var product = await _db.Products
                .Include(x => x.ProductType)
                .Include(x => x.SpecialTag)
                .FirstOrDefaultAsync(x => x.Id == id);

            // Переадресовываем на метод Index
            return View(product);
        }
        public IActionResult Remove(int id)
        {
            var listOfShopinfCart = HttpContext.Session.Get<List<int>>(SD.SessionKey);
            if (listOfShopinfCart.Count > 0 && listOfShopinfCart.Contains(id))
                listOfShopinfCart.Remove(id);
            HttpContext.Session.Set(SD.SessionKey, listOfShopinfCart);
            //TempData["SM"] = "Product removed from your cart";
            return RedirectToAction(nameof(Details), new { id = id });
        }
    }
}
