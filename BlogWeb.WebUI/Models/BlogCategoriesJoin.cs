using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWeb.WebUI.Models
{
    public class BlogCategoriesJoin
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string CommentId { get; set; }
        public bool IsActive { get; set; }
        public bool IsMailSend { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime LastEditDate { get; set; }
    }
}
