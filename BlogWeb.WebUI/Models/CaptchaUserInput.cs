using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWeb.WebUI.Models
{
    public class CaptchaUserInput
    {
        //Onaylama(Validate) işleminde kullanmak için kullanıcının girdiği Captcha kodu alınır.
        public string CaptchaCode { get; set; }
    }
}
