using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Abstract
{
    public interface IGalleryRepository
    {
        Gallery GetById(int galleryId);
        IQueryable<Gallery> GetAllByBlogId(int blogId);
        IQueryable<Gallery> GetAll();
        bool AddGallery(Gallery gallery);
        bool UpdateGallery(Gallery gallery);
        bool DeleteGallery(int galleryId);
    }
}
