
using LibrarySystem.Entities;

namespace LibrarySystem.Interface
{
    public interface IBorrowedBookRepository
    {
        int Create(BorrowedBook book);
        void Update(BorrowedBook book);
        void Delete(int id);
        BorrowedBook GetById(int id);
        List<BorrowedBook> GetAll();
        List<BorrowedBook> GetByUserId(int userId);
    }
}
