using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class HoaDonController : Controller
    {
        private HotelContext db = new HotelContext();

        // Hàm kiểm tra quyền Admin
        private bool CheckAdminAccess()
        {
            return Session["VaiTro"] != null && Session["VaiTro"].ToString().ToLower() == "admin";
        }

        // GET: Admin/HoaDon
        // Hiển thị danh sách tất cả hóa đơn đã hoàn thành
        public ActionResult Index()
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            // Lấy tất cả hóa đơn, bao gồm thông tin đặt phòng và người dùng
            var hoaDons = db.HoaDons
                .Include(h => h.DatPhong)
                .Include(h => h.DatPhong.NguoiDung)
                .OrderByDescending(h => h.NgayXuat)
                .ToList();

            return View(hoaDons);
        }

        // GET: Admin/HoaDon/Details/5
        // Xem chi tiết hóa đơn (Phòng, Dịch vụ sử dụng)
        public ActionResult Details(int? id)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            // Tìm kiếm Hóa đơn theo DatPhongId (vì DatPhongId là Khóa Chính của HoaDon)
            HoaDon hoaDon = db.HoaDons
                .Include(h => h.DatPhong)
                .Include(h => h.DatPhong.NguoiDung)
                .Include(h => h.DatPhong.DatPhongPhongs.Select(dpp => dpp.Phong.LoaiPhong)) // Chi tiết phòng
                .Include(h => h.DatPhong.DichVuSuDungs.Select(dvs => dvs.DichVu)) // Chi tiết dịch vụ
                .SingleOrDefault(h => h.DatPhongId == id);

            if (hoaDon == null) return HttpNotFound();

            return View(hoaDon);
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