using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.ViewModels
{
    public class ReportViewModel
    {
        public string ArticleId { get; set; }
        public string UserId { get; set; }
        [Required(ErrorMessage = "Это поле обязательное!")]
        [Display(Name = "Содержание")]
        [MinLength(16, ErrorMessage = "Минимальное количесвто символов - 16")]
        [MaxLength(250, ErrorMessage = "Максимальное количесвто символов - 250")]
        public string Content { get; set; }
    }
}
