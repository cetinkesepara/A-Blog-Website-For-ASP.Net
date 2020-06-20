using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Abstract
{
    public interface IBlogRepository
    {
        Blog GetById(int blogId);
        IQueryable<Blog> GetAll();
        bool AddBlog(Blog blog);
        bool UpdateBlog(Blog blog);
        bool DeleteBlog(int blogId);
    }
}
