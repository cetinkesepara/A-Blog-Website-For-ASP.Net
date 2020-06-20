using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Abstract
{
    public interface IAdminRepository
    {
        IQueryable<Admin> GetAll();
    }
}
