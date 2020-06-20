using BlogWeb.Data.Abstract;
using BlogWeb.Data.Concrete.EFCore;
using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Concrete
{
    public class BlogRepository : IBlogRepository
    {
        private readonly DBContext context;
        public BlogRepository(DBContext _context)
        {
            context = _context;
        }
        public bool AddBlog(Blog blog)
        {
            try {
                blog.CreateDate = DateTime.Now;
                blog.VisitorHit = "0";
                context.Blogs.Add(blog);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteBlog(int blogId)
        {
            try {
                var blog = context.Blogs.First(p => p.BlogId == blogId);
                if (blog != null)
                {
                    context.Blogs.Remove(blog);
                    context.SaveChanges();
                }
                return true;
            }
            catch {

                return false;
            }
        }

        public IQueryable<Blog> GetAll()
        {
            return context.Blogs;
        }

        public Blog GetById(int blogId)
        {
            return context.Blogs.First(p => p.BlogId == blogId);
        }

        public bool UpdateBlog(Blog blog)
        {
            try
            {
                var blogUpdated = GetById(blog.BlogId);
                if (blogUpdated != null)
                {
                    blogUpdated.Description = blog.Description;
                    blogUpdated.Title = blog.Title;
                    blogUpdated.Explanation = blog.Explanation;
                    blogUpdated.Text = blog.Text;
                    blogUpdated.ImageUrl = blog.ImageUrl;
                    if (blog.IsActive == true && blogUpdated.IsActive == false) { blogUpdated.PublishedDate = DateTime.Now; }
                    blogUpdated.IsActive = blog.IsActive;
                    blogUpdated.CategoryId = blog.CategoryId;
                    blogUpdated.LastEditDate = DateTime.Now;
                    blogUpdated.VisitorHit = blog.VisitorHit;
                    blogUpdated.IsMailSend = blog.IsMailSend;

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
