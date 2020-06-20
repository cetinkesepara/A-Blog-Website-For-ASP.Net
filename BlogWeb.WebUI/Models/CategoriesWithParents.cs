using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWeb.WebUI.Models
{
    public class CategoriesWithParents
    {
        public int CategoryId { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ParentName { get; set; }
    }
}
