using System.ComponentModel.DataAnnotations;

namespace DatVeMayBay.Models
{
    // Bảng trung gian: sân bay nào phục vụ thành phố nào (quan hệ nhiều-nhiều)
    public class SanBayThanhPho
    {
        [StringLength(3)]
        public string SanBayMa { get; set; } = "";
        public SanBay? SanBay { get; set; }

        public int ThanhPhoId { get; set; }
        public ThanhPho? ThanhPho { get; set; }
    }
}