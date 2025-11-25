// File: Models/DichVu.cs
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhachSan.Models
{
    public class DichVu
    {
        [Key]
        [Display(Name = "Mã Dịch Vụ")]
        public int DichVuId { get; set; }

        [Required]
        [Display(Name = "Tên Dịch Vụ")]
        public string TenDichVu { get; set; }

        [Required]
        [Display(Name = "Giá")]
        public decimal Gia { get; set; }

        // Liên kết:
        public virtual ICollection<DichVuSuDung> DichVuSuDungs { get; set; }
    }
}