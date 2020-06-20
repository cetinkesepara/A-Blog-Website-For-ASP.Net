using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWeb.WebUI.Models
{
    public class PostComment
    {
        public int BlogId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Comment { get; set; }
        public int? CommentId { get; set; }
        public string ToName { get; set; }
    }
}
