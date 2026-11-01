using DatVeMayBay.Data;
using DatVeMayBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatVeMayBay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuanLyThanhPhoController : Controller
    {
        private readonly AppDbContext _context;

        public QuanLyThanhPhoController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            await NapDanhSach();
            return View(new ThanhPho());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ten")] ThanhPho thanhPho)
        {
            thanhPho.Ten = thanhPho.Ten?.Trim() ?? "";

            if (await _context.ThanhPhos.AnyAsync(t => t.Ten == thanhPho.Ten))
                ModelState.AddModelError(nameof(ThanhPho.Ten), "Thành phố này đã có trong danh sách");

            if (!ModelState.IsValid)
            {
                await NapDanhSach();
                return View(nameof(Index), thanhPho);
            }

            _context.ThanhPhos.Add(thanhPho);
            await _context.SaveChangesAsync();
            TempData["ThongBao"] = $"Đã thêm thành phố {thanhPho.Ten}.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var thanhPho = await _context.ThanhPhos.FindAsync(id);
            if (thanhPho == null) return NotFound();
            return View(thanhPho);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ten")] ThanhPho thanhPho)
        {
            if (id != thanhPho.Id) return NotFound();

            thanhPho.Ten = thanhPho.Ten?.Trim() ?? "";

            if (await _context.ThanhPhos.AnyAsync(t => t.Ten == thanhPho.Ten && t.Id != thanhPho.Id))
                ModelState.AddModelError(nameof(ThanhPho.Ten), "Thành phố này đã có trong danh sách");

            if (!ModelState.IsValid) return View(thanhPho);

            _context.Update(thanhPho);
            await _context.SaveChangesAsync();
            TempData["ThongBao"] = $"Đã cập nhật thành phố {thanhPho.Ten}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var thanhPho = await _context.ThanhPhos
                .Include(t => t.SanBayThanhPhos)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (thanhPho == null) return NotFound();

            if (thanhPho.SanBayThanhPhos.Any())
            {
                var dsMa = string.Join(", ", thanhPho.SanBayThanhPhos.Select(x => x.SanBayMa));
                TempData["Loi"] = $"Không thể xoá {thanhPho.Ten} vì đang được sân bay {dsMa} phục vụ. Bỏ tick thành phố này ở sân bay trước.";
            }
            else
            {
                _context.ThanhPhos.Remove(thanhPho);
                await _context.SaveChangesAsync();
                TempData["ThongBao"] = $"Đã xoá thành phố {thanhPho.Ten}.";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task NapDanhSach()
        {
            ViewBag.DanhSach = await _context.ThanhPhos
                .Include(t => t.SanBayThanhPhos)
                .OrderBy(t => t.Ten)
                .ToListAsync();
        }
    }
}