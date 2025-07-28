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
    public class NewsController : Controller
    {
        private readonly FmStyleDbContext _context;

        public INotyfService _notyfService { get; }
        public static string? image;
        public NewsController(FmStyleDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/News
        public async Task<IActionResult> Index()
        {
            return View(await _context.News.ToListAsync());
        }

        // GET: Admin/News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        // GET: Admin/News/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(News news, IFormFile fAvatars)
        {
            if (_context.News.Any(p => p.Title == news.Title))
            {
                _notyfService.Error("Title đã tồn tại.");
                return View(news);
            }
            if (fAvatars != null)
            {
                string extennsion = Path.GetExtension(fAvatars.FileName);
                image = Utilities.ToUrlFriendly((news.Title)) + extennsion;
                news.Images = await Utilities.UploadFile(fAvatars, @"News", image.ToLower());
            }
            news.Status = true;
            _context.Add(news);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }
            return View(news);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, News news, IFormFile fAvatars)
        {
            if (id != news.Id)
            {
                return NotFound();
            }

            try
            {
                if (_context.News.Any(p => p.Title == news.Title && p.Id != id))
                {
                    _notyfService.Error("Tên bộ suu tập đã tồn tại.");
                    return View(news);
                }
                if (fAvatars != null)
                {
                    string extennsion = Path.GetExtension(fAvatars.FileName);
                    image = Utilities.ToUrlFriendly((news.Title)) + extennsion;
                    news.Images = await Utilities.UploadFile(fAvatars, @"News", image.ToLower());
                }
                else
                {
                    news.Images = _context.News.Where(x => x.Id == news.Id).Select(x => x.Images).FirstOrDefault();
                }



                _context.Update(news);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(news.Id))
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

            var news = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (news == null)
            {
                return NotFound();
            }

            return View(news);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}
