using Forum.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModels
{
    public class ArticleViewModel
    {
        [Required(ErrorMessage = "Это поле обязательное!")]
        [Display(Name = "Название")]
        [MinLength(4, ErrorMessage = "Минимальное количесвто символов - 4")]
        [MaxLength(32, ErrorMessage = "Максимальное количесвто символов - 32")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Это поле обязательное!")]
        [Display(Name = "Содержание")]
        [MinLength(10, ErrorMessage = "Минимальное количесвто символов - 10")]
        public string Content { get; set; }
    }
}
