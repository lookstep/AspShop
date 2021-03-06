using CoreAspShop.Data;
using CoreAspShop.Helpers;
using CoreAspShop.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAspShop.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class SpecialTagsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public SpecialTagsController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }
        public IActionResult Adding()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Adding(SpecialTag model)
        {
            if (!ModelState.IsValid)
                return View(model);
            _db.Add(model);
            await _db.SaveChangesAsync();
            TempData["SM"] = "Special Tag: Special tags added successfully";
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            return await FindPageAsync(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SpecialTag model)
        {
            if (model.Id != id)
                return NotFound();
            if (!ModelState.IsValid)
                return View(model);
            _db.Update(model);
            await _db.SaveChangesAsync();
            TempData["SM"] = "Special Tag: Special tags was edit";
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var findObject = await _db.SpecialTags.FindAsync(id);
            if (findObject == null)
                return NotFound();
            _db.SpecialTags.Remove(findObject);
            await _db.SaveChangesAsync();
            TempData["SM"] = "Special Tag: Special tags was deleted";
            return RedirectToAction(nameof(Index));
            
        }

        private async Task<IActionResult> FindPageAsync(int? id) 
        {
            if (id.IsNullable() && id == null)
            {
                return NotFound();
            }
            var findObject = await _db.SpecialTags.FindAsync(id);
            if (findObject == null)
                return NotFound();
            return View(findObject);
        }
    }
}
