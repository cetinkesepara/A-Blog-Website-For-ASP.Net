using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Abstract
{
    public interface IEmailRegRepository
    {
        EmailRegistration GetById(int emailRegId);
        IQueryable<EmailRegistration> GetAll();
        bool AddEmailReg(EmailRegistration entity);
        bool DeleteEmailReg(EmailRegistration entity);
    }
}
