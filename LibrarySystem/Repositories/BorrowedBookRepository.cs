using LibrarySystem.Entities;
using LibrarySystem.Infrastructure;
using LibrarySystem.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class BorrowedBookRepository : IBorrowedBookRepository
    {
        AppDBContext _context = new AppDBContext();
        public int Create(BorrowedBook book)
        {
            _context.BorrowedBooks.Add(book);
            _context.SaveChanges();
            return book.Id;
        }
        public void Delete(int id)
        {
            var borrowedBook = _context.BorrowedBooks.Find(id);
            if (borrowedBook != null)
            {
                _context.BorrowedBooks.Remove(borrowedBook);
                _context.SaveChanges();
            }
        }
        public List<BorrowedBook> GetAll()
        {
            return _context.BorrowedBooks
                .Include(bb => bb.Book)
                .ThenInclude(b => b.Category)
                .Include(bb => bb.User)
                .ToList();

        }
        public BorrowedBook? GetById(int id)
        {
            return _context.BorrowedBooks
                .Include(bb => bb.User)
                .FirstOrDefault(bb => bb.Id == id);
        }

        public List<BorrowedBook> GetByUserId(int userId)
        {
            return _context.BorrowedBooks
                .Include(bb => bb.Book)
                .ThenInclude(b => b.Category)
                .Where(bb => bb.UserId == userId)
                .ToList();
        }
        public void Update(BorrowedBook book)
        {
            var b = _context.BorrowedBooks.Find(book.Id);

            if ( b != null)
            {
                b.UserId = book.UserId;
                b.BookId = book.BookId;
                b.BorrowTime = book.BorrowTime;
                _context.SaveChanges();
            }
        }
    }
}
