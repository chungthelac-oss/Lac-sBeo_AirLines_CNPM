using DatVeMayBay.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DatVeMayBay.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;      // tạo, tìm tài khoản
        private readonly SignInManager<IdentityUser> _signInManager;  // đăng nhập, đăng xuất

        public TaiKhoanController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // ===== ĐĂNG NHẬP =====

        [HttpGet]
        public IActionResult DangNhap(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangNhap(DangNhapViewModel model, string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            if (!ModelState.IsValid) return View(model);

            var ketQua = await _signInManager.PasswordSignInAsync(
                model.Email, model.MatKhau, model.GhiNho, lockoutOnFailure: false);

            if (ketQua.Succeeded)
            {
                // Quay lại trang đang muốn vào trước khi bị bắt đăng nhập
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Email hoặc mật khẩu không đúng.");
            return View(model);
        }

        // ===== ĐĂNG KÝ =====

        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangKy(DangKyViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var ketQua = await _userManager.CreateAsync(user, model.MatKhau);

            if (ketQua.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var loi in ketQua.Errors)
            {
                var thongBao = loi.Code switch
                {
                    "DuplicateUserName" or "DuplicateEmail" => "Email này đã được đăng ký.",
                    "PasswordRequiresDigit" => "Mật khẩu phải có ít nhất 1 chữ số.",
                    "PasswordRequiresLower" => "Mật khẩu phải có ít nhất 1 chữ thường.",
                    "PasswordTooShort" => "Mật khẩu phải có ít nhất 6 ký tự.",
                    _ => loi.Description
                };
                if (!ModelState.Values.SelectMany(v => v.Errors).Any(e => e.ErrorMessage == thongBao))
                    ModelState.AddModelError("", thongBao);
            }
            return View(model);
        }

        // ===== ĐĂNG XUẤT =====

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DangXuat()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Trang báo không đủ quyền
        public IActionResult TuChoi()
        {
            return View();
        }
    }
}