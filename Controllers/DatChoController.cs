using DatVeMayBay.Data;
using DatVeMayBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatVeMayBay.Controllers
{
    public class DatChoController : Controller
    {
        private const int ToiDaHanhKhach = 9;

        private readonly AppDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public DatChoController(AppDbContext db, UserManager<IdentityUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // ===== ĐẶT VÉ (phải đăng nhập) =====

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create(int chuyenBayId, int soKhach = 1)
        {
            var cb = await LayChuyenBay(chuyenBayId);
            if (cb == null) return NotFound();

            if (!cb.MoDatVe || cb.SoGheTrong <= 0)
            {
                TempData["Loi"] = $"Chuyến {cb.SoHieu} hiện không nhận đặt vé.";
                return RedirectToAction("Index", "ChuyenBay");
            }

            // Số hành khách từ 1 đến 9, và không quá số ghế còn trống
            soKhach = Math.Clamp(soKhach, 1, Math.Min(ToiDaHanhKhach, cb.SoGheTrong));

            var model = new DatVeViewModel
            {
                ChuyenBayId = chuyenBayId,
                Email = User.Identity?.Name ?? "",
                HanhKhachs = Enumerable.Range(0, soKhach).Select(_ => new HanhKhach()).ToList()
            };

            ViewBag.ChuyenBay = cb;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DatVeViewModel model)
        {
            var cb = await LayChuyenBay(model.ChuyenBayId);
            if (cb == null) return NotFound();

            if (model.HanhKhachs.Count == 0)
                ModelState.AddModelError("", "Cần ít nhất 1 hành khách.");

            if (!cb.MoDatVe)
                ModelState.AddModelError("", "Chuyến bay này đã ngừng nhận đặt vé.");
            else if (cb.SoGheTrong < model.HanhKhachs.Count)
                ModelState.AddModelError("", $"Chuyến bay chỉ còn {cb.SoGheTrong} ghế trống.");

            for (int i = 0; i < model.HanhKhachs.Count; i++)
            {
                if (model.HanhKhachs[i].NgaySinh > DateTime.Today)
                    ModelState.AddModelError($"HanhKhachs[{i}].NgaySinh", "Ngày sinh không được ở tương lai");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.ChuyenBay = cb;
                return View(model);
            }

            var khachHangId = _userManager.GetUserId(User)!;
            var dsMa = new List<string>();

            // Mỗi hành khách = 1 vé riêng (1 chuyến bay + 1 hành khách)
            foreach (var hk in model.HanhKhachs)
            {
                var ma = TaoMaDatCho(dsMa);
                dsMa.Add(ma);

                _db.DatChos.Add(new DatCho
                {
                    MaDatCho = ma,
                    ChuyenBayId = cb.Id,
                    HanhKhach = new HanhKhach
                    {
                        HoTen = hk.HoTen.Trim(),
                        NgaySinh = hk.NgaySinh,
                        SoGiayTo = hk.SoGiayTo?.Trim()
                    },
                    KhachHangId = khachHangId,
                    SoDienThoai = model.SoDienThoai.Trim(),
                    Email = model.Email.Trim(),
                    NgayDat = DateTime.Now,
                    TongTien = cb.GiaVe,
                    TrangThai = TrangThaiDatCho.ChoXacNhan
                });
            }

            cb.SoGheTrong -= model.HanhKhachs.Count;
            await _db.SaveChangesAsync();   // lưu tất cả vé trong 1 lần

            TempData["ThongBao"] =
                $"Đặt thành công {dsMa.Count} vé chuyến {cb.SoHieu}. Mã đặt chỗ: {string.Join(", ", dsMa)}. Vé đang chờ xác nhận.";
            return RedirectToAction(nameof(VeCuaToi));
        }

        // ===== VÉ CỦA TÔI =====

        [Authorize]
        public async Task<IActionResult> VeCuaToi()
        {
            var khachHangId = _userManager.GetUserId(User);

            var ds = await _db.DatChos
                .Include(d => d.HanhKhach)
                .Include(d => d.ChuyenBay).ThenInclude(c => c!.HangHangKhong)
                .Where(d => d.KhachHangId == khachHangId)
                .OrderByDescending(d => d.NgayDat)
                .ToListAsync();

            return View(ds);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HuyVeCuaToi(int id)
        {
            var khachHangId = _userManager.GetUserId(User);

            // Chỉ huỷ được vé của chính mình
            var dc = await _db.DatChos
                .Include(d => d.ChuyenBay)
                .FirstOrDefaultAsync(d => d.Id == id && d.KhachHangId == khachHangId);
            if (dc == null) return NotFound();

            var loi = dc.Huy(batBuocTruoc24h: true);
            if (loi != null)
            {
                TempData["Loi"] = loi;
            }
            else
            {
                await _db.SaveChangesAsync();
                TempData["ThongBao"] = $"Đã huỷ vé {dc.MaDatCho}. Ghế đã được trả lại cho chuyến bay.";
            }
            return RedirectToAction(nameof(VeCuaToi));
        }

        // ===== TRA CỨU BẰNG MÃ + EMAIL (không cần đăng nhập) =====

        [HttpGet]
        public IActionResult TraCuu()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TraCuu(string maDatCho, string email)
        {
            var ma = (maDatCho ?? "").Trim().ToUpper();
            var em = (email ?? "").Trim();

            var dc = await LayVe(d => d.MaDatCho == ma && d.Email == em);

            if (dc == null)
            {
                ViewBag.Loi = "Không tìm thấy đặt chỗ. Kiểm tra lại mã 6 ký tự và email đã dùng khi đặt vé.";
                ViewBag.MaDatCho = maDatCho;
                ViewBag.Email = email;
                return View();
            }

            return View("ChiTiet", dc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Huy(int id, string email)
        {
            var dc = await LayVe(d => d.Id == id && d.Email == email);
            if (dc == null) return NotFound();

            var loi = dc.Huy(batBuocTruoc24h: true);
            if (loi != null)
            {
                TempData["Loi"] = loi;
            }
            else
            {
                await _db.SaveChangesAsync();
                TempData["ThongBao"] = "Đã huỷ vé thành công. Ghế đã được trả lại cho chuyến bay.";
            }

            return View("ChiTiet", dc);
        }

        // ===== HÀM PHỤ =====

        private Task<ChuyenBay?> LayChuyenBay(int id)
        {
            return _db.ChuyenBays
                .Include(c => c.HangHangKhong)
                .Include(c => c.ChangDungs).ThenInclude(x => x.SanBay)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        private Task<DatCho?> LayVe(System.Linq.Expressions.Expression<Func<DatCho, bool>> dieuKien)
        {
            return _db.DatChos
                .Include(d => d.HanhKhach)
                .Include(d => d.ChuyenBay).ThenInclude(c => c!.HangHangKhong)
                .FirstOrDefaultAsync(dieuKien);
        }

        // Tạo mã 6 ký tự, không trùng database và không trùng các mã vừa tạo trong lần đặt này
        private string TaoMaDatCho(List<string> daTao)
        {
            const string kyTu = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            string ma;
            do
            {
                ma = new string(Enumerable.Range(0, 6)
                    .Select(_ => kyTu[Random.Shared.Next(kyTu.Length)]).ToArray());
            }
            while (daTao.Contains(ma) || _db.DatChos.Any(d => d.MaDatCho == ma));
            return ma;
        }
    }
}