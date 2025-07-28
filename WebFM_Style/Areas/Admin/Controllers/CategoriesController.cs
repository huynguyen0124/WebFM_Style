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
//    public class CategoriesController : Controller
//    {
//        private readonly FmStyleDbContext _context;

//        public INotyfService _notyfService { get; }
//        public CategoriesController(FmStyleDbContext context, INotyfService notyfService)
//        {
//            _context = context;
//            _notyfService = notyfService;
//        }
//        public async Task<IActionResult> Index()
//        {
//            var fmStyleDbContext = _context.Categories.Include(c => c.Supplier);
//            return View(await fmStyleDbContext.ToListAsync());
//        }

//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var category = await _context.Categories
//                .Include(c => c.Supplier)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (category == null)
//            {
//                return NotFound();
//            }

//            return View(category);
//        }

//        public IActionResult Create()
//        {
//            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name");
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Category category)
//        {
//            if (_context.Categories.Any(c => c.Name == category.Name))
//            {
//                _notyfService.Error("Tên danh mục đã tồn tại.");
//                ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", category.SupplierId);
//                return View(category);
//            }

//            category.Status = true; 
//            _context.Add(category);
//            await _context.SaveChangesAsync();
//            _notyfService.Success("Thêm danh mục thành công.");
//            return RedirectToAction(nameof(Index));
//        }

//        // GET: Admin/Categories/Edit/5
//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var category = await _context.Categories.FindAsync(id);
//            if (category == null)
//            {
//                return NotFound();
//            }
//            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", category.SupplierId);
//            return View(category);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SupplierId,Description,Status,Cdt")] Category category)
//        {
//            if (id != category.Id)
//            {
//                return NotFound();
//            }

//            if (_context.Categories.Any(c => c.Name == category.Name && c.Id != id))
//            {
//                _notyfService.Error("Tên danh mục đã tồn tại.");
//                ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", category.SupplierId);
//                return View(category);
//            }

//                try
//                {
//                    _context.Update(category);
//                    await _context.SaveChangesAsync();
//                    _notyfService.Success("Cập nhật danh mục thành công.");
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!CategoryExists(category.Id))
//                    {
//                        return NotFound();
//                    }
//                    else
//                    {
//                        throw;
//                    }
//                }
//                return RedirectToAction(nameof(Index));
//        }

//        // GET: Admin/Categories/Delete/5
//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var category = await _context.Categories
//                .Include(c => c.Supplier)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (category == null)
//            {
//                return NotFound();
//            }

//            return View(category);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var category = await _context.Categories.FindAsync(id);
//            if (category != null)
//            {
//                _context.Categories.Remove(category);
//                await _context.SaveChangesAsync();
//                _notyfService.Success("Xóa danh mục thành công.");
//            }
//            return RedirectToAction(nameof(Index));
//        }

//        private bool CategoryExists(int id)
//        {
//            return _context.Categories.Any(e => e.Id == id);
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
    public class CategoriesController : Controller
    {
        private readonly FmStyleDbContext _context;

        public INotyfService _notyfService { get; }
        public CategoriesController(FmStyleDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index()
        {
            var fmStyleDbContext = _context.Categories.Include(c => c.Supplier);
            return View(await fmStyleDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            ViewData["SupplierId"] = new SelectList(_context.Suppliers.Where(x => x.Status == true), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (_context.Categories.Any(c => c.Name == category.Name))
            {
                _notyfService.Error("Tên danh mục đã tồn tại.");
                ViewData["SupplierId"] = new SelectList(_context.Suppliers.Where(x => x.Status == true), "Id", "Name", category.SupplierId);
                return View(category);
            }

            category.Status = true;
            _context.Add(category);
            await _context.SaveChangesAsync();
            _notyfService.Success("Thêm danh mục thành công.");
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            ViewData["SupplierId"] = new SelectList(_context.Suppliers.Where(x => x.Status == true), "Id", "Name", category.SupplierId);
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,SupplierId,Description,Status,Cdt")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (_context.Categories.Any(c => c.Name == category.Name && c.Id != id))
            {
                _notyfService.Error("Tên danh mục đã tồn tại.");
                ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", category.SupplierId);
                return View(category);
            }

            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();
                _notyfService.Success("Cập nhật danh mục thành công.");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(category.Id))
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

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                _notyfService.Success("Xóa danh mục thành công.");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
