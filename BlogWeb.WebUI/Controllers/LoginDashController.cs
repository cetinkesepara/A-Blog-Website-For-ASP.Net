using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogWeb.Data.Abstract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlogWeb.WebUI.Controllers
{
    public class LoginDashController : Controller
    {
        private readonly IAdminRepository adminRepository;

        public LoginDashController(IAdminRepository _adminRepository)
        {
            adminRepository = _adminRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string userName, string password)
        {
            var admin = adminRepository.GetAll().Where(p => p.IsActive == true && p.Username == userName && p.Password == password).ToList();

            if (admin.Any())
            {
                HttpContext.Session.SetString("username", userName);
                return RedirectToAction("Index", "General");
            }

            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index");
        }
    }
}