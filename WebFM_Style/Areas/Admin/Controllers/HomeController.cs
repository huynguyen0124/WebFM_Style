using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFM_Style.Models;

namespace WebFM_Style.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class HomeController : Controller
    {
        private FmStyleDbContext _context; public static string? image;
        public INotyfService _notyfService { get; }
        private readonly IConfiguration _configuration;
        public HomeController(FmStyleDbContext repo, INotyfService notyfService, IConfiguration configuration)
        {
            _context = repo;
            _notyfService = notyfService;
            _configuration = configuration;
        }
        [Route("/admin")]
        public async Task<IActionResult> Index()
        {
            ///truy vấn dữ liệu 

            var donhuy = await _context.Orders.Where(x => x.Status == 5).ToArrayAsync();
            var User = await _context.Accounts.Where(x => x.RoleId == 3).ToArrayAsync();
            var DonDoanhThu = await _context.Orders.Where(x => x.Status != 1 && x.Status != 5).ToListAsync();
            var sanpham = await _context.Products.Where(x => x.Status == 1).ToListAsync();
            var Giamgia = await _context.Discounts.Where(x => x.Status == 1).ToListAsync();
            //// tính toán dữ liệu 
            int soluongSP = sanpham.Count();
            decimal? tongTien = DonDoanhThu.Sum(x => x.Total);
            int tongdonhang =  DonDoanhThu.Count() + donhuy.Count();
            int soLuong = User.Count();

            ViewBag.User = soLuong;
            ViewBag.DoanhThu = tongTien;
            ViewBag.Sanpham = soluongSP;
            ViewBag.DonHuy = donhuy.Count();
            ViewBag.TongDonHang = tongdonhang;
            ViewBag.MaGiamGia = Giamgia.Count();

            return View();
        }
    }
}
