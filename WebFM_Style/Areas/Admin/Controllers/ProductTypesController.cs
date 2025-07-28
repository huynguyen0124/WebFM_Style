//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using WebFM_Style.Models;

//namespace WebFM_Style.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class ProductTypesController : Controller
//    {
//        private readonly FmStyleDbContext _context;

//        public ProductTypesController(FmStyleDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            var fmStyleDbContext = _context.ProductTypes.Include(p => p.Category);
//            return View(await fmStyleDbContext.ToListAsync());
//        }

//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var productType = await _context.ProductTypes
//                .Include(p => p.Category)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (productType == null)
//            {
//                return NotFound();
//            }

//            return View(productType);
//        }

//        public IActionResult Create()
//        {
//            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(ProductType productType)
//        {
//            productType.Status = 1; 
//                _context.Add(productType);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productType.CategoryId);
//            //return View(productType);
//        }

//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var productType = await _context.ProductTypes.FindAsync(id);
//            if (productType == null)
//            {
//                return NotFound();
//            }
//            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productType.CategoryId);
//            return View(productType);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, ProductType productType)
//        {
//            if (id != productType.Id)
//            {
//                return NotFound();
//            }

//                try
//                {
//                    _context.Update(productType);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ProductTypeExists(productType.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productType.CategoryId);
//            //return View(productType);
//        }

//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var productType = await _context.ProductTypes
//                .Include(p => p.Category)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (productType == null)
//            {
//                return NotFound();
//            }

//            return View(productType);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var productType = await _context.ProductTypes.FindAsync(id);
//            if (productType != null)
//            {
//                _context.ProductTypes.Remove(productType);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ProductTypeExists(int id)
//        {
//            return _context.ProductTypes.Any(e => e.Id == id);
//        }
//    }
//}

// cái mới ở dưới

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFM_Style.Models;

namespace WebFM_Style.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
    {
        private readonly FmStyleDbContext _context;

        public ProductTypesController(FmStyleDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var fmStyleDbContext = _context.ProductTypes.Include(p => p.Category);
            return View(await fmStyleDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(x => x.Status == true), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType productType)
        {
            productType.Status = 1;
            _context.Add(productType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productType.CategoryId);
            //return View(productType);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories.Where(x => x.Status == true), "Id", "Name", productType.CategoryId);
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductType productType)
        {
            if (id != productType.Id)
            {
                return NotFound();
            }

            try
            {
                _context.Update(productType);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductTypeExists(productType.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", productType.CategoryId);
            //return View(productType);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productType = await _context.ProductTypes
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productType == null)
            {
                return NotFound();
            }

            return View(productType);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productType = await _context.ProductTypes.FindAsync(id);
            if (productType != null)
            {
                _context.ProductTypes.Remove(productType);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductTypeExists(int id)
        {
            return _context.ProductTypes.Any(e => e.Id == id);
        }
    }
}
