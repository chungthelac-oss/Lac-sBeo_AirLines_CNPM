using System.ComponentModel.DataAnnotations;

namespace DatVeMayBay.Models
{
    // Hành khách (Passenger): người thực sự đi máy bay, có thể khác người đặt vé
    public class HanhKhach
    {
        public int Id { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Nhập họ tên hành khách")]
        [StringLength(100)]
        public string HoTen { get; set; } = "";

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Nhập ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Số CCCD / Hộ chiếu")]
        [StringLength(20)]
        public string? SoGiayTo { get; set; }

        public List<DatCho> DatChos { get; set; } = new();
    }
}