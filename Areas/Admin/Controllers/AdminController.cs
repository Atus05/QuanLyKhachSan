using QuanLyKhachSan.DAL;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private HotelContext db = new HotelContext();

        // GET: Admin/Dashboard
        public ActionResult Dashboard()
        {
            // Kiểm tra session user
            if (Session["VaiTro"] == null || Session["VaiTro"].ToString().ToLower() != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });
            }

            // Lấy dữ liệu cho các thẻ số liệu
            try
            {
                DateTime today = DateTime.Today;

                // 1. Tổng Khách hàng (Giả định VaiTroID = 3 là Khách hàng)
                ViewBag.TongKhachHang = db.NguoiDungs.Count(n => n.VaiTroID == 3);

                // 2. Tổng Phòng
                ViewBag.TongPhong = db.Phongs.Count();

                // 3. Đặt phòng hôm nay (Đếm các đơn đặt được tạo hôm nay)
                ViewBag.DatPhongHomNay = db.DatPhongs
                    .Count(dp => DbFunctions.TruncateTime(dp.NgayDat) == today);

                // 4. Doanh thu hôm nay (Tính tổng hóa đơn đã thanh toán hôm nay)
                ViewBag.DoanhThuHomNay = db.HoaDons
                    .Where(hd => DbFunctions.TruncateTime(hd.NgayXuat) == today && hd.TrangThaiThanhToan == "Đã thanh toán")
                    .Sum(hd => (decimal?)hd.TongTien) ?? 0;
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Lỗi khi truy vấn dữ liệu Dashboard: " + ex.Message;
                // Gán giá trị mặc định nếu có lỗi
                ViewBag.TongKhachHang = 0;
                ViewBag.TongPhong = 0;
                ViewBag.DatPhongHomNay = 0;
                ViewBag.DoanhThuHomNay = 0;
            }

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult ThongKe()
        {
            // Kiểm tra session user
            if (Session["VaiTro"] == null || Session["VaiTro"].ToString().ToLower() != "admin")
            {
                return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });
            }

            // Ở đây bạn có thể tính toán các dữ liệu phức tạp hơn 
            // (ví dụ: Doanh thu 12 tháng gần nhất, phòng được đặt nhiều nhất,...)

            // Ví dụ: Lấy danh sách 10 phòng có giá cao nhất
            ViewBag.Top10PhongGiaCao = db.Phongs.OrderByDescending(p => p.GiaPhong).Take(10).ToList();

            return View();
        }
    }
}