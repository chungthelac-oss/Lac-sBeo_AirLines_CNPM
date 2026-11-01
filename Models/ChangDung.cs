using System.ComponentModel.DataAnnotations;

namespace DatVeMayBay.Models
{
    // Chặng dừng (Stopover): chuyến bay ghé một sân bay giữa đường
    public class ChangDung
    {
        public int Id { get; set; }

        public int ChuyenBayId { get; set; }
        public ChuyenBay? ChuyenBay { get; set; }

        [Display(Name = "Sân bay dừng")]
        [Required(ErrorMessage = "Chọn sân bay dừng")]
        [StringLength(3)]
        public string SanBayMa { get; set; } = "";
        public SanBay? SanBay { get; set; }

        [Display(Name = "Giờ đến")]
        public DateTime GioDen { get; set; }

        [Display(Name = "Giờ đi")]
        public DateTime GioDi { get; set; }
    }
}