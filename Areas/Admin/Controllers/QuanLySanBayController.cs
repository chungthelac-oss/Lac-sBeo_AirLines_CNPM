using DatVeMayBay.Data;
using DatVeMayBay.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatVeMayBay.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class QuanLySanBayController : Controller
    {
        private readonly AppDbContext _context;

        public QuanLySanBayController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.LayDanhSachSanBay());
        }

        // ----- Thêm -----

        public IActionResult Create()
        {
            NapThanhPho();
            return View("Form", new SanBayViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SanBayViewModel vm)
        {
            vm.Ma = vm.Ma?.Trim().ToUpper() ?? "";
            vm.DangSua = false;

            KiemTraThanhPho(vm);
            if (await _context.SanBays.AnyAsync(s => s.Ma == vm.Ma))
                ModelState.AddModelError(nameof(SanBayViewModel.Ma), "Mã sân bay này đã tồn tại");

            if (!ModelState.IsValid)
            {
                NapThanhPho();
                return View("Form", vm);
            }

            var sanBay = new SanBay { Ma = vm.Ma, Ten = vm.Ten.Trim() };
            foreach (var tpId in vm.ThanhPhoIds.Distinct())
                sanBay.SanBayThanhPhos.Add(new SanBayThanhPho { ThanhPhoId = tpId });

            _context.SanBays.Add(sanBay);
            await _context.SaveChangesAsync();
            TempData["ThongBao"] = $"Đã thêm sân bay {sanBay.Ten} ({sanBay.Ma}).";
            return RedirectToAction(nameof(Index));
        }

        // ----- Sửa -----

        public async Task<IActionResult> Edit(string id)
        {
            var sanBay = await _context.SanBays
                .Include(s => s.SanBayThanhPhos)
                .FirstOrDefaultAsync(s => s.Ma == id);
            if (sanBay == null) return NotFound();

            NapThanhPho();
            return View("Form", new SanBayViewModel
            {
                Ma = sanBay.Ma,
                Ten = sanBay.Ten,
                ThanhPhoIds = sanBay.SanBayThanhPhos.Select(x => x.ThanhPhoId).ToList(),
                DangSua = true
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, SanBayViewModel vm)
        {
            var sanBay = await _context.SanBays
                .Include(s => s.SanBayThanhPhos)
                .FirstOrDefaultAsync(s => s.Ma == id);
            if (sanBay == null) return NotFound();

            vm.Ma = sanBay.Ma;   // không cho đổi mã sân bay
            vm.DangSua = true;
            ModelState.Remove(nameof(SanBayViewModel.Ma));

            KiemTraThanhPho(vm);

            if (!ModelState.IsValid)
            {
                NapThanhPho();
                return View("Form", vm);
            }

            sanBay.Ten = vm.Ten.Trim();

            // Cập nhật danh sách thành phố: bỏ cái không tick, thêm cái mới tick
            var daCo = sanBay.SanBayThanhPhos.Select(x => x.ThanhPhoId).ToList();

            foreach (var lienKet in sanBay.SanBayThanhPhos.Where(x => !vm.ThanhPhoIds.Contains(x.ThanhPhoId)).ToList())
                _context.SanBayThanhPhos.Remove(lienKet);

            foreach (var tpId in vm.ThanhPhoIds.Distinct().Except(daCo))
                sanBay.SanBayThanhPhos.Add(new SanBayThanhPho { SanBayMa = sanBay.Ma, ThanhPhoId = tpId });

            await _context.SaveChangesAsync();
            TempData["ThongBao"] = $"Đã cập nhật sân bay {sanBay.Ten} ({sanBay.Ma}).";
            return RedirectToAction(nameof(Index));
        }

        // ----- Xoá -----

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var sanBay = await _context.SanBays.FindAsync(id);
            if (sanBay == null) return NotFound();

            var dangDung = await _context.ChuyenBays.AnyAsync(c => c.DiemDi == id || c.DiemDen == id)
                        || await _context.ChangDungs.AnyAsync(x => x.SanBayMa == id);

            if (dangDung)
            {
                TempData["Loi"] = $"Không thể xoá sân bay {id} vì đang có chuyến bay đi, đến hoặc dừng tại đây.";
            }
            else
            {
                _context.SanBays.Remove(sanBay);   // các liên kết thành phố tự xoá theo (Cascade)
                await _context.SaveChangesAsync();
                TempData["ThongBao"] = $"Đã xoá sân bay {sanBay.Ten} ({id}).";
            }
            return RedirectToAction(nameof(Index));
        }

        // ===== HÀM PHỤ =====

        // Đề yêu cầu: mỗi sân bay phục vụ MỘT HOẶC NHIỀU thành phố
        private void KiemTraThanhPho(SanBayViewModel vm)
        {
            if (vm.ThanhPhoIds.Count == 0)
                ModelState.AddModelError(nameof(SanBayViewModel.ThanhPhoIds), "Chọn ít nhất 1 thành phố mà sân bay phục vụ");
        }

        private void NapThanhPho()
        {
            ViewBag.ThanhPhos = _context.ThanhPhos.OrderBy(t => t.Ten).ToList();
        }
    }
}