using QuanLyKhachSan.DAL;     // <<< THÊM DÒNG NÀY
using QuanLyKhachSan.Models;
using System;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace QuanLyKhachSan.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<HotelContext>   // <<< SỬA Ở ĐÂY
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(HotelContext context)   // <<< SỬA Ở ĐÂY
        {
            // Thêm vai trò admin và user
            context.VaiTros.AddOrUpdate(
                x => x.TenVaiTro,
                new VaiTro { TenVaiTro = "admin" },
                new VaiTro { TenVaiTro = "user" }
            );

            context.SaveChanges();

            // Lấy VaiTroID
            var adminRole = context.VaiTros.FirstOrDefault(x => x.TenVaiTro == "admin");

            // Thêm user admin mặc định nếu chưa có
            context.NguoiDungs.AddOrUpdate(
                x => x.TenDangNhap,
                new NguoiDung
                {
                    TenDangNhap = "admin",
                    MatKhauMaHoa = MaHoaSHA256("123456"),
                    HoTen = "Administrator",
                    Email = "admin@rap.com",
                    SoDienThoai = "0900000000",
                    GioiTinh = "Nam",
                    NgaySinh = new DateTime(2000, 1, 1),
                    VaiTroID = adminRole.VaiTroID,
                    TrangThai = true,
                    NgayTao = DateTime.Now
                }
            );
        }

        // Hàm hash SHA256
        private string MaHoaSHA256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var sb = new StringBuilder();
                foreach (var b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}
