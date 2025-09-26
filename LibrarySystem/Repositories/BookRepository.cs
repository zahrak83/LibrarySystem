using LibrarySystem.Entities;
using LibrarySystem.Infrastructure;
using LibrarySystem.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class BookRepository : IBookRepository
    {
        AppDBContext _context = new AppDBContext();

        public int Create(Book book)
        {
            _context.Books.Add(book);
            _context.SaveChanges();
            return book.Id;
        }
        public void Delete(int id)
        {
            var b = _context.Books.Find(id);

            if (b != null)
            {
                _context.Books.Remove(b);
                _context.SaveChanges();
            }
        }
        public List<Book> GetAll()
        {
            return _context.Books
                .Include(b => b.Category)
                .Include(b => b.Reviews)
                .ThenInclude(r => r.User)
                .ToList();
        }
        public Book? GetById(int id)
        {
            return _context.Books.Include(b => b.Category).FirstOrDefault(b => b.Id == id);

        }
        public void Update(int id, string title, string author, int categoryId)
        {
            var book = _context.Books.Find(id);

            if (book != null)
            {
                book.Title = title;
                book.Author = author;
                book.CategoryId = categoryId;
                _context.SaveChanges();
            }
        }
    }
}
