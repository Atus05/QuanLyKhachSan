using QuanLyKhachSan.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class VaiTro
{
    [Key]
    public int VaiTroID { get; set; }

    [Required]
    [StringLength(50)]
    public string TenVaiTro { get; set; }

    // 1 Vai trò có nhiều người dùng
    public virtual ICollection<NguoiDung> NguoiDungs { get; set; }
}