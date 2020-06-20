using System;
using System.Collections.Generic;
using System.Text;

namespace BlogWeb.Entity
{
    public class EmailRegistration
    {
        public int EmailRegistrationId { get; set; }
        public string EmailAddress { get; set; }
        public string ControlCode { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
