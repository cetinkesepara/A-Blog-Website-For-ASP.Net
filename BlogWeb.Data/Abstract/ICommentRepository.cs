using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Abstract
{
    public interface ICommentRepository
    {
        Comment GetById(int commentId);
        IQueryable<Comment> GetAll();
        bool AddComment(Comment comment);
        bool UpdateComment(Comment comment);
        bool DeleteComment(int commentId);
    }
}
