// File: Models/DatPhong.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhachSan.Models
{
    public class DatPhong
    {
        [Key]
        [Display(Name = "Mã Đặt Phòng")]
        public int DatPhongId { get; set; }

        [Required]
        [Display(Name = "Ngày Check-in")]
        public DateTime NgayCheckIn { get; set; }

        [Required]
        [Display(Name = "Ngày Check-out")]
        public DateTime NgayCheckOut { get; set; }

        [Display(Name = "Ngày Đặt")]
        public DateTime NgayDat { get; set; } = DateTime.Now;

        [Display(Name = "Tiền Đặt Cọc")] // Đặt cọc
        public decimal TienDatCoc { get; set; }

        [Required]
        [Display(Name = "Trạng Thái")] // Trạng thái đặt phòng, xác nhận đặt
        public string TrangThai { get; set; }

        // Khóa ngoại:
        [Display(Name = "Mã Người Dùng")]
        public int NguoiDungId { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }

        // Liên kết:
        // ĐÃ SỬA: Liên kết M-N với Phong, thông qua bảng trung gian DatPhongPhong
        // Liên kết:
        // ĐÃ SỬA: Liên kết M-N với Phong, thông qua bảng trung gian DatPhongPhong
        public virtual ICollection<DatPhongPhong> DatPhongPhongs { get; set; }

        // ĐÃ THÊM: Liên kết 1-N với DichVuSuDung (Một Đặt phòng có nhiều Dịch vụ sử dụng)
        public virtual ICollection<DichVuSuDung> DichVuSuDungs { get; set; } // << THÊM MỚI >>

        // ĐÃ THÊM: Liên kết 1-0..1 với HoaDon (Một Đặt phòng có tối đa một Hóa đơn)
        public virtual HoaDon HoaDon { get; set; } // (Đã có trong snippet bạn gửi)
    }
}