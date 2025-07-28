using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebFM_Style.Models;

namespace WebFM_Style.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CollectionProductsController : Controller
    {
        private readonly FmStyleDbContext _context;

        public CollectionProductsController(FmStyleDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var fmStyleDbContext = _context.CollectionProducts.Include(c => c.Collection).Include(c => c.Product).ThenInclude(x => x.Images);
            return View(await fmStyleDbContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectionProduct = await _context.CollectionProducts
                .Include(c => c.Collection)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collectionProduct == null)
            {
                return NotFound();
            }

            return View(collectionProduct);
        }

        public IActionResult Create()
        {
            ViewData["CollectionId"] = new SelectList(_context.Collections.Where(x => x.Status == true), "Id", "Name");
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.Status == 1), "Id", "Name");
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CollectionProduct collectionProduct)
        {
            if (ModelState.IsValid)
            {
                collectionProduct.Cdt = DateTime.Now;
                _context.Add(collectionProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CollectionId"] = new SelectList(_context.Collections.Where(x => x.Status == true), "Id", "Name", collectionProduct.CollectionId);
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.Status == 1), "Id", "Name", collectionProduct.ProductId);
            return View(collectionProduct);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectionProduct = await _context.CollectionProducts.FindAsync(id);
            if (collectionProduct == null)
            {
                return NotFound();
            }
            ViewData["CollectionId"] = new SelectList(_context.Collections.Where(x => x.Status == true), "Id", "Name", collectionProduct.CollectionId);
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.Status == 1), "Id", "Name", collectionProduct.ProductId);
            return View(collectionProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CollectionProduct collectionProduct)
        {
            if (id != collectionProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(collectionProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CollectionProductExists(collectionProduct.Id))
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
            ViewData["CollectionId"] = new SelectList(_context.Collections.Where(x => x.Status == true), "Id", "Name", collectionProduct.CollectionId);
            ViewData["ProductId"] = new SelectList(_context.Products.Where(x => x.Status == 1), "Id", "Name", collectionProduct.ProductId);
            return View(collectionProduct);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var collectionProduct = await _context.CollectionProducts
                .Include(c => c.Collection)
                .Include(c => c.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (collectionProduct == null)
            {
                return NotFound();
            }

            return View(collectionProduct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var collectionProduct = await _context.CollectionProducts.FindAsync(id);
            if (collectionProduct != null)
            {
                _context.CollectionProducts.Remove(collectionProduct);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CollectionProductExists(int id)
        {
            return _context.CollectionProducts.Any(e => e.Id == id);
        }
    }
}
