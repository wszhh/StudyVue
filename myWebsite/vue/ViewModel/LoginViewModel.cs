using System.ComponentModel.DataAnnotations;

namespace vue.ViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "用户名不能为空！")]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空！")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}
