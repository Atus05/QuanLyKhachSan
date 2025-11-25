using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;
using System.Collections.Generic; // Cần thiết cho các truy vấn Include

// Đã xác định Namespace theo cấu trúc bạn cung cấp
namespace QuanLyKhachSan.Areas.Admin.App_Code
{
    public class DatPhongController : Controller
    {
        private HotelContext db = new HotelContext();

        // ------------------------------------------------------------------
        // HÀM TIỆN ÍCH TÍNH TỔNG TIỀN (ĐƯỢC TÍCH HỢP VÀO CONTROLLER)
        // ------------------------------------------------------------------
        private decimal TinhTongTienHoaDon(int datPhongId)
        {
            // Lấy thông tin đặt phòng đầy đủ (Phòng, Dịch vụ sử dụng)
            var datPhong = db.DatPhongs
                .Include(dp => dp.DatPhongPhongs.Select(dpp => dpp.Phong))
                .Include(dp => dp.DichVuSuDungs.Select(dvs => dvs.DichVu))
                .SingleOrDefault(dp => dp.DatPhongId == datPhongId);

            if (datPhong == null) return 0;

            // 1. Tính tiền phòng
            // Lấy số đêm lưu trú (ít nhất là 1 ngày nếu Check-in và Check-out cùng ngày)
            int soDem = (int)(datPhong.NgayCheckOut - datPhong.NgayCheckIn).TotalDays;
            if (soDem <= 0) soDem = 1;

            decimal tongTienPhong = 0;

            // Tính tổng tiền phòng từ các phòng đã đặt
            foreach (var dpp in datPhong.DatPhongPhongs)
            {
                tongTienPhong += dpp.Phong.GiaPhong * soDem;
            }

            // 2. Tính tiền dịch vụ
            decimal tongTienDichVu = 0;
            if (datPhong.DichVuSuDungs != null)
            {
                // Tổng (Số lượng * Giá dịch vụ)
                tongTienDichVu = datPhong.DichVuSuDungs
                    .Sum(dvs => (decimal?)dvs.SoLuong * dvs.DichVu.Gia) ?? 0;
            }

            // 3. Tính Tổng tiền cuối cùng
            // Tổng tiền = (Tiền phòng + Tiền dịch vụ) - Tiền đặt cọc
            decimal tongTienFinal = (tongTienPhong + tongTienDichVu) - datPhong.TienDatCoc;

            // Đảm bảo không âm (nếu tiền đặt cọc > tổng chi phí)
            return Math.Max(0, tongTienFinal);
        }

        // ------------------------------------------------------------------
        // CÁC HÀM CONTROLLER KHÁC (GIỮ NGUYÊN)
        // ------------------------------------------------------------------

        private bool CheckAdminAccess()
        {
            return Session["VaiTro"] != null && Session["VaiTro"].ToString().ToLower() == "admin";
        }

        // GET: Admin/DatPhong/Index
        public ActionResult Index()
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            var datPhongs = db.DatPhongs
                .Include(dp => dp.NguoiDung)
                .Where(dp => dp.TrangThai == "Đã xác nhận" || dp.TrangThai == "Đang ở")
                .OrderBy(dp => dp.NgayCheckIn)
                .ToList();
            return View(datPhongs);
        }

        // POST: Admin/DatPhong/CheckInConfirmed/5
        [HttpPost]
        public ActionResult CheckInConfirmed(int id)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            var datPhong = db.DatPhongs
                .Include(dp => dp.DatPhongPhongs.Select(dpp => dpp.Phong))
                .SingleOrDefault(dp => dp.DatPhongId == id);

            if (datPhong == null || datPhong.TrangThai != "Đã xác nhận")
            {
                TempData["ErrorMessage"] = "Không thể Check-in.";
                return RedirectToAction("Index");
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    datPhong.TrangThai = "Đang ở";
                    db.Entry(datPhong).State = EntityState.Modified;

                    foreach (var dpp in datPhong.DatPhongPhongs)
                    {
                        var phong = dpp.Phong;
                        if (phong != null)
                        {
                            phong.TinhTrang = "Đang ở";
                            db.Entry(phong).State = EntityState.Modified;
                        }
                    }

                    db.SaveChanges();
                    transaction.Commit();
                    TempData["SuccessMessage"] = $"Check-in cho đơn hàng #{id} thành công!";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["ErrorMessage"] = $"Lỗi Check-in: {ex.Message}";
                }
            }
            return RedirectToAction("Index");
        }

        // GET: Admin/DatPhong/CheckOut/5 (Hiển thị trang thanh toán)
        public ActionResult CheckOut(int id)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            var datPhong = db.DatPhongs
                .Include(dp => dp.NguoiDung)
                .Include(dp => dp.DatPhongPhongs.Select(dpp => dpp.Phong.LoaiPhong))
                .Include(dp => dp.DichVuSuDungs.Select(dvs => dvs.DichVu))
                .Include(dp => dp.HoaDon)
                .SingleOrDefault(dp => dp.DatPhongId == id && dp.TrangThai == "Đang ở");

            if (datPhong == null)
            {
                TempData["ErrorMessage"] = "Không thể Check-out. Đặt phòng không tồn tại hoặc chưa Check-in.";
                return RedirectToAction("Index");
            }

            // GỌI HÀM TÍNH TOÁN ĐÃ ĐƯỢC TÍCH HỢP
            decimal tongTienCanThanhToan = TinhTongTienHoaDon(id);

            ViewBag.TongTienFinal = tongTienCanThanhToan;
            return View(datPhong); // Trả về View để xác nhận thanh toán
        }

        // POST: Admin/DatPhong/CheckOutConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckOutConfirmed(int datPhongId, decimal tongTien, string phuongThucThanhToan)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            var datPhong = db.DatPhongs
                .Include(dp => dp.DatPhongPhongs)
                .SingleOrDefault(dp => dp.DatPhongId == datPhongId && dp.TrangThai == "Đang ở");

            if (datPhong == null)
            {
                TempData["ErrorMessage"] = "Đặt phòng không hợp lệ để Check-out.";
                return RedirectToAction("Index");
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 1. Tạo Hóa Đơn MỚI
                    var hoaDonMoi = new HoaDon
                    {
                        DatPhongId = datPhong.DatPhongId,
                        TongTien = tongTien,
                        PhuongThucThanhToan = phuongThucThanhToan,
                        TrangThaiThanhToan = "Đã thanh toán",
                        NgayXuat = DateTime.Now
                    };
                    db.HoaDons.Add(hoaDonMoi);

                    // 2. Cập nhật trạng thái Đặt phòng
                    datPhong.TrangThai = "Đã hoàn thành";
                    db.Entry(datPhong).State = EntityState.Modified;

                    // 3. Cập nhật trạng thái TẤT CẢ các phòng liên quan
                    foreach (var dpp in datPhong.DatPhongPhongs)
                    {
                        var phong = db.Phongs.Find(dpp.PhongId);
                        if (phong != null)
                        {
                            phong.TinhTrang = "Đang dọn"; // Chờ nhân viên dọn dẹp
                            db.Entry(phong).State = EntityState.Modified;
                        }
                    }

                    db.SaveChanges();
                    transaction.Commit();
                    TempData["SuccessMessage"] = $"Check-out thành công! Hóa đơn đã được tạo.";
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["ErrorMessage"] = $"Lỗi Check-out: {ex.Message}";
                }
            }

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}