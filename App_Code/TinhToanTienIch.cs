using System;
using System.Collections.Generic;
using System.Linq;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.App_Code
{
    public static class TinhToanTienIch
    {
        /// <summary>
        /// Tính số ngày ở giữa ngày check-in và check-out
        /// </summary>
        public static int TinhSoNgayO(DateTime ngayCheckIn, DateTime ngayCheckOut)
        {
            if (ngayCheckOut <= ngayCheckIn)
                return 0;
            
            return (ngayCheckOut - ngayCheckIn).Days;
        }

        /// <summary>
        /// Tính tổng tiền phòng dựa trên giá phòng và số ngày ở
        /// </summary>
        public static decimal TinhTienPhong(decimal giaPhong, int soNgayO)
        {
            return giaPhong * soNgayO;
        }

        /// <summary>
        /// Tính tổng tiền dịch vụ từ danh sách dịch vụ sử dụng
        /// </summary>
        public static decimal TinhTienDichVu(ICollection<DichVuSuDung> dichVuSuDungs)
        {
            if (dichVuSuDungs == null || !dichVuSuDungs.Any())
                return 0;

            return dichVuSuDungs.Sum(dv => dv.SoLuong * (dv.DichVu?.Gia ?? 0));
        }

        /// <summary>
        /// Tính tổng tiền đặt phòng (tiền phòng + tiền dịch vụ)
        /// </summary>
        public static decimal TinhTongTienDatPhong(DatPhong datPhong)
        {
            if (datPhong == null)
                return 0;

            int soNgayO = TinhSoNgayO(datPhong.NgayCheckIn, datPhong.NgayCheckOut);
            
            // Tính tổng tiền phòng từ các phòng đã đặt
            decimal tienPhong = 0;
            if (datPhong.DatPhongPhongs != null && datPhong.DatPhongPhongs.Any())
            {
                tienPhong = datPhong.DatPhongPhongs
                    .Sum(dpp => TinhTienPhong(dpp.Phong?.GiaPhong ?? 0, soNgayO));
            }

            // Tính tổng tiền dịch vụ
            decimal tienDichVu = TinhTienDichVu(datPhong.DichVuSuDungs);

            return tienPhong + tienDichVu;
        }

        /// <summary>
        /// Tính tiền còn lại phải trả (tổng tiền - tiền đặt cọc)
        /// </summary>
        public static decimal TinhTienConLai(decimal tongTien, decimal tienDatCoc)
        {
            decimal tienConLai = tongTien - tienDatCoc;
            return tienConLai > 0 ? tienConLai : 0;
        }
    }
}

