using LibrarySystem.Entities;
using LibrarySystem.Infrastructure;
using LibrarySystem.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        AppDBContext _context = new AppDBContext();
        public int Create(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            _context.SaveChanges();
            return wishlist.Id;
        }
        public void Delete(int id)
        {
           var w = _context.Wishlists.Find(id);

            if (w != null)
            {
                _context.Wishlists.Remove(w);
                _context.SaveChanges();
            }

        }
        public List<Wishlist> GetByBookId(int bookId)
        {
            return _context.Wishlists
                .Include(w => w.User)
                .Where(w => w.BookId == bookId)
                .ToList();
        }
        public List<Wishlist> GetByUserId(int userId)
        {
            return _context.Wishlists
                .Include(w => w.Book)
                .Where(w => w.UserId == userId)
                .ToList();
        }
    }
}
