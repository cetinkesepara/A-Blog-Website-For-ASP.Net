using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogWeb.WebUI.Models;
using BlogWeb.Data.Abstract;
using BlogWeb.Entity;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Filters;
using Limilabs.Mail;
using Limilabs.Mail.Headers;
using Limilabs.Client.SMTP;

namespace BlogWeb.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly IBlogRepository blogRepository;
        private readonly ICommentRepository commentRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ISettingRepository settingRepository;
        private readonly IEmailRegRepository emailRegRepository;

        public HomeController(IBlogRepository _blogRepository, 
            ICommentRepository _commentRepository, 
            ICategoryRepository _categoryRepository,
            ISettingRepository _settingRepository,
            IEmailRegRepository _emailRegRepository)
        {
            blogRepository = _blogRepository;
            commentRepository = _commentRepository;
            categoryRepository = _categoryRepository;
            settingRepository = _settingRepository;
            emailRegRepository = _emailRegRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.Categories = categoryRepository.GetAll().Where(p => p.IsActive == true).ToList();

            // Side Content
            var setting = settingRepository.GetById(1);
            ViewBag.Settings = setting;

            var BlogsMostRead = blogRepository.GetAll().Where(p => p.IsActive == true).OrderByDescending(p => Convert.ToInt32(p.VisitorHit)).Take(setting.TakeSideMostReadCount).ToList();
            ViewBag.MostReadBlogs = BlogShow.GetShowItems(BlogsMostRead, commentRepository, categoryRepository);

            var BlogsMostComment = blogRepository.GetAll().Where(p => p.IsActive == true).ToList();
            ViewBag.MostCommentBlogs = BlogShow.GetShowItems(BlogsMostComment, commentRepository, categoryRepository).OrderByDescending(p => p.CommentCount).Take(setting.TakeSideMostCommentCount);

            var BlogsLastPublished = blogRepository.GetAll().Where(p => p.IsActive == true).OrderByDescending(p => p.PublishedDate).Take(setting.TakeSideLastPublishedCount).ToList();
            ViewBag.LastPublishedBlogs = BlogShow.GetShowItems(BlogsLastPublished, commentRepository, categoryRepository);

            int BlogsCount = blogRepository.GetAll().Where(p => p.IsActive == true).Count();
            Random r = new Random();
            List<Blog> blogs = new List<Blog>();
            List<int> randomList = new List<int>();
            int RandomBlogCount = setting.TakeSideRandomBlogCount;
            if (RandomBlogCount > BlogsCount)
            {
                RandomBlogCount = BlogsCount;
            }

            for (int i = 0; i < RandomBlogCount; i++)
            {
                int offset;
                do
                {
                    offset = r.Next(0, BlogsCount);
                }
                while (randomList.Contains(offset));

                randomList.Add(offset);
                blogs.Add(blogRepository.GetAll().Where(p => p.IsActive == true).Skip(offset).FirstOrDefault());
            }
            ViewBag.RandomBlogs = BlogShow.GetShowItems(blogs, commentRepository, categoryRepository);
        }

        public IActionResult Index()
        {
            var setting = settingRepository.GetById(1);
            int TakeBlogCount = setting.TakeBlogCount;
            ViewBag.Title = setting.SiteName + " | " +setting.SiteTitle;
            ViewBag.Description = setting.Description;

            var blogs = blogRepository.GetAll().Where(p => p.IsActive == true).OrderByDescending(p => p.PublishedDate).Take(TakeBlogCount).ToList();
            var blogShow = BlogShow.GetShowItems(blogs, commentRepository, categoryRepository);

            return View(blogShow);
        }

        public IActionResult GetBlogsByCategory(int id)
        {

            var setting = settingRepository.GetById(1);
            int TakeBlogCount = setting.TakeBlogCount;

            var category = categoryRepository.GetById(id);
            List<Blog> allSubBlogs = new List<Blog>();

            if (category.ParentId == 0)
            { 
                var subCategories = categoryRepository.GetAll().Where(p => p.IsActive == true && p.ParentId == category.CategoryId).ToList();
                foreach(var item in subCategories)
                {
                    var subBlogs = blogRepository.GetAll().Where(p => p.IsActive == true && p.CategoryId == item.CategoryId).ToList();
                    allSubBlogs.AddRange(subBlogs);
                }
                ViewBag.CategoryActive = id;
            }
            else
            {
               allSubBlogs = blogRepository.GetAll().Where(p => p.IsActive == true && p.CategoryId == id).ToList();
               ViewBag.CategorySubActive = id;
            }


            string categoryName = category.Name;
            ViewBag.Title = char.ToUpper(categoryName[0]) + categoryName.Substring(1).ToLower() + " | " + settingRepository.GetById(1).SiteName;
            ViewBag.Description = category.Description;
            ViewBag.CategoryId = id;

            var blogs = allSubBlogs.OrderByDescending(p => p.PublishedDate).Take(TakeBlogCount).ToList();
            var blogShow = BlogShow.GetShowItems(blogs, commentRepository, categoryRepository);

            return View(blogShow);
        }

        [HttpPost]
        public IActionResult GetMoreBlog(int blogCounts)
        {
            var setting = settingRepository.GetById(1);
            int TakeBlogCount = setting.TakeBlogCount;

            var blogAll = blogRepository.GetAll().Where(p => p.IsActive == true).OrderByDescending(p => p.PublishedDate);
            var blogGet = blogAll.Skip(blogCounts).Take(TakeBlogCount).ToList();
            var blogShow = BlogShow.GetShowItems(blogGet, commentRepository, categoryRepository);

            var bf = true;
            if(blogGet.Count() == 0) { bf = false; }

            return new JsonResult(new { newBlogCounts = blogCounts + TakeBlogCount, 
                                         blogFind = bf, 
                                         blogs = blogShow
            });
        }

        public IActionResult GetMoreCategoryBlogs(int blogCounts, int CategoryId)
        {
            var setting = settingRepository.GetById(1);
            int TakeBlogCount = setting.TakeBlogCount;

            var category = categoryRepository.GetById(CategoryId);
            List<Blog> allSubBlogs = new List<Blog>();

            if (category.ParentId == 0)
            {
                List<Blog> all = new List<Blog>();
                var subCategories = categoryRepository.GetAll().Where(p => p.IsActive == true && p.ParentId == category.CategoryId).ToList();
                foreach (var item in subCategories)
                {
                    var subBlogs = blogRepository.GetAll().Where(p => p.IsActive == true && p.CategoryId == item.CategoryId);
                    all.AddRange(subBlogs);
                }
                allSubBlogs = all.OrderByDescending(p => p.PublishedDate).Skip(blogCounts).Take(TakeBlogCount).ToList();
            }
            else
            {
                allSubBlogs = blogRepository.GetAll()
                    .Where(p => p.IsActive == true && p.CategoryId == CategoryId)
                    .OrderByDescending(p => p.PublishedDate)
                    .Skip(blogCounts).Take(TakeBlogCount).ToList();
            }

            var blogShow = BlogShow.GetShowItems(allSubBlogs, commentRepository, categoryRepository);

            var bf = true;
            if (allSubBlogs.Count() == 0) { bf = false; }

            return new JsonResult(new
            {
                newBlogCounts = blogCounts + TakeBlogCount,
                blogFind = bf,
                blogs = blogShow
            });
        }

        public IActionResult Detail(int id)
        {
            var setting = settingRepository.GetById(1);
            // Comments
            int commentGetCount = setting.TakeCommentCount;
            int answerGetCount = setting.TakeAnswerCount;
            var comments = commentRepository.GetAll().Where(p => p.BlogId == id && p.IsActive == true);
            if(comments.Count() != 0)
            {
                var commentsParent = comments.Where(p => p.ParentId == 0).OrderByDescending(c=>c.CreateTime).Take(commentGetCount).OrderBy(c=>c.CreateTime).ToList();

                List<Comment> answers = new List<Comment>();
                List<CommentAnswerCounts> commentAnswerCounts = new List<CommentAnswerCounts>();
                foreach(var item in commentsParent)
                {
                    var ans = comments.Where(d => d.ParentId == item.CommentId).OrderByDescending(c => c.CreateTime).Take(answerGetCount).OrderBy(c => c.CreateTime).ToList();
                    commentAnswerCounts.Add(
                        new CommentAnswerCounts
                        {
                            ParentId = item.CommentId,
                            AnswerCount = comments.Where(d => d.ParentId == item.CommentId).Count() - ans.Count()
                        }); 
                    answers.AddRange(ans);
                }

                ViewBag.CommentParents = commentsParent;
                ViewBag.CommentAnswers = answers;
                ViewBag.CommentParentCount = comments.Where(p => p.ParentId == 0).Count() - commentsParent.Count();
                ViewBag.CommentAnswerCount = commentAnswerCounts;
                ViewBag.CommentAny = true;
            }
            else
            {
                ViewBag.CommentAny = false;
            }

            // Hits Cookie
            var blog = blogRepository.GetById(id);
            if(Request.Cookies["VisitorHitId"] == null)
            {
                CookieOptions cookie = new CookieOptions
                {
                    Expires = DateTime.Now.AddYears(1)
                };
                Response.Cookies.Append("VisitorHitId", "["+blog.BlogId.ToString()+"]", cookie);

                string blogHit = blog.VisitorHit;
                blog.VisitorHit = (Convert.ToInt32(blog.VisitorHit) + 1).ToString();
                blogRepository.UpdateBlog(blog);
            }
            else
            {
                string hitIds = Request.Cookies["VisitorHitId"];
                if(hitIds.Contains("[" + blog.BlogId.ToString() + "]") == false) {
                    Response.Cookies.Delete("VisitorHitId");

                    CookieOptions cookie = new CookieOptions
                    {
                        Expires = DateTime.Now.AddYears(1)
                    };
                    Response.Cookies.Append("VisitorHitId", hitIds + "[" + blog.BlogId.ToString() + "]", cookie);

                    string blogHit = blog.VisitorHit;
                    blog.VisitorHit = (Convert.ToInt32(blog.VisitorHit) + 1).ToString();
                    blogRepository.UpdateBlog(blog);
                }
            }

            // Detail Side
            var BlogsMostRead = blogRepository.GetAll().Where(p => p.IsActive == true).OrderByDescending(p=>Convert.ToInt32(p.VisitorHit)).Take(setting.TakeSideMostReadCount).ToList();
            ViewBag.MostReadBlogs = BlogShow.GetShowItems(BlogsMostRead, commentRepository, categoryRepository);

            var BlogsMostComment = blogRepository.GetAll().Where(p => p.IsActive == true).ToList();
            ViewBag.MostCommentBlogs = BlogShow.GetShowItems(BlogsMostComment, commentRepository, categoryRepository).OrderByDescending(p=>p.CommentCount).Take(setting.TakeSideMostCommentCount);

            var BlogsLastPublished = blogRepository.GetAll().Where(p => p.IsActive == true).OrderByDescending(p => p.PublishedDate).Take(setting.TakeSideLastPublishedCount).ToList();
            ViewBag.LastPublishedBlogs = BlogShow.GetShowItems(BlogsLastPublished, commentRepository, categoryRepository);

            int BlogsCount = blogRepository.GetAll().Where(p => p.IsActive == true).Count();
            Random r = new Random();
            List<Blog> blogs = new List<Blog>();
            List<int> randomList = new List<int>();
            int RandomBlogCount = setting.TakeSideRandomBlogCount;
            if(RandomBlogCount > BlogsCount)
            {
                RandomBlogCount = BlogsCount;
            }

            for (int i=0; i < RandomBlogCount; i++)
            {
                int offset;
                do
                {
                    offset = r.Next(0, BlogsCount);
                }
                while (randomList.Contains(offset));
                
                randomList.Add(offset);
                blogs.Add(blogRepository.GetAll().Where(p => p.IsActive == true).Skip(offset).FirstOrDefault());
            }
            ViewBag.RandomBlogs = BlogShow.GetShowItems(blogs, commentRepository, categoryRepository);

            ViewBag.FacebookSharing = setting.FacebookSharing;
            ViewBag.TwitterSharing = setting.TwitterSharing;
            ViewBag.LinkedinSharing = setting.LinkedInSharing;

            ViewBag.Title = char.ToUpper(blog.Title[0]) + blog.Title.Substring(1).ToLower() + " | " + settingRepository.GetById(1).SiteName;
            ViewBag.Description = blog.Description;
            return View(blog);
        }

        [HttpPost]
        public IActionResult GetMoreComment(int comCount, int blogid)
        {
            var setting = settingRepository.GetById(1);
            int commentGetCount = setting.TakeCommentCount;
            int answerGetCount = setting.TakeAnswerCount;

            var comments = commentRepository.GetAll().Where(p => p.BlogId == blogid && p.IsActive == true);

           
            var commentsParent = comments.Where(p => p.ParentId == 0).OrderByDescending(c => c.CreateTime).Skip(comCount).Take(commentGetCount).OrderBy(c => c.CreateTime).ToList();

            List<Comment> answers = new List<Comment>();
            List<CommentAnswerCounts> commentAnswerCounts = new List<CommentAnswerCounts>();
            foreach (var item in commentsParent)
            {
                var ans = comments.Where(d => d.ParentId == item.CommentId).OrderByDescending(c => c.CreateTime).Take(answerGetCount).OrderBy(c => c.CreateTime).ToList();
                commentAnswerCounts.Add(
                    new CommentAnswerCounts
                    {
                        ParentId = item.CommentId,
                        AnswerCount = comments.Where(d => d.ParentId == item.CommentId).Count() - ans.Count()
                    });
                answers.AddRange(ans);
            }

            int CommentParentCount = comments.Where(p => p.ParentId == 0).Count() - commentsParent.Count() - comCount;

            var filteringParents = commentsParent
                .Select(p => new
                {
                    p.Fullname,
                    p.Text,
                    p.Type,
                    p.CreateTime,
                    p.AnswerToName,
                    p.CommentId
                });

            var filteringAnswers = answers
                .Select(p => new
                {
                    p.Fullname,
                    p.Text,
                    p.Type,
                    p.CreateTime,
                    p.AnswerToName,
                    p.ParentId
                });


            return new JsonResult(new { commParents = filteringParents, 
                                        commAnswers = filteringAnswers, 
                                        commCount = comCount + commentGetCount,
                                        commParentCount = CommentParentCount,
                                        commAnswerCount = commentAnswerCounts
            });
        }

        [HttpPost]
        public IActionResult GetMoreAnswer(int comId, int ansCount)
        {
            var setting = settingRepository.GetById(1);
            int answerGetCount = setting.TakeAnswerCount;

            var answers = commentRepository.GetAll()
                .Where(p => p.ParentId == comId && p.IsActive == true)
                .OrderByDescending(c => c.CreateTime)
                .Skip(ansCount)
                .Take(answerGetCount)
                .OrderBy(c => c.CreateTime)
                .Select(p => new { 
                    p.Fullname,
                    p.Text,
                    p.Type,
                    p.CreateTime,
                    p.AnswerToName
                }).ToList();

            var answerCount = commentRepository.GetAll().Where(p => p.ParentId == comId && p.IsActive == true).Count() - answers.Count() - ansCount;

            return new JsonResult(new { commAnswers = answers, answCount = answerCount });
        }

        [HttpPost]
        public IActionResult PostComment(PostComment postComment)
        {
            Comment comment = new Comment
            {
                Fullname = postComment.FullName,
                Email = postComment.Email,
                Text = postComment.Comment,
                Type = "visitor",
                CreateTime = DateTime.Now,
                IsActive = false,
                BlogId = postComment.BlogId,
                ParentId = 0
            };

            if(postComment.CommentId != null)
            {
                comment.ParentId = Convert.ToInt32(postComment.CommentId);
            }

            if(postComment.ToName != null)
            {
                comment.AnswerToName = postComment.ToName;
            }

           
            return new JsonResult(commentRepository.AddComment(comment));
        }

        [Route("captcha-image-loading")]
        public IActionResult CaptchaImageLoading()
        {
            // 5 Harfli 140x40 boyutlarında Captcha oluşturuluyor.
            var captchaResult = Captcha.GenerateCaptcha(140, 40, 5);

            // Oluşturulan Captcha için kodu, oturum oluşturarak "CaptchaCode", string'inde tutuyoruz.
            HttpContext.Session.SetString("CaptchaCode", captchaResult.CaptchaStringCode);

            // Bellekten resmimize ulaşıyoruz.
            Stream stream = new MemoryStream(captchaResult.CaptchaByteDatas);

            // Resmi Gif formatında gönderiyoruz.
            return new FileStreamResult(stream, "image/gif");
        }

        [HttpPost]
        public IActionResult PostCaptchaUserInputCode(CaptchaUserInput captchaUserInput)
        {
            if (ModelState.IsValid)
            {
                if (!Captcha.CaptchaCodeValidation(captchaUserInput, HttpContext))
                {
                    return new JsonResult(new { validate = false });
                }
            }

            return new JsonResult(new { validate = true });
        }

        [HttpPost]
        public IActionResult PostEmailRegistration(EmailRegistration emailRegistration)
        {
            var email = emailRegRepository.GetAll().Where(p => p.EmailAddress == emailRegistration.EmailAddress).ToList();
            string postResultString;
            int messageNumber;

            if (!email.Any())
            {
                emailRegistration.ControlCode = Guid.NewGuid().ToString();
                emailRegistration.CreateTime = DateTime.Now;
                if (emailRegRepository.AddEmailReg(emailRegistration))
                {
                    postResultString = "E-posta listemize kaydedildiniz. Teşekkür ederiz.";
                    messageNumber = 2;
                }
                else
                {
                    postResultString = "E-posta kaydı yapılırken bir hata oluştu! Lütfen daha sonra tekrar deneyiniz.";
                    messageNumber = 3;
                }
            }
            else
            {
                postResultString = "Bu e-posta adresi zaten kayıtlı!";
                messageNumber = 1;
            }

            return new JsonResult(new { message = postResultString, messageType = messageNumber });
        }

        public IActionResult EmailCancellation()
        {
            if (TempData["EmailCancelledSuccess"] != null) { ViewBag.EmailCancelledSuccess = TempData["EmailCancelledSuccess"]; }
            if (TempData["EmailCancelledWarning"] != null) { ViewBag.EmailCancelledWarning = TempData["EmailCancelledWarning"]; }
            if (TempData["EmailCancelledDanger"] != null) { ViewBag.EmailCancelledDanger = TempData["EmailCancelledDanger"]; }

            return View();
        }

        [HttpPost]
        public IActionResult EmailCancellation(EmailRegistration emailCancell)
        {
            var emailCancelled = emailRegRepository.GetAll().Where(p => p.EmailAddress == emailCancell.EmailAddress && p.ControlCode == emailCancell.ControlCode);

            if (emailCancelled.Any())
            {
                if (ModelState.IsValid)
                {
                    if (emailRegRepository.DeleteEmailReg(emailCancelled.First()))
                    {
                        TempData["EmailCancelledSuccess"] = "E-posta aboneliğiniz iptal edildi. Artık yeni makale yayınlandığında bildirim almayacaksınız.";
                    }
                    else
                    {
                        TempData["EmailCancelledDanger"] = "Bir hata oluştu. Lütfen daha sonra tekrar deneyiniz.";
                    }
                }
            }
            else
            {
                TempData["EmailCancelledWarning"] = "Girilen e-posta adresi veya kontrol kodu için bir kayıt bulunamadı! Lütfen doğru bilgileri girdiğinizden emin olunuz.";
            }

            return RedirectToAction("EmailCancellation");
        }

        [HttpPost]
        public IActionResult GetEmailControlCode(EmailRegistration emailCancell)
        {
            string postResultString;
            int messageNumber;
            var emailCancelled = emailRegRepository.GetAll().Where(p => p.EmailAddress == emailCancell.EmailAddress);

            if (emailCancelled.Any())
            {
                if (ModelState.IsValid)
                {
                   
                        var setting = settingRepository.GetById(1);
                        string messageHtml = "" +
                            "<div style='background-color:#F7F7F7;padding:5px;'>" +
                                "<div style='background-color:#FFF;padding:5px;'>" +
                                    "<p>Kontrol Kodunuz: "+ emailCancelled.First().ControlCode +"</p>"+
                                "</div>" +
                             "</div>";

                        MailBuilder builder = new MailBuilder();

                        builder.From.Add(new MailBox(setting.SMTPServerFrom, setting.SMTPServerFromName));
                        builder.To.Add(new MailBox(emailCancell.EmailAddress));

                        builder.Subject = "Abonelik İptali İçin Kontrol Kodu";
                        builder.Html = messageHtml;

                        IMail email = builder.Create();

                        using Smtp smtp = new Smtp();
                        smtp.Connect(setting.SMTPServerHost, Convert.ToInt32(setting.SMTPServerPort));   // or ConnectSSL for SSL
                        smtp.UseBestLogin(setting.SMTPServerUsername, setting.SMTPServerPassword); // remove if not needed
                        smtp.SendMessage(email);
                        smtp.Close();

                        postResultString = "Kontrol kodu, e-posta adresinize gönderildi.";
                        messageNumber = 2;


                }
                else
                {
                    postResultString = "Kontrol kodu gönderilirken bir hata oluştu! Lütfen daha sonra tekrar deneyiniz veya iletişime geçiniz.";
                    messageNumber = 3;

                }
            }
            else
            {
                postResultString = "Girilen e-posta adresi için bir kayıt bulunamadı! Lütfen doğru bilgileri girdiğinizden emin olunuz.";
                messageNumber = 1;
            }

            return new JsonResult(new { message = postResultString, messageType = messageNumber });
        }

        [HttpPost]
        public IActionResult Search(string searchText)
        {
            var setting = settingRepository.GetById(1);
            int TakeBlogCount = setting.TakeBlogCount;
            ViewBag.Title = "'" + searchText + "' | " + setting.SiteName + " | " + setting.SiteTitle;
            ViewBag.Description = setting.Description;

            var blogs = blogRepository.GetAll().Where(p => p.IsActive == true).OrderByDescending(p=>p.PublishedDate).ToList();
            List<Blog> blogSearch = new List<Blog>();
            foreach(var item in blogs)
            {
                if (item.Title.ToLower().Contains(searchText.ToLower()) || item.Description.ToLower().Contains(searchText.ToLower()) || item.Explanation.ToLower().Contains(searchText.ToLower()))
                {
                    blogSearch.Add(item);
                }
            }

            var blogShow = BlogShow.GetShowItems(blogSearch, commentRepository, categoryRepository);

            ViewBag.SearchText = searchText;

            return View(blogShow.ToList());
        }

        public IActionResult About()
        {
            var setting = settingRepository.GetById(1);

            return View(setting);
        }

        public IActionResult Contact()
        {
            var setting = settingRepository.GetById(1);

            return View(setting);
        }

        public IActionResult FrequentlyAsked()
        {
            var setting = settingRepository.GetById(1);

            return View(setting);
        }

        public IActionResult PrivacyPolicy()
        {
            var setting = settingRepository.GetById(1);

            return View(setting);
        }

        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
