using System;
using System.Collections.Generic;
using System.Text;

namespace BlogWeb.Entity
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public DateTime CreateTime { get; set; }
        public bool IsActive { get; set; }
        public string AnswerToName { get; set; }
        public int ParentId { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
