using AspNetCoreHero.ToastNotification.Abstractions;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebFM_Style.Models;

namespace WebFM_Style.Controllers
{
    public class HomeController : Controller
    {

        private FmStyleDbContext _context;
        public INotyfService _notyfService { get; }
        public HomeController(FmStyleDbContext repo, INotyfService notyfService)
        {
            _context = repo;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> Index()
        {
            var product = await _context.Products.Include(x => x.Images).Include(x => x.ProductSizeColors).ThenInclude(x => x.Color).Where(x => x.Status == 1).ToListAsync();
            ViewBag.Collection = await _context.Collections.ToListAsync();
            ViewBag.News = await _context.News.Take(3).ToListAsync();
            return View(product);
        }
        public IActionResult Contacts()
        {
            return View();
        }
        public async Task<IActionResult> Blog()
        {
            return View(await _context.News.ToListAsync());
        }
        public IActionResult BlogDetails(int id)
        {
            var News = _context.News.FirstOrDefault(x => x.Id == id);
            return View(News);
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult count()
        {
            // Lấy thông tin người dùng từ claims
            var makhclaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var maKH = makhclaim?.Value; // Sử dụng toán tử null-conditional để tránh lỗi nếu không tìm thấy claim

            // Kiểm tra xem maKH có giá trị không
            if (maKH == null)
            {
                return Ok(new { count = 0 }); // Nếu không có người dùng, trả về 0
            }

            // Đếm số lượng sản phẩm trong giỏ hàng của người dùng
            var count = _context.OrderItems
                .Include(x => x.Oder) // Bao gồm thông tin đơn hàng
                .Where(x => x.Oder.AccountId == int.Parse(maKH) && x.Oder.Status == 1) // Lọc theo AccountId
                .Count(); // Đếm số lượng

            return Ok(new { count }); // Trả về số lượng dưới dạng JSON
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
