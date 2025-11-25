// File: Controllers/TaiKhoanController.cs (NỘI DUNG ĐÃ SỬA CHÍNH XÁC LỖI CS1671)

using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;
using QuanLyKhachSan.ViewModels;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuanLyKhachSan.Controllers // KHÔNG ĐẶT ATTRIBUTE TRÊN DÒNG NÀY
{
    // *** ATTRIBUTE PHẢI ĐẶT TRÊN CLASS ***
    [RoutePrefix("Account")]
    public class TaiKhoanController : Controller
    {
        private HotelContext context = new HotelContext();

        // GET: DangNhap
        [HttpGet]
        [Route("Login")] // URL: /Account/Login
        public ActionResult DangNhap()
        {
            return View();
        }

        // POST: DangNhap
        [HttpPost]
        [Route("Login")] // URL: /Account/Login
        [ValidateAntiForgeryToken]
        public ActionResult DangNhap(string TenDangNhap, string MatKhau)
        {
            // Logic đăng nhập giữ nguyên
            if (string.IsNullOrEmpty(TenDangNhap) || string.IsNullOrEmpty(MatKhau))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu";
                return View();
            }

            string hashedPassword = MaHoaSHA256(MatKhau);
            var user = context.NguoiDungs
                .FirstOrDefault(x => x.TenDangNhap == TenDangNhap && x.MatKhauMaHoa == hashedPassword);

            if (user != null)
            {
                if (!user.TrangThai)
                {
                    ViewBag.Error = "Tài khoản đang bị khóa";
                    return View();
                }

                Session["UserId"] = user.NguoiDungID;
                Session["TenDangNhap"] = user.TenDangNhap;
                Session["HoTen"] = user.HoTen;
                Session["VaiTro"] = context.VaiTros.Find(user.VaiTroID)?.TenVaiTro;

                if (Session["VaiTro"] != null && Session["VaiTro"].ToString() == "admin")
                    return RedirectToAction("Dashboard", "Admin", new { area = "Admin" });
                else
                    return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng";
                return View();
            }
        }

        // GET: DangKy
        [HttpGet]
        [Route("Register")] // URL: /Account/Register
        public ActionResult DangKy()
        {
            return View();
        }

        // POST: DangKy
        [HttpPost]
        [Route("Register")] // URL: /Account/Register
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(DangKyViewModel model)
        {
            // Logic đăng ký giữ nguyên
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (context.NguoiDungs.Any(x => x.TenDangNhap == model.TenDangNhap))
            {
                ViewBag.Error = "Tên đăng nhập đã tồn tại!";
                return View(model);
            }

            string hash = MaHoaSHA256(model.MatKhau);

            NguoiDung user = new NguoiDung
            {
                TenDangNhap = model.TenDangNhap,
                MatKhauMaHoa = hash,
                HoTen = model.HoTen,
                Email = model.Email,
                SoDienThoai = model.SoDienThoai,
                GioiTinh = model.GioiTinh,
                NgaySinh = model.NgaySinh,
                TrangThai = true,
                VaiTroID = 2 // user thường
            };

            context.NguoiDungs.Add(user);
            context.SaveChanges();

            TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction("DangNhap");
        }

        // ACTION ĐĂNG XUẤT
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Logout")] // URL: /Account/Logout
        public ActionResult DangXuat()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        // Hàm hash SHA256
        private string MaHoaSHA256(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte b in bytes)
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
        }
    }
}