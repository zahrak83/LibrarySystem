using LibrarySystem.Entities;
using LibrarySystem.Interface;
using LibrarySystem.Interface.Service;
using LibrarySystem.Repositories;

namespace LibrarySystem.Services
{
    public class BookService : IBookService
    {
        private readonly BookRepository _bookRepository = new BookRepository();

        public int AddBook(string title, string author, int categoryId)
        {
            var book = new Book
            {
                Title = title,
                Author = author,
                CategoryId = categoryId
            };
            return _bookRepository.Create(book);
        }
        public void UpdateBook(int id, string title, string author, int categoryId)
        {
            _bookRepository.Update(id, title, author, categoryId);
        }
        public void DeleteBook(int id)
        {
            _bookRepository.Delete(id);
        }
        public List<Book> GetAllBooks()
        {
            return _bookRepository.GetAll();
        }
        public Book? GetBookById(int id)
        {
            return _bookRepository.GetById(id);
        }
    }
}
