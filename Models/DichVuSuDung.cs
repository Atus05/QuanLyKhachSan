// File: Models/DichVuSuDung.cs
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhachSan.Models
{
    public class DichVuSuDung
    {
        [Key]
        [Display(Name = "Mã Dịch Vụ Sử Dụng")]
        public int DichVuSuDungId { get; set; }

        [Required]
        [Display(Name = "Số Lượng")]
        public int SoLuong { get; set; }

        // Khóa ngoại:
        public int DatPhongId { get; set; }
        public virtual DatPhong DatPhong { get; set; } // Liên kết đến lịch sử đặt phòng

        public int DichVuId { get; set; }
        public virtual DichVu DichVu { get; set; }
    }
}