using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFM_Style.Models;

namespace WebFM_Style.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private FmStyleDbContext _context; public static string? image;
        public INotyfService _notyfService { get; }
        private readonly IConfiguration _configuration;
        public OrderController(FmStyleDbContext repo, INotyfService notyfService, IConfiguration configuration)
        {
            _context = repo;
            _notyfService = notyfService;
            _configuration = configuration;
        }
        [Route("Admin/Order/XacNhan")]
        public async Task<IActionResult> Index()
        {
            var donhang = await _context.Orders.Include(x => x.Account).ThenInclude(x => x.Addresses).Where(x => x.Status == 2).ToListAsync();
            return View(donhang);
        }

        public async Task<IActionResult> ConfimOrder(int id)
        {
            var oder = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (oder == null)
            {
                return NotFound();
            }
            oder.Status = 3;
            await _context.SaveChangesAsync();
            _notyfService.Success("Xác nhận thành công");
            return RedirectToAction("Index");
        } 
        public async Task<IActionResult> CanOrderby(int id)
        {
            var oder = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (oder == null)
            {
                return NotFound();
            }
            oder.Status = 5;
            await _context.SaveChangesAsync();
            _notyfService.Success("Hủy đơn thành công");
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CanOrder()
        {
            var donhang = await _context.Orders.Include(x => x.Account).ThenInclude(x => x.Addresses).Where(x => x.Status == 5).ToListAsync();
            return View(donhang);
        }

        public async Task<IActionResult> OrderShip()
        {
            var donhang = await _context.Orders
                .Include(x => x.Account)
                .ThenInclude(x => x.Addresses)
                .Where(x => x.Status == 3 ).ToListAsync();
            return View(donhang);
        }
        public async Task<IActionResult> ConfimShip(int id)
        {
            var ship = await _context.Orders.FirstOrDefaultAsync(x => x.Id == id);
            if (ship == null)
            {
                return NotFound();
            }
            ship.Status = 2;
            await _context.SaveChangesAsync();
            _notyfService.Success("Xác nhận giao hàng thành công");
            return RedirectToAction("Index");
        }


        //public async Task<IActionResult> OrderDone()
        //{
        //    var donhang = await _context.Orders.Include(x => x.Account).
        //        ThenInclude(x => x.Addresses)
        //        .Where(x => x.Status == 3 && x.Shipments.FirstOrDefault().Status == 2).ToListAsync();
        //    return View(donhang);
        //}
    }
}
