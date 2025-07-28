using System.ComponentModel.DataAnnotations;
namespace WebFM_Style.Models
{
    public class ResetPasswordViewModel
    {

        [Required(ErrorMessage = "Vui lòng nhập token.")]
        public string? Token { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu mới.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp.")]
        public string? ConfirmPassword { get; set; }
    }
}
