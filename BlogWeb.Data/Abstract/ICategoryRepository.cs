using System;
using BlogWeb.Entity;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BlogWeb.Data.Abstract
{
    public interface ICategoryRepository
    {
        Category GetById(int categoryId);
        IQueryable<Category> GetAll();
        bool AddCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(int categoryId);
    }
}
