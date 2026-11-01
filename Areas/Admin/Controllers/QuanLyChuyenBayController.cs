using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DatVeMayBay.Data;
using DatVeMayBay.Models;

namespace DatVeMayBay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuanLyChuyenBayController : Controller
    {
        private const string CacTruong =
            "Id,HangHangKhongId,SoHieu,DiemDi,DiemDen,GioCatCanh,GioHaCanh,GiaVe,SoGheTrong,MoDatVe";

        private readonly AppDbContext _context;

        public QuanLyChuyenBayController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ds = await _context.ChuyenBays
                .Include(c => c.HangHangKhong)
                .Include(c => c.ChangDungs)
                .OrderBy(c => c.GioCatCanh)
                .ToListAsync();
            return View(ds);
        }

        // Xem chi tiết: lấy kèm hãng, chặng dừng và đếm số vé đã bán
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var chuyenBay = await LayChuyenBayKemChangDung(id.Value);
            if (chuyenBay == null) return NotFound();

            // Số vé đã bán (không tính vé đã huỷ)
            ViewBag.SoVe = await _context.DatChos
                .CountAsync(d => d.ChuyenBayId == id && d.TrangThai != TrangThaiDatCho.DaHuy);

            return View(chuyenBay);
        }

        public IActionResult Create()
        {
            NapDanhSach();
            var ngay = DateTime.Today.AddDays(7);
            return View(new ChuyenBay
            {
                GioCatCanh = ngay.AddHours(8),
                GioHaCanh = ngay.AddHours(10),
                SoGheTrong = 180,
                MoDatVe = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind(CacTruong)] ChuyenBay chuyenBay)
        {
            KiemTraLogic(chuyenBay);

            if (ModelState.IsValid)
            {
                _context.Add(chuyenBay);
                await _context.SaveChangesAsync();
                TempData["ThongBao"] = $"Đã thêm chuyến {chuyenBay.SoHieu}.";
                return RedirectToAction(nameof(Index));
            }
            NapDanhSach();
            return View(chuyenBay);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var chuyenBay = await _context.ChuyenBays.FindAsync(id);
            if (chuyenBay == null) return NotFound();

            NapDanhSach();
            return View(chuyenBay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind(CacTruong)] ChuyenBay chuyenBay)
        {
            if (id != chuyenBay.Id) return NotFound();

            KiemTraLogic(chuyenBay);

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chuyenBay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChuyenBayExists(chuyenBay.Id)) return NotFound();
                    throw;
                }
                TempData["ThongBao"] = $"Đã cập nhật chuyến {chuyenBay.SoHieu}.";
                return RedirectToAction(nameof(Index));
            }
            NapDanhSach();
            return View(chuyenBay);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatTatDatVe(int id)
        {
            var chuyenBay = await _context.ChuyenBays.FindAsync(id);
            if (chuyenBay == null) return NotFound();

            chuyenBay.MoDatVe = !chuyenBay.MoDatVe;
            await _context.SaveChangesAsync();

            TempData["ThongBao"] = chuyenBay.MoDatVe
                ? $"Đã MỞ đặt vé cho chuyến {chuyenBay.SoHieu}."
                : $"Đã ĐÓNG đặt vé cho chuyến {chuyenBay.SoHieu}.";
            return RedirectToAction(nameof(Index));
        }

        // Trang xác nhận xoá: báo trước nếu chuyến đã có người đặt
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var chuyenBay = await LayChuyenBayKemChangDung(id.Value);
            if (chuyenBay == null) return NotFound();

            ViewBag.CoVe = await _context.DatChos.AnyAsync(d => d.ChuyenBayId == id);
            return View(chuyenBay);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (await _context.DatChos.AnyAsync(d => d.ChuyenBayId == id))
            {
                TempData["Loi"] = "Không thể xoá chuyến bay đã có người đặt vé. Hãy đóng đặt vé thay vì xoá.";
                return RedirectToAction(nameof(Index));
            }

            var chuyenBay = await _context.ChuyenBays.FindAsync(id);
            if (chuyenBay != null)
            {
                _context.ChuyenBays.Remove(chuyenBay);
                await _context.SaveChangesAsync();
                TempData["ThongBao"] = $"Đã xoá chuyến {chuyenBay.SoHieu}.";
            }
            return RedirectToAction(nameof(Index));
        }

        // ===== CHẶNG DỪNG =====

        // Trang xem và thêm chặng dừng của một chuyến bay
        public async Task<IActionResult> ChangDungs(int id)
        {
            var cb = await LayChuyenBayKemChangDung(id);
            if (cb == null) return NotFound();

            NapDanhSach();
            ViewBag.ChuyenBay = cb;

            // Điền sẵn giờ gợi ý: đến sau cất cánh 1 tiếng, dừng 40 phút
            return View(new ChangDung
            {
                ChuyenBayId = id,
                GioDen = cb.GioCatCanh.AddHours(1),
                GioDi = cb.GioCatCanh.AddHours(1).AddMinutes(40)
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ThemChangDung([Bind("ChuyenBayId,SanBayMa,GioDen,GioDi")] ChangDung changDung)
        {
            var cb = await LayChuyenBayKemChangDung(changDung.ChuyenBayId);
            if (cb == null) return NotFound();

            // Các quy tắc thời gian của chặng dừng
            if (changDung.GioDi <= changDung.GioDen)
                ModelState.AddModelError(nameof(ChangDung.GioDi), "Giờ đi phải sau giờ đến");

            if (changDung.GioDen <= cb.GioCatCanh || changDung.GioDi >= cb.GioHaCanh)
                ModelState.AddModelError("",
                    $"Chặng dừng phải nằm giữa giờ cất cánh ({cb.GioCatCanh:HH:mm dd/MM}) và giờ hạ cánh ({cb.GioHaCanh:HH:mm dd/MM}).");

            if (changDung.SanBayMa == cb.DiemDi || changDung.SanBayMa == cb.DiemDen)
                ModelState.AddModelError(nameof(ChangDung.SanBayMa), "Sân bay dừng phải khác điểm đi và điểm đến");

            if (cb.ChangDungs.Any(x => changDung.GioDen < x.GioDi && changDung.GioDi > x.GioDen))
                ModelState.AddModelError("", "Thời gian bị trùng với một chặng dừng khác của chuyến này.");

            if (!ModelState.IsValid)
            {
                NapDanhSach();
                ViewBag.ChuyenBay = cb;
                return View(nameof(ChangDungs), changDung);
            }

            _context.ChangDungs.Add(changDung);
            await _context.SaveChangesAsync();
            TempData["ThongBao"] = $"Đã thêm chặng dừng tại {changDung.SanBayMa}.";
            return RedirectToAction(nameof(ChangDungs), new { id = changDung.ChuyenBayId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XoaChangDung(int id)
        {
            var cd = await _context.ChangDungs.FindAsync(id);
            if (cd == null) return NotFound();

            _context.ChangDungs.Remove(cd);
            await _context.SaveChangesAsync();
            TempData["ThongBao"] = $"Đã xoá chặng dừng tại {cd.SanBayMa}.";
            return RedirectToAction(nameof(ChangDungs), new { id = cd.ChuyenBayId });
        }

        // ===== HÀM PHỤ =====

        private Task<ChuyenBay?> LayChuyenBayKemChangDung(int id)
        {
            return _context.ChuyenBays
                .Include(c => c.HangHangKhong)
                .Include(c => c.ChangDungs).ThenInclude(x => x.SanBay)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        private void NapDanhSach()
        {
            var sanBay = _context.LayDanhSachSanBay()
                .Select(s => new { s.Ma, HienThi = s.ThanhPho + " (" + s.Ma + ")" })
                .ToList();
            ViewBag.SanBayList = new SelectList(sanBay, "Ma", "HienThi");

            var hang = _context.HangHangKhongs.OrderBy(h => h.Ten).ToList();
            ViewBag.HangList = new SelectList(hang, "Id", "Ten");
        }

        private void KiemTraLogic(ChuyenBay chuyenBay)
        {
            if (chuyenBay.GioHaCanh <= chuyenBay.GioCatCanh)
                ModelState.AddModelError(nameof(ChuyenBay.GioHaCanh), "Giờ hạ cánh phải sau giờ cất cánh");

            if (chuyenBay.DiemDi == chuyenBay.DiemDen)
                ModelState.AddModelError(nameof(ChuyenBay.DiemDen), "Điểm đến phải khác điểm đi");
        }

        private bool ChuyenBayExists(int id)
        {
            return _context.ChuyenBays.Any(e => e.Id == id);
        }
    }
}