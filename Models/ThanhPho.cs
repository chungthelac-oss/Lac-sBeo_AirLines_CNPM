using System.ComponentModel.DataAnnotations;

namespace DatVeMayBay.Models
{
    // Thành phố (City)
    public class ThanhPho
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Tên thành phố")]
        public string Ten { get; set; } = "";

        // Các sân bay phục vụ thành phố này
        public List<SanBayThanhPho> SanBayThanhPhos { get; set; } = new();
    }
}