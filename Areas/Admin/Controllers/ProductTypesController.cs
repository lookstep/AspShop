using CoreAspShop.Data;
using CoreAspShop.Helpers;
using CoreAspShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAspShop.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class ProductTypesController : Controller
    {
        // 2. Создаём переменную для кэширования методов Entity
        private readonly ApplicationDbContext _db;

        // 1. Создаём конструктор класса
        public ProductTypesController(ApplicationDbContext db)
        {
            // 3. Переносим данные из пафраметра конструктора в переменную
            _db = db;
        }
        
        public IActionResult Index()
        {
            // Задача: Получить из базы данных все типы продуктов и вернуть их в представление (в листе)
            //var ProductTypesFromDb = _db.ProductTypes.ToList();
            return View(_db.ProductTypes.ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType model)
        {
            // 1. Проверяем модель на валидность
            if (!ModelState.IsValid)
                return View(model);

            _db.Add(model);

            await _db.SaveChangesAsync();

            TempData["SM"] = $"Product type: {model.Name} was added";

            // 1.4 Переадресовываем на страницу вывода всех категорий
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            return await FindPageAsync(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductType model)
        {
            if (id != model.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(model);
            
            _db.Update(model);

            await _db.SaveChangesAsync();

            TempData["SM"] = $"Product type: {model.Name} was edit";

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int? id)
        {
            return await FindPageAsync(id);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await FindPageAsync(id);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int id)
        {
            var findIndo = await _db.ProductTypes.FindAsync(id);
            if (findIndo == null)
            {
                TempData["SM"] = $"Product type: {findIndo.Id} deleted filed. Pleas, try again";
                return RedirectToAction(nameof(Index));
            }
            _db.ProductTypes.Remove(findIndo);
            await _db.SaveChangesAsync();
            TempData["SM"] = $"Product type: {findIndo.Id} was deleted";
            return RedirectToAction(nameof(Index));
        }
        private async Task<IActionResult> FindPageAsync(int? id)
        {
            if (id.IsNullable() && id == null)
            {
                return NotFound();
            }
            var findObject = await _db.ProductTypes.FindAsync(id);
            if (findObject == null)
                return NotFound();
            return View(findObject);
        }
    }
}
