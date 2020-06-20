using BlogWeb.Data.Abstract;
using BlogWeb.Data.Concrete.EFCore;
using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Concrete
{
    public class GalleryRepository : IGalleryRepository
    {
        private readonly DBContext context;
        public GalleryRepository(DBContext _context)
        {
            context = _context;
        }
        public bool AddGallery(Gallery gallery)
        {
            try {
                context.Galleries.Add(gallery);
                context.SaveChanges();
                return true;
            } 
            catch {
                return false;
            }
        }

        public bool DeleteGallery(int galleryId)
        {
            try
            {
                var gal = context.Galleries.First(p => p.GalleryId == galleryId);
                if (gal != null)
                {
                    context.Galleries.Remove(gal);
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

        public IQueryable<Gallery> GetAll()
        {
            return context.Galleries;
        }

        public IQueryable<Gallery> GetAllByBlogId(int blogId)
        {
            return context.Galleries.Where(p => p.BlogId == blogId);
        }

        public Gallery GetById(int galleryId)
        {
            return context.Galleries.First(p => p.GalleryId == galleryId);
        }

        public bool UpdateGallery(Gallery gallery)
        {
            try
            {
                var galUpdated = GetById(gallery.GalleryId);
                if (galUpdated != null)
                {
                    galUpdated.ImageUrl = gallery.ImageUrl;
                    galUpdated.IsCover = gallery.IsCover;
                    galUpdated.BlogId = gallery.BlogId;

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
