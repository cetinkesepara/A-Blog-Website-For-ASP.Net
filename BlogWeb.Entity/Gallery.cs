using System;
using System.Collections.Generic;
using System.Text;

namespace BlogWeb.Entity
{
   public class Gallery
    {
        public int GalleryId { get; set; }
        public string ImageUrl { get; set; }
        public bool IsCover { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
