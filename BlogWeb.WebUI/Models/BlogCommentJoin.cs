using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWeb.WebUI.Models
{
    public class BlogCommentJoin
    {
        public int BlogId { get; set; }
        public string BlogTitle { get; set; }
        public int CommentId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsActive { get; set; }
        public string AnswerToName { get; set; }
    }
}
