using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatVeMayBay.Models
{
    public class SanBay
    {
        [Key]
        [StringLength(3)]
        [Display(Name = "Mã sân bay")]
        public string Ma { get; set; } = "";

        [Required, StringLength(100)]
        [Display(Name = "Tên sân bay")]
        public string Ten { get; set; } = "";

        // Một sân bay phục vụ một hoặc nhiều thành phố
        public List<SanBayThanhPho> SanBayThanhPhos { get; set; } = new();

        // Không lưu vào database, tự ghép tên các thành phố để hiển thị.
        // VD: "Đà Nẵng, Hội An"
        [NotMapped]
        [Display(Name = "Thành phố")]
        public string ThanhPho => string.Join(", ",
            SanBayThanhPhos.OrderBy(x => x.ThanhPhoId).Select(x => x.ThanhPho?.Ten));
    }
}