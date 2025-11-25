// File: Models/HoaDon.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSan.Models
{
    public class HoaDon
    {
        // ĐÃ SỬA: Loại bỏ [Key] trên HoaDonId. Chúng ta sẽ để DatPhongId làm PK.
        [Display(Name = "Mã Hóa Đơn")]
        public int HoaDonId { get; set; } // Giữ lại như một ID thường, không phải khóa chính

        [Required]
        [Display(Name = "Tổng Tiền")]
        public decimal TongTien { get; set; }

        [Display(Name = "Phương Thức Thanh Toán")] // Phương thức thanh toán
        public string PhuongThucThanhToan { get; set; }

        [Display(Name = "Ngày Xuất Hóa Đơn")]
        public DateTime NgayXuat { get; set; } = DateTime.Now; // Gán giá trị mặc định

        [Display(Name = "Trạng Thái Thanh Toán")]
        public string TrangThaiThanhToan { get; set; }

        // ĐÃ SỬA: Khóa ngoại ĐỒNG THỜI là Khóa Chính (1-0..1 relationship)
        [Key] // << KHÓA CHÍNH MỚI >>
        [ForeignKey("DatPhong")]
        [Display(Name = "Mã Đặt Phòng")]
        public int DatPhongId { get; set; }

        public virtual DatPhong DatPhong { get; set; } // Liên kết đến DatPhong
    }
}