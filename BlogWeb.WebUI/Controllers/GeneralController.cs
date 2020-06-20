using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogWeb.WebUI.Controllers
{
    public class GeneralController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);

            if(HttpContext.Session.GetString("username") == null)
            {
                HttpContext.Response.Redirect("/Home/PageNotFound");
            }

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);
            ViewBag.SidebarActive = "General";
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}