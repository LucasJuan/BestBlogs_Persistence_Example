using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    public class CommentRepository
    {
        private readonly BlogContext _context;

        public CommentRepository(BlogContext context)
        {
            _context = context;
        }

        public IEnumerable<Comment> GetAll()
        {
            return _context.Comments.ToList();
        }

        public Comment Get(Guid id)
        {
            return _context.Comments.Where(c => c.Id.Equals(id)).FirstOrDefault();
        }

        public Comment Create(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
            return _context.Comments.Where(p => p.Id.Equals(comment.Id)).FirstOrDefault();
        }

        public Comment Update(Comment comment)
        {
            Comment updateValue = _context.Comments.Where(c => c.Id.Equals(comment.Id)).FirstOrDefault();
            updateValue.Author = comment.Author;
            updateValue.Content = comment.Content;
            updateValue.PostId = comment.PostId;
            _context.Entry(updateValue).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return updateValue;
        }

        public bool Delete(Guid id)
        {

            Comment objRemove = _context.Comments.Where(c => c.Id.Equals(id)).FirstOrDefault();

            if (objRemove != null)
            {
                _context.Remove(objRemove).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            else
                return false;

        }

        public IEnumerable<Comment> GetByPostId(Guid postId)
        {
            return _context.Comments.Where(c => c.Id.Equals(postId)).ToList();
        }

    }
}