using System.ComponentModel.DataAnnotations;

namespace DatVeMayBay.Models
{
    // Dữ liệu của form đặt vé: 1 chuyến bay, nhiều hành khách
    public class DatVeViewModel
    {
        public int ChuyenBayId { get; set; }

        [Display(Name = "Số điện thoại liên hệ")]
        [Required(ErrorMessage = "Nhập số điện thoại")]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại gồm 10 chữ số, bắt đầu bằng 0")]
        public string SoDienThoai { get; set; } = "";

        [Display(Name = "Email liên hệ")]
        [Required(ErrorMessage = "Nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = "";

        public List<HanhKhach> HanhKhachs { get; set; } = new();
    }
}