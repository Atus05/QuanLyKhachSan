// File: Models/Phong.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhachSan.Models
{
    public class Phong
    {
        [Key]
        [Display(Name = "Mã Phòng")]
        public int PhongId { get; set; }

        [Required]
        [Display(Name = "Số Phòng")]
        public string SoPhong { get; set; }

        [Required]
        [Display(Name = "Giá Phòng")] // Giá phòng
        public decimal GiaPhong { get; set; }

        [Display(Name = "Mô Tả")]
        public string MoTa { get; set; }

        [Required]
        [Display(Name = "Tình Trạng")] // Tình trạng phòng
        public string TinhTrang { get; set; } // Ví dụ: 'Trong', 'DaDat', 'DangO'

        // Khóa ngoại:
        [Display(Name = "Mã Loại Phòng")]
        public int LoaiPhongId { get; set; }
        public virtual LoaiPhong LoaiPhong { get; set; } // Loại phòng

        // Liên kết:
        // ĐÃ SỬA: Liên kết M-N với DatPhong, thông qua bảng trung gian DatPhongPhong
        public virtual ICollection<DatPhongPhong> DatPhongPhongs { get; set; }
    }
}