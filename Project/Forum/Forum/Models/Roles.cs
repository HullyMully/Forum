using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public enum Roles
    {
        [Display(Name = "Admin")]
        Admin,
        [Display(Name = "User")]
        User
    }
}
