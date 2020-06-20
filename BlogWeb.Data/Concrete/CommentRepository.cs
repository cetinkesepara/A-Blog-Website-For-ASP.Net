using BlogWeb.Data.Abstract;
using BlogWeb.Data.Concrete.EFCore;
using BlogWeb.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlogWeb.Data.Concrete
{
    public class CommentRepository : ICommentRepository
    {
        private readonly DBContext context;
        public CommentRepository(DBContext _context)
        {
            context = _context;
        }
        public bool AddComment(Comment comment)
        {
            try
            {
                context.Comments.Add(comment);
                context.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteComment(int commentId)
        {
            try
            {
                var comment = context.Comments.First(p => p.CommentId == commentId);
                if(comment != null)
                {
                    context.Comments.Remove(comment);
                    context.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<Comment> GetAll()
        {
            return context.Comments;
        }

        public Comment GetById(int commentId)
        {
            return context.Comments.First(p => p.CommentId == commentId);
        }

        public bool UpdateComment(Comment comment)
        {
            try
            {
                var commentUpdated = GetById(comment.CommentId);
                if(commentUpdated != null)
                {
                    commentUpdated.Fullname = comment.Fullname;
                    commentUpdated.Email = comment.Email;
                    commentUpdated.Type = comment.Type;
                    commentUpdated.CreateTime = comment.CreateTime;
                    commentUpdated.ParentId = comment.ParentId;
                    commentUpdated.BlogId = comment.BlogId;
                    commentUpdated.Text = comment.Text;
                    commentUpdated.IsActive = comment.IsActive;
                    commentUpdated.AnswerToName = comment.AnswerToName;
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
