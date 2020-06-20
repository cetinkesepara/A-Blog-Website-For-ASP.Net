using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWeb.WebUI.Models
{
    public class CaptchaResults
    {
        // Captcha'i resme çizmek ve oturumda saklamak için rastgele üretilen kodun string değeri alınır.
        public string CaptchaStringCode { get; set; }
        // Captcha'i hafıza(memory)'da saklamak için kodun byte değerleri alınır.
        public byte[] CaptchaByteDatas { get; set; }
    }
}
