using System;
using System.Collections.Generic;
using System.Text;

namespace BlogWeb.Entity
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public int ParentId { get; set; }

        public List<Blog> Blogs { get; set; }
    }
}
