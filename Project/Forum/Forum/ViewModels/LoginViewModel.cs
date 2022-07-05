using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModels
{
    public class LoginViewModel
    {
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

        public string ReturnUrl { get; set; }
    }
}
