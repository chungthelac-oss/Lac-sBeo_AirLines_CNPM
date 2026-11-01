using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace DatVeMayBay.Models
{
    public enum TrangThaiDatCho
    {
        ChoXacNhan,
        DaXacNhan,
        DaHuy
    }

    // Vé được đặt (Booking): đúng 1 chuyến bay + 1 hành khách
    public class DatCho
    {
        public int Id { get; set; }

        [StringLength(6)]
        public string MaDatCho { get; set; } = "";

        public int ChuyenBayId { get; set; }
        public ChuyenBay? ChuyenBay { get; set; }

        public int HanhKhachId { get; set; }
        public HanhKhach? HanhKhach { get; set; }

        // Khách hàng (tài khoản đăng nhập) đã đặt vé này
        public string KhachHangId { get; set; } = "";
        public IdentityUser? KhachHang { get; set; }

        // Thông tin liên hệ của người đặt
        [StringLength(15)]
        public string SoDienThoai { get; set; } = "";

        [StringLength(100)]
        public string Email { get; set; } = "";

        public DateTime NgayDat { get; set; }
        public decimal TongTien { get; set; }

        public TrangThaiDatCho TrangThai { get; set; } = TrangThaiDatCho.ChoXacNhan;

        // ----- Hiển thị (không lưu vào database) -----
        [NotMapped]
        public string TenTrangThai => TrangThai switch
        {
            TrangThaiDatCho.ChoXacNhan => "Chờ xác nhận",
            TrangThaiDatCho.DaXacNhan => "Đã xác nhận",
            _ => "Đã huỷ"
        };

        [NotMapped]
        public string MauTrangThai => TrangThai switch
        {
            TrangThaiDatCho.ChoXacNhan => "bg-warning text-dark",
            TrangThaiDatCho.DaXacNhan => "bg-success",
            _ => "bg-secondary"
        };

        // ----- Chuyển trạng thái (trả về thông báo lỗi, hoặc null nếu thành công) -----

        // Chờ xác nhận → Đã xác nhận
        public string? XacNhan()
        {
            if (TrangThai != TrangThaiDatCho.ChoXacNhan)
                return $"Vé {MaDatCho} không ở trạng thái chờ xác nhận.";

            TrangThai = TrangThaiDatCho.DaXacNhan;
            return null;
        }

        // Chờ xác nhận / Đã xác nhận → Đã huỷ, trả lại 1 ghế cho chuyến bay
        public string? Huy(bool batBuocTruoc24h)
        {
            if (TrangThai == TrangThaiDatCho.DaHuy)
                return $"Vé {MaDatCho} đã được huỷ trước đó.";

            if (ChuyenBay == null)
                return "Thiếu thông tin chuyến bay.";

            if (batBuocTruoc24h && ChuyenBay.GioCatCanh <= DateTime.Now.AddHours(24))
                return "Chỉ được huỷ vé trước giờ cất cánh ít nhất 24 giờ.";

            TrangThai = TrangThaiDatCho.DaHuy;
            ChuyenBay.SoGheTrong++;
            return null;
        }
    }
}