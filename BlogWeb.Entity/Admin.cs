using System;
using System.Collections.Generic;
using System.Text;

namespace BlogWeb.Entity
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
