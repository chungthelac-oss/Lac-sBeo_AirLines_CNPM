using DatVeMayBay.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatVeMayBay.Controllers
{
    public class ChuyenBayController : Controller
    {
        private readonly AppDbContext _db;

        public ChuyenBayController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string? diemDi, string? diemDen, DateTime? ngayDi)
        {
            var ketQua = _db.ChuyenBays
                .Include(c => c.HangHangKhong)
                .Include(c => c.SanBayDi).ThenInclude(s => s!.SanBayThanhPhos).ThenInclude(x => x.ThanhPho)
                .Include(c => c.SanBayDen).ThenInclude(s => s!.SanBayThanhPhos).ThenInclude(x => x.ThanhPho)
                .Include(c => c.ChangDungs).ThenInclude(x => x.SanBay)   // lấy kèm chặng dừng
                .Where(c => c.GioCatCanh > DateTime.Now);

            if (!string.IsNullOrEmpty(diemDi))
                ketQua = ketQua.Where(c => c.DiemDi == diemDi);

            if (!string.IsNullOrEmpty(diemDen))
                ketQua = ketQua.Where(c => c.DiemDen == diemDen);

            if (ngayDi.HasValue)
            {
                var ngay = ngayDi.Value.Date;
                ketQua = ketQua.Where(c => c.GioCatCanh.Date == ngay);
            }

            ViewBag.SanBays = _db.LayDanhSachSanBay();
            ViewBag.DiemDi = diemDi;
            ViewBag.DiemDen = diemDen;
            ViewBag.NgayDi = ngayDi;

            return View(ketQua.OrderBy(c => c.GioCatCanh).ToList());
        }
    }
}