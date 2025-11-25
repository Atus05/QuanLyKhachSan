// File: Models/LoaiPhong.cs

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhachSan.Models
{
    public class LoaiPhong
    {
        [Key]
        [Display(Name = "Mã Loại Phòng")]
        public int LoaiPhongId { get; set; } // PK

        [Required]
        [Display(Name = "Tên Loại Phòng")]
        public string TenLoai { get; set; } // Ví dụ: Standard, Deluxe, Suite

        [Display(Name = "Mô Tả Chi Tiết")]
        public string MoTa { get; set; }

        // Liên kết:
        public virtual ICollection<Phong> Phongs { get; set; } // Một loại phòng có nhiều phòng
    }
}