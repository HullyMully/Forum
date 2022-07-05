using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Article
    {
        public string UserId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Like { get; set; }
        public int Dislike { get; set; }

    }
}
