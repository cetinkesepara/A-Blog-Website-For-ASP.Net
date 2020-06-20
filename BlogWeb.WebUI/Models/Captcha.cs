using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWeb.WebUI.Models
{
    public class Captcha
    {
        public static CaptchaResults GenerateCaptcha(int captchaWidth, int captchaHeight, int captchaLength)
        {
            // Captcha için rastgele bir kod üretiliyor.
            string CaptchaGeneratedCode = CaptchaGenerateCode(captchaLength);

            // Tanımlanan veya üretilen resmin pixel verileri ile çalışmak için bir 'Bitmap' oluşturuyoruz.
            using Bitmap bitmap = new Bitmap(captchaWidth, captchaHeight);

            // Belirlenen 'Bitmap'de çizim yapmak için bir grafik oluşturuluyor.
            using Graphics graphics = Graphics.FromImage(bitmap);

            // Captcha için arkaplan rengi ayarlanıyor.
            graphics.Clear(CaptchaBackgroundColor());

            // Üretilen kod grafik üzerine çiziliyor.
            CaptchaDrawing(graphics,captchaWidth,captchaHeight,CaptchaGeneratedCode);

            // Captcha zorlaştırmak için grafik üzerine çizgiler ekliyoruz.
            CaptchaLineDrawing(graphics, captchaWidth, captchaHeight);

            // Çizilen grafiği geçici bellekte tutmak için 'stream' oluşturuyoruz.
            using MemoryStream stream = new MemoryStream();

            // Oluşturduğumuz bitmap dosyasını bellek üzerinde Gif formatında kaydediyoruz.
            bitmap.Save(stream, ImageFormat.Gif);

            // Bellekte kayıtlı olan Captcha byte verilerimizi alıyoruz. Bu veriler ile tekrardan resime ulaşabileceğiz.
            byte[] captchaByteDatas = stream.ToArray();

            return new CaptchaResults { CaptchaStringCode = CaptchaGeneratedCode, CaptchaByteDatas = captchaByteDatas };
        }

        // Captcha için 'captchaLength' uzunluğunda sayı üretir.
        public static string CaptchaGenerateCode(int captchaLength)
        {
            string code = "";
            string letters = "ABCDEFGHIJKLMNOPRSTUVYZXWQabcdefghijklmnprstuvyzxwq0123456789";

            Random rand = new Random();
            for (int i = 0; i < captchaLength; i++)
            {
                code += letters[rand.Next(letters.Length - 1)];
            }

            return code;
        }

        // Captcha için arkaplan rengini rastgele oluşturur.
        public static Color CaptchaBackgroundColor()
        {
            // Renkler 255'e doğru daha açık olacağından bu aralarda değer üretiyoruz.
            int lowValue = 180, highValue = 220;
            Random rand = new Random();
            return Color.FromArgb(rand.Next(lowValue, highValue), 
                                  rand.Next(lowValue, highValue), 
                                  rand.Next(lowValue, highValue));
        }

        // Kod harfleri ve çizgileri için renk oluşturuyoruz.
        public static Color CaptchaLetterColor()
        {
            // Renkler 0'e doğru daha koyu olacağından bu aralarda değer üretiyoruz.
            int lowValue = 100, highValue = 180;
            Random rand = new Random();
            return Color.FromArgb(rand.Next(lowValue, highValue),
                                  rand.Next(lowValue, highValue),
                                  rand.Next(lowValue, highValue));
        }

        // Captcha için kodu grafik üzerine çiziyoruz.
        public static void CaptchaDrawing(Graphics graphics, int captchaWidth, int captchaHeight, string CaptchaGeneratedCode)
        {
            Random rand = new Random();

            // Çizim için bir fırça oluşturuyoruz ve başlangıç rengini siyah olarak ayarlıyoruz.
            SolidBrush brush = new SolidBrush(Color.Black);

            // Üretilen kodun Captch çerçevesine sığması için uygun font boyutunu vermeliyiz. Bu yüzden genişlik değeri ile kod uzunluğunu oranlıyoruz.
            int CaptchaFontSize = captchaWidth / CaptchaGeneratedCode.Length;

            // Kodumuzu yazmak için font türü GenericSerif, kalın ve pixel ile boyutlandırılmış bir şekilde bir font üretiyoruz. 
            Font font = new Font(FontFamily.GenericSerif, CaptchaFontSize, FontStyle.Bold, GraphicsUnit.Pixel);

            for(int i = 0; i < CaptchaGeneratedCode.Length; i++)
            {
                // Her harf için yeni bir renk tanımlıyoruz.(Yani başlangıç rengi alınmıyor.)
                brush.Color = CaptchaLetterColor();

                // Harfi x konumunda rastgele kaydırmak için 4'te 1'lik kısmını alıyoruz.
                int xShift = CaptchaFontSize / 4;

                // Harfin x konumundaki yeri
                float x = i * CaptchaFontSize + rand.Next(-xShift, xShift);

                /* Başlangıçtaki değerleri değiştirebileceğimizden bu şekilde bir kontrol eklememiz gerekiyor.
                   Örneğin 'captchaWidth = 100px', 'captchaHeight = 30px', 'CaptchaGeneratedCode.Length = 2' olsun.
                   CaptchaFontSize değeri 100 / 2'den 50px olacaktır. Yani harfler genişliğe sığacaktır. Ancak
                   CaptchaFontSize > captchaHeight olacakğından yükseklik olarak taşacaktır.*/
                int MaxY = captchaHeight - CaptchaFontSize;
                if (MaxY < 0) MaxY = 0;

                // Harfin y konumundaki yeri taşmayacak şekilde rastgele oluşturuluyor.
                float y = rand.Next(0, MaxY);

                // Her harfi belirlediğimiz ayarlar ile grafiğe çiziyoruz.
                graphics.DrawString(CaptchaGeneratedCode[i].ToString(), font, brush, x, y);
            }
        }

        // Kod harflerinin okunmasını zorlaştırmak için çizgi çiziyoruz.
        public static void CaptchaLineDrawing(Graphics graphics, int captchaWidth, int captchaHeight)
        {
            Random rand = new Random();

            // Çizgileri çizmek için bir kalem oluşturuyoruz. Kalem için siyah bir fırça türü ve kalem genişliğini veriyoruz.
            Pen pen = new Pen(new SolidBrush(Color.Black), 2);

            // Captcha üzerine 2-5 arasında çizgi çizilecek.
            int lineCount = rand.Next(2, 7);

            for (int i=0; i < lineCount; i++)
            {
                // Her bir çizgi için renk ayarlıyoruz.
                pen.Color = CaptchaLetterColor();

                // Çizgi için başlangıç ve bitiş konumlarını alıyoruz.
                Point startPoint = new Point(rand.Next(captchaWidth), rand.Next(captchaHeight));
                Point endPoint = new Point(rand.Next(captchaWidth), rand.Next(captchaHeight));

                // Grafik üzerine çizgilerimizi çiziyoruz.
                graphics.DrawLine(pen, startPoint, endPoint);
            }
        }

        // Kullanıcı girdisinin, Captcha kodu ile doğrulanmasının kontrolünü yapıyoruz.
        public static bool CaptchaCodeValidation(CaptchaUserInput captchaUserInput, HttpContext context)
        {
            bool isvalid;
            if(captchaUserInput.CaptchaCode == context.Session.GetString("CaptchaCode"))
            {
                isvalid = true;
            }
            else
            {
                isvalid = false;
            }
            context.Session.Remove("CaptchaCode");
            return isvalid;
        }
    }
}
