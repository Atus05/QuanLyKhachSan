using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class DichVuController : Controller
    {
        private HotelContext db = new HotelContext();

        // Hàm kiểm tra quyền Admin
        private bool CheckAdminAccess()
        {
            return Session["VaiTro"] != null && Session["VaiTro"].ToString().ToLower() == "admin";
        }

        // GET: Admin/DichVu
        public ActionResult Index()
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            var dichVus = db.DichVus.ToList();
            return View(dichVus);
        }

        // GET: Admin/DichVu/Create
        public ActionResult Create()
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });
            return View();
        }

        // POST: Admin/DichVu/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenDichVu,Gia")] DichVu dichVu)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (ModelState.IsValid)
            {
                db.DichVus.Add(dichVu);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Thêm dịch vụ mới thành công!";
                return RedirectToAction("Index");
            }
            return View(dichVu);
        }

        // GET: Admin/DichVu/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            DichVu dichVu = db.DichVus.Find(id);
            if (dichVu == null) return HttpNotFound();

            return View(dichVu);
        }

        // POST: Admin/DichVu/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DichVuId,TenDichVu,Gia")] DichVu dichVu)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (ModelState.IsValid)
            {
                db.Entry(dichVu).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật dịch vụ thành công!";
                return RedirectToAction("Index");
            }
            return View(dichVu);
        }

        // POST: Admin/DichVu/DeleteConfirmed
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!CheckAdminAccess())
            {
                return Json(new { success = false, message = "Bạn không có quyền thực hiện thao tác này." });
            }

            try
            {
                DichVu dichVu = db.DichVus.Find(id);

                if (dichVu == null)
                {
                    return Json(new { success = false, message = "Không tìm thấy dịch vụ." });
                }

                // Kiểm tra ràng buộc: Nếu dịch vụ này đã từng được sử dụng (có trong DichVuSuDungs), không cho phép xóa
                if (db.DichVuSuDungs.Any(dvs => dvs.DichVuId == id))
                {
                    return Json(new { success = false, message = "Không thể xóa dịch vụ này vì đã có lịch sử sử dụng." });
                }

                db.DichVus.Remove(dichVu);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Xóa dịch vụ thành công!";
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