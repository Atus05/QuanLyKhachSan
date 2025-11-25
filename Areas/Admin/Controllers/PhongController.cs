using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class PhongController : Controller
    {
        private HotelContext db = new HotelContext();

        // Hàm kiểm tra Admin
        private bool CheckAdminAccess()
        {
            return Session["VaiTro"] != null && Session["VaiTro"].ToString().ToLower() == "admin";
        }

        // GET: Admin/Phong
        public ActionResult Index()
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });
            // Load thông tin Loại phòng kèm theo
            var phongs = db.Phongs.Include(p => p.LoaiPhong).ToList();
            return View(phongs);
        }

        // GET: Admin/Phong/Create
        public ActionResult Create()
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });
            // Truyền danh sách Loại phòng để chọn trong View
            ViewBag.LoaiPhongId = new SelectList(db.LoaiPhongs, "LoaiPhongId", "TenLoai");
            return View();
        }

        // POST: Admin/Phong/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SoPhong,GiaPhong,MoTa,TinhTrang,LoaiPhongId")] Phong phong)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            // KHẮC PHỤC LỖI QUAN TRỌNG: Gán giá trị mặc định cho TinhTrang vì nó là [Required]
            phong.TinhTrang = "Trống";

            if (ModelState.IsValid)
            {
                // THÊM LOGIC: Kiểm tra trùng lặp Số Phòng
                if (db.Phongs.Any(p => p.SoPhong == phong.SoPhong))
                {
                    ModelState.AddModelError("SoPhong", "Số phòng này đã tồn tại. Vui lòng chọn số khác.");
                }
                else
                {
                    db.Phongs.Add(phong);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = $"Thêm phòng {phong.SoPhong} thành công.";
                    return RedirectToAction("Index");
                }
            }

            // Nếu xảy ra lỗi Validation (bao gồm cả lỗi trùng lặp số phòng)
            ViewBag.LoaiPhongId = new SelectList(db.LoaiPhongs, "LoaiPhongId", "TenLoai", phong.LoaiPhongId);
            TempData["ErrorMessage"] = "Thêm phòng thất bại. Vui lòng kiểm tra lại thông tin nhập vào.";
            return View(phong);
        }

        // GET: Admin/Phong/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Phong phong = db.Phongs.Find(id);
            if (phong == null) return HttpNotFound();

            ViewBag.LoaiPhongId = new SelectList(db.LoaiPhongs, "LoaiPhongId", "TenLoai", phong.LoaiPhongId);
            return View(phong);
        }

        // POST: Admin/Phong/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhongId,SoPhong,GiaPhong,MoTa,TinhTrang,LoaiPhongId")] Phong phong)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (ModelState.IsValid)
            {
                // THÊM LOGIC: Kiểm tra trùng lặp Số Phòng (loại trừ chính nó)
                if (db.Phongs.Any(p => p.SoPhong == phong.SoPhong && p.PhongId != phong.PhongId))
                {
                    ModelState.AddModelError("SoPhong", "Số phòng này đã tồn tại với phòng khác. Vui lòng chọn số khác.");
                }
                else
                {
                    db.Entry(phong).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = $"Cập nhật phòng {phong.SoPhong} thành công.";
                    return RedirectToAction("Index");
                }
            }

            // Nếu Validation thất bại
            ViewBag.LoaiPhongId = new SelectList(db.LoaiPhongs, "LoaiPhongId", "TenLoai", phong.LoaiPhongId);
            TempData["ErrorMessage"] = "Cập nhật thất bại. Vui lòng kiểm tra lại thông tin.";
            return View(phong);
        }

        // POST: Admin/Phong/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            // Kiểm tra nếu phòng này đang có trong bất kỳ đơn đặt phòng nào
            if (db.DatPhongPhongs.Any(dpp => dpp.PhongId == id))
            {
                TempData["ErrorMessage"] = "Không thể xóa. Phòng này có liên quan đến các đơn đặt phòng.";
                return RedirectToAction("Index");
            }

            Phong phong = db.Phongs.Find(id);
            db.Phongs.Remove(phong);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Xóa phòng thành công.";
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