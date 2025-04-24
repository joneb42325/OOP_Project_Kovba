using System.ComponentModel.DataAnnotations;

namespace OOP_Project_Kovba.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Електронна пошта обов'язкова")]
        [EmailAddress(ErrorMessage = "Некоректний формат електронної пошти")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обов'язковий")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public bool RememberMe { get; set; }
    }
}
