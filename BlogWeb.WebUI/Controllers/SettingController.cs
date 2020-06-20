using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWeb.Data.Abstract;
using BlogWeb.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogWeb.WebUI.Controllers
{
    public class SettingController : Controller
    {
        private readonly ISettingRepository settingRepository;
        private readonly IBlogRepository blogRepository;
        public SettingController(ISettingRepository _settingRepository, IBlogRepository _blogRepository)
        {
            settingRepository = _settingRepository;
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
            ViewBag.SidebarActive = "Setting";
        }
        public IActionResult Index()
        {
            if (TempData["SettingUpdateSuccess"] != null) { ViewBag.SettingUpdateSuccess = TempData["SettingUpdateSuccess"]; }
            if (TempData["SettingUpdateError"] != null) { ViewBag.SettingUpdateError = TempData["SettingUpdateError"]; }

            return View(settingRepository.GetById(1));
        }

        [HttpPost]
        public IActionResult UpdateSetting(Setting setting)
        {
            if (ModelState.IsValid)
            {
                if (settingRepository.UpdateSetting(setting))
                {
                    TempData["SettingUpdateSuccess"] = "Ayarlar güncellendi.";
                }
                else
                {
                    TempData["SettingUpdateError"] = "Ayarlar güncellenirken bir hata oluştu.";
                }
            }
            return RedirectToAction("Index");
        }
    }
}