using System.ComponentModel.DataAnnotations;

namespace Shop_Maintain.Models
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự.")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc.")]
        [Compare("Password", ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp.")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
