using LibrarySystem.Entities;
using LibrarySystem.Infrastructure;
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

           
            var isAlreadyBorrowed = _borrowedBookRepository.GetAll()
                .Any(bb => bb.BookId == bookId && bb.UserId != userId);

            if (isAlreadyBorrowed)
                throw new Exception("This book is already borrowed by another user.");

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
        public int ReturnBook(int borrowedBookId)
        {
            var borrowedBook = _borrowedBookRepository.GetById(borrowedBookId);
            if (borrowedBook == null)
                throw new Exception("Borrowed book not found.");

            var daysBorrowed = (DateTime.Now - borrowedBook.BorrowTime).Days;
            int penalty = 0;
            if (daysBorrowed > 7)
            {
                penalty = (daysBorrowed - 7) * 10000;
                borrowedBook.User.PenaltyAmount += penalty;
            }

            _borrowedBookRepository.Delete(borrowedBookId);

            return penalty; 
        }

    }
}
