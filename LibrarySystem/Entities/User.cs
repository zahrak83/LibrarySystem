
using LibrarySystem.Enum;

namespace LibrarySystem.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public RoleEnum Role { get; set; }
        public List<BorrowedBook> BorrowedBook { get; set; } = new();
        public List<Review> Reviews { get; set; } = new();
        public List<Wishlist> Wishlists { get; set; } = new();
        public decimal PenaltyAmount { get; set; } = 0;
    }
}
