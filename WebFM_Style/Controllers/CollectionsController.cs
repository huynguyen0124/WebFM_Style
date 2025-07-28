
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFM_Style.Models;

namespace WebFM_Style.Controllers
{
    public class CollectionsController : Controller
    {
        private readonly FmStyleDbContext _context;

        public CollectionsController(FmStyleDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var collection = await _context.Collections.Where(x => x.Status == true).ToListAsync();
            return View(collection);
        }
        public IActionResult Details(int id)
        {
            var collection = _context.Collections.Include(x => x.CollectionProducts)
                .ThenInclude(x => x.Product).ThenInclude(x => x.Images)
                .Include(x => x.CollectionProducts).ThenInclude(x => x.Product)
                .ThenInclude(x => x.ProductSizeColors).FirstOrDefault(x => x.Id == id);

            return View(collection);
        }
    }
}
