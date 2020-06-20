using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWeb.Data.Abstract;
using BlogWeb.Entity;
using BlogWeb.WebUI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogWeb.WebUI.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository commentRepository;
        private readonly IBlogRepository blogRepository;
        public CommentController(ICommentRepository _commentRepository, IBlogRepository _blogRepository)
        {
            commentRepository = _commentRepository;
            blogRepository = _blogRepository;
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
            ViewBag.SidebarActive = "Comment";
        }
        public IActionResult Index()
        {
            var comments = commentRepository.GetAll();
            ViewBag.ApprovedComments = comments.Where(p => p.IsActive == true && p.Type == "visitor").Count();
            ViewBag.UnApprovedComments = comments.Where(p => p.IsActive == false && p.Type == "visitor").Count();
            ViewBag.AdminComments = comments.Where(p => p.Type == "admin").Count();
            return View();
        }

        public IActionResult UnApprovedComments()
        {
            if (TempData["DeleteCommentSuccess"] != null) { ViewBag.DeleteCommentSuccess = TempData["DeleteCommentSuccess"]; }
            if (TempData["DeleteCommentError"] != null) { ViewBag.DeleteCommentError = TempData["DeleteCommentError"]; }

            var comments = commentRepository.GetAll().Where(p => p.IsActive == false && p.Type == "visitor")
                .Join(
                    blogRepository.GetAll(),
                    comment => comment.BlogId,
                    blog => blog.BlogId,
                    (comment,blog) => new BlogCommentJoin()
                    {
                        BlogId = blog.BlogId,
                        BlogTitle = blog.Title,
                        CommentId = comment.CommentId,
                        CreateTime = comment.CreateTime,
                        Fullname = comment.Fullname,
                        AnswerToName = comment.AnswerToName,
                        Email = comment.Email,
                        IsActive = comment.IsActive
                    }
                ).OrderByDescending(c => c.CreateTime).ToList();

            return View(comments);
        }

        public IActionResult ApprovedComments()
        {
            if (TempData["DeleteCommentSuccess"] != null) { ViewBag.DeleteCommentSuccess = TempData["DeleteCommentSuccess"]; }
            if (TempData["DeleteCommentError"] != null) { ViewBag.DeleteCommentError = TempData["DeleteCommentError"]; }

            var comments = commentRepository.GetAll().Where(p => p.IsActive == true && p.Type == "visitor")
                .Join(
                    blogRepository.GetAll(),
                    comment => comment.BlogId,
                    blog => blog.BlogId,
                    (comment,blog) => new BlogCommentJoin()
                    {
                        BlogId = blog.BlogId,
                        BlogTitle = blog.Title,
                        CommentId = comment.CommentId,
                        CreateTime = comment.CreateTime,
                        Fullname = comment.Fullname,
                        AnswerToName = comment.AnswerToName,
                        Email = comment.Email,
                        IsActive = comment.IsActive
                    }
                ).OrderByDescending(c => c.CreateTime).ToList();

            return View(comments);
        }

        public IActionResult AdminComments()
        {
            if (TempData["DeleteCommentSuccess"] != null) { ViewBag.DeleteCommentSuccess = TempData["DeleteCommentSuccess"]; }
            if (TempData["DeleteCommentError"] != null) { ViewBag.DeleteCommentError = TempData["DeleteCommentError"]; }

            var comments = commentRepository.GetAll().Where(p => p.Type == "admin")
                .Join(
                    blogRepository.GetAll(),
                    comment => comment.BlogId,
                    blog => blog.BlogId,
                    (comment, blog) => new BlogCommentJoin()
                    {
                        BlogId = blog.BlogId,
                        BlogTitle = blog.Title,
                        CommentId = comment.CommentId,
                        CreateTime = comment.CreateTime,
                        Fullname = comment.Fullname,
                        AnswerToName = comment.AnswerToName,
                        Email = comment.Email,
                        IsActive = comment.IsActive
                    }
                ).OrderByDescending(c => c.CreateTime).ToList();

            return View(comments);
        }

        [HttpPost]
        public IActionResult GetComment(int commentId)
        {
            var comment = commentRepository.GetById(commentId);
            return new JsonResult(comment.Text);
        }

        [HttpPost]
        public IActionResult ValidateComment(int commentId)
        {
            var comment = commentRepository.GetById(commentId);
            comment.IsActive = true;
            
            return new JsonResult(commentRepository.UpdateComment(comment));
        }

        [HttpPost]
        public IActionResult ReplyValidateComment(int CommentId, string CommentText, string CommentName, string CommentEmail)
        {
            if (ModelState.IsValid)
            {
                var comment = commentRepository.GetById(CommentId);
                comment.IsActive = true;
                if (commentRepository.UpdateComment(comment))
                {
                    int parentID;
                    if(comment.ParentId == 0)
                    {
                        parentID = comment.CommentId;
                    }
                    else
                    {
                        parentID = comment.ParentId;
                    }

                    Comment com = new Comment {
                        Fullname = CommentName,
                        Email = CommentEmail,
                        Text = CommentText,
                        Type = "admin",
                        CreateTime = DateTime.Now,
                        IsActive = true,
                        BlogId = comment.BlogId,
                        AnswerToName = comment.Fullname,
                        ParentId = parentID
                    };

                    if (commentRepository.AddComment(com))
                    {
                        return new JsonResult(true);
                    }
                }
            }

            return new JsonResult(false);
        }

        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                if (commentRepository.DeleteComment(id))
                {
                    TempData["DeleteCommentSuccess"] = "Yorum başarılı bir şekilde silindi.";
                }
                else
                {
                    TempData["DeleteCommentError"] = "Yorum silinirken bir hata oluştu!";
                }
            }

            return RedirectToAction("UnApprovedComments");
        }

        public IActionResult DeleteValidated(int id)
        {
            if (ModelState.IsValid)
            {
                if (commentRepository.DeleteComment(id))
                {
                    TempData["DeleteCommentSuccess"] = "Yorum başarılı bir şekilde silindi.";
                }
                else
                {
                    TempData["DeleteCommentError"] = "Yorum silinirken bir hata oluştu!";
                }
            }

            return RedirectToAction("ApprovedComments");
        }

        public IActionResult DeleteAdmin(int id)
        {
            if (ModelState.IsValid)
            {
                if (commentRepository.DeleteComment(id))
                {
                    TempData["DeleteCommentSuccess"] = "Yorum başarılı bir şekilde silindi.";
                }
                else
                {
                    TempData["DeleteCommentError"] = "Yorum silinirken bir hata oluştu!";
                }
            }

            return RedirectToAction("AdminComments");
        }

        public IActionResult UpdateAdmin(int id)
        {
            if (TempData["UpdateCommentSuccess"] != null) { ViewBag.UpdateCommentSuccess = TempData["UpdateCommentSuccess"]; }
            if (TempData["UpdateCommentError"] != null) { ViewBag.UpdateCommentError = TempData["UpdateCommentError"]; }

            var comment = commentRepository.GetById(id);
            return View(comment);
        }

        public IActionResult EditAdminComment(Comment comment)
        {
            if (ModelState.IsValid)
            {
                if (commentRepository.UpdateComment(comment))
                {
                    TempData["UpdateCommentSuccess"] = "Yorum bilgileri güncellendi.";
                }
                else
                {
                    TempData["UpdateCommentError"] = "Yorum bilgileri güncellenirken bir hata oluştu! Lütfen daha sonra tekrar deneyiniz.";
                }
            }

            return RedirectToAction("UpdateAdmin", new { id = comment.CommentId });
        }
    }
}