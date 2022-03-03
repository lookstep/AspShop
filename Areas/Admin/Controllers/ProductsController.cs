using CoreAspShop.Data;
using CoreAspShop.Models.ViewModels;
using CoreAspShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using CoreAspShop.Utility;
using Microsoft.AspNetCore.Authorization;
using System;

namespace CoreAspShop.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area(nameof(Admin))]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _hostingEnvironment;

        [BindProperty]
        public ProductsViewModel ProductVM { get; set; }

        public ProductsController(ApplicationDbContext db, IWebHostEnvironment hostingEnvironment)
        {
            _db = db;
            _hostingEnvironment = hostingEnvironment;

            ProductVM = new ProductsViewModel()
            {
                Product = new Product(),
                ProductTypes = _db.ProductTypes.ToList(),
                SpecialTags = _db.SpecialTags.ToList()
            };
        }
        public async Task<IActionResult> Index()
        {
            var products = _db.Products
                              .Include(x => x.ProductType)
                              .Include(x => x.SpecialTag);

            return View(await products.ToListAsync());
        }
        public IActionResult Create()
        {
            return View(ProductVM);
        }
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost()
        {
            // Проверяем модель на валидность
            if (!ModelState.IsValid)
                return View(ProductVM);
            // Если модель валидна, добавляем информацию в сущности Entity
            await _db.Products.AddAsync(ProductVM.Product);

            // Сохраняем изменения в базе данных
            await _db.SaveChangesAsync();

            // Логика сохранения картинок //
            // Формируем путь до корневого каталога проекта
            string webRootPath = _hostingEnvironment.WebRootPath;

            // Получаем файлы из формы
            var files = HttpContext.Request.Form.Files;

            // Получаем сохранённый товар из базы данных
            var productFromDb = await _db.Products.FindAsync(ProductVM.Product.Id);

            // Проверяем, получены ли файлы из формы
            if (files.Count != 0)
            {

                // Комбинируем полный путь к каталогу сохранения
                string uploadPath = Path.Combine(webRootPath, SD.ImageFolder);

                // Получаем расширение файла
                var extension = Path.GetExtension(files[0].FileName);

                if (extension != ".jpg" && extension != ".png")
                {
                    TempData["SM"] = "Photo have not a true extension.";
                    var defaultPhotoUploadPath = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);
                    System.IO.File.Copy(defaultPhotoUploadPath, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductVM.Product.Id + ".png");
                    productFromDb.Image = $"\\{SD.ImageFolder}\\{ProductVM.Product.Id}.png";
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }

                // Сохраняем изображение на сервере
                using var fs = new FileStream(Path.Combine(uploadPath, ProductVM.Product.Id + extension), FileMode.Create);
                await files[0].CopyToAsync(fs);

                // Записываем в базу данных путь к сохранённому изображению
                productFromDb.Image = $"\\{SD.ImageFolder}\\{ProductVM.Product.Id}{extension}";
            }
            // Если проверка не прошла
            else
            {
                // Формируем путь к изображению по умолчанию
                var uploadPath = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductImage);

                // Копируем дефолтное изображение
                System.IO.File.Copy(uploadPath, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductVM.Product.Id + ".png");

                // Записываем в базу данных путь к скопированному изображению
                productFromDb.Image = $"\\{SD.ImageFolder}\\{ProductVM.Product.Id}.png";
            }

            // Сохраняем изменения в базе данных
            await _db.SaveChangesAsync();
            // Добавляем сообщение о успешном добавлении
            TempData["SM"] = $"Product: {ProductVM.Product.Name} added succesfull";
            // Переадресовываем на страницу Index
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            return await FindPageAsync(id);
        }
        public async Task<IActionResult> Details(int? id)
        {
            return await FindPageAsync(id);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            return await FindPageAsync(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> EditAsync()
        {
            if (!ModelState.IsValid)
                return View(ProductVM);
            var rootPath = _hostingEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;

            var tempProduct = await _db.Products.FindAsync(ProductVM.Product.Id);
            if (tempProduct is null)
                return NotFound();

            if (files.Count != 0)
            {

                var uploadParh = Path.Combine(rootPath, SD.ImageFolder);

                var extension = Path.GetExtension(files[0].FileName);

                if (extension != ".jpg" && extension != ".png")
                {

                    tempProduct.Name = ProductVM.Product.Name;
                    tempProduct.Available = ProductVM.Product.Available;
                    tempProduct.Price = ProductVM.Product.Price;
                    tempProduct.ShadeColor = ProductVM.Product.ShadeColor;
                    tempProduct.SpecialTagsId = ProductVM.Product.SpecialTagsId;
                    tempProduct.ProductTypeId = ProductVM.Product.ProductTypeId;


                    _db.Update(tempProduct);

                    await _db.SaveChangesAsync();

                    TempData["SM"] = "Photo have not a true extension.";

                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    var oldExtension = Path.GetExtension(tempProduct.Image);

                    if (System.IO.File.Exists(Path.Combine(rootPath, tempProduct.Id + oldExtension)) is true)
                        System.IO.File.Delete(Path.Combine(rootPath, tempProduct.Id + oldExtension));

                    using var fs = new FileStream(Path.Combine(uploadParh, ProductVM.Product.Id + extension), FileMode.Create);

                    await files[0].CopyToAsync(fs);

                    tempProduct.Image = $"\\{SD.ImageFolder}\\{ProductVM.Product.Id}{extension}";
                }
            }

            tempProduct.Name = ProductVM.Product.Name;
            tempProduct.Available = ProductVM.Product.Available;
            tempProduct.Price = ProductVM.Product.Price;
            tempProduct.ShadeColor = ProductVM.Product.ShadeColor;
            tempProduct.SpecialTagsId = ProductVM.Product.SpecialTagsId;
            tempProduct.ProductTypeId = ProductVM.Product.ProductTypeId;


            _db.Update(tempProduct);

            await _db.SaveChangesAsync();

            TempData["SM"] = $"Product: {ProductVM.Product.Name} edit succesfull";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteAsync(int? id)
        {
            if (id is null)
                return NotFound();

            var rootPath = _hostingEnvironment.WebRootPath;

            var tempProduct = await _db.Products.FindAsync(id);
            if (tempProduct is null)
                return NotFound();

            var uploadPath = Path.Combine(rootPath, SD.ImageFolder);
            var extension = Path.GetExtension(tempProduct.Image);

            if (System.IO.File.Exists(Path.Combine(uploadPath, tempProduct.Id + extension)))
                System.IO.File.Delete(Path.Combine(uploadPath, tempProduct.Id + extension));

            TempData["SM"] = $"Product: delete succesfull";

            _db.Products.Remove(tempProduct);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<IActionResult> FindPageAsync(int? id)
        {
            if (id is null)
                return NotFound();
            ProductVM.Product = await _db.Products.FindAsync(id);
            if (ProductVM.Product is null)
                return NotFound();
            return View(ProductVM);
        }
    }
}
