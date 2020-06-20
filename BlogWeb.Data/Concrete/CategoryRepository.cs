using BlogWeb.Data.Abstract;
using BlogWeb.Data.Concrete.EFCore;
using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Concrete
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DBContext context;
        public CategoryRepository(DBContext _context)
        {
            context = _context;
        }
        public bool AddCategory(Category category)
        {
            try {
                context.Categories.Add(category);
                context.SaveChanges();
                return true;
            } 
            catch {

                return false;
            }
        }

        public bool DeleteCategory(int categoryId)
        {
            try
            {
                var category = context.Categories.First(p => p.CategoryId == categoryId);
                if (category != null)
                {
                    context.Categories.Remove(category);
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {

                return false;
            }
        }

        public IQueryable<Category> GetAll()
        {
            return context.Categories;
        }

        public Category GetById(int categoryId)
        {
            return context.Categories.First(p => p.CategoryId == categoryId);
        }

        public bool UpdateCategory(Category category)
        {
            try
            {
                var catUpdated = GetById(category.CategoryId);
                if (catUpdated != null)
                {
                    catUpdated.Description = category.Description;
                    catUpdated.Name = category.Name;
                    catUpdated.IsActive = category.IsActive;
                    catUpdated.ParentId = category.ParentId;

                    context.SaveChanges();
                }
                return true;
            }
            catch
            {

                return false;
            }
        }
    }
}
