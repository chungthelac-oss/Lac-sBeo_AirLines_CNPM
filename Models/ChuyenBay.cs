using System.ComponentModel.DataAnnotations;

namespace DatVeMayBay.Models
{
    public class ChuyenBay
    {
        public List<ChangDung> ChangDungs { get; set; } = new();
        public int Id { get; set; }

        [Display(Name = "Hãng hàng không")]
        [Range(1, int.MaxValue, ErrorMessage = "Chọn hãng hàng không")]
        public int HangHangKhongId { get; set; }
        public HangHangKhong? HangHangKhong { get; set; }

        [Display(Name = "Số hiệu")]
        [Required(ErrorMessage = "Nhập số hiệu chuyến bay")]
        public string SoHieu { get; set; } = "";

        [Display(Name = "Điểm đi")]
        [Required(ErrorMessage = "Chọn sân bay đi")]
        [StringLength(3)]
        public string DiemDi { get; set; } = "";

        [Display(Name = "Điểm đến")]
        [Required(ErrorMessage = "Chọn sân bay đến")]
        [StringLength(3)]
        public string DiemDen { get; set; } = "";

        [Display(Name = "Giờ cất cánh")]
        public DateTime GioCatCanh { get; set; }

        [Display(Name = "Giờ hạ cánh")]
        public DateTime GioHaCanh { get; set; }

        [Display(Name = "Giá vé (₫)")]
        [Range(typeof(decimal), "100000", "50000000", ErrorMessage = "Giá vé từ 100.000 đến 50.000.000 ₫")]
        public decimal GiaVe { get; set; }

        [Display(Name = "Số ghế trống")]
        [Range(0, 300, ErrorMessage = "Số ghế từ 0 đến 300")]
        public int SoGheTrong { get; set; }

        // Hãng mở hoặc đóng việc nhận đặt vé cho chuyến này
        [Display(Name = "Đang mở bán vé")]
        public bool MoDatVe { get; set; } = true;

        public SanBay? SanBayDi { get; set; }
        public SanBay? SanBayDen { get; set; }
    }
}