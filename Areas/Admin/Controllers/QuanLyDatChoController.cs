using DatVeMayBay.Data;
using DatVeMayBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatVeMayBay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuanLyDatChoController : Controller
    {
        private readonly AppDbContext _context;

        public QuanLyDatChoController(AppDbContext context)
        {
            _context = context;
        }

        // Danh sách vé, lọc theo trạng thái
        public async Task<IActionResult> Index(TrangThaiDatCho? trangThai)
        {
            var ds = _context.DatChos
                .Include(d => d.HanhKhach)
                .Include(d => d.KhachHang)
                .Include(d => d.ChuyenBay)
                .AsQueryable();

            if (trangThai.HasValue)
                ds = ds.Where(d => d.TrangThai == trangThai.Value);

            ViewBag.TrangThai = trangThai;
            ViewBag.SoChoXacNhan = await _context.DatChos.CountAsync(d => d.TrangThai == TrangThaiDatCho.ChoXacNhan);

            return View(await ds.OrderByDescending(d => d.NgayDat).ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> XacNhan(int id, TrangThaiDatCho? trangThai)
        {
            var dc = await _context.DatChos.FindAsync(id);
            if (dc == null) return NotFound();

            var loi = dc.XacNhan();
            if (loi != null)
            {
                TempData["Loi"] = loi;
            }
            else
            {
                await _context.SaveChangesAsync();
                TempData["ThongBao"] = $"Đã xác nhận vé {dc.MaDatCho}.";
            }
            return RedirectToAction(nameof(Index), new { trangThai });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Huy(int id, TrangThaiDatCho? trangThai)
        {
            var dc = await _context.DatChos
                .Include(d => d.ChuyenBay)
                .FirstOrDefaultAsync(d => d.Id == id);
            if (dc == null) return NotFound();

            // Admin huỷ được bất cứ lúc nào, không cần trước 24 giờ
            var loi = dc.Huy(batBuocTruoc24h: false);
            if (loi != null)
            {
                TempData["Loi"] = loi;
            }
            else
            {
                await _context.SaveChangesAsync();
                TempData["ThongBao"] = $"Đã huỷ vé {dc.MaDatCho}, trả lại 1 ghế cho chuyến {dc.ChuyenBay?.SoHieu}.";
            }
            return RedirectToAction(nameof(Index), new { trangThai });
        }
    }
}