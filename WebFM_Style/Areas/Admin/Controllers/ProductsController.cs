//using AspNetCoreHero.ToastNotification.Abstractions;
//using DocumentFormat.OpenXml.Spreadsheet;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using WebFM_Style.Helper;
//using WebFM_Style.Models;

//namespace WebFM_Style.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class ProductsController : Controller
//    {
//        private readonly FmStyleDbContext _context;
//        public INotyfService _notyfService { get; }
//        public static string? image;
//        public ProductsController(FmStyleDbContext context, INotyfService notyfService)
//        {
//            _context = context;
//            _notyfService = notyfService;
//        }
//        [Route("/Admin/product")]
//        public async Task<IActionResult> Index()
//        {
//            var fmStyleDbContext = _context.Products.Include(p => p.ProductType).Include(x=>x.Images);
//            return View(await fmStyleDbContext.ToListAsync());
//        }

//        public async Task<IActionResult> Details(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Products
//                .Include(p => p.ProductType)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (product == null)
//            {
//                return NotFound();
//            }

//            return View(product);
//        }

//        public IActionResult Create()
//        {
//            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Product product, List<IFormFile> fAvatars)
//        {
//            if (_context.Products.Any(p => p.Name == product.Name))
//            {
//                _notyfService.Error("Tên sản phẩm đã tồn tại.");
//                return View(product);
//            }
//            if (fAvatars.Count == 0)
//            {
//                _notyfService.Error("Vui lòng chọn ảnh ");
//                return View(product);
//            }


//            _context.Add(product);
//            await _context.SaveChangesAsync();
//            var ListImage = new List<Image>();
//            int i = 1;
//            foreach (var file in fAvatars)
//            {
//                var imagemodel = new Image();
//                if (file.Length > 0)
//                {
//                    string extennsion = Path.GetExtension(file.FileName);
//                    image = Utilities.ToUrlFriendly(product.Name + i) + extennsion;
//                    imagemodel.Url = await Utilities.UploadFile(file, @"Product", image.ToLower());
//                    imagemodel.ProductId = product.Id;
//                    imagemodel.Status = true;
//                }
//                i++;
//                ListImage.Add(imagemodel);
//            }
//            _context.AddRange(ListImage);
//            await _context.SaveChangesAsync();
//            _notyfService.Success("Thêm sản phẩm thành công.");
//            return RedirectToAction(nameof(Index));
//        }

//        public async Task<IActionResult> Edit(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Products.Include(x=>x.Images).FirstOrDefaultAsync(x=>x.Id == id);
//            if (product == null)
//            {
//                return NotFound();
//            }
//            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
//            return View(product);
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Edit(int id, Product product, List<IFormFile> images)
//        {
//            if (id != product.Id)
//            {
//                return NotFound();
//            }

//            if (_context.Products.Any(p => p.Name == product.Name && p.Id != id))
//            {
//                _notyfService.Error("Tên sản phẩm đã tồn tại.");
//                ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
//                return View(product);
//            }

//            if (ModelState.IsValid)
//            {
//                try
//                {

//                    if (images.Count() >0)
//                    {
//                        var imageDB = _context.Images.Where(x => x.ProductId == id).ToList();
//                        _context.RemoveRange(imageDB);
//                        await _context.SaveChangesAsync();
//                        var ListImage = new List<Image>();
//                        int i = 1;
//                        foreach (var file in images)
//                        {
//                            var imagemodel = new Image();
//                            if (file.Length > 0)
//                            {
//                                string extennsion = Path.GetExtension(file.FileName);
//                                image = Utilities.ToUrlFriendly(product.Name + i) + extennsion;
//                                imagemodel.Url = await Utilities.UploadFile(file, @"Product", image.ToLower());
//                                imagemodel.ProductId = product.Id;
//                                imagemodel.Status = true;
//                            }
//                            i++;
//                            ListImage.Add(imagemodel);
//                        }
//                        _context.AddRange(ListImage);


//                    }
//                    _context.Update(product);
//                    await _context.SaveChangesAsync();
//                    _notyfService.Success("Cập nhật sản phẩm thành công.");
//                }
//                catch (DbUpdateConcurrencyException)
//                {
//                    if (!ProductExists(product.Id))
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
//            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", product.ProductTypeId);
//            return View(product);
//        }

//        public async Task<IActionResult> Delete(int? id)
//        {
//            if (id == null)
//            {
//                return NotFound();
//            }

//            var product = await _context.Products
//                .Include(p => p.ProductType)
//                .FirstOrDefaultAsync(m => m.Id == id);
//            if (product == null)
//            {
//                return NotFound();
//            }

//            return View(product);
//        }

//        [HttpPost, ActionName("Delete")]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> DeleteConfirmed(int id)
//        {
//            var product = await _context.Products.FindAsync(id);
//            if (product != null)
//            {
//                product.Status = 2;
//                _context.Products.Update(product);
//                await _context.SaveChangesAsync();
//                _notyfService.Success("Xóa sản phẩm thành công.");
//            }
//            return RedirectToAction(nameof(Index));
//        }

//        private bool ProductExists(int id)
//        {
//            return _context.Products.Any(e => e.Id == id);
//        }
//    }
//}

// cái mới ở dưới
using AspNetCoreHero.ToastNotification.Abstractions;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFM_Style.Helper;
using WebFM_Style.Models;

namespace WebFM_Style.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly FmStyleDbContext _context;
        public INotyfService _notyfService { get; }
        public static string? image;
        public ProductsController(FmStyleDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        [Route("/Admin/product")]
        public async Task<IActionResult> Index()
        {
            var fmStyleDbContext = _context.Products.Include(p => p.ProductType).Include(x => x.Images);
            return View(await fmStyleDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes.Where(x => x.Status == 1), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product, List<IFormFile> fAvatars)
        {
            if (_context.Products.Any(p => p.Name == product.Name))
            {
                _notyfService.Error("Tên sản phẩm đã tồn tại.");
                return View(product);
            }
            if (fAvatars.Count == 0)
            {
                _notyfService.Error("Vui lòng chọn ảnh ");
                return View(product);
            }


            _context.Add(product);
            await _context.SaveChangesAsync();
            var ListImage = new List<Image>();
            int i = 1;
            foreach (var file in fAvatars)
            {
                var imagemodel = new Image();
                if (file.Length > 0)
                {
                    string extennsion = Path.GetExtension(file.FileName);
                    image = Utilities.ToUrlFriendly(product.Name + i) + extennsion;
                    imagemodel.Url = await Utilities.UploadFile(file, @"Product", image.ToLower());
                    imagemodel.ProductId = product.Id;
                    imagemodel.Status = true;
                }
                i++;
                ListImage.Add(imagemodel);
            }
            _context.AddRange(ListImage);
            await _context.SaveChangesAsync();
            _notyfService.Success("Thêm sản phẩm thành công.");
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes.Where(x => x.Status == 1), "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product, List<IFormFile> images)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (_context.Products.Any(p => p.Name == product.Name && p.Id != id))
            {
                _notyfService.Error("Tên sản phẩm đã tồn tại.");
                ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes.Where(x => x.Status == 1), "Id", "Name", product.ProductTypeId);
                return View(product);
            }

            if (ModelState.IsValid)
            {
                try
                {

                    if (images.Count() > 0)
                    {
                        var imageDB = _context.Images.Where(x => x.ProductId == id).ToList();
                        _context.RemoveRange(imageDB);
                        await _context.SaveChangesAsync();
                        var ListImage = new List<Image>();
                        int i = 1;
                        foreach (var file in images)
                        {
                            var imagemodel = new Image();
                            if (file.Length > 0)
                            {
                                string extennsion = Path.GetExtension(file.FileName);
                                image = Utilities.ToUrlFriendly(product.Name + i) + extennsion;
                                imagemodel.Url = await Utilities.UploadFile(file, @"Product", image.ToLower());
                                imagemodel.ProductId = product.Id;
                                imagemodel.Status = true;
                            }
                            i++;
                            ListImage.Add(imagemodel);
                        }
                        _context.AddRange(ListImage);


                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật sản phẩm thành công.");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes.Where(x => x.Status == 1), "Id", "Name", product.ProductTypeId);
            return View(product);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                product.Status = 2;
                _context.Products.Update(product);
                await _context.SaveChangesAsync();
                _notyfService.Success("Xóa sản phẩm thành công.");
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}

