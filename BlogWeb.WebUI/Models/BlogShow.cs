using BlogWeb.Data.Abstract;
using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogWeb.WebUI.Models
{
    public class BlogShow
    {
        public static List<BlogShowItems> GetShowItems(List<Blog> blogs, ICommentRepository commentRepository, ICategoryRepository categoryRepository)
        {
            List<BlogShowItems> blogShows = new List<BlogShowItems>();
            foreach (var item in blogs)
            {
                var category = categoryRepository.GetById(item.CategoryId);
                int commentCount = commentRepository.GetAll().Where(p => p.IsActive == true && p.BlogId == item.BlogId).ToList().Count();
                string visitorHit = item.VisitorHit;
                string visitorHitFiltered;
                if(visitorHit.Length >= 4 && visitorHit.Length < 7)
                {
                    visitorHitFiltered = visitorHit.Substring(0, visitorHit.Length - 3) + "." + visitorHit.Substring(visitorHit.Length - 3);
                }
                else if(visitorHit.Length >= 7)
                {
                    visitorHitFiltered = visitorHit.Substring(0, visitorHit.Length - 6) + "M+";
                }
                else
                {
                    visitorHitFiltered = visitorHit;
                }

                blogShows.Add(new BlogShowItems
                {
                    BlogId = item.BlogId,
                    Title = item.Title,
                    Explanation = item.Explanation,
                    ImageUrl = item.ImageUrl,
                    VisitorHit = visitorHitFiltered,
                    PublishedDate = item.PublishedDate,
                    CategoryName = category.Name,
                    CategoryId = category.CategoryId,
                    CommentCount = commentCount
                });
            }
            return blogShows;
        }
    }
}
