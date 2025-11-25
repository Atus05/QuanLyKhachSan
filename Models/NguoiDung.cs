using QuanLyKhachSan.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSan.Models
{
    public class NguoiDung
    {
        [Key]
        public int NguoiDungID { get; set; }

        [Required(ErrorMessage = "Không được bỏ trống tên đăng nhập")]
        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [Required]
        public string MatKhauMaHoa { get; set; }  // Bắt buộc dùng tên này

        [Required]
        public string HoTen { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string SoDienThoai { get; set; }

        public string GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string Avatar { get; set; }

        public bool TrangThai { get; set; } = false; // chờ duyệt
        public DateTime NgayTao { get; set; } = DateTime.Now;

        [ForeignKey("VaiTro")]
        public int VaiTroID { get; set; }
        public virtual VaiTro VaiTro { get; set; }
    }
}