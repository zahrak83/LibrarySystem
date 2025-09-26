using LibrarySystem.Entities;
using LibrarySystem.Repositories;

public class WishlistService : IWishlistService
{
    private readonly WishlistRepository _wishlistRepository = new WishlistRepository();

    public void AddToWishlist(int userId, int bookId)
    {
        var item = new Wishlist { UserId = userId, BookId = bookId };
        _wishlistRepository.Create(item);
    }
    public void RemoveFromWishlist(int wishlistId)
    {
        _wishlistRepository.Delete(wishlistId);
    }
    public List<Wishlist> GetWishlistByUser(int userId)
    {
        return _wishlistRepository.GetByUserId(userId);
    }
    public List<Wishlist> GetWishlistByBook(int bookId)
    {
        return _wishlistRepository.GetByBookId(bookId);
    }

}

