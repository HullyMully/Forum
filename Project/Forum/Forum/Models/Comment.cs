using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ArticleId { get; set; }
        public string Content { get; set; }
    }
}
