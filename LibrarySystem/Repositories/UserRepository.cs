using LibrarySystem.Entities;
using LibrarySystem.Infrastructure;
using LibrarySystem.Interface;
using Microsoft.EntityFrameworkCore;

namespace LibrarySystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        AppDBContext _context = new AppDBContext();
        public int Create(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }
        public void Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }
        public User? GetById(int id)
        {
            return _context.Users
                .Include(u => u.BorrowedBook)
                .ThenInclude(bb => bb.Book)
                .ThenInclude(b => b.Category)
                .FirstOrDefault(u => u.Id == id);
        }
        public User GetByUsername(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);
        }
        public void Update(User user)
        {
            var u = _context.Users.Find(user.Id);
            if (u != null)
            {
                u.UserName = user.UserName;
                u.Password = user.Password;
                _context.SaveChanges();
            }
        }
    }
}
