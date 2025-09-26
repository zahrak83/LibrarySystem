using LibrarySystem.Entities;

namespace LibrarySystem.Interface
{
    public interface IWishlistRepository
    {
        int Create(Wishlist wishlist);
        void Delete(int id);
        List<Wishlist> GetByUserId(int userId);
        List<Wishlist> GetByBookId(int bookId);
    }
}
