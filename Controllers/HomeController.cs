using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
// Đảm bảo namespace trùng khớp với dự án của bạn
namespace QuanLyKhachSan.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index
        // Đây là trang chủ, nơi hiển thị form tìm kiếm phòng.
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Home/About (Giới thiệu về khách sạn)
        public ActionResult About()
        {
            ViewBag.Message = "Thông tin giới thiệu về Khách Sạn ABC và các tiện nghi.";
            return View();
        }

        // GET: /Home/Contact (Thông tin liên hệ)
        public ActionResult Contact()
        {
            ViewBag.Message = "Thông tin liên hệ của Khách Sạn ABC.";
            return View();
        }
        public ActionResult Search(string q)
        {
            // Logic tìm kiếm của bạn dựa trên chuỗi q
            ViewBag.SearchQuery = q;
            return View(); // Trả về view kết quả tìm kiếm
        }
        // *Không cần thêm các Action liên quan đến phòng hay booking vào đây.*
        // *Các chức năng đó đã được chuyển sang RoomController và BookingController.*
    }
}