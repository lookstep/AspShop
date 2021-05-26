using CoreAspShop.Data;
using CoreAspShop.Helpers;
using CoreAspShop.Models;
using CoreAspShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreAspShop.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.SuperAdminEndUser)]
    [Area(nameof(Admin))]
    public class AdminUsersController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AdminUsersController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.ApplicationUsers.ToListAsync());
        }
        public async Task<IActionResult> Edit(string id)
        {
            return await FindPageAsync(id);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> EditAdminUsersPost(string id, ApplicationUser model)
        {
            if (!id.Equals(model.Id))
                return NotFound();
            if (!ModelState.IsValid)
                return View(model);
            var user = await _db.ApplicationUsers.FindAsync(id);
            if (user == null)
                return NotFound();
            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string id)
        {
            return await FindPageAsync(id);
        }
        private async Task<IActionResult> FindPageAsync(string id)
        {
            if (id == null || id.Trim().Length == 0)
                return NotFound();
            var findObject = await _db.ApplicationUsers.FindAsync(id);
            if (findObject == null)
                return NotFound();
            return View(findObject);
        }
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Ban(string id, int banTime)
        {
            if (id == null)
                return NotFound();

            var user = await _db.ApplicationUsers.FindAsync(id);
            if (user == null)
                return NotFound();
            user.LockoutEnd = DateTime.Now.AddHours(banTime);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}
