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
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository categoryRepository;
        private readonly IBlogRepository blogRepository;
        public CategoryController(ICategoryRepository _categoryRepository, IBlogRepository _blogRepository)
        {
            categoryRepository = _categoryRepository;
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
            ViewBag.SidebarActive = "Category";
        }

        public IActionResult Index()
        {
            return RedirectToAction("List");
        }
        public IActionResult List()
        {
            if (TempData["AddCategorySuccess"] != null) { ViewBag.AddCategorySuccess = TempData["AddCategorySuccess"]; }
            if (TempData["AddCategoryDanger"] != null) { ViewBag.AddCategoryDanger = TempData["AddCategoryDanger"]; }
            if (TempData["DeleteCategorySuccess"] != null) { ViewBag.DeleteCategorySuccess = TempData["DeleteCategorySuccess"]; }
            if (TempData["DeleteCategoryDanger"] != null) { ViewBag.DeleteCategoryDanger = TempData["DeleteCategoryDanger"]; }
            if (TempData["DelCatRelationshipWarning"] != null) { ViewBag.DelCatRelationshipWarning = TempData["DelCatRelationshipWarning"]; }

            List<CategoriesWithParents> categoriesWithParents = new List<CategoriesWithParents>();
            foreach (var item in categoryRepository.GetAll().ToList())
            {
                string parentName;

                if (item.ParentId != 0)
                {
                    var cat = categoryRepository.GetById(item.ParentId);
                    parentName = cat.Name;
                }
                else
                {
                    parentName = "-";
                }

                categoriesWithParents.Add( new CategoriesWithParents { 
                    CategoryId = item.CategoryId,
                    Description = item.Description,
                    IsActive = item.IsActive,
                    Name = item.Name,
                    ParentName = parentName
                });
            }
            

            return View(categoriesWithParents.OrderBy(p=>p.ParentName));
        }

        public IActionResult Create()
        {
            ViewBag.Categories = categoryRepository.GetAll().Where(p=>p.ParentId == 0).ToList();
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category entity)
        {
            if (ModelState.IsValid)
            {
                if (categoryRepository.AddCategory(entity))
                {
                    TempData["AddCategorySuccess"] = entity.Name.ToUpper() + " kategorisi eklendi.";
                }
                else
                {
                    TempData["AddCategoryDanger"] = entity.Name.ToUpper() + " kategorisi eklenirken bir hata oluştu!";
                }
            }

            return RedirectToAction("List");
        }

    
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                var category = categoryRepository.GetById(id);
                if(category.ParentId != 0)
                {
                    var blog = blogRepository.GetAll().Where(p => p.CategoryId == id);

                    if (blog.Any() == false)
                    {
                        if (categoryRepository.DeleteCategory(id))
                        {
                            TempData["DeleteCategorySuccess"] = category.Name.ToUpper() + " kategorisi silindi.";
                        }
                        else
                        {
                            TempData["DeleteCategoryDanger"] = category.Name.ToUpper() + " kategorisi silinirken bir hata oluştu!";
                        }
                    }
                    else
                    {
                        TempData["DelCatRelationshipWarning"] = category.Name.ToUpper() + " kategorisi bir blog için kullanılmakta olduğundan silinemez!";
                    }
                }
                else
                {
                    var subCategories = categoryRepository.GetAll().Where(p => p.ParentId == category.CategoryId);

                    if (subCategories.Any() == false)
                    {
                        if (categoryRepository.DeleteCategory(id))
                        {
                            TempData["DeleteCategorySuccess"] = category.Name.ToUpper() + " kategorisi silindi.";
                        }
                        else
                        {
                            TempData["DeleteCategoryDanger"] = category.Name.ToUpper() + " kategorisi silinirken bir hata oluştu!";
                        }
                    }
                    else
                    {
                        TempData["DelCatRelationshipWarning"] = category.Name.ToUpper() + " kategorisi bir alt kategori için kullanılmakta olduğundan silinemez!";
                    }
                }


            }

            return RedirectToAction("List");
        }

        public IActionResult Update(int id)
        {
            if (TempData["UpdateCategorySuccess"] != null) { ViewBag.UpdateCategorySuccess = TempData["UpdateCategorySuccess"]; }
            if (TempData["UpdateCategoryDanger"] != null) { ViewBag.UpdateCategoryDanger = TempData["UpdateCategoryDanger"]; }

            ViewBag.Categories = categoryRepository.GetAll().Where(p => p.ParentId == 0);

            return View(categoryRepository.GetById(id));
        }

        [HttpPost]
        public IActionResult Update(Category category)
        {
            if (ModelState.IsValid)
            {
                if (categoryRepository.UpdateCategory(category))
                {
                    TempData["UpdateCategorySuccess"] = "Kategori güncellendi.";
                }
                else
                {
                    TempData["UpdateCategoryDanger"] = "Kategori güncellenirken bir hata oluştu!";
                }
            }

            return RedirectToAction("Update", new { id = category.CategoryId});
        }
    }
}