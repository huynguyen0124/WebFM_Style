//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using AspNetCoreHero.ToastNotification.Abstractions;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using WebFM_Style.Models;

//namespace WebFM_Style.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class ReceiptProductsController : Controller
//    {
//        private readonly FmStyleDbContext _context;

//        public INotyfService _notyfService { get; }
//        public ReceiptProductsController(FmStyleDbContext context, INotyfService notyfService)
//        {
//            _context = context;
//            _notyfService = notyfService;
//        }
//        public async Task<IActionResult> Index()
//        {
//            var fmStyleDbContext = _context.ReceiptProducts.Include(r => r.ProductSizeColor);
//            return View(await fmStyleDbContext.ToListAsync());
//        }

//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var receiptProduct = await _context.ReceiptProducts
//                .Include(r => r.ProductSizeColor)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (receiptProduct == null)
//            {
//                return NotFound();
//            }

//            return View(receiptProduct);
//        }

//        public IActionResult Create()
//        {
//            ViewData["ProductSizeColorId"] = new SelectList(_context.ProductSizeColors, "Id", "Code");
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(ReceiptProduct receiptProduct)
//        {

//            if (ModelState.IsValid)
//            {
//                var inventory = _context.ProductsInventorys.Include(r => r.ProductSizeColors).FirstOrDefault(x=>x.ProductSizeColors.FirstOrDefault().Id == receiptProduct.ProductSizeColorId);
//                inventory.Quantity += receiptProduct.Quantity;
//                receiptProduct.Status = 1;
//                _context.Update(inventory);

//                _context.Add(receiptProduct);
//                await _context.SaveChangesAsync();
//                _notyfService.Success("Thêm sản phẩm thành công.");
//                return RedirectToAction(nameof(Index));
//            }
//            ViewData["ProductSizeColorId"] = new SelectList(_context.ProductSizeColors, "Id", "Code", receiptProduct.ProductSizeColorId);
//            return View(receiptProduct);
//        }
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var receiptProduct = await _context.ReceiptProducts.FindAsync(id);
//            if (receiptProduct == null)
//            {
//                return NotFound();
//            }
//            ViewData["ProductSizeColorId"] = new SelectList(_context.ProductSizeColors, "Id", "SCodeize", receiptProduct.ProductSizeColorId);
//            return View(receiptProduct);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, ReceiptProduct receiptProduct)
//        {
//            if (id != receiptProduct.Id)
//            {
//                return NotFound();
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {
//                    _context.Update(receiptProduct);
//                    await _context.SaveChangesAsync();
//                    _notyfService.Success("Cập nhật sản phẩm thành công.");
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ReceiptProductExists(receiptProduct.Id))
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
//            ViewData["ProductSizeColorId"] = new SelectList(_context.ProductSizeColors, "Id", "Code", receiptProduct.ProductSizeColorId);
//            return View(receiptProduct);
//        }
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var receiptProduct = await _context.ReceiptProducts
//                .Include(r => r.ProductSizeColor)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (receiptProduct == null)
//            {
//                return NotFound();
//            }

//            return View(receiptProduct);
//        }
//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var receiptProduct = await _context.ReceiptProducts.FindAsync(id);
//            if (receiptProduct != null)
//            {
//                _context.ReceiptProducts.Remove(receiptProduct);
//                await _context.SaveChangesAsync();
//                _notyfService.Success("Xóa sản phẩm thành công.");
//            }
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ReceiptProductExists(int id)
//        {
//            return _context.ReceiptProducts.Any(e => e.Id == id);
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
using WebFM_Style.Models;

namespace WebFM_Style.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReceiptProductsController : Controller
    {
        private readonly FmStyleDbContext _context;

        public INotyfService _notyfService { get; }
        public ReceiptProductsController(FmStyleDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index()
        {
            var fmStyleDbContext = _context.ReceiptProducts.Include(r => r.ProductSizeColor);
            return View(await fmStyleDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiptProduct = await _context.ReceiptProducts
                .Include(r => r.ProductSizeColor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receiptProduct == null)
            {
                return NotFound();
            }

            return View(receiptProduct);
        }

        public IActionResult Create()
        {
            ViewData["ProductSizeColorId"] = new SelectList(_context.ProductSizeColors, "Id", "Code");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReceiptProduct receiptProduct)
        {

            if (ModelState.IsValid)
            {
                if (receiptProduct.Quantity < 0)
                {
                    _notyfService.Error("sản phẩm không được < 0.");
                    ViewData["ProductSizeColorId"] = new SelectList(_context.ProductSizeColors, "Id", "Code", receiptProduct.ProductSizeColorId);
                    return View(receiptProduct);
                }
                if (receiptProduct.Price < 0)
                {
                    _notyfService.Error("giá không được  < 0.");
                    ViewData["ProductSizeColorId"] = new SelectList(_context.ProductSizeColors, "Id", "Code", receiptProduct.ProductSizeColorId);
                    return View(receiptProduct);
                }
                var inventory = _context.ProductsInventorys.Include(r => r.ProductSizeColors).FirstOrDefault(x => x.ProductSizeColors.FirstOrDefault().Id == receiptProduct.ProductSizeColorId);
                inventory.Quantity += receiptProduct.Quantity;
                receiptProduct.Status = 1;
                _context.Update(inventory);

                _context.Add(receiptProduct);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm sản phẩm thành công.");
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductSizeColorId"] = new SelectList(_context.ProductSizeColors, "Id", "Code", receiptProduct.ProductSizeColorId);
            return View(receiptProduct);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiptProduct = await _context.ReceiptProducts.FindAsync(id);
            if (receiptProduct == null)
            {
                return NotFound();
            }
            ViewData["ProductSizeColorId"] = new SelectList(_context.ProductSizeColors, "Id", "SCodeize", receiptProduct.ProductSizeColorId);
            return View(receiptProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ReceiptProduct receiptProduct)
        {
            if (id != receiptProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(receiptProduct);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật sản phẩm thành công.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReceiptProductExists(receiptProduct.Id))
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
            ViewData["ProductSizeColorId"] = new SelectList(_context.ProductSizeColors, "Id", "Code", receiptProduct.ProductSizeColorId);
            return View(receiptProduct);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var receiptProduct = await _context.ReceiptProducts
                .Include(r => r.ProductSizeColor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (receiptProduct == null)
            {
                return NotFound();
            }

            return View(receiptProduct);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var receiptProduct = await _context.ReceiptProducts.FindAsync(id);
            if (receiptProduct != null)
            {
                _context.ReceiptProducts.Remove(receiptProduct);
                await _context.SaveChangesAsync();
                _notyfService.Success("Xóa sản phẩm thành công.");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ReceiptProductExists(int id)
        {
            return _context.ReceiptProducts.Any(e => e.Id == id);
        }
    }
}
