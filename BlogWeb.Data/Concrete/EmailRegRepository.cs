using BlogWeb.Data.Abstract;
using BlogWeb.Data.Concrete.EFCore;
using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Concrete
{
    public class EmailRegRepository : IEmailRegRepository
    {
        private readonly DBContext context;
        public EmailRegRepository(DBContext _context)
        {
            context = _context;
        }

        public bool AddEmailReg(EmailRegistration entity)
        {
            try
            {
                context.EmailRegistrations.Add(entity);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteEmailReg(EmailRegistration entity)
        {
            try
            {
                context.EmailRegistrations.Remove(entity);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<EmailRegistration> GetAll()
        {
            return context.EmailRegistrations;
        }

        public EmailRegistration GetById(int emailRegId)
        {
            return context.EmailRegistrations.First(p => p.EmailRegistrationId == emailRegId);
        }
    }
}
