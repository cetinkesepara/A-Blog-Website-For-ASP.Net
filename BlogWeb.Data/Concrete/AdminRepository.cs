using BlogWeb.Data.Abstract;
using BlogWeb.Data.Concrete.EFCore;
using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Concrete
{
    public class AdminRepository : IAdminRepository
    {
        private readonly DBContext context;
        public AdminRepository(DBContext _context)
        {
            context = _context;
        }
        public IQueryable<Admin> GetAll()
        {
            return context.Admins;
        }
    }
}
