using LibrarySystem.Entities;

namespace LibrarySystem.Interface
{
    public interface IReviewRepository
    {
        int Create(Review review);
        void Update(Review review);
        void Delete(int id);
        Review? GetById(int id);
        List<Review> GetAll();
        List<Review> GetByBookId(int bookId);
        List<Review> GetByUserId(int userId);
    }
}
