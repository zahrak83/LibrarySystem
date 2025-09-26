using LibrarySystem.Entities;

namespace LibrarySystem.Interface.Service
{
    public interface IBookService
    {
        int AddBook(string title, string author, int categoryId);
        void UpdateBook(int id, string title, string author, int categoryId);
        void DeleteBook(int id);
        List<Book> GetAllBooks();
        Book? GetBookById(int id);
    }
}
