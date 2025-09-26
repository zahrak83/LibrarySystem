using LibrarySystem.Entities;
using LibrarySystem.Interface;
using LibrarySystem.Interface.Service;
using LibrarySystem.Repositories;

namespace LibrarySystem.Services
{
    public class ReviewService : IReviewService
    {
        private readonly ReviewRepository _reviewRepository = new ReviewRepository();

        public int AddReview(int userId, int bookId, string? comment, int rating)
        {
            if (rating < 1 || rating > 5)
                throw new Exception("Rating must be between 1 and 5.");

            var review = new Review
            {
                UserId = userId,
                BookId = bookId,
                Comment = comment,
                Rating = rating,
                CreatedAt = DateTime.Now
            };

            return _reviewRepository.Create(review);
        }

        public void ApproveReview(int reviewId)
        {
            var review = _reviewRepository.GetById(reviewId);
            if (review == null)
                throw new Exception("Review not found.");

            review.IsApproved = true;
            _reviewRepository.Update(review);
        }

        public void DeleteReview(int reviewId)
        {
            _reviewRepository.Delete(reviewId);
        }

        public void EditReview(int reviewId, string? comment, int rating)
        {
            if (rating < 1 || rating > 5)
                throw new Exception("Rating must be between 1 and 5.");

            var review = _reviewRepository.GetById(reviewId);
            if (review == null)
                throw new Exception("Review not found.");

            review.Comment = comment;
            review.Rating = rating;
            _reviewRepository.Update(review);
        }

        public List<Review> GetAllReviews()
        {
            return _reviewRepository.GetAll();
        }

        public List<Review> GetReviewsByBook(int bookId)
        {
            return _reviewRepository.GetByBookId(bookId);
        }

        public List<Review> GetReviewsByUser(int userId)
        {
            return _reviewRepository.GetByUserId(userId);
        }
    }
}

