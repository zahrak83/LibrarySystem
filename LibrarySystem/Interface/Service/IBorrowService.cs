using LibrarySystem.Entities;

namespace LibrarySystem.Interface.Service
{
    public interface IBorrowService
    {
        int BorrowBook(int userId, int bookId);
        List<BorrowedBook> GetBorrowedBooksByUser(int userId);
        public List<BorrowedBook> GetAllBorrowedBooks();
        int ReturnBook(int borrowedBookId);

    }
}
