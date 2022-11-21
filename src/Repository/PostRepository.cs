using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model;

namespace Repository
{
    public class PostRepository
    {
        private readonly BlogContext _context;

        public PostRepository(BlogContext context)
        {
            _context = context;
        }

        public IEnumerable<Post> GetAll()
        {
            return _context.Posts.ToList();
        }

        public Post Get(Guid id)
        {
            return _context.Posts.Where(c => c.Id.Equals(id)).FirstOrDefault();
        }

        public Post Create(Post Post)
        {
            _context.Posts.Add(Post);
            _context.SaveChanges();
            return _context.Posts.Where(p => p.Id.Equals(Post.Id)).FirstOrDefault();
        }

        public Post Update(Post Post)
        {
            Post updateValue = _context.Posts.Where(c => c.Id.Equals(Post.Id)).FirstOrDefault();
            updateValue.Title = Post.Title;
            updateValue.Content = Post.Content;
            updateValue.CreationDate = Post.CreationDate;
            _context.Entry(updateValue).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();

            return updateValue;
        }

        public bool Delete(Guid id)
        {
            Post objRemove = _context.Posts.Where(c => c.Id.Equals(id)).FirstOrDefault();

            if (objRemove != null)
            {
                _context.Remove(objRemove).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
                _context.SaveChanges();
                return true;
            }
            else
                return false;

        }

    }
}