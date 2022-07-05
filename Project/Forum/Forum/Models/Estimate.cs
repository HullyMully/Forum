using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Estimate
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ArticleId { get; set; }
        public bool Like { get; set; }
        public bool Dislike { get; set; } 
    }
}
