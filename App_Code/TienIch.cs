using System;
using System.Linq;
using QuanLyKhachSan.Models;
using QuanLyKhachSan.DAL;
using System.Data.Entity;

namespace QuanLyKhachSan.Areas.Admin.App_Code 
{
    public static class TinhToanTienIch
    {
        public static decimal TinhTongTienHoaDon(int datPhongId, HotelContext db)
        {
            // Lấy thông tin đặt phòng cần thiết
            var datPhong = db.DatPhongs
                .Include(dp => dp.DatPhongPhongs.Select(dpp => dpp.Phong))
                .Include(dp => dp.DichVuSuDungs.Select(dvs => dvs.DichVu))
                .SingleOrDefault(dp => dp.DatPhongId == datPhongId);

            if (datPhong == null) return 0;

            // 1. Tính tiền phòng
            int soDem = (int)(datPhong.NgayCheckOut - datPhong.NgayCheckIn).TotalDays;
            if (soDem <= 0) soDem = 1; // Nếu check-in/out trong cùng ngày

            decimal tongTienPhong = 0;

            // Lặp qua tất cả các phòng đã đặt
            foreach (var dpp in datPhong.DatPhongPhongs)
            {
                // Sử dụng Giá Phòng cố định đã ghi nhận trong Model Phong
                tongTienPhong += dpp.Phong.GiaPhong * soDem;
            }

            // 2. Tính tiền dịch vụ
            decimal tongTienDichVu = 0;
            if (datPhong.DichVuSuDungs != null)
            {
                tongTienDichVu = datPhong.DichVuSuDungs
                    .Sum(dvs => dvs.SoLuong * dvs.DichVu.Gia);
            }

            // 3. Tính Tổng tiền cuối cùng
            // Tổng tiền = (Tiền phòng + Tiền dịch vụ) - Tiền đặt cọc
            decimal tongTienFinal = (tongTienPhong + tongTienDichVu) - datPhong.TienDatCoc;

            // Đảm bảo không âm
            return Math.Max(0, tongTienFinal);
        }
    }
}