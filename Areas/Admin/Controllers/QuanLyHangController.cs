using DatVeMayBay.Data;
using DatVeMayBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatVeMayBay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuanLyHangController : Controller
    {
        private readonly AppDbContext _context;

        public QuanLyHangController(AppDbContext context)
        {
            _context = context;
        }

        // Danh sách + form thêm mới trên cùng một trang
        public async Task<IActionResult> Index()
        {
            await NapDanhSach();
            return View(new HangHangKhong());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ma,Ten")] HangHangKhong hang)
        {
            hang.Ma = hang.Ma?.Trim().ToUpper() ?? "";

            if (await _context.HangHangKhongs.AnyAsync(h => h.Ma == hang.Ma))
                ModelState.AddModelError(nameof(HangHangKhong.Ma), "Mã hãng này đã tồn tại");

            if (!ModelState.IsValid)
            {
                await NapDanhSach();
                return View(nameof(Index), hang);
            }

            _context.HangHangKhongs.Add(hang);
            await _context.SaveChangesAsync();
            TempData["ThongBao"] = $"Đã thêm hãng {hang.Ten}.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var hang = await _context.HangHangKhongs.FindAsync(id);
            if (hang == null) return NotFound();
            return View(hang);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ma,Ten")] HangHangKhong hang)
        {
            if (id != hang.Id) return NotFound();

            hang.Ma = hang.Ma?.Trim().ToUpper() ?? "";

            if (await _context.HangHangKhongs.AnyAsync(h => h.Ma == hang.Ma && h.Id != hang.Id))
                ModelState.AddModelError(nameof(HangHangKhong.Ma), "Mã hãng này đã tồn tại");

            if (!ModelState.IsValid) return View(hang);

            _context.Update(hang);
            await _context.SaveChangesAsync();
            TempData["ThongBao"] = $"Đã cập nhật hãng {hang.Ten}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var hang = await _context.HangHangKhongs
                .Include(h => h.ChuyenBays)
                .FirstOrDefaultAsync(h => h.Id == id);
            if (hang == null) return NotFound();

            if (hang.ChuyenBays.Any())
            {
                TempData["Loi"] = $"Không thể xoá {hang.Ten} vì hãng còn {hang.ChuyenBays.Count} chuyến bay.";
            }
            else
            {
                _context.HangHangKhongs.Remove(hang);
                await _context.SaveChangesAsync();
                TempData["ThongBao"] = $"Đã xoá hãng {hang.Ten}.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task NapDanhSach()
        {
            ViewBag.DanhSach = await _context.HangHangKhongs
                .Include(h => h.ChuyenBays)
                .OrderBy(h => h.Ten)
                .ToListAsync();
        }
    }
}