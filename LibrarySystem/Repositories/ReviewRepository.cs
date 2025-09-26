using LibrarySystem.Entities;
using LibrarySystem.Infrastructure;
using LibrarySystem.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        AppDBContext _context = new AppDBContext();

        public int Create(Review review)
        {
            review.CreatedAt = DateTime.Now;
            _context.Reviews.Add(review);
            _context.SaveChanges();
            return review.Id;
        }
        public void Delete(int id)
        {
            var review = _context.Reviews.Find(id);
            if (review != null)
            {
                _context.Reviews.Remove(review);
                _context.SaveChanges();
            }
        }
        public List<Review> GetAll()
        {
            return _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Book)
                .ThenInclude(b => b.Category)
                .ToList();
        }
        public Review? GetById(int id)
        {
            return _context.Reviews
                .Include(r => r.User)
                .Include(r => r.Book)
                .ThenInclude(b => b.Category)
                .FirstOrDefault(r => r.Id == id);
        }
        public List<Review> GetByBookId(int bookId)
        {
            return _context.Reviews
                .Include(r => r.User)
                .Where(r => r.BookId == bookId && r.IsApproved)
                .ToList();
        }
        public List<Review> GetByUserId(int userId)
        {
            return _context.Reviews
                .Include(r => r.Book)
                .ThenInclude(b => b.Category)
                .Where(r => r.UserId == userId)
                .ToList();
        }
        public void Update(Review review)
        {
            var r = _context.Reviews.Find(review.Id);
            if (r != null)
            {
                r.Comment = review.Comment;
                r.Rating = review.Rating;
                r.IsApproved = review.IsApproved;
                _context.SaveChanges();
            }
        }
    }
}

