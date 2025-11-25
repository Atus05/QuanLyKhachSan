// File: Models/DatPhongPhong.cs (Xác nhận/Tạo mới)

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuanLyKhachSan.Models
{
    // Model này không cần thuộc tính ID riêng nếu bạn định nghĩa khóa kép trong Context
    public class DatPhongPhong
    {
        // Khóa Ngoại 1 (thành phần của Khóa Chính Kép)
        [Key, Column(Order = 0)]
        public int DatPhongId { get; set; }
        public virtual DatPhong DatPhong { get; set; }

        // Khóa Ngoại 2 (thành phần của Khóa Chính Kép)
        [Key, Column(Order = 1)]
        public int PhongId { get; set; }
        public virtual Phong Phong { get; set; }
    }
}