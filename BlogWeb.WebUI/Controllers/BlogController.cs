using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BlogWeb.Data.Abstract;
using BlogWeb.Entity;
using BlogWeb.WebUI.Models;
using Limilabs.Client.SMTP;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogWeb.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository blogRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IGalleryRepository galleryRepository;
        private readonly ISettingRepository settingRepository;
        private readonly IEmailRegRepository emailRegRepository;
        public BlogController(IBlogRepository _blogRepository, ICategoryRepository _categoryRepository, IGalleryRepository _galleryRepository, ISettingRepository _settingRepository, IEmailRegRepository _emailRegRepository)
        {
            blogRepository = _blogRepository;
            categoryRepository = _categoryRepository;
            galleryRepository = _galleryRepository;
            settingRepository = _settingRepository;
            emailRegRepository = _emailRegRepository;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if (HttpContext.Session.GetString("username") == null)
            {
                HttpContext.Response.Redirect("/Home/PageNotFound");
            }

        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.SidebarActive = "Blog";
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }

        public IActionResult List()
        {
            if (TempData["MailNotificationSuccess"] != null) { ViewBag.MailNotificationSuccess = TempData["MailNotificationSuccess"]; }
            if (TempData["MailNotificationDanger"] != null) { ViewBag.MailNotificationDanger = TempData["MailNotificationDanger"]; }

            var blogsCat = blogRepository.GetAll()
                                 .Join(
                                     categoryRepository.GetAll(),
                                     b => b.CategoryId,
                                     c => c.CategoryId,
                                     (b, c) => new BlogCategoriesJoin()
                                     {
                                         BlogId = b.BlogId,
                                         Title = b.Title.ToUpper(),
                                         IsActive = b.IsActive,
                                         IsMailSend = b.IsMailSend,
                                         CreateDate = b.CreateDate,
                                         PublishedDate = b.PublishedDate,
                                         LastEditDate = b.LastEditDate,
                                         CategoryName = c.Name.ToUpper()
                                     }
                                 ).ToList().OrderByDescending(p=>p.CreateDate);

            return View(blogsCat);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = categoryRepository.GetAll().Where(p=>p.ParentId != 0);
            return View();
        }

        [HttpPost]
        public IActionResult Create(Blog entity)
        {
            if(ModelState.IsValid)
            {
                if (blogRepository.AddBlog(entity))
                {
                    TempData["CreateBlogSuccess"] = "Blog başarılı bir şekilde oluşturuldu.";
                }
                else
                {
                    TempData["CreateBlogError"] = "Blog oluşturulurken bir hata oluştu!";
                }
            }

            return RedirectToAction("EditSelect", new { id = entity.BlogId });
        }

        public IActionResult Delete(int id)
        {
            blogRepository.DeleteBlog(id);
            return RedirectToAction("List");
        }

        public IActionResult EditSelect(int id)
        {
            if (TempData["CreateBlogSuccess"] != null) { ViewBag.CreateBlogSuccess = TempData["CreateBlogSuccess"]; }
            if (TempData["CreateBlogError"] != null) { ViewBag.CreateBlogError = TempData["CreateBlogError"]; }


            var blog = blogRepository.GetById(id);
            ViewBag.BlogTitle = blog.Title.ToUpper();
            ViewBag.BlogId = id;
            return View();
        }

        public IActionResult EditBlogText(int id)
        {
            if (TempData["UpdateBlogText"] != null) { ViewBag.UpdateBlogText = TempData["UpdateBlogText"]; }
            if (TempData["UpdateBlogTextDanger"] != null) { ViewBag.UpdateBlogTextDanger = TempData["UpdateBlogTextDanger"]; }

            ViewBag.Images = galleryRepository.GetAllByBlogId(id);

            return View(blogRepository.GetById(id));
        }

        [HttpPost]
        public IActionResult EditBlogText(Blog entity)
        {
            if (ModelState.IsValid)
            {
                var blog = blogRepository.GetById(entity.BlogId);
                blog.Text = entity.Text;

                if (blogRepository.UpdateBlog(blog))
                {
                    TempData["UpdateBlogText"] = "Blog kaydedildi.";
                }
                else
                {
                    TempData["UpdateBlogTextDanger"] = "Blog kaydedilirken bir hata oluştu.";
                }
            }

            return RedirectToAction("EditBlogText", new { id = entity.BlogId });
        }

        public IActionResult EditBlogOptions(int id)
        {
            if (TempData["EditBlogOptions"] != null) { ViewBag.EditBlogOptions = TempData["EditBlogOptions"]; }

            ViewBag.Categories = categoryRepository.GetAll().Where(p => p.ParentId != 0);
            return View(blogRepository.GetById(id));
        }

        [HttpPost]
        public IActionResult EditBlogOptions(Blog entity)
        {
            if (ModelState.IsValid)
            {
                if (blogRepository.UpdateBlog(entity))
                {
                    TempData["EditBlogOptions"] = "Blog ayarları güncellendi.";
                }
            }

            return RedirectToAction("EditBlogOptions", new { id = entity.BlogId});
        }

        public IActionResult EditBlogGaleries(int id)
        {
            if (TempData["ImageUploadSuccess"] != null) { ViewBag.ImageUploadSuccess = TempData["ImageUploadSuccess"]; }
            if (TempData["ImageContentError"] != null) { ViewBag.ImageContentError = TempData["ImageContentError"]; }
            if (TempData["ImageNullError"] != null) { ViewBag.ImageNullError = TempData["ImageNullError"]; }
            if (TempData["BlogCoverSuccess"] != null) { ViewBag.BlogCoverSuccess = TempData["BlogCoverSuccess"]; }
            if (TempData["BlogRemoveCoverSuccess"] != null) { ViewBag.BlogRemoveCoverSuccess = TempData["BlogRemoveCoverSuccess"]; }
            if (TempData["DeleteGallery"] != null) { ViewBag.DeleteGallery = TempData["DeleteGallery"]; }

            ViewBag.BlogId = id;
            ViewBag.BlogTitle = blogRepository.GetById(id).Title.ToUpper();
            return View(galleryRepository.GetAllByBlogId(id));
        }

        [HttpPost]
        public async Task<IActionResult> EditBlogGaleriesAsync(int id, IFormFile imageFile)
        {
            var BlogId = id;
            if (ModelState.IsValid)
            {
                if(imageFile != null)
                {
                    if(imageFile.ContentType == "image/jpeg" || imageFile.ContentType == "image/png")
                    {
                        string noExImageName = Path.GetFileNameWithoutExtension(imageFile.FileName);
                        string exImage = Path.GetExtension(imageFile.FileName);

                        var ImageNames = galleryRepository.GetAll().Where(p=>p.ImageUrl == imageFile.FileName);
                        int inc = 1;
                        string temp = null;
                        while (ImageNames.Any())
                        {
                            temp = noExImageName;
                            temp += inc;
                            inc += 1;
                            ImageNames = galleryRepository.GetAll().Where(p => p.ImageUrl == (temp + exImage));
                            if (ImageNames.Any() == false) { noExImageName = temp; }
                        }

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", noExImageName + exImage);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(stream);
                        }

                        Gallery gal = new Gallery
                        {
                            BlogId = id,
                            ImageUrl = noExImageName + exImage,
                            IsCover = false
                        };

                        if (galleryRepository.AddGallery(gal))
                        {
                            TempData["ImageUploadSuccess"] = "Resim yüklendi.";
                        }
                    }
                    else
                    {
                        TempData["ImageContentError"] = "Sadece 'Image/Jpeg' ve 'Image/png' türündeki dosyalar yüklenebilir.";
                    }
                }
                else
                {
                    TempData["ImageNullError"] = "Herhangi bir resim dosyası seçilmedi!";
                }
 
            }

            return RedirectToAction("EditBlogGaleries", new { id = BlogId });
        }

        public IActionResult GaleriesEditCover(int galleryId, int blogId)
        {
            if (ModelState.IsValid)
            {
                var beforeCover = galleryRepository.GetAllByBlogId(blogId).Where(p=>p.IsCover == true);
                if (beforeCover.Any())
                {
                    foreach(var item in beforeCover)
                    {
                        item.IsCover = false;
                        galleryRepository.UpdateGallery(item);
                    }
                }

                Gallery gallery = galleryRepository.GetById(galleryId);
                gallery.IsCover = true;
                if (galleryRepository.UpdateGallery(gallery))
                {
                    Blog blog = blogRepository.GetById(blogId);
                    blog.ImageUrl = gallery.ImageUrl;
                    if (blogRepository.UpdateBlog(blog))
                    {
                        TempData["BlogCoverSuccess"] = "'" + blog.Title + "' bloğuna '" + gallery.ImageUrl + "' resmi, kapak olarak ayarlandı.";
                    }
                }

            }

            return RedirectToAction("EditBlogGaleries", new { id = blogId });
        }

        public IActionResult GaleriesRemoveCover(int galleryId, int blogId)
        {
            if (ModelState.IsValid)
            {
                Gallery gallery = galleryRepository.GetById(galleryId);
                gallery.IsCover = false;
                if (galleryRepository.UpdateGallery(gallery))
                {
                    Blog blog = blogRepository.GetById(blogId);
                    blog.ImageUrl = null;
                    if (blogRepository.UpdateBlog(blog))
                    {
                        TempData["BlogRemoveCoverSuccess"] = "'" + blog.Title + "' bloğun'dan '" + gallery.ImageUrl + "' kapak resmi kaldırıldı.";
                    }
                }
            }

            return RedirectToAction("EditBlogGaleries", new { id = blogId });
        }

        public IActionResult GaleriesDelete(int galleryId, int blogId)
        {
            if (ModelState.IsValid)
            {
                Gallery gallery = galleryRepository.GetById(galleryId);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", gallery.ImageUrl);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);

                    if (galleryRepository.DeleteGallery(gallery.GalleryId))
                    {
                        TempData["DeleteGallery"] = "'"+ gallery.ImageUrl + "' resmi silindi.";
                    }
                }
            }

            return RedirectToAction("EditBlogGaleries", new { id = blogId });
        }

        public IActionResult BlogNotificationMailSend(int id)
        {
            try
            {
                var setting = settingRepository.GetById(1);
                var emailList = emailRegRepository.GetAll().Select(p => p.EmailAddress).ToList();
                var blog = blogRepository.GetById(id);
                string messageHtml = "" +
                    "<div style='background-color:#F7F7F7;padding:5px;'>" +
                        "<div style='background-color:#FFF;padding:5px;'>" +
                            "<h3 style='text-align:center'>" + blog.Title + "</h3>" +
                            "<p style='text-align:center'><img src='https://"+setting.SiteName+ "/img/" + blog.ImageUrl +"' width='400px'/></p>" +
                            "<p style='text-align:justify'>" + blog.Explanation + "...</p>" +
                            "<a href='https://"+setting.SiteName+"/Home/Detail/"+blog.BlogId+"' style='text-decoration:none;text-align:center;display:block; background-color:#007bff;color:#fff;padding:10px 15px;border-radius:15px'>Makeleye Git</a>" +
                            "<hr/>" +
                            "<p>E-Posta aboneliğimizden çıkmak için <a href='https://" + setting.SiteName + "/Home/EmailCancellation'>buraya tıklayarak</a> ilgili adrese gidiniz.</p>" +
                        "</div>" +
                     "</div>";

                MailBuilder builder = new MailBuilder();

                builder.From.Add(new MailBox(setting.SMTPServerFrom, setting.SMTPServerFromName));
                builder.To.Add(new MailGroup("Undisclosed recipients"));

                foreach (string item in emailList)
                {
                    builder.Bcc.Add(new MailBox(item));
                }

                builder.Subject = blog.Title;
                builder.Html = messageHtml;

                IMail email = builder.Create();

                using Smtp smtp = new Smtp();
                smtp.Connect(setting.SMTPServerHost, Convert.ToInt32(setting.SMTPServerPort));   // or ConnectSSL for SSL
                smtp.UseBestLogin(setting.SMTPServerUsername, setting.SMTPServerPassword); // remove if not needed
                smtp.SendMessage(email);
                smtp.Close();

                blog.IsMailSend = true;
                if (blogRepository.UpdateBlog(blog))
                {
                    TempData["MailNotificationSuccess"] = "'" + blog.Title + "', bloğu için e-posta bildirimleri gönderildi."; 
                }
            }
            catch
            {
                TempData["MailNotificationDanger"] = "E-Posta bildirimi gönderilirken bir hata oluştu!";
            }

            return RedirectToAction("List");
        }
    }
}