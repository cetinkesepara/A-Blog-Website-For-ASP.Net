using BlogWeb.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlogWeb.Data.Concrete.EFCore
{
    public class DBContext:DbContext
    {
        public DBContext(DbContextOptions<DBContext> options):base(options)
        {

        }

        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<EmailRegistration> EmailRegistrations { get; set; }
        public DbSet<Admin> Admins { get; set; }

    }
}
