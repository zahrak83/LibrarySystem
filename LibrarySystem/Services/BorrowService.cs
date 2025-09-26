using LibrarySystem.Entities;
using LibrarySystem.Interface.Service;
using LibrarySystem.Repositories;

namespace LibrarySystem.Services
{
    public class BorrowService : IBorrowService
    {
        private readonly BorrowedBookRepository _borrowedBookRepository = new BorrowedBookRepository();

        public int BorrowBook(int userId, int bookId)
        {

            var alreadyBorrowedByUser = _borrowedBookRepository
                .GetByUserId(userId)
                .Any(bb => bb.BookId == bookId);

            if (alreadyBorrowedByUser)
                throw new Exception("You already borrowed this book.");


            var borrowedBook = new BorrowedBook
            {
                UserId = userId,
                BookId = bookId,
                BorrowTime = DateTime.Now
            };

            return _borrowedBookRepository.Create(borrowedBook);
        }
        public List<BorrowedBook> GetBorrowedBooksByUser(int userId)
        {
            return _borrowedBookRepository.GetByUserId(userId);
        }
        public List<BorrowedBook> GetAllBorrowedBooks()
        {
            return _borrowedBookRepository.GetAll();
        }
        public void ReturnBook(int borrowedBookId)
        {
            var borrowedBook = _borrowedBookRepository.GetById(borrowedBookId);
            if (borrowedBook != null)
            {
                _borrowedBookRepository.Delete(borrowedBookId);
            }
            else
            {
                throw new Exception("Borrowed book not found.");
            }
        }

    }
}
