using LibrarySystem.Entities;

public interface IWishlistService
{
    void AddToWishlist(int userId, int bookId);
    void RemoveFromWishlist(int wishlistId);
    List<Wishlist> GetWishlistByUser(int userId);
    public List<Wishlist> GetWishlistByBook(int bookId);
}
