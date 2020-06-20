using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWeb.Data.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogWeb.WebUI.Controllers
{
    public class EmailRegistrationController : Controller
    {
        private readonly IEmailRegRepository emailRegRepository;
        public EmailRegistrationController(IEmailRegRepository _emailRegRepository)
        {
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
            ViewBag.SidebarActive = "EmailRegistration";
        }

        public IActionResult Index()
        {
            if (TempData["EmailRegDeleteSuccess"] != null) { ViewBag.EmailRegDeleteSuccess = TempData["EmailRegDeleteSuccess"]; }
            if (TempData["EmailRegDeleteError"] != null) { ViewBag.EmailRegDeleteError = TempData["EmailRegDeleteError"]; }
            return View(emailRegRepository.GetAll().OrderByDescending(p=>p.CreateTime).ToList());
        }

        public IActionResult Delete(int id)
        {
            var email = emailRegRepository.GetById(id);
            if (ModelState.IsValid)
            {
                if (emailRegRepository.DeleteEmailReg(email))
                {
                    TempData["EmailRegDeleteSuccess"] = "'" + email.EmailAddress + "', e-posta adresi başarıyla silindi. ";
                }
                else
                {
                    TempData["EmailRegDeleteError"] = "E-posta adresi silinirken bir hata meydana geldi! Lütfen daha sonra tekrar deneyiniz.";
                }
            }
            return RedirectToAction("Index");
        }
    }
}