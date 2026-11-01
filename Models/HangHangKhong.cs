using System.ComponentModel.DataAnnotations;

namespace DatVeMayBay.Models
{
    // Công ty hàng không (AirlineCompany): cung cấp nhiều chuyến bay
    public class HangHangKhong
    {
        public int Id { get; set; }

        [Required, StringLength(2)]
        [Display(Name = "Mã hãng")]
        public string Ma { get; set; } = "";

        [Required, StringLength(100)]
        [Display(Name = "Tên hãng")]
        public string Ten { get; set; } = "";

        // Một hãng có nhiều chuyến bay
        public List<ChuyenBay> ChuyenBays { get; set; } = new();
    }
}