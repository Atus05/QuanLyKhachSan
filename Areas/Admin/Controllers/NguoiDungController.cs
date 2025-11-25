using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using QuanLyKhachSan.DAL;
using QuanLyKhachSan.Models;

namespace QuanLyKhachSan.Areas.Admin.Controllers
{
    public class NguoiDungController : Controller
    {
        private HotelContext db = new HotelContext();

        // Hàm kiểm tra quyền Admin
        private bool CheckAdminAccess()
        {
            return Session["VaiTro"] != null && Session["VaiTro"].ToString().ToLower() == "admin";
        }

        // GET: Admin/NguoiDung
        // Liệt kê tất cả người dùng, bao gồm cả vai trò
        public ActionResult Index()
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            var nguoiDungs = db.NguoiDungs.Include(n => n.VaiTro).ToList();
            return View(nguoiDungs);
        }

        // GET: Admin/NguoiDung/Details/5
        public ActionResult Details(int? id)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            NguoiDung nguoiDung = db.NguoiDungs.Include(n => n.VaiTro).SingleOrDefault(n => n.NguoiDungID == id);

            if (nguoiDung == null) return HttpNotFound();

            return View(nguoiDung);
        }

        // GET: Admin/NguoiDung/Edit/5
        public ActionResult Edit(int? id)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            NguoiDung nguoiDung = db.NguoiDungs.Find(id);
            if (nguoiDung == null) return HttpNotFound();

            // Truyền danh sách vai trò để Admin có thể thay đổi
            ViewBag.VaiTroID = new SelectList(db.VaiTros, "VaiTroID", "TenVaiTro", nguoiDung.VaiTroID);
            return View(nguoiDung);
        }

        // POST: Admin/NguoiDung/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NguoiDungID,HoTen,Email,SoDienThoai,GioiTinh,NgaySinh,TrangThai,VaiTroID,TenDangNhap,MatKhauMaHoa")] NguoiDung nguoiDung)
        {
            if (!CheckAdminAccess()) return RedirectToAction("DangNhap", "TaiKhoan", new { area = "" });

            if (ModelState.IsValid)
            {
                // Giữ lại các trường không cho phép sửa qua Edit này (như TenDangNhap, MatKhauMaHoa)
                // Lưu ý: Nếu không sử dụng cách này, bạn cần lấy đối tượng cũ ra và chỉ cập nhật các trường được phép.

                db.Entry(nguoiDung).State = EntityState.Modified;

                // Loại trừ TenDangNhap và MatKhauMaHoa khỏi việc cập nhật nếu chúng không thay đổi
                db.Entry(nguoiDung).Property(x => x.TenDangNhap).IsModified = false;
                db.Entry(nguoiDung).Property(x => x.MatKhauMaHoa).IsModified = false;
                db.Entry(nguoiDung).Property(x => x.NgayTao).IsModified = false;

                db.SaveChanges();
                TempData["SuccessMessage"] = $"Cập nhật thông tin người dùng {nguoiDung.HoTen} thành công!";
                return RedirectToAction("Index");
            }

            ViewBag.VaiTroID = new SelectList(db.VaiTros, "VaiTroID", "TenVaiTro", nguoiDung.VaiTroID);
            return View(nguoiDung);
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