using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class User : IdentityUser
    {
        public string Role { get; set; } = "User";
        //Ссылка на автарку пользователя
        public string AvatarUrl { get; set; }

        //Социальные сети
        public string Github { get; set; }
        public string Website { get; set; }
        public string Twitter { get; set; }
        public string Discord { get; set; }
        public string Facebook { get; set; }
        //

        //Статьи
    }
}
