using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWeb.WebUI.Models
{
    public class BlogShowItems
    {
        public int BlogId { get; set; }
        public string Title { get; set; }
        public string Explanation { get; set; }
        public string ImageUrl { get; set; }
        public string VisitorHit { get; set; }
        public DateTime PublishedDate { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
        public int CommentCount { get; set; }
    }
}
