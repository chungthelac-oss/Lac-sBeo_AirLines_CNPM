using DatVeMayBay.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatVeMayBay.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<HangHangKhong> HangHangKhongs { get; set; }
        public DbSet<ThanhPho> ThanhPhos { get; set; }
        public DbSet<SanBay> SanBays { get; set; }
        public DbSet<SanBayThanhPho> SanBayThanhPhos { get; set; }
        public DbSet<ChuyenBay> ChuyenBays { get; set; }
        public DbSet<ChangDung> ChangDungs { get; set; }
        public DbSet<DatCho> DatChos { get; set; }
        public DbSet<HanhKhach> HanhKhachs { get; set; }

        // Danh sách sân bay kèm thành phố, sắp theo tên thành phố
        public List<SanBay> LayDanhSachSanBay()
        {
            return SanBays
                .Include(s => s.SanBayThanhPhos).ThenInclude(x => x.ThanhPho)
                .AsEnumerable()
                .OrderBy(s => s.ThanhPho)
                .ToList();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ChuyenBay>().Property(c => c.GiaVe).HasPrecision(18, 0);
            modelBuilder.Entity<DatCho>().Property(d => d.TongTien).HasPrecision(18, 0);
            modelBuilder.Entity<DatCho>().HasIndex(d => d.MaDatCho).IsUnique();
            modelBuilder.Entity<HangHangKhong>().HasIndex(h => h.Ma).IsUnique();

            // ----- Vé (DatCho) -----
            // Lưu trạng thái dạng chữ (ChoXacNhan / DaXacNhan / DaHuy) cho dễ đọc trong database
            modelBuilder.Entity<DatCho>()
                .Property(d => d.TrangThai)
                .HasConversion<string>()
                .HasMaxLength(20);

            // Mỗi vé thuộc 1 chuyến bay; không cho xoá chuyến bay còn vé
            modelBuilder.Entity<DatCho>()
                .HasOne(d => d.ChuyenBay).WithMany()
                .HasForeignKey(d => d.ChuyenBayId)
                .OnDelete(DeleteBehavior.Restrict);

            // Mỗi vé có đúng 1 hành khách
            modelBuilder.Entity<DatCho>()
                .HasOne(d => d.HanhKhach).WithMany(h => h.DatChos)
                .HasForeignKey(d => d.HanhKhachId)
                .OnDelete(DeleteBehavior.Restrict);

            // Mỗi vé do 1 khách hàng (tài khoản) đặt
            modelBuilder.Entity<DatCho>()
                .HasOne(d => d.KhachHang).WithMany()
                .HasForeignKey(d => d.KhachHangId)
                .OnDelete(DeleteBehavior.Restrict);

            // ----- Sân bay ⟷ Thành phố (nhiều-nhiều) -----
            modelBuilder.Entity<SanBayThanhPho>()
                .HasKey(x => new { x.SanBayMa, x.ThanhPhoId });

            modelBuilder.Entity<SanBayThanhPho>()
                .HasOne(x => x.SanBay).WithMany(s => s.SanBayThanhPhos)
                .HasForeignKey(x => x.SanBayMa);

            modelBuilder.Entity<SanBayThanhPho>()
                .HasOne(x => x.ThanhPho).WithMany(t => t.SanBayThanhPhos)
                .HasForeignKey(x => x.ThanhPhoId);

            // ----- Chuyến bay -----
            modelBuilder.Entity<ChuyenBay>()
                .HasOne(c => c.HangHangKhong).WithMany(h => h.ChuyenBays)
                .HasForeignKey(c => c.HangHangKhongId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChuyenBay>()
                .HasOne(c => c.SanBayDi).WithMany()
                .HasForeignKey(c => c.DiemDi)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ChuyenBay>()
                .HasOne(c => c.SanBayDen).WithMany()
                .HasForeignKey(c => c.DiemDen)
                .OnDelete(DeleteBehavior.Restrict);

            // ----- Chặng dừng -----
            // Xoá chuyến bay thì xoá luôn các chặng dừng của nó (Cascade)
            modelBuilder.Entity<ChangDung>()
                .HasOne(x => x.ChuyenBay).WithMany(c => c.ChangDungs)
                .HasForeignKey(x => x.ChuyenBayId)
                .OnDelete(DeleteBehavior.Cascade);

            // Không cho xoá sân bay đang là điểm dừng của chuyến nào đó
            modelBuilder.Entity<ChangDung>()
                .HasOne(x => x.SanBay).WithMany()
                .HasForeignKey(x => x.SanBayMa)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== DỮ LIỆU MẪU =====

            modelBuilder.Entity<HangHangKhong>().HasData(
                new HangHangKhong { Id = 1, Ma = "CD", Ten = "Cánh Diều Airlines" },
                new HangHangKhong { Id = 2, Ma = "SV", Ten = "Sao Việt Air" },
                new HangHangKhong { Id = 3, Ma = "MT", Ten = "Mây Trời Airways" }
            );

            modelBuilder.Entity<ThanhPho>().HasData(
                new ThanhPho { Id = 1, Ten = "TP. Hồ Chí Minh" },
                new ThanhPho { Id = 2, Ten = "Bình Dương" },
                new ThanhPho { Id = 3, Ten = "Hà Nội" },
                new ThanhPho { Id = 4, Ten = "Bắc Ninh" },
                new ThanhPho { Id = 5, Ten = "Đà Nẵng" },
                new ThanhPho { Id = 6, Ten = "Hội An" },
                new ThanhPho { Id = 7, Ten = "Phú Quốc" },
                new ThanhPho { Id = 8, Ten = "Nha Trang" },
                new ThanhPho { Id = 9, Ten = "Cam Ranh" },
                new ThanhPho { Id = 10, Ten = "Hải Phòng" },
                new ThanhPho { Id = 11, Ten = "Huế" },
                new ThanhPho { Id = 12, Ten = "Đà Lạt" },
                new ThanhPho { Id = 13, Ten = "Cần Thơ" }
            );

            modelBuilder.Entity<SanBay>().HasData(
                new SanBay { Ma = "SGN", Ten = "Tân Sơn Nhất" },
                new SanBay { Ma = "HAN", Ten = "Nội Bài" },
                new SanBay { Ma = "DAD", Ten = "Đà Nẵng" },
                new SanBay { Ma = "PQC", Ten = "Phú Quốc" },
                new SanBay { Ma = "CXR", Ten = "Cam Ranh" },
                new SanBay { Ma = "HPH", Ten = "Cát Bi" },
                new SanBay { Ma = "HUI", Ten = "Phú Bài" },
                new SanBay { Ma = "DLI", Ten = "Liên Khương" },
                new SanBay { Ma = "VCA", Ten = "Cần Thơ" }
            );

            modelBuilder.Entity<SanBayThanhPho>().HasData(
                new SanBayThanhPho { SanBayMa = "SGN", ThanhPhoId = 1 },
                new SanBayThanhPho { SanBayMa = "SGN", ThanhPhoId = 2 },
                new SanBayThanhPho { SanBayMa = "HAN", ThanhPhoId = 3 },
                new SanBayThanhPho { SanBayMa = "HAN", ThanhPhoId = 4 },
                new SanBayThanhPho { SanBayMa = "DAD", ThanhPhoId = 5 },
                new SanBayThanhPho { SanBayMa = "DAD", ThanhPhoId = 6 },
                new SanBayThanhPho { SanBayMa = "PQC", ThanhPhoId = 7 },
                new SanBayThanhPho { SanBayMa = "CXR", ThanhPhoId = 8 },
                new SanBayThanhPho { SanBayMa = "CXR", ThanhPhoId = 9 },
                new SanBayThanhPho { SanBayMa = "HPH", ThanhPhoId = 10 },
                new SanBayThanhPho { SanBayMa = "HUI", ThanhPhoId = 11 },
                new SanBayThanhPho { SanBayMa = "DLI", ThanhPhoId = 12 },
                new SanBayThanhPho { SanBayMa = "VCA", ThanhPhoId = 13 }
            );

            modelBuilder.Entity<ChuyenBay>().HasData(
                new ChuyenBay
                {
                    Id = 1,
                    HangHangKhongId = 1,
                    SoHieu = "CD 101",
                    DiemDi = "SGN",
                    DiemDen = "HAN",
                    GioCatCanh = new DateTime(2026, 12, 5, 6, 15, 0),
                    GioHaCanh = new DateTime(2026, 12, 5, 8, 25, 0),
                    GiaVe = 1290000,
                    SoGheTrong = 12,
                    MoDatVe = true
                },
                new ChuyenBay
                {
                    Id = 2,
                    HangHangKhongId = 1,
                    SoHieu = "CD 103",
                    DiemDi = "SGN",
                    DiemDen = "HAN",
                    GioCatCanh = new DateTime(2026, 12, 5, 11, 40, 0),
                    GioHaCanh = new DateTime(2026, 12, 5, 13, 50, 0),
                    GiaVe = 1490000,
                    SoGheTrong = 3,
                    MoDatVe = true
                },
                new ChuyenBay
                {
                    Id = 3,
                    HangHangKhongId = 1,
                    SoHieu = "CD 205",
                    DiemDi = "SGN",
                    DiemDen = "DAD",
                    GioCatCanh = new DateTime(2026, 12, 5, 9, 0, 0),
                    GioHaCanh = new DateTime(2026, 12, 5, 10, 20, 0),
                    GiaVe = 890000,
                    SoGheTrong = 20,
                    MoDatVe = true
                },
                new ChuyenBay
                {
                    Id = 4,
                    HangHangKhongId = 1,
                    SoHieu = "CD 307",
                    DiemDi = "HAN",
                    DiemDen = "PQC",
                    GioCatCanh = new DateTime(2026, 12, 6, 14, 30, 0),
                    GioHaCanh = new DateTime(2026, 12, 6, 16, 45, 0),
                    GiaVe = 1750000,
                    SoGheTrong = 8,
                    MoDatVe = true
                },
                new ChuyenBay
                {
                    Id = 5,
                    HangHangKhongId = 1,
                    SoHieu = "CD 102",
                    DiemDi = "HAN",
                    DiemDen = "SGN",
                    GioCatCanh = new DateTime(2026, 12, 7, 18, 10, 0),
                    GioHaCanh = new DateTime(2026, 12, 7, 20, 20, 0),
                    GiaVe = 1390000,
                    SoGheTrong = 15,
                    MoDatVe = true
                },
                new ChuyenBay
                {
                    Id = 6,
                    HangHangKhongId = 2,
                    SoHieu = "SV 211",
                    DiemDi = "SGN",
                    DiemDen = "HAN",
                    GioCatCanh = new DateTime(2026, 12, 5, 8, 0, 0),
                    GioHaCanh = new DateTime(2026, 12, 5, 10, 10, 0),
                    GiaVe = 1190000,
                    SoGheTrong = 30,
                    MoDatVe = true
                },
                new ChuyenBay
                {
                    Id = 7,
                    HangHangKhongId = 3,
                    SoHieu = "MT 515",
                    DiemDi = "SGN",
                    DiemDen = "CXR",
                    GioCatCanh = new DateTime(2026, 12, 6, 7, 30, 0),
                    GioHaCanh = new DateTime(2026, 12, 6, 8, 35, 0),
                    GiaVe = 790000,
                    SoGheTrong = 25,
                    MoDatVe = true
                },
                new ChuyenBay
                {
                    Id = 8,
                    HangHangKhongId = 2,
                    SoHieu = "SV 330",
                    DiemDi = "HAN",
                    DiemDen = "DAD",
                    GioCatCanh = new DateTime(2026, 12, 6, 16, 0, 0),
                    GioHaCanh = new DateTime(2026, 12, 6, 17, 20, 0),
                    GiaVe = 950000,
                    SoGheTrong = 18,
                    MoDatVe = false
                },

                // Chuyến có chặng dừng. Dùng Id 101, 102 để không trùng chuyến bạn tự thêm
                new ChuyenBay
                {
                    Id = 101,
                    HangHangKhongId = 3,
                    SoHieu = "MT 701",
                    DiemDi = "SGN",
                    DiemDen = "HAN",
                    GioCatCanh = new DateTime(2026, 12, 8, 6, 0, 0),
                    GioHaCanh = new DateTime(2026, 12, 8, 9, 30, 0),
                    GiaVe = 990000,
                    SoGheTrong = 40,
                    MoDatVe = true
                },
                new ChuyenBay
                {
                    Id = 102,
                    HangHangKhongId = 2,
                    SoHieu = "SV 455",
                    DiemDi = "HAN",
                    DiemDen = "PQC",
                    GioCatCanh = new DateTime(2026, 12, 9, 10, 0, 0),
                    GioHaCanh = new DateTime(2026, 12, 9, 14, 10, 0),
                    GiaVe = 1450000,
                    SoGheTrong = 22,
                    MoDatVe = true
                }
            );

            modelBuilder.Entity<ChangDung>().HasData(
                new ChangDung
                {
                    Id = 1,
                    ChuyenBayId = 101,
                    SanBayMa = "DAD",
                    GioDen = new DateTime(2026, 12, 8, 7, 20, 0),
                    GioDi = new DateTime(2026, 12, 8, 8, 0, 0)
                },
                new ChangDung
                {
                    Id = 2,
                    ChuyenBayId = 102,
                    SanBayMa = "DLI",
                    GioDen = new DateTime(2026, 12, 9, 11, 50, 0),
                    GioDi = new DateTime(2026, 12, 9, 12, 30, 0)
                }
            );
        }
    }
}