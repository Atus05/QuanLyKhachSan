using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class LoaiPhongController : Controller
    {
        private HotelContext db = new HotelContext();

        // Hàm kiểm tra quyền Admin
        private bool CheckAdminAccess()
        {
            // Kiểm tra xem Session["VaiTro"] có tồn tại và có giá trị là "admin" không
            return Session["VaiTro"] != null && Session["VaiTro"].ToString().ToLower() == "admin";
        }

        // GET: Admin/LoaiPhong
        public ActionResult Index()
        {
            if (!CheckAdminAccess())
            {
                // Nếu không phải Admin, chuyển hướng về trang Đăng nhập
                return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });
            }

            // Lấy tất cả loại phòng và truyền sang View
            var loaiPhongs = db.LoaiPhongs.ToList();
            return View(loaiPhongs);
        }

        // GET: Admin/LoaiPhong/Create
        public ActionResult Create()
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });
            return View();
        }

        // POST: Admin/LoaiPhong/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenLoai,MoTa")] LoaiPhong loaiPhong)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (ModelState.IsValid)
            {
                // Thêm vào DbSet
                db.LoaiPhongs.Add(loaiPhong);
                // Lưu vào database
                db.SaveChanges();
                TempData["SuccessMessage"] = "Thêm loại phòng mới thành công!";
                return RedirectToAction("Index");
            }
            return View(loaiPhong);
        }

        // GET: Admin/LoaiPhong/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Tìm loại phòng theo ID
            LoaiPhong loaiPhong = db.LoaiPhongs.Find(id);
            if (loaiPhong == null)
            {
                return HttpNotFound();
            }
            return View(loaiPhong);
        }

        // POST: Admin/LoaiPhong/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LoaiPhongId,TenLoai,MoTa")] LoaiPhong loaiPhong)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (ModelState.IsValid)
            {
                // Đánh dấu đối tượng là đã thay đổi
                db.Entry(loaiPhong).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật loại phòng thành công!";
                return RedirectToAction("Index");
            }
            return View(loaiPhong);
        }

        // POST: Admin/LoaiPhong/DeleteConfirmed
        // Xóa sử dụng POST để tránh bị giả mạo qua URL
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!CheckAdminAccess())
            {
                return Json(new { success = false, message = "Bạn không có quyền thực hiện thao tác này." });
            }

            try
            {
                LoaiPhong loaiPhong = db.LoaiPhongs
                    .Include(lp => lp.Phongs) // Kiểm tra xem có phòng nào thuộc loại này không
                    .SingleOrDefault(lp => lp.LoaiPhongId == id);

                if (loaiPhong == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy loại phòng." });
                }

                // Kiểm tra ràng buộc: Nếu có phòng nào đang sử dụng loại này, không cho phép xóa
                if (loaiPhong.Phongs != null && loaiPhong.Phongs.Any())
                {
                    return Json(new { success = false, message = "Không thể xóa loại phòng này vì có phòng đang sử dụng. Vui lòng xóa hoặc chuyển phòng liên quan trước." });
                }

                db.LoaiPhongs.Remove(loaiPhong);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Xóa loại phòng thành công!";
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi hệ thống: {ex.Message}" });
            }
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