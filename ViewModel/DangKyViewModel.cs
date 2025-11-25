using System;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhachSan.ViewModels
{
    public class DangKyViewModel
    {
        [Required(ErrorMessage = "Không được bỏ trống tên đăng nhập")]
        [RegularExpression(@"^[a-zA-Z0-9]+$", ErrorMessage = "Tên đăng nhập viết liền không dấu, chỉ chứa chữ và số.")]
        public string TenDangNhap { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu ít nhất 6 ký tự")]
        public string MatKhau { get; set; }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [RegularExpression(@"^0\d{9,10}$", ErrorMessage = "Số điện thoại phải bắt đầu bằng 0 và đủ 10-11 số")]
        public string SoDienThoai { get; set; }

        public string GioiTinh { get; set; }

        [DataType(DataType.Date)]
        [CustomValidation(typeof(DangKyViewModel), nameof(KiemTraNgaySinh))]
        public DateTime? NgaySinh { get; set; }

        // Validation ngày sinh
        public static ValidationResult KiemTraNgaySinh(DateTime? ngaySinh, ValidationContext context)
        {
            if (ngaySinh.HasValue && ngaySinh.Value > DateTime.Now)
                return new ValidationResult("Ngày sinh không được lớn hơn ngày hiện tại");
            return ValidationResult.Success;
        }
    }
}