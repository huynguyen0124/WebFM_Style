using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using WebFM_Style.Helper;
using WebFM_Style.Models;

namespace WebFM_Style.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CollectionsController : Controller
    {
        private readonly FmStyleDbContext _context;

        public INotyfService _notyfService { get; }
        public static string? image;
        public CollectionsController(FmStyleDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Collections.ToListAsync());
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Collection collection, IFormFile fAvatars, IFormFile fAvatars1)
        {
            if (ModelState.IsValid)
            {
                if (_context.Collections.Any(p => p.Name == collection.Name))
                {
                    _notyfService.Error("Tên bộ suu tập đã tồn tại.");
                    return View(collection);
                }
                if (fAvatars != null)
                {
                    string extennsion = Path.GetExtension(fAvatars.FileName);
                    image = Utilities.ToUrlFriendly((collection.Name)) + extennsion;
                    collection.Avatar = await Utilities.UploadFile(fAvatars, @"Collection", image.ToLower());
                }
                if (fAvatars1 != null)
                {
                    string extennsion = Path.GetExtension(fAvatars1.FileName);
                    image = Utilities.ToUrlFriendly((collection.Name + "-Baner")) + extennsion;
                    collection.Baner = await Utilities.UploadFile(fAvatars1, @"Collection", image.ToLower());
                }

                _context.Add(collection);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(collection);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections.FindAsync(id);
            if (collection == null)
            {
                return NotFound();
            }
            return View(collection);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Collection collection, IFormFile fAvatars, IFormFile fAvatars1)
        {
            if (id != collection.Id)
            {
                return NotFound();
            }

            try
            {
                if (_context.Collections.Any(p => p.Name == collection.Name && p.Id != id))
                {
                    _notyfService.Error("Tên bộ sưu tập đã tồn tại.");
                    return View(collection);
                }
                if (fAvatars != null)
                {
                    string extennsion = Path.GetExtension(fAvatars.FileName);
                    image = Utilities.ToUrlFriendly((collection.Name)) + extennsion;
                    collection.Avatar = await Utilities.UploadFile(fAvatars, @"Collection", image.ToLower());
                }
                else
                {
                    collection.Avatar = _context.Collections.Where(x => x.Id == collection.Id).Select(x => x.Avatar).FirstOrDefault();
                }
                if (fAvatars1 != null)
                {
                    string extennsion = Path.GetExtension(fAvatars1.FileName);
                    image = Utilities.ToUrlFriendly((collection.Name + "-Baner")) + extennsion;
                    collection.Baner = await Utilities.UploadFile(fAvatars1, @"Collection", image.ToLower());
                }
                else
                {
                    collection.Baner = _context.Collections.Where(x => x.Id == collection.Id).Select(x => x.Baner).FirstOrDefault();
                }
                _context.Update(collection);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CollectionExists(collection.Id))
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

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collection = await _context.Collections
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collection == null)
            {
                return NotFound();
            }

            return View(collection);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collection = await _context.Collections.FindAsync(id);
            if (collection != null)
            {
                _context.Collections.Remove(collection);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollectionExists(int id)
        {
            return _context.Collections.Any(e => e.Id == id);
        }
    }
}
