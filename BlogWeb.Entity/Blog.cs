using System;
using System.Collections.Generic;
using System.Text;

namespace BlogWeb.Entity
{
    public class Blog
    {
        public int BlogId { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string Explanation { get; set; }
        public string Text { get; set; }
        public string ImageUrl { get; set; }
        public string VisitorHit { get; set; }
        public bool IsActive { get; set; }
        public bool IsMailSend { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PublishedDate { get; set; }
        public DateTime LastEditDate { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public List<Gallery> Galleries { get; set; }
        public List<Comment> Comments { get; set; }

    }
}
