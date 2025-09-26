using LibrarySystem.Entities;

namespace LibrarySystem.Interface
{
    public interface IBookRepository
    {
        int Create(Book book);
        void Update(int id, string title,string author, int categoryId);
        void Delete(int id);
        Book GetById(int id);
        List<Book> GetAll();
    }
}
