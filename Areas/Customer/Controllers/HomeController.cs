using CoreAspShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    }
}
