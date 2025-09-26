using LibrarySystem.Entities;

namespace LibrarySystem.Interface.Service
{
    public interface IReviewService
    {
        int AddReview(int userId, int bookId, string? comment, int rating);
        void EditReview(int reviewId, string? comment, int rating);
        void DeleteReview(int reviewId);
        void ApproveReview(int reviewId); // فقط Admin
        List<Review> GetReviewsByBook(int bookId);
        List<Review> GetReviewsByUser(int userId);
        List<Review> GetAllReviews(); // برای Admin
    }
}
