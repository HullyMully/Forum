using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Forum.Models
{
    public class Report
    {
        public string Id { get; set; }
        public string Content { get; set; }
        public string ArticleId { get; set; }
        public string SenderUserId { get; set; }
        public string HostUserId { get; set; }
    }
}
