// File: DAL/HotelContext.cs

using System.Data.Entity;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.DAL
{
    // Kế thừa từ DbContext
    public class HotelContext : DbContext
    {
        // 1. Chỉ định chuỗi kết nối: Phải khớp với tên trong Web.config (QuanLyKhachSan)
        public HotelContext() : base("name=QuanLyKhachSan")
        {
        }

        // 2. Khai báo các Bảng (DbSet)
        public DbSet<Phong> Phongs { get; set; }
        public DbSet<LoaiPhong> LoaiPhongs { get; set; }
        public DbSet<DatPhong> DatPhongs { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }
        public DbSet<DichVu> DichVus { get; set; }
        public DbSet<DichVuSuDung> DichVuSuDungs { get; set; }
        public DbSet<DatPhongPhong> DatPhongPhongs { get; set; }
        public DbSet<VaiTro> VaiTros { get; set; }
        public DbSet<NguoiDung> NguoiDungs { get; set; }
    }
}