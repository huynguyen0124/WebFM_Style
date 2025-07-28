//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AspNetCoreHero.ToastNotification.Abstractions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using WebFM_Style.Helper;
//using WebFM_Style.Models;

//namespace WebFM_Style.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class ProductSizeColorsController : Controller
//    {
//        private readonly FmStyleDbContext _context;
//        public INotyfService _notyfService { get; }
//        public ProductSizeColorsController(FmStyleDbContext context, INotyfService notyfService)
//        {
//            _context = context;
//            _notyfService = notyfService;
//        }
//        public async Task<IActionResult> Index()
//        {
//            var fmStyleDbContext = _context.ProductSizeColors.Include(p => p.Color).Include(p => p.Product).ThenInclude(x=>x.Images).Include(p => p.ProductInventory).Include(p => p.Size);
//            return View(await fmStyleDbContext.ToListAsync());
//        }

//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var productSizeColor = await _context.ProductSizeColors
//                .Include(p => p.Color)
//                .Include(p => p.Product)
//                .Include(p => p.ProductInventory)
//                .Include(p => p.Size)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (productSizeColor == null)
//            {
//                return NotFound();
//            }

//            return View(productSizeColor);
//        }

//        public IActionResult Create()
//        {
//            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Color1");
//            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
//            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Size1");
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(ProductSizeColor productSizeColor)
//        {
//            if (ModelState.IsValid)
//            {
//                var product = _context.Products.FirstOrDefault(p => p.Id == productSizeColor.ProductId);
//                var color = _context.Colors.FirstOrDefault(p => p.Id == productSizeColor.ColorId);
//                var size = _context.Sizes.FirstOrDefault(p => p.Id == productSizeColor.SizeId);

//                var inventory = new ProductsInventory();
//                inventory.Quantity = 0;
//                inventory.QuantitySold = 0;
//                _context.Add(inventory);
//                _context.SaveChanges();
//                productSizeColor.ProductInventoryId = inventory.Id;
//                productSizeColor.Code = Utilities.ToUrlFriendly(product.Name + "-" + size.Size1 + "-" + color.Color1);
//                _context.Add(productSizeColor);
//                await _context.SaveChangesAsync();
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Color1", productSizeColor.ColorId);
//            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productSizeColor.ProductId);
//            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Size1", productSizeColor.SizeId);
//            return View(productSizeColor);
//        }

//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var productSizeColor = await _context.ProductSizeColors.Include(x=>x.ProductInventory).FirstOrDefaultAsync(x=>x.Id == id);
//            if (productSizeColor == null)
//            {
//                return NotFound();
//            }
//            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Color1", productSizeColor.ColorId);
//            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productSizeColor.ProductId);
//            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Size1", productSizeColor.SizeId);
//            return View(productSizeColor);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id,ProductSizeColor productSizeColor)
//        {
//            if (id != productSizeColor.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(productSizeColor);
//                    await _context.SaveChangesAsync();
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ProductSizeColorExists(productSizeColor.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Color1", productSizeColor.ColorId);
//            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productSizeColor.ProductId);
//            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Size1", productSizeColor.SizeId);
//            return View(productSizeColor);
//        }

//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var productSizeColor = await _context.ProductSizeColors
//                .Include(p => p.Color)
//                .Include(p => p.Product)
//                .Include(p => p.ProductInventory)
//                .Include(p => p.Size)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (productSizeColor == null)
//            {
//                return NotFound();
//            }

//            return View(productSizeColor);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var productSizeColor = await _context.ProductSizeColors.FindAsync(id);
//            if (productSizeColor != null)
//            {
//                _context.ProductSizeColors.Remove(productSizeColor);
//            }

//            await _context.SaveChangesAsync();
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ProductSizeColorExists(int id)
//        {
//            return _context.ProductSizeColors.Any(e => e.Id == id);
//        }
//    }
//}

// cái mới ở dưới
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFM_Style.Helper;
using WebFM_Style.Models;

namespace WebFM_Style.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductSizeColorsController : Controller
    {
        private readonly FmStyleDbContext _context;
        public INotyfService _notyfService { get; }
        public ProductSizeColorsController(FmStyleDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index()
        {
            var fmStyleDbContext = _context.ProductSizeColors.Include(p => p.Color).Include(p => p.Product).ThenInclude(x => x.Images).Include(p => p.ProductInventory).Include(p => p.Size);
            return View(await fmStyleDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSizeColor = await _context.ProductSizeColors
                .Include(p => p.Color)
                .Include(p => p.Product)
                .Include(p => p.ProductInventory)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSizeColor == null)
            {
                return NotFound();
            }

            return View(productSizeColor);
        }

        public IActionResult Create()
        {
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Color1");
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.Status == 1), "Id", "Name");
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Size1");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductSizeColor productSizeColor)
        {
            if (ModelState.IsValid)
            {
                var product = _context.Products.FirstOrDefault(p => p.Id == productSizeColor.ProductId);
                var color = _context.Colors.FirstOrDefault(p => p.Id == productSizeColor.ColorId);
                var size = _context.Sizes.FirstOrDefault(p => p.Id == productSizeColor.SizeId);

                var inventory = new ProductsInventory();
                inventory.Quantity = 0;
                inventory.QuantitySold = 0;
                _context.Add(inventory);
                _context.SaveChanges();
                productSizeColor.ProductInventoryId = inventory.Id;
                productSizeColor.Code = Utilities.ToUrlFriendly(product.Name + "-" + size.Size1 + "-" + color.Color1);
                _context.Add(productSizeColor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Color1", productSizeColor.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.Status == 1), "Id", "Name", productSizeColor.ProductId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Size1", productSizeColor.SizeId);
            return View(productSizeColor);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSizeColor = await _context.ProductSizeColors.Include(x => x.ProductInventory).FirstOrDefaultAsync(x => x.Id == id);
            if (productSizeColor == null)
            {
                return NotFound();
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Color1", productSizeColor.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.Status == 1), "Id", "Name", productSizeColor.ProductId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Size1", productSizeColor.SizeId);
            return View(productSizeColor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductSizeColor productSizeColor)
        {
            if (id != productSizeColor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSizeColor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSizeColorExists(productSizeColor.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ColorId"] = new SelectList(_context.Colors, "Id", "Color1", productSizeColor.ColorId);
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.Status == 1), "Id", "Name", productSizeColor.ProductId);
            ViewData["SizeId"] = new SelectList(_context.Sizes, "Id", "Size1", productSizeColor.SizeId);
            return View(productSizeColor);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSizeColor = await _context.ProductSizeColors
                .Include(p => p.Color)
                .Include(p => p.Product)
                .Include(p => p.ProductInventory)
                .Include(p => p.Size)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSizeColor == null)
            {
                return NotFound();
            }

            return View(productSizeColor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productSizeColor = await _context.ProductSizeColors.FindAsync(id);
            if (productSizeColor != null)
            {
                _context.ProductSizeColors.Remove(productSizeColor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSizeColorExists(int id)
        {
            return _context.ProductSizeColors.Any(e => e.Id == id);
        }
    }
}

