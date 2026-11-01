using System.ComponentModel.DataAnnotations;

namespace DatVeMayBay.Models
{
    // Dữ liệu của form Đăng nhập
    public class DangNhapViewModel
    {
        [Required(ErrorMessage = "Nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = "";

        [Display(Name = "Ghi nhớ đăng nhập")]
        public bool GhiNho { get; set; }
    }

    // Dữ liệu của form Đăng ký
    public class DangKyViewModel
    {
        [Required(ErrorMessage = "Nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Nhập mật khẩu")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu từ 6 ký tự, có ít nhất 1 chữ số")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string MatKhau { get; set; } = "";

        [Required(ErrorMessage = "Nhập lại mật khẩu")]
        [Compare("MatKhau", ErrorMessage = "Mật khẩu nhập lại không khớp")]
        [DataType(DataType.Password)]
        [Display(Name = "Nhập lại mật khẩu")]
        public string XacNhanMatKhau { get; set; } = "";
    }
}