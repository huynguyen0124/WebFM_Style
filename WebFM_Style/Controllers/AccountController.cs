using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebFM_Style.Extension;
using WebFM_Style.Models;
using Microsoft.EntityFrameworkCore;
using WebFM_Style.Helper;
using DocumentFormat.OpenXml.Vml;
using MimeKit;
using MailKit.Security;

namespace WebFM_Style.Controllers
{
    public class AccountController : Controller
    {
        private readonly FmStyleDbContext _context;
        public static string image;
        public INotyfService _notyfService { get; }
        public AccountController(FmStyleDbContext context, INotyfService notyfService)
        {

            _context = context;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string user, string pass)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var password = pass.ToMD5();
            var account = await _context.Accounts.Include(x => x.Addresses).FirstOrDefaultAsync(u => u.UserName == user && u.Password == password);

            if (account == null)
            {
                // Tên đăng nhập hoặc mật khẩu không đúng
                _notyfService.Error("Thông tin đăng nhập không chính xác");
                return RedirectToAction("Login", "Account");
            }
            if (account?.RoleId == 1 || account?.RoleId == 2)
            {
                _notyfService.Error("Tài khoản của bạn là tài khoản Admin");
                return RedirectToAction("Login", "Account");
            }
            if (account?.Status == 2)
            {
                _notyfService.Error("Tài khoản đã bị khóa");
                return RedirectToAction("Login", "Account");
            }
            if (account != null)
            {
                // Lưu thông tin người dùng vào cookie xác thực
                List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, account.FullName),
                        new Claim("UserName" , account.UserName),
                        new Claim("Id" , account.Id.ToString()),
                         new Claim("Avartar", "/contents/Images/User/" + account.Avartar) 
                    };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                _notyfService.Success("Đăng nhập thành công");
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _notyfService.Warning("Tên đăng nhập hoặc mật khẩu không đúng");
                return RedirectToAction("Login", "Account");
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            _notyfService.Success("Đăng xuất thành công");
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string pass, string newpass, string confirmpass)
        {
            if (ModelState.IsValid)
            {
                var tendangnhapclam = User.Claims.SingleOrDefault(c => c.Type == "UserName");
                var tendangnhap = "";
                if (tendangnhapclam != null)
                { tendangnhap = tendangnhapclam.Value; }
                var password = pass.ToMD5();
                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.UserName == tendangnhap);
                if (user?.Password != password)
                {
                    _notyfService.Error("Mật khẩu cũ không chính xác");
                    return RedirectToAction("Index", "Home");
                }
                if (newpass.Length < 6 && newpass.Length < 100)
                {
                    _notyfService.Error("Mật khẩu mới phải trên 6 ký tự và nhỏ hơn 100 ký tự ");
                    return RedirectToAction("Index", "Home");
                }
                if (newpass != confirmpass)
                {
                    _notyfService.Error("Mật khẩu mới không đúng với mật khẩu xác nhận !");
                    return RedirectToAction("Index", "Home");
                }
                user.Password = newpass.ToMD5();
                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                _notyfService.Error("Vui lòng nhập đầy đủ thông mật khẩu !");

            }
            _notyfService.Success("Đổi mật khẩu thành công!");
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(Account account)
        {
            if (account.UserName?.Length < 6)
            {
                _notyfService.Error("Tài khoản không bé hơn 6 kí tự");
                return View(account);
            }
            if (account.Password?.Length < 6)
            {
                _notyfService.Error("Mật khẩu không bé hơn 6 kí tự");
                return View(account);
            }
            if (account.Phone?.Length != 10)
            {
                _notyfService.Error("Số điện thoại là 10 số");
                return View(account);
            }
            var exaccount = await _context.Accounts.FirstOrDefaultAsync(x => x.Email == account.Email || x.UserName == account.UserName);
            if (exaccount != null)
            {
                _notyfService.Error("Email hoặc Username đã tồn tại");
                return View(account);
            }
            account.Avartar = "UserDemo.jpg";
            account.Password = (account.Password)?.ToMD5();
            account.Status = 1;
            account.AccountTypeId = 1;
            account.Point = 0;
            account.RoleId = 3;

            _context.Update(account);
            await _context.SaveChangesAsync();
            _notyfService.Success("Đăng ký thành công");
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public async Task<IActionResult> Create(string streest, string ward, string distrist, string city, string contry, string url)
        {
            var Idclam = User.Claims.SingleOrDefault(c => c.Type == "Id");
            int Id = 0;
            if (Idclam != null)
            { Id = Int32.Parse(Idclam.Value); }

            Address address = new Address
            {
                Street = streest,
                City = city,
                Ward = ward,
                District = distrist,
                Country = contry,
                AccountId = Id
            };
            _context.Update(address);
            await _context.SaveChangesAsync();
            _notyfService.Success("Thêm địa chỉ thành công");
            if (url != null)
            {
                return Redirect(url);
            }
            return RedirectToAction("Profile", "Account");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(address);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int addresid, string streest, string ward, string distrist, string city, string contry)
        {
            var addresr = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == addresid);
            if (addresr == null)
            {
                return NotFound();
            }
            addresr.Street = streest;
            addresr.City = city;
            addresr.Ward = ward;
            addresr.District = distrist;
            addresr.Country = contry;
            await _context.SaveChangesAsync();
            _notyfService.Success("Sửa địa chỉ thành công");
            return RedirectToAction("Profile", "Account");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAd(int addid)
        {
            var addresr = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == addid);
            if (addresr == null)
            {
                return NotFound();
            }
            _context.Addresses.Remove(addresr);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa địa chỉ thành công");
            return RedirectToAction("Profile", "Account");
        }
        public async Task<IActionResult> Profile()
        {
            var Idclam = User.Claims.SingleOrDefault(c => c.Type == "Id");
            int Id = 0;
            if (Idclam != null)
            { Id = Int32.Parse(Idclam.Value); }
            var donhang = await _context.Orders.Where(x => x.AccountId == Id)
                .Include(x => x.OderItems).ThenInclude(x=>x.ProductSizeColor)
                .ThenInclude(x=>x.Product).ThenInclude(x=>x.Images).Include(x => x.Account)
                .ThenInclude(x => x.Addresses).Where(x=>x.Status != 1 && x.Status != null).ToListAsync();
            if (donhang == null)
            {
                return NotFound();
            }
            ViewBag.Donhang = donhang;
            ViewBag.Addresses = await _context.Addresses.Where(x => x.AccountId == Id).ToArrayAsync();


            return View(await _context.Accounts.Include(x => x.Addresses).FirstOrDefaultAsync(x => x.Id == Id));
        }

        public async Task<IActionResult> EditProfile()
        {
            var Idclam = User.Claims.SingleOrDefault(c => c.Type == "Id");
            int Id = 0;
            if (Idclam != null)
            { Id = Int32.Parse(Idclam.Value); }

            return View(await _context.Accounts.Include(x => x.Addresses).FirstOrDefaultAsync(x => x.Id == Id));
        }
        [HttpPost]
        public async Task<IActionResult> EditProfile(Account account, IFormFile fAvatar)
        {
            if (account.UserName?.Length < 6)
            {
                _notyfService.Error("Tài khoản không bé hơn 6 kí tự");
                return View(account);
            }
            if (account.Password?.Length < 6)
            {
                _notyfService.Error("Mật khẩu không bé hơn 6 kí tự");
                return View(account);
            }
            if (account.Phone?.Length != 10)
            {
                _notyfService.Error("Số điện thoại là 10 số");
                return View(account);
            }
            var khachang = await _context.Accounts.FirstOrDefaultAsync(x => x.Id == account.Id);
            if (khachang == null)
            {
                return NotFound();
            }
            var ktemail = await _context.Accounts.FirstOrDefaultAsync(x => x.Id != account.Id
                && (x.Email == account.Email));
            if (ktemail != null)
            {
                _notyfService.Error("Email đã tồn tại trong hệ thống!");
                return View(khachang);
            }
            if (fAvatar != null)
            {
                string extennsion = System.IO.Path.GetExtension(fAvatar.FileName);
                image = Utilities.ToUrlFriendly(khachang.UserName) + extennsion;
                khachang.Avartar = await Utilities.UploadFile(fAvatar, @"User", image.ToLower());
            }


            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Lưu thông tin người dùng vào cookie xác thực
            List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, account.FullName),
                        new Claim("UserName" , account.UserName),
                        new Claim("Id" , account.Id.ToString()),
                         new Claim("Avartar", "/contents/Images/User/" + khachang.Avartar) // Thêm đường dẫn đến ảnh đại diện vào claims
                    };
            //   Response.Cookies.Append("AnhDaiDien", "Images/GiaoVien/" + user.AnhDaiDien);
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            khachang.Email = account.Email;
            khachang.FullName = account.FullName;
            khachang.Gender = account.Gender;
            khachang.Phone = account.Phone;
            khachang.Birthday = account.Birthday;

            _notyfService.Success("Sửa thành công!");
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

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
            return RedirectToAction("Profile");
        }



        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == Email);
                if (user != null)
                {
                    var random = new Random();
                    var token = random.Next(10000000, 99999999).ToString();
                    user.ResetToken = token;
                    user.ResetTokenExpiry = DateTime.UtcNow.AddHours(1);
                    user.Email = Email;
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("ResetToken", token);
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("ResetTokenExpiry", user.ResetTokenExpiry.ToString());
                    var email = new MimeMessage();
                    email.From.Add(new MailboxAddress("AdminDotnet", "admin@example.com"));
                    email.To.Add(MailboxAddress.Parse($"{Email}"));
                    email.Subject = "Yêu cầu đặt lại mật khẩu";

                    email.Body = new TextPart("plain")
                    {
                        Text = $"Để đặt lại mật khẩu, vui lòng sử dụng token sau đây: {token} mã token có thời hạn là 10 phút"
                    };
                    using var smtp = new MailKit.Net.Smtp.SmtpClient();
                    smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    smtp.Authenticate("khangchannel19@gmail.com", "jnal cnyl mlit izco");
                    smtp.Send(email);
                    smtp.Disconnect(true);
                    return RedirectToAction(nameof(ResetPassword));

                }
                else
                {
                    _notyfService.Warning("Email không tồn tại trong hệ thống ");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost()]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var resetToken = HttpContext.Session.GetString("ResetToken");
                var resetTokenExpiry = HttpContext.Session.GetString("ResetTokenExpiry");
                var email = HttpContext.Session.GetString("Email");
                var user = await _context.Accounts.FirstOrDefaultAsync(u => u.Email == email);
                if (user != null && resetToken == model.Token)
                {
                    if (model.Password != model.ConfirmPassword)
                    {
                        TempData["ResetPasswordErrorMessage"] = "Mật khẩu mới và mật khẩu xác nhận không khớp.";
                        return View(model);
                    }
                    user.Password = (model.Password).ToMD5();
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Mật khẩu của bạn đã được đặt lại thành công.");
                    return RedirectToAction("Index", "Home");
                }

                _notyfService.Error("Yêu cầu đặt lại mật khẩu không hợp lệ hoặc đã hết hạn.");
            }

            return View(model);
        }

    }
}
