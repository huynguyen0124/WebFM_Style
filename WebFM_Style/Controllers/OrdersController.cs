using AspNetCoreHero.ToastNotification.Abstractions;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using WebFM_Style.Models;
using WebFM_Style.Services;

namespace WebFM_Style.Controllers
{
    public class OrdersController : Controller
    {
        private readonly FmStyleDbContext _context;
        public INotyfService _notyfService { get; }
        private readonly IVnPayService _vnPayService;

        public OrdersController(FmStyleDbContext context, INotyfService notyfService, IVnPayService vnPayService)
        {
            _context = context;
            _notyfService = notyfService;
            _vnPayService = vnPayService;
        }

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, string size, string color, int quantity)
        {
            if (string.IsNullOrEmpty(size) || string.IsNullOrEmpty(color))
            {
                return Json(new { success = false, message = "Vui lòng chọn kích cỡ và màu sắc trước khi thêm vào giỏ hàng." });
            }

            var makhclaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (makhclaim == null)
                return Json(new { success = false, message = "Vui lòng đăng nhập" });

            var maKH = makhclaim.Value;
            var productColorSize = _context.ProductSizeColors.Include(x => x.Size).Include(x => x.Color).Include(x => x.ProductInventory)
                .FirstOrDefault(x => x.ProductId == productId && x.Color.Color1 == color && x.Size.Size1 == size);

            if (productColorSize == null)
                return Json(new { success = false, message = "Sản phẩm không tồn tại" });

            var soLuongConLai = (productColorSize.ProductInventory.Quantity ?? 0) - (productColorSize.ProductInventory.QuantitySold ?? 0);
            var dondathang = await _context.Orders.FirstOrDefaultAsync(x => x.AccountId == int.Parse(maKH) && x.Status == 1);
            if (dondathang == null)
            {
                dondathang = new Order { AccountId = int.Parse(maKH), Status = 1 };
                _context.Orders.Add(dondathang);
                await _context.SaveChangesAsync();
            }

            var chitietdonthang = await _context.OrderItems.FirstOrDefaultAsync(x => x.OrderId == dondathang.Id && x.ProductSizeColorId == productColorSize.Id);
            int soLuongHienTai = chitietdonthang?.Quantity ?? 0;

            if (soLuongHienTai + quantity > soLuongConLai)
                return Json(new { success = false, message = $"Sản phẩm chỉ còn {soLuongConLai - soLuongHienTai} sản phẩm trong kho." });

            if (chitietdonthang == null)
            {
                chitietdonthang = new OrderItem { OrderId = dondathang.Id, ProductSizeColorId = productColorSize.Id, Quantity = quantity };
                _context.OrderItems.Add(chitietdonthang);
            }
            else
            {
                chitietdonthang.Quantity += quantity;
            }
            await _context.SaveChangesAsync();
            _notyfService.Success("Thêm sản phẩm thành công");
            return Json(new { success = true, message = "Sản phẩm đã được thêm vào giỏ hàng." });
        }

        public async Task<IActionResult> UpdateOrderAdd(int? id)
        {
            var makhclaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (makhclaim == null)
            {
                _notyfService.Error("Vui lòng đăng nhập trước khi mua hàng");
                return RedirectToAction("Index", "Home");
            }
            var maKH = makhclaim.Value;
            var dondathang = _context.OrderItems.Include(x => x.Oder).Where(x => x.Oder.AccountId == int.Parse(maKH) && x.Oder.Status == 1);
            var sanpham = dondathang.FirstOrDefault(x => x.ProductSizeColorId == id);

            if (sanpham != null)
            {
                var productColorSize = _context.ProductSizeColors.Include(p => p.ProductInventory).FirstOrDefault(p => p.Id == sanpham.ProductSizeColorId);
                var soLuongConLai = (productColorSize.ProductInventory.Quantity ?? 0) - (productColorSize.ProductInventory.QuantitySold ?? 0);

                if (sanpham.Quantity + 1 > soLuongConLai)
                {
                    _notyfService.Error($"Sản phẩm chỉ còn {soLuongConLai - sanpham.Quantity} sản phẩm trong kho.");
                    return RedirectToAction("ShoppingCart", "Products");
                }

                sanpham.Quantity += 1;
                _context.SaveChanges();
            }
            return RedirectToAction("ShoppingCart", "Products");
        }

        public IActionResult UpdateOrder(int id)
        {
            var makhclaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (makhclaim == null)
            {
                _notyfService.Error("Vui lòng đăng nhập");
                return RedirectToAction("Login", "Accounts");
            }
            var maKH = int.Parse(makhclaim.Value);
            var order = _context.Orders.FirstOrDefault(x => x.AccountId == maKH && x.Status == 1);
            if (order == null)
            {
                _notyfService.Error("Không tìm thấy đơn hàng");
                return RedirectToAction("Index", "Home");
            }
            var orderItem = _context.OrderItems.FirstOrDefault(x => x.OrderId == order.Id && x.ProductSizeColorId == id);
            if (orderItem == null)
            {
                _notyfService.Error("Không tìm thấy sản phẩm trong giỏ hàng");
                return RedirectToAction("ShoppingCart", "Products");
            }

            orderItem.Quantity--;
            if (orderItem.Quantity <= 0)
            {
                _context.OrderItems.Remove(orderItem);
                _notyfService.Success("Đã xóa sản phẩm khỏi giỏ hàng");
            }
            else
            {
                _notyfService.Success("Đã cập nhật số lượng");
            }

            _context.SaveChanges();
            return RedirectToAction("ShoppingCart", "Products");
        }

        public IActionResult RemoveCart()
        {
            var makhclaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (makhclaim == null)
            {
                _notyfService.Error("Vui lòng đăng nhập trước khi mua hàng");
                return RedirectToAction("Index", "Home");
            }
            var maKH = makhclaim.Value;
            var dondathang = _context.OrderItems.Include(x => x.Oder)
                .Where(x => x.Oder.AccountId == int.Parse(maKH) && x.Oder.Status == 1);
            _context.OrderItems.RemoveRange(dondathang);
            _context.SaveChanges();
            _notyfService.Success("Xóa giỏ hàng thành công");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveProduct(int id)
        {
            var makhclaim = User.Claims.FirstOrDefault(c => c.Type == "Id");
            if (makhclaim == null)
            {
                _notyfService.Error("Vui lòng đăng nhập trước khi mua hàng");
                return RedirectToAction("Index", "Home");
            }
            var maKH = makhclaim.Value;
            var sanpham = _context.OrderItems.Include(x => x.Oder)
                .FirstOrDefault(x => x.ProductSizeColorId == id && x.Oder.AccountId == int.Parse(maKH) && x.Oder.Status == 1);

            if (sanpham != null)
            {
                _context.OrderItems.Remove(sanpham);
                _context.SaveChanges();
            }
            return RedirectToAction("ShoppingCart", "Products");
        }
    }
}
