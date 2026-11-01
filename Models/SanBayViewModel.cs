using System.ComponentModel.DataAnnotations;

namespace DatVeMayBay.Models
{
    // Dữ liệu form Thêm / Sửa sân bay (kèm danh sách thành phố được tick)
    public class SanBayViewModel
    {
        [Display(Name = "Mã sân bay")]
        [Required(ErrorMessage = "Nhập mã sân bay")]
        [RegularExpression("^[A-Za-z]{3}$", ErrorMessage = "Mã sân bay gồm đúng 3 chữ cái, VD: SGN")]
        public string Ma { get; set; } = "";

        [Display(Name = "Tên sân bay")]
        [Required(ErrorMessage = "Nhập tên sân bay")]
        [StringLength(100)]
        public string Ten { get; set; } = "";

        [Display(Name = "Thành phố phục vụ")]
        public List<int> ThanhPhoIds { get; set; } = new();

        // true = đang sửa (không cho đổi mã), false = đang thêm mới
        public bool DangSua { get; set; }
    }
}