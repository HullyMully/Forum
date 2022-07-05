using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Это поле обязательное!")]
        [EmailAddress(ErrorMessage = "Укажите настоящий Email-адрес")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Это поле обязательное!")]
        [Display(Name = "Имя")]
        [MinLength(4, ErrorMessage = "Минимальное количесвто символов - 4")]
        [MaxLength(32, ErrorMessage = "Максимальное количесвто символов - 32")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Это поле обязательное!")]
        [MinLength(6, ErrorMessage = "Минимальное количесвто символов - 6")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Это поле обязательное!")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]      
        [DataType(DataType.Password)]
        [Display(Name = "Подтвердить пароль")]
        public string PasswordConfirm { get; set; }
    }
}
